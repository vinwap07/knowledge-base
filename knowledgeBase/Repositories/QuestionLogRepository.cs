using System.Data;
using knowledgeBase.Entities;
using knowledgeBase.DataBase;

namespace knowledgeBase.Repositories;

public class QuestionLogRepository : BaseRepository<QuestionLog, int>
{
    private readonly IDatabaseConnection _databaseConnection;
    
    public QuestionLogRepository(IDatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }

    public async override Task<QuestionLog> GetById(int id)
    {
        var sql = @"SELECT Id, Question, Answer, Assessment, UserComment FROM Sessions WHERE Id = @Id";
        var parameters = new Dictionary<string, object>
        {
            ["@Id"] = id
        };

        using var reader = await _databaseConnection.ExecuteReader(sql, parameters);
        if (reader.Read())
        {
            return Mapper.MapToQuestionLog(reader);
        }

        return null;
    }

    public async override Task<List<QuestionLog>> GetAll()
    {
        var sql = @"select * from QuestionLog";
        var questionLogs = new List<QuestionLog>();
        
        using var reader = await _databaseConnection.ExecuteReader(sql);
        if (reader.Read())
        {
            questionLogs.Add(Mapper.MapToQuestionLog(reader));
        }
        
        return questionLogs;
    }

    public async override Task<bool> Create(QuestionLog qLog)
    {
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

        return await _databaseConnection.ExecuteNonQuery(createSql, createParameters) > 0;
    }

    public async override Task<bool> Update(QuestionLog qLog)
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

        return await _databaseConnection.ExecuteNonQuery(sql, parameters) > 0;
    }

    public async override Task<bool> Delete(int id)
    {
        var sql = @"delete from QuestionLog where Id = @Id";
        var parameters = new Dictionary<string, object>
        {
            ["@Id"] = id,
        };

        return await _databaseConnection.ExecuteNonQuery(sql, parameters) > 0;
    }
}