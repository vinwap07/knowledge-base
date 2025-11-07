using System.Data;
using knowledgeBase.Entities;
using knowledgeBase.DataBase;

namespace knowledgeBase.Repositories;

public class ArticleCategoryRepository : BaseRepository<ArticleCategory, int>
{
    private readonly IDatabaseConnection _databaseConnection;
    
    public ArticleCategoryRepository(IDatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }
    
    public async override Task<ArticleCategory?> GetById(int id)
    {
        var sql = @"SELECT Id, ArticleId, CategoryId FROM ArticleCategory WHERE Id = @Id";
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

    public async override Task<List<ArticleCategory>> GetAll()
    {
        var sql = @"select * from ArticleCategory";
        var articlesCategories = new List<ArticleCategory>();
        
        using var reader = await _databaseConnection.ExecuteReader(sql);
        if (reader.Read())
        {
            articlesCategories.Add(MapFromReader(reader));
        }
        
        return articlesCategories;
    }

    public async override Task<bool> Create(ArticleCategory artCtg)
    {
        var createSql = @"insert into ArticleCategory (Id, ArticleId, CategoryId) 
                values (@Id, @ArticleId, @CategoryId);";
        var createParameters = new Dictionary<string, object>
        {
            ["@Id"] = artCtg.Id,
            ["@ArticleId"] = artCtg.ArticleId,
            ["@CategoryId"] = artCtg.CategoryId
        };

        return await _databaseConnection.ExecuteNonQuery(createSql, createParameters) > 0;
    }

    public async override Task<bool> Update(ArticleCategory artCtg)
    {
        var sql = @"update ArticleCategory 
                set ArticleId = @ArticleId, CategoryId = @CategoryId
                where Id = @Id";
        var parameters = new Dictionary<string, object>
        {
            ["@Id"] = artCtg.Id,
            ["@ArticleId"] = artCtg.ArticleId,
            ["@CategoryId"] = artCtg.CategoryId
        };

        return await _databaseConnection.ExecuteNonQuery(sql, parameters) > 0;
    }

    public async override Task<bool> Delete(int id)
    {
        var sql = @"delete from ArticleCategory where Id = @Id";
        var parameters = new Dictionary<string, object>
        {
            ["@Id"] = id,
        };

        return await _databaseConnection.ExecuteNonQuery(sql, parameters) > 0;
    }

    protected override ArticleCategory MapFromReader(IDataReader reader)
    {
        return new ArticleCategory()
        {
            Id = (int)reader["Id"],
            ArticleId = (int)reader["ArticleId"],
            CategoryId = (int)reader["CategoryId"]
        };
    }
}