using knowledgeBase.Entities;
using knowledgeBase.DataBase;

namespace knowledgeBase.Repositories;

public class UserArticleRepository : BaseRepository<UserArticle, (string, int)>
{
    private readonly IDatabaseConnection _databaseConnection;
    
    public UserArticleRepository(IDatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }
    
    public override Task<List<UserArticle>> GetAll()
    {
        throw new NotImplementedException();
    }

    public override Task<UserArticle> GetById((string, int) id)
    {
        throw new NotImplementedException();
    }

    public async override Task<bool> Create(UserArticle entity)
    {
        var createSql = @"INSERT INTO UserArticle (User, Article)
                          VALUES (@user, @article);";
        var createParameters = new Dictionary<string, object>
        {
            ["@user"] = entity.User, 
            ["@article"] = entity.Article
        };

        return await _databaseConnection.ExecuteNonQuery(createSql, createParameters) > 0;
    }

    public override Task<bool> Update(UserArticle entity)
    {
        throw new NotImplementedException();
    }

    public async override Task<bool> Delete((string, int) id)
    {
        var sql = @"delete from UserArticle where user = @user and article = @article";
        var parameters = new Dictionary<string, object>
        {
            ["@user"] = id.Item1,
            ["@article"] = id.Item2
        };

        return await _databaseConnection.ExecuteNonQuery(sql, parameters) > 0;
    }

    public async Task<List<Article>> GetArticlesByUserEmail(string email)
    {
        var sql = @"SELECT Article.*
                    FROM UserArticle JOIN Article ON UserArticle.Article = Article.Id 
                    WHERE UserArticle.User = @email";
        var parameters = new Dictionary<string, object>
        {
            { "@email", email }
        };
        
        var articles = new List<Article>();
        
        using var reader = await _databaseConnection.ExecuteReader(sql, parameters);
        if (reader.Read())
        {
            articles.Add(Mapper.MapToArticle(reader));
        }
        
        return articles;
    }
}