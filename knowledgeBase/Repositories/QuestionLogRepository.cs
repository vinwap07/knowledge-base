using System.Data;
using knowledgeBase.Entities;

namespace knowledgeBase.Repositories;

public class QuestionLogRepository : BaseRepository<QuestionLog, int>
{
    private readonly IDatabaseConnection _databaseConnection;
    
    public QuestionLogRepository(IDatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }

    public override QuestionLog? GetById(int id)
    {
        var sql = @"SELECT Id, Question, Answer, Assessment, UserComment FROM Sessions WHERE Id = @Id";
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

    public override List<QuestionLog> GetAll()
    {
        var sql = @"select * from QuestionLog";
        var questionLogs = new List<QuestionLog>();
        
        using var reader = _databaseConnection.ExecuteReader(sql);
        if (reader.Read())
        {
            questionLogs.Add(MapFromReader(reader));
        }
        
        return questionLogs;
    }

    public override bool Create(QuestionLog qLog)
    {
        var isAlreadyExists = GetById(qLog.Id) != null;

        if (isAlreadyExists)
        {
            return false;
        }
        
        var createSql = @"insert into QuestionLog (Id, Question, Answer, Assessment, UserComment) 
                values (@Id, @Question, @Answer, @Assessment, @UserComment);";
        var createParameters = new Dictionary<string, object>
        {
            ["@Id"] = qLog.Id,
            ["@Question"] = qLog.Question,
            ["@Answer"] = qLog.Answer,
            ["@Assessment"] = qLog.Assessment,
            ["@UserComment"] = qLog.UserComment
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

    public override bool Update(QuestionLog qLog)
    {
        var sql = @"update QuestionLog
                set Question = @Question, Answer = @Answer, Assessment = @Assessment, UserComment = @UserComment 
                where Id = @Id";
        var parameters = new Dictionary<string, object>
        {
            ["@Id"] = qLog.Id,
            ["@Question"] = qLog.Question,
            ["@Answer"] = qLog.Answer,
            ["@Assessment"] = qLog.Assessment,
            ["@UserComment"] = qLog.UserComment
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
        var sql = @"delete from QuestionLog where Id = @Id";
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

    protected override QuestionLog MapFromReader(IDataReader reader)
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
}