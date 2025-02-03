using System.Data;
using CleanArch.Domain.Abstractions;
using CleanArch.Domain.Entities;
using Dapper;

namespace CleanArch.Infrastructure.Repositories;

public class MemberDapperRepository : IMemberDapperRepository
{
    private readonly IDbConnection _dbConnection;
    public MemberDapperRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<Member>> GetMembers()
    {
        string query = "SELECT * FROM Members";
        return await _dbConnection.QueryAsync<Member>(query);
    }

    public Task<Member> GetMemberById(int id)
    {
        string query = "SELECT * FROM Members WHERE Id = @Id";
        return _dbConnection.QueryFirstOrDefaultAsync<Member>(query, new { Id = id });
    }
}
