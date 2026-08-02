using Microsoft.AspNetCore.Identity;

namespace Shared.Abstractions.Common
{
    public abstract class IBaseIndentityUser : IdentityUser<Guid>, IBaseModel
    {
        public DateTime? CreatedAt { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
