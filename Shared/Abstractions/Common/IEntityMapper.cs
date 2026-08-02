namespace Shared.Abstractions.Common
{
    public interface IEntityMapper<TEntity, TDto>
    {
        TDto ToDto(TEntity dto);
    }
}
