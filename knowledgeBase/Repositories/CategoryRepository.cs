using System.Data;
using knowledgeBase.Entities;
using knowledgeBase.DataBase;

namespace knowledgeBase.Repositories;

public class CategoryRepository : BaseRepository<Category, int>
{
    private readonly IDatabaseConnection _databaseConnection;
    
    public CategoryRepository(IDatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }
    
    public async override Task<Category?> GetById(int id)
    {
        var sql = @"select * from Category where Id = @Id";
        var parameters = new Dictionary<string, object>
        {
            ["@Id"] = id
        };
        
        using var reader = await _databaseConnection.ExecuteReader(sql, parameters);
        if (reader.Read())
        {
            return MapFromReader(reader);
        }
        
        return null;
    }

    public async override Task<List<Category>> GetAll()
    {
        var sql = @"select * from Category";
        var questionLogs = new List<Category>();
        
        using var reader = await _databaseConnection.ExecuteReader(sql);
        if (reader.Read())
        {
            questionLogs.Add(MapFromReader(reader));
        }
        
        return questionLogs;
    }

    public async override Task<bool> Create(Category category)
    {
        var createSql = @"insert into Category (Id, Name, Slug) 
                values (@Id, @Name, @Slug);";
        var createParameters = new Dictionary<string, object>
        {
            ["@Id"] = category.Id,
            ["@Name"] = category.Name,
            ["@Slug"] = category.Slug
        };

        return await _databaseConnection.ExecuteNonQuery(createSql, createParameters) > 0;
    }

    public async override Task<bool> Update(Category category)
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

        return await _databaseConnection.ExecuteNonQuery(sql, parameters) > 0;
    }

    public async override Task<bool> Delete(int id)
    {
        var sql = @"delete from Category where Id = @Id";
        var parameters = new Dictionary<string, object>
        {
            ["@Id"] = id,
        };

        return await _databaseConnection.ExecuteNonQuery(sql, parameters) > 0;
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

