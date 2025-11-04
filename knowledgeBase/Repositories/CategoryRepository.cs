using System.Data;
using knowledgeBase.Entities;

namespace knowledgeBase.Repositories;

public class CategoryRepository : BaseRepository<Category, int>
{
    private readonly IDatabaseConnection _databaseConnection;
    
    public CategoryRepository(IDatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }
    
    public override Category? GetById(int id)
    {
        var sql = @"select * from Category where Id = @Id";
        var parameters = new Dictionary<string, object>
        {
            ["@Id"] = id
        };
        
        using var reader = _databaseConnection.ExecuteReader(sql, parameters);
        if (reader.Read())
        {
            return MapFromReader(reader);
        }
        
        return null;
    }

    public override List<Category> GetAll()
    {
        var sql = @"select * from Category";
        var questionLogs = new List<Category>();
        
        using var reader = _databaseConnection.ExecuteReader(sql);
        if (reader.Read())
        {
            questionLogs.Add(MapFromReader(reader));
        }
        
        return questionLogs;
    }

    public override bool Create(Category category)
    {
        var isAlreadyExists = GetById(category.Id) != null;

        if (isAlreadyExists)
        {
            return false;
        }
        
        var createSql = @"insert into Category (Id, Name, Slug) 
                values (@Id, @Name, @Slug);";
        var createParameters = new Dictionary<string, object>
        {
            ["@Id"] = category.Id,
            ["@Name"] = category.Name,
            ["@Slug"] = category.Slug
        };

        try
        {
            _databaseConnection.ExecuteNonQuery(createSql, createParameters);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public override bool Update(Category category)
    {
        var sql = @"update Category
                set Name = @Name, Slug = @Slug
                where Id = @Id";
        var parameters = new Dictionary<string, object>
        {
            ["@Id"] = category.Id,
            ["@Name"] = category.Name,
            ["@Slug"] = category.Slug
        };

        try
        {
            _databaseConnection.ExecuteNonQuery(sql, parameters);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public override bool Delete(int id)
    {
        var sql = @"delete from Category where Id = @Id";
        var parameters = new Dictionary<string, object>
        {
            ["@Id"] = id,
        };

        try
        {
            _databaseConnection.ExecuteNonQuery(sql, parameters);
            return true;
        }
        catch
        {
            return false;
        }
    }

    protected override Category MapFromReader(IDataReader reader)
    {
        return new Category()
        {
            Id = (int)reader["Id"],
            Name = (string)reader["Name"],
            Slug = (string)reader["Slug"]
        };
    }
}

