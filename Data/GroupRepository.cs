using Dapper;
using Microsoft.Data.SqlClient;
using DBConnect.Models;

namespace DBConnect.Data
{
    public class GroupRepository
    {
        private readonly string _connectionString;

        public GroupRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // УРОК 9: Query<T>
        public List<Group> GetAll()
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.Query<Group>(
                "SELECT GroupId, GroupName FROM Groups"
            ).ToList();
        }

        // УРОК 9: QueryFirstOrDefault<T>
        public Group? GetById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.QueryFirstOrDefault<Group>(
                "SELECT GroupId, GroupName FROM Groups WHERE GroupId = @Id",
                new { Id = id }
            );
        }

        // QuerySingle<int> — возвращает новый ID
        public int CreateGroup(Group group)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
                INSERT INTO Groups (GroupName)
                VALUES (@GroupName);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return connection.QuerySingle<int>(sql, group);
        }

        public void Update(Group group)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Execute(@"
                UPDATE Groups
                SET GroupName = @GroupName
                WHERE GroupId = @GroupId", group);
        }

        public void Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Execute("DELETE FROM Groups WHERE GroupId = @Id", new { Id = id });
        }
    }
}