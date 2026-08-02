namespace Shared.Abstractions.Common
{
    public interface IBaseModel
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
