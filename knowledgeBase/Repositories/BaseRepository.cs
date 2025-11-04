using System.Data;

namespace knowledgeBase.Repositories;

public abstract class BaseRepository<T, IdType>
{
    protected IDatabaseConnection _connection;
    public abstract List<T> GetAll();
    public abstract T? GetById(IdType id);
    public abstract bool Create(T entity);
    public abstract bool Update(T entity);
    public abstract bool Delete(IdType id);
    protected abstract T MapFromReader(IDataReader reader);
}