using System;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

public class User
{
    private readonly string connectionString = "Data Source=your_database_path.db;Version=3;";

    public string Username { get; set; }
    public string PasswordHash { get; set; }

    public User(string username, string password)
    {
        Username = username;
        PasswordHash = HashPassword(password);
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

    public bool CheckOrRegisterUser()
    {
        using (var conn = new SQLiteConnection(connectionString))
        {
            conn.Open();

            
            var selectCmd = new SQLiteCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username", conn);
            selectCmd.Parameters.AddWithValue("@Username", Username);
            long userCount = (long)selectCmd.ExecuteScalar();

            if (userCount > 0)
            {
                
                var passwordCmd = new SQLiteCommand("SELECT PasswordHash FROM Users WHERE Username = @Username", conn);
                passwordCmd.Parameters.AddWithValue("@Username", Username);
                string storedHash = (string)passwordCmd.ExecuteScalar();

                return storedHash == PasswordHash;
            }
            else
            {
              
                var insertCmd = new SQLiteCommand("INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)", conn);
                insertCmd.Parameters.AddWithValue("@Username", Username);
                insertCmd.Parameters.AddWithValue("@PasswordHash", PasswordHash);
                insertCmd.ExecuteNonQuery();

                return true;
            }
        }
    }
}
