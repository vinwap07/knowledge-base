using System.Data;
using knowledgeBase.Entities;
using knowledgeBase.DataBase;

namespace knowledgeBase.Repositories;

public class ArticleRepository : BaseRepository<Article, int>
{
    private readonly IDatabaseConnection _databaseConnection;
    
    public ArticleRepository(IDatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }
    
    public async Task<List<Article>> GetByCategoryId(int categoryId)
    {
        var sql = @"select * from Article WHERE CategoryId = @categoryId";
        var parameters = new Dictionary<string, object>
        {
            { "@categoryId", categoryId }
        };
        
        var articles = new List<Article>();
        
        using var reader = await _databaseConnection.ExecuteReader(sql, parameters);
        if (reader.Read())
        {
            articles.Add(MapFromReader(reader));
        }
        
        return articles;
    }
    
    public async override Task<Article?> GetById(int id)
    {
        var sql = @"select * from Article where Id = @Id";
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

    public async override Task<List<Article>> GetAll()
    {
        var sql = @"select * from Article";
        var articles = new List<Article>();
        
        using var reader = await _databaseConnection.ExecuteReader(sql);
        if (reader.Read())
        {
            articles.Add(MapFromReader(reader));
        }
        
        return articles;
    }

    public async override Task<bool> Create(Article article)
    {
        var createSql = @"insert into Article (Id, Title, Content, Author, PublishDate) 
                values (@Id, @Title, @Content, @Author, @PublishDate);";
        var createParameters = new Dictionary<string, object>
        {
            ["@Id"] = article.Id,
            ["@Title"] = article.Title,
            ["@Content"] = article.Content,
            ["@Author"] = article.Author,
            ["@PublishDate"] = article.PublishDate
        };

        return await _databaseConnection.ExecuteNonQuery(createSql, createParameters) > 0;
    }
    
    public async override Task<bool> Update(Article article)
    {
        var sql = @"update Article
                set Title = @Title, Content = @Content, Author = @Author, PublishDate = @PublishDate
                where Id = @Id";
        var parameters = new Dictionary<string, object>
        {
            ["@Id"] = article.Id,
            ["@Title"] = article.Title,
            ["@Content"] = article.Content,
            ["@Author"] = article.Author,
            ["@PublishDate"] = article.PublishDate
        };

        return await _databaseConnection.ExecuteNonQuery(sql, parameters) > 0;
    }

    public async override Task<bool> Delete(int id)
    {
        var sql = @"delete from Article where Id = @Id";
        var parameters = new Dictionary<string, object>
        {
            ["@Id"] = id,
        };

        return await _databaseConnection.ExecuteNonQuery(sql, parameters) > 0;
    }

    protected override Article MapFromReader(IDataReader reader)
    {
        return new Article()
        {
            Id = (int)reader["Id"],
            Title = (string)reader["Title"],
            Content = (string)reader["Content"],
            Author = (string)reader["Author"],
            PublishDate = (DateOnly)reader["PublishDate"],
        };
    }
}