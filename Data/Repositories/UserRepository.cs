using Data.Context;
using Data.Repositories.Common;
using Microsoft.AspNetCore.Identity;
using Shared.Abstractions.Repositories;
using Shared.Entities;

namespace Data.Repositories
{
    public class UserRepository(DataDbContext context, UserManager<UserEntity> manager) : Repository<UserEntity, Guid>(context), IUserRepository
    {
        public UserManager<UserEntity> _accountManager { get; } = manager;
    }
}
