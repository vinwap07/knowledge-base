using System.Data;
using knowledgeBase.DataBase;

namespace knowledgeBase.Repositories;

public abstract class BaseRepository<T, IdType>
{
    protected IDatabaseConnection _connection;
    public abstract Task<List<T>> GetAll();
    public abstract Task<T?> GetById(IdType id);
    public abstract Task<bool> Create(T entity);
    public abstract Task<bool> Update(T entity);
    public abstract Task<bool> Delete(IdType id);
    protected abstract T MapFromReader(IDataReader reader);
}