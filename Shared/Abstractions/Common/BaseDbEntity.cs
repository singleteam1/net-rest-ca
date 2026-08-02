namespace Shared.Abstractions.Common
{
    public abstract class BaseDbEntity<TId> : IBaseModel
    {
        public TId Id { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
