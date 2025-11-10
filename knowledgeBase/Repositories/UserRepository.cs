using System.Data;
using knowledgeBase.Entities;
using knowledgeBase.DataBase;

namespace knowledgeBase.Repositories;

public class UserRepository: BaseRepository<User, string>
{
    private readonly IDatabaseConnection _databaseConnection;

    public UserRepository(IDatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }
    
    public async override Task<User> GetById(string email)
    {
        var sql = @"select * from User where Email = @Email";
        var parameters = new Dictionary<string, object>
        {
            ["@Email"] = email
        };
        
        using var reader = await _databaseConnection.ExecuteReader(sql, parameters);
        if (reader.Read())
        {
            return Mapper.MapToUser(reader);
        }
        
        return new User();
    }

    public async override Task<List<User>> GetAll()
    {
        var sql = @"select * from User";
        var users = new List<User>();
        
        using var reader = await _databaseConnection.ExecuteReader(sql);
        if (reader.Read())
        {
            users.Add(Mapper.MapToUser(reader));
        }
        
        return users;
    }

    public async override Task<bool> Create(User user)
    {
        var createSql = @"insert into User (Name, Email, Password, RoleId) 
                values (@Email, @Name, @Password, @RoleId)";
        var createParameters = new Dictionary<string, object>
        {
            ["@Email"] = user.Email,
            ["@Name"] = user.Name,
            ["@Password"] = user.Password,
            ["@RoleId"] = user.RoleId
        };

        return await _databaseConnection.ExecuteNonQuery(createSql, createParameters) > 0;
    }

    public async override Task<bool> Update(User user)
    {
        var sql = @"update user
                set Name = @Name, Password = @Password, RoleId = @RoleId 
                where Email = @Email";
        var parameters = new Dictionary<string, object>
        {
            ["@Email"] = user.Email,
            ["@Name"] = user.Name,
            ["@Password"] = user.Password,
            ["@RoleId"] = user.RoleId
        };

        return await _databaseConnection.ExecuteNonQuery(sql, parameters) > 0;
    }

    public async override Task<bool> Delete(string id)
    {
        var sql = @"delete from User where Id = @Id";
        var parameters = new Dictionary<string, object>
        {
            ["@Id"] = id
        };

        return await _databaseConnection.ExecuteNonQuery(sql, parameters) > 0;
    }
    
    public async Task<List<Article>> GetFavorileArticles(string email)
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

    public async Task<bool> AddArticleToFavorite(string email, int articleId)
    {
        var sql = @"INSERT INTO UserArticle (User, Article)
                    VALUES (@User, @ArticleId)";
        var parameters = new Dictionary<string, object>
        {
            ["@User"] = email,
            ["@ArticleId"] = articleId
        };
        
        return await _databaseConnection.ExecuteNonQuery(sql, parameters) > 0;
    }

    public async Task<bool> RemoveArticleFromFavorite(string email, int articleId)
    {
        var sql = @"DELETE FROM UserArticle
                    WHERE user = @user AND article = @article;";
        var parameters = new Dictionary<string, object>
        {
            ["@user"] = email,
            ["@article"] = articleId
        };

        return await _databaseConnection.ExecuteNonQuery(sql, parameters) > 0;
    }
    
    public async Task<string> GetRoleById(int id)
    {
        var sql = @"select * from Role where RoleId = @Id";
        var parameters = new Dictionary<string, object>
        {
            ["@Id"] = id
        };
        
        using var reader = await _databaseConnection.ExecuteReader(sql, parameters);
        if (reader.Read())
        {
            return (string)reader["Name"];
        }
        
        return "unknown";
    }

    public async Task<List<Article>> GetAllFavoriteArticles(string email)
    {
        var sql = @"SELECT Article.*
                    FROM Article JOIN UserArticle ON UserArticle.Article = Article.Id
                    WHERE UserArticle.User = @email";
        var parameters = new Dictionary<string, object>
        {
            ["@email"] = email
        };
        var articles = new List<Article>();
        
        using var reader = await _databaseConnection.ExecuteReader(sql);
        if (reader.Read())
        {
            articles.Add(Mapper.MapToArticle(reader));
        }
        
        return articles;
    }
}