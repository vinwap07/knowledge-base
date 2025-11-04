using System.Data;
using knowledgeBase.Entities;

namespace knowledgeBase.Repositories;

public class UserRepository: BaseRepository<User, string>
{
    private readonly IDatabaseConnection _databaseConnection;

    public UserRepository(IDatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }
    
    public override User? GetById(string email)
    {
        var sql = @"select * from User where Email = @Email";
        var parameters = new Dictionary<string, object>
        {
            ["@Email"] = email
        };
        
        using var reader = _databaseConnection.ExecuteReader(sql, parameters);
        if (reader.Read())
        {
            return MapFromReader(reader);
        }
        
        return null;
    }

    public override List<User> GetAll()
    {
        var sql = @"select * from User";
        var users = new List<User>();
        
        using var reader = _databaseConnection.ExecuteReader(sql);
        if (reader.Read())
        {
            users.Add(MapFromReader(reader));
        }
        
        return users;
    }

    public override bool Create(User user)
    {
        var isAlreadyExists = GetById(user.Email) != null;

        if (isAlreadyExists)
        {
            return false;
        }
        
        var createSql = @"insert into User (Name, Email, Password, RoleId) 
                values (@Email, @Name, @Password, @RoleId)";
        var createParameters = new Dictionary<string, object>
        {
            ["@Email"] = user.Email,
            ["@Name"] = user.Name,
            ["@Password"] = user.Password,
            ["@RoleId"] = user.RoleId
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

    public override bool Update(User user)
    {
        var sql = @"update user
                set Name = @Name, Email = @Email, Password = @Password, RoleId = @RoleId 
                where Id = @Id";
        var parameters = new Dictionary<string, object>
        {
            ["@Email"] = user.Email,
            ["@Name"] = user.Name,
            ["@Password"] = user.Password,
            ["@RoleId"] = user.RoleId
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

    public override bool Delete(string id)
    {
        var sql = @"delete from User where Id = @Id";
        var parameters = new Dictionary<string, object>
        {
            ["@Id"] = id
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

    protected override User MapFromReader(IDataReader reader)
    {
        return new User
        {
            Email = (string)reader["Email"],
            Name = (string)reader["Name"],
            Password = (string)reader["Password"],
            RoleId = (int)reader["RoleId"]
        };
    }
}