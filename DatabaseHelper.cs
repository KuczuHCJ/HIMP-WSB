using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

public class DatabaseHelper
{
    private string connectionString = "Data Source=przedmioty.db;Version=3;";

    public DatabaseHelper()
    {
        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();

            // tworzenie w DB tabeli odp za przedmioty
            string createTableQuery = @"CREATE TABLE IF NOT EXISTS Przedmioty (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Nazwa TEXT NOT NULL,
                                        Lokalizacja TEXT NOT NULL,
                                        Opis TEXT NOT NULL);";
            SQLiteCommand command = new SQLiteCommand(createTableQuery, conn);
            command.ExecuteNonQuery();

            // tworzenie tabeli użytkoników
            string createUsersTableQuery = @"CREATE TABLE IF NOT EXISTS Users (
                                             Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                             Username TEXT NOT NULL UNIQUE,
                                             PasswordHash TEXT NOT NULL);";
            SQLiteCommand createUserTableCommand = new SQLiteCommand(createUsersTableQuery, conn);
            createUserTableCommand.ExecuteNonQuery();
        }
    }

    // dodaje przedmiot do DB
    public void DodajPrzedmiot(Przedmiot przedmiot)
    {
        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();
            string insertQuery = "INSERT INTO Przedmioty (Nazwa, Lokalizacja, Opis) VALUES (@nazwa, @lokalizacja, @opis)";
            SQLiteCommand command = new SQLiteCommand(insertQuery, conn);
            command.Parameters.AddWithValue("@nazwa", przedmiot.Nazwa);
            command.Parameters.AddWithValue("@lokalizacja", przedmiot.Lokalizacja);
            command.Parameters.AddWithValue("@opis", przedmiot.Opis);
            command.ExecuteNonQuery();
        }
    }

    // wyświetla przemioty
    public List<Przedmiot> PobierzWszystkiePrzedmioty()
    {
        List<Przedmiot> przedmioty = new List<Przedmiot>();

        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();
            string selectQuery = "SELECT * FROM Przedmioty";
            SQLiteCommand command = new SQLiteCommand(selectQuery, conn);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string nazwa = reader.GetString(1);
                string lokalizacja = reader.GetString(2);
                string opis = reader.GetString(3);
                Przedmiot przedmiot = new Przedmiot(id, nazwa, lokalizacja, opis);
                przedmioty.Add(przedmiot);
            }
        }

        return przedmioty;
    }

    // usuwa przedmioty
    public void UsunPrzedmioty(List<int> idDoUsuniecia)
    {
        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();
            using (var transaction = conn.BeginTransaction())
            {
                foreach (int id in idDoUsuniecia)
                {
                    string deleteQuery = "DELETE FROM Przedmioty WHERE Id = @id";
                    SQLiteCommand command = new SQLiteCommand(deleteQuery, conn);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
            }
        }
    }

    // rejestracja usera
    public void RegisterUser(string username, string password)
    {
        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();

            if (UserExists(username))
            {
                throw new Exception("Użytkownik już istnieje.");
            }

            string passwordHash = HashPassword(password);
            string insertQuery = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
            SQLiteCommand command = new SQLiteCommand(insertQuery, conn);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@PasswordHash", passwordHash);
            command.ExecuteNonQuery();
        }
    }

    // sprawdza czy user istnieje w DB
    public bool UserExists(string username)
    {
        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            SQLiteCommand command = new SQLiteCommand(query, conn);
            command.Parameters.AddWithValue("@Username", username);
            long count = Convert.ToInt64(command.ExecuteScalar());
            return count > 0;
        }
    }

    public bool ValidateUser(string username, string hashedPassword)
    {
        try
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(1) FROM Users WHERE Username = @username AND PasswordHash = @password";
                using (var command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", hashedPassword);
                    long count = (long)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Błąd podczas walidacji użytkownika: {ex.Message}");
            return false;
        }
    }

    public string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
