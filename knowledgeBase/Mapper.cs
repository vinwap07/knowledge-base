using System.Data;
using knowledgeBase.Entities;
namespace knowledgeBase;

public static class Mapper
{
    public static User MapToUser(IDataReader reader)
    {
        return new User
        {
            Email = (string)reader["Email"],
            Name = (string)reader["Name"],
            Password = (string)reader["Password"],
            RoleId = (int)reader["RoleId"]
        };
    }

    public static Article MapToArticle(IDataReader reader)
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

    public static Session MapToSession(IDataReader reader)
    {
        return new Session()
        {
            SesisonId = (string)reader["SessionId"],
            User = (string)reader["User"],
            EndTime = (DateTime)reader["EndTime"],
        };
    }

    public static QuestionLog MapToQuestionLog(IDataReader reader)
    {
        return new QuestionLog()
        {
            Id = (int)reader["Id"],
            Question = (string)reader["Question"],
            Answer = (string)reader["Answer"],
            Assessment = (int)reader["Assessment"],
            UserComment = (string)reader["UserComment"]
        };
    }

    public static Role MapToRole(IDataReader reader)
    {
        return new Role()
        {
            RoleId = (int)reader["Id"],
            Name = (string)reader["Name"]
        };
    }

    public static Category MapToCategory(IDataReader reader)
    {
        return new Category()
        {
            Name = (string)reader["Name"],
            Slug = (string)reader["Slug"]
        };
    }

    public static ArticleCategory MapToArticleCategory(IDataReader reader)
    {
        return new ArticleCategory()
        {
            Id = (int)reader["Id"],
            ArticleId = (int)reader["ArticleId"],
            CategoryId = (string)reader["CategoryId"]
        };
    }

    public static UserArticle MapToUserArticle(IDataReader reader)
    {
        return new UserArticle()
        {
            User = (string)reader["User"],
            Article = (int)reader["Article"],
        };
    }
}