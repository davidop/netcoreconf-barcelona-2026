using System.Text.Json;
using Microsoft.Data.Sqlite;

namespace MerlinOps.Api;

/// <summary>
/// SQLite persistence for the development/simulation mode. Cloud storage adapter TBD.
/// </summary>
public sealed class SqliteRunStore
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly string _connectionString;

    public SqliteRunStore(string path)
    {
        var directory = Path.GetDirectoryName(Path.GetFullPath(path))!;
        Directory.CreateDirectory(directory);
        _connectionString = new SqliteConnectionStringBuilder { DataSource = path }.ToString();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS runs (
                id TEXT PRIMARY KEY,
                scenario TEXT NOT NULL,
                mode TEXT NOT NULL CHECK(mode = 'SIMULATED' OR mode = 'REPLAY'),
                snapshot TEXT NOT NULL,
                created_at TEXT NOT NULL
            );
            """;
        command.ExecuteNonQuery();
    }

    public async Task SaveAsync(RunSnapshot snapshot, CancellationToken ct = default)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(ct);
        await using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO runs(id, scenario, mode, snapshot, created_at)
            VALUES ($id, $scenario, $mode, $snapshot, $created)
            """;
        command.Parameters.AddWithValue("$id", snapshot.Id);
        command.Parameters.AddWithValue("$scenario", snapshot.Scenario);
        command.Parameters.AddWithValue("$mode", snapshot.Mode);
        command.Parameters.AddWithValue("$snapshot", JsonSerializer.Serialize(snapshot, JsonOptions));
        command.Parameters.AddWithValue("$created", snapshot.CreatedAt.ToString("O"));
        await command.ExecuteNonQueryAsync(ct);
    }

    public async Task<RunSnapshot?> GetAsync(string id, CancellationToken ct = default)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(ct);
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT snapshot FROM runs WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        var value = await command.ExecuteScalarAsync(ct);
        return value is string json ? JsonSerializer.Deserialize<RunSnapshot>(json, JsonOptions) : null;
    }
}
