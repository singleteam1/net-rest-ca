using Microsoft.AspNetCore.Identity;
using Shared.Abstractions.Common;
using Shared.Entities;

namespace Shared.Abstractions.Repositories
{
    public interface IUserRepository : IRepository<UserEntity, Guid>
    {
        public UserManager<UserEntity> _accountManager { get; }
    }
}
