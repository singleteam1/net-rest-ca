using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared.Abstractions.Repositories;
using Shared.Abstractions.Services;
using Shared.Common.Auth;
using Shared.Common.Configurations;
using Shared.Dtos.Auth;
using Shared.Entities;
using Shared.Exceptions;
using System.Security.Claims;
using Shared.Constants;
using Usecase.Utils.Jwt;

namespace Usecase.Services.Auth
{
    public class AuthService(IOptions<JwtEnvironment> jwtEnvironment, IUserRepository userRepo, SignInManager<UserEntity> signManager, IJwtUtility jwtUtility, IRoleService roleService) : IAuthService
    {
        private readonly SignInManager<UserEntity> _signManager = signManager;
        private readonly IUserRepository _userRepo = userRepo;
        private readonly JwtEnvironment _jwtEnvironment = jwtEnvironment.Value;
        private readonly IJwtUtility _jwtUtility = jwtUtility;
        private readonly IRoleService _roleService = roleService;

        private IEnumerable<Claim> MakeClaims(string email, IEnumerable<string> roles)
        {
            return new List<Claim>() {
                new("email", email),
                new(ClaimTypes.Role, string.Join(",", roles)),
            };
        }

        public async Task<UserDto> Me(string email)
        {
            UserEntity? exist = await _userRepo._accountManager.FindByEmailAsync(email);
            if (exist is null)
            {
                throw new UnauthorizedException("User not found");
            }

            return exist.ToUserDto();
        }

        public async Task<AuthorizationResponse<UserDto>> Register(RegisterDto dto)
        {
            UserEntity? exist = await _userRepo._accountManager.FindByEmailAsync(dto.email);
            if (exist is not null) 
            {
                throw new UnauthorizedException("User already exist");
            }

            IEnumerable<Claim> claims = MakeClaims(dto.email, [UserRoleTypes.Customer]);
            JwtTokenResponse tokens = _jwtUtility.CreateToken(claims);
            UserEntity newUser = new UserEntity()
            {
                Email = dto.email,
                UserName = dto.username,
                DisplayName = dto.username,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                RefreshToken = tokens.RefreshToken,
                RefreshTokenExpiryTime = DateTime.UtcNow.AddSeconds(int.Parse(_jwtEnvironment.RefreshTokenValiditySeconds)),
            };

            var createdResult = await _userRepo._accountManager.CreateAsync(newUser, dto.password);
            if (!createdResult.Succeeded)
            {
                throw new UnauthorizedException("User couldn't create");
            }
            await _signManager.SignInAsync(newUser, false);

            return new AuthorizationResponse<UserDto>()
            {
                User = newUser.ToUserDto(),
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
            };
        }

        public async Task<AuthorizationResponse<UserDto>> Login(LoginDto dto)
        {
            UserEntity? exist = await _userRepo._accountManager.FindByEmailAsync(dto.email);
            if (exist is null) throw new UnauthorizedException("User not found");

            bool isPasswordValid = await _userRepo._accountManager.CheckPasswordAsync(exist, dto.password);
            if (!isPasswordValid) throw new UnauthorizedException("Password is not valid");
            var roles = await _roleService.MyRoles(exist.Email);
            IEnumerable<Claim> claims = MakeClaims(dto.email, roles);
            JwtTokenResponse tokens = _jwtUtility.CreateToken(claims);

            exist.RefreshToken = tokens.RefreshToken;
            exist.RefreshTokenExpiryTime = DateTime.UtcNow.AddSeconds(int.Parse(_jwtEnvironment.RefreshTokenValiditySeconds));
            UserEntity updatedUser = await _userRepo.UpdateAsync(exist);
            return new AuthorizationResponse<UserDto>()
            {
                User = updatedUser.ToUserDto(),
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
            };
        }

        public async Task<AuthorizationResponse<UserDto>> RefreshToken(RefreshTokenDto dto)
        {
            var principal = _jwtUtility.GetPrincipalFromExpiredToken(dto.AccessToken);
            var email = principal.Identities.FirstOrDefault()?.Claims.FirstOrDefault()?.Value;
            var roles = principal.Identities.FirstOrDefault()?.Claims.Skip(1).FirstOrDefault()?.Value;
            if(email is null || roles is null) throw new UnauthorizedException("Invalid claims");
            UserEntity? exist = await _userRepo._accountManager.FindByEmailAsync(email);
            if (exist is null) throw new UnauthorizedException("User not found");
            if (exist.RefreshToken is null || exist.RefreshToken != dto.RefreshToken || exist.RefreshTokenExpiryTime is null)
            {
                throw new BadDataException("Refresh token is invalid");
            }

            if (exist.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new BadDataException("Refresh token is expired");
            }
            IEnumerable<Claim> claims = MakeClaims(email, roles.Split(",", StringSplitOptions.RemoveEmptyEntries));
            JwtTokenResponse tokens = _jwtUtility.CreateToken(claims);

            exist.RefreshToken = tokens.RefreshToken;
            exist.RefreshTokenExpiryTime = DateTime.UtcNow.AddSeconds(int.Parse(_jwtEnvironment.RefreshTokenValiditySeconds));
            UserEntity updatedUser = await _userRepo.UpdateAsync(exist);
            return new AuthorizationResponse<UserDto>()
            {
                User = updatedUser.ToUserDto(),
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
            };
        }

        public async Task<bool> Logout(string email)
        {
            UserEntity? exist = await _userRepo._accountManager.FindByEmailAsync(email);
            if (exist is null) throw new UnauthorizedException("User not found");
            exist.RefreshToken = null;
            exist.RefreshTokenExpiryTime = null;
            UserEntity updatedUser = await _userRepo.UpdateAsync(exist);
            return true;
        }
    }
}
