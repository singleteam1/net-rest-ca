namespace Shared.Abstractions.Common
{
    public interface IDataTransferObjectMapper<TDto, TEntity>
    {
        TEntity ToEntity(TDto dto);
    }
}
