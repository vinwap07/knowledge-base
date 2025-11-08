using System.Data;
using knowledgeBase.Entities;
using knowledgeBase.DataBase;

namespace knowledgeBase.Repositories;

public class RoleRepository : BaseRepository<Role, int>
{
    private readonly IDatabaseConnection _databaseConnection;
    
    public RoleRepository(IDatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }
    
    public async override Task<List<Role>> GetAll()
    {
        var sql = @"select * from Role";
        var roles = new List<Role>();
        
        using var reader = await _databaseConnection.ExecuteReader(sql);
        if (reader.Read())
        {
            roles.Add(Mapper.MapToRole(reader));
        }
        
        return roles;
    }

    public async override Task<Role> GetById(int id)
    {
        var sql = @"select * from Role where RoleId = @Id";
        var parameters = new Dictionary<string, object>
        {
            ["@Id"] = id
        };
        
        using var reader = await _databaseConnection.ExecuteReader(sql, parameters);
        if (reader.Read())
        {
            return Mapper.MapToRole(reader);
        }
        
        return null;
    }

    public override Task<bool> Create(Role entity)
    {
        throw new NotImplementedException();
    }

    public override Task<bool> Update(Role entity)
    {
        throw new NotImplementedException();
    }

    public override Task<bool> Delete(int id)
    {
        throw new NotImplementedException();
    }
}