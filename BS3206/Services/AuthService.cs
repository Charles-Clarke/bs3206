using Microsoft.Data.SqlClient;
using BS3206.Helpers;

namespace BS3206.Services
{
    public static class AuthService
    {
        private static readonly string connectionString = "Server=tcp:bs3206server.database.windows.net,1433;Initial Catalog=BS3206;Persist Security Info=False;User ID=sqladmin;Password=BS3206!!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public static async Task<bool> RegisterUserAsync(string fullName, string email, string password)
        {
            using var conn = new SqlConnection(connectionString);
            var cmd = new SqlCommand(@"
                INSERT INTO Users (FullName, Email, PasswordHash, IsMfaVerified, Role)
                VALUES (@FullName, @Email, @PasswordHash, @IsMfaVerified, @Role)", conn);

            cmd.Parameters.AddWithValue("@FullName", fullName);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@PasswordHash", HashHelper.HashPassword(password));
            cmd.Parameters.AddWithValue("@IsMfaVerified", true);
            cmd.Parameters.AddWithValue("@Role", "User");

            try
            {
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> ValidateLoginAsync(string email, string password)
        {
            using var conn = new SqlConnection(connectionString);
            var cmd = new SqlCommand("SELECT PasswordHash FROM Users WHERE Email = @Email", conn);
            cmd.Parameters.AddWithValue("@Email", email);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            if (result == null) return false;

            return HashHelper.VerifyPassword(password, result.ToString());
        }
    }
}
