using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MusicServiceCRUD.Models;

namespace MusicServiceCRUD.Services;

public class AdoNetService
{
    private readonly string _connectionString;

    public AdoNetService(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("MusicService")
            ?? throw new Exception("Строка подключения не найдена");
    }

    public List<Artist> GetAllArtists()
    {
        var artists = new List<Artist>();

        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand("SELECT ArtistId, Name, Country, IsVerified, CreatedAt FROM Artists", connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            artists.Add(new Artist
            {
                ArtistId = reader.GetGuid(0),
                Name = reader.GetString(1),
                Country = reader.IsDBNull(2) ? null : reader.GetString(2),
                IsVerified = reader.GetBoolean(3),
                CreatedAt = reader.GetDateTime(4)
            });
        }

        return artists;
    }

    public void AddArtist(string name, string? country, bool isVerified)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand(
            "INSERT INTO Artists (Name, Country, IsVerified) VALUES (@name, @country, @isVerified)", connection);

        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@country", (object?)country ?? DBNull.Value);
        command.Parameters.AddWithValue("@isVerified", isVerified);

        command.ExecuteNonQuery();
    }

    public void UpdateArtist(Guid id, string name, string? country, bool isVerified)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand(
            "UPDATE Artists SET Name = @name, Country = @country, IsVerified = @isVerified WHERE ArtistId = @id", connection);

        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@country", (object?)country ?? DBNull.Value);
        command.Parameters.AddWithValue("@isVerified", isVerified);

        command.ExecuteNonQuery();
    }

    public void DeleteArtist(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand("DELETE FROM Artists WHERE ArtistId = @id", connection);
        command.Parameters.AddWithValue("@id", id);

        command.ExecuteNonQuery();
    }
}