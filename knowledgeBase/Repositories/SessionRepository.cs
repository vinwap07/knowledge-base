using System.Data;
using knowledgeBase.Entities;

namespace knowledgeBase.Repositories;

public class SessionRepository : BaseRepository<Session, string>
{
    private readonly IDatabaseConnection _databaseConnection;
    
    public SessionRepository(IDatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }
    
    public override Session? GetById(string sessionId)
    {
        var sql = @"SELECT Id, Email, Password, Name, RoleId FROM Sessions WHERE Id = @Id";
        var parameters = new Dictionary<string, object>
        {
            ["@Id"] = sessionId
        };
        
        using var reader = _databaseConnection.ExecuteReader(sql, parameters);
        if (reader.Read())
        {
            return MapFromReader(reader);
        }
        
        return null;
    }

    public override List<Session> GetAll()
    {
        var sql = @"select * from Sessions";
        var sessions = new List<Session>();
        
        using var reader = _databaseConnection.ExecuteReader(sql);
        if (reader.Read())
        {
            sessions.Add(MapFromReader(reader));
        }
        
        return sessions;
    }

    public override bool Create(Session session)
    {
        var isAlreadyExists = GetById(session.SesisonId) != null;

        if (isAlreadyExists)
        {
            return false;
        }
        
        var createSql = @"insert into Session (SessionId, User, EndTime) 
                values (@SessionId, @User, @EndTime);";
        var createParameters = new Dictionary<string, object>
        {
            ["@SessionId"] = session.SesisonId,
            ["@User"] = session.User,
            ["@EndTime"] = session.EndTime
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

    public override bool Update(Session session)
    {
        var sql = @"update Session 
                set User = @User, EndTime = @EndTime 
                where SessionId = @SessionId";
        var parameters = new Dictionary<string, object>
        {
            ["@SessionId"] = session.SesisonId,
            ["@User"] = session.User,
            ["@EndTime"] = session.EndTime
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

    public override bool Delete(string sessionId)
    {
        var sql = @"delete from Session where SessionId = @SessionId";
        var parameters = new Dictionary<string, object>
        {
            ["@SessionId"] = sessionId,
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

    protected override Session MapFromReader(IDataReader reader)
    {
        return new Session()
        {
            SesisonId = (string)reader["SessionId"],
            User = (string)reader["User"],
            EndTime = (DateTime)reader["EndTime"],
        };
    }
}