using System.Data.Common;
using System.Data.SQLite;
using Dapper;
using NetGet.Core.Contracts.DbContexts;
using NetGet.Core.Contracts.Services;
using NetGet.Core.Models;

namespace NetGet.Core.DbContexts;
public class NetGetContext : INetGetContext
{
    private readonly IConfigurationService _configurationService;

    public DbConnection Connection => new SQLiteConnection($"Data Source={_configurationService.NetGetDatabasePath};");

    public NetGetContext(IConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    /// <summary>
    /// Creates the NetGet database if it does not exist.
    /// </summary>
    public async Task CreateDatabaseAndTableIfNotExistsAsync()
    {
        if (!File.Exists(_configurationService.NetGetDatabasePath))
        {
            Directory.CreateDirectory(_configurationService.DataDirectoryPath);
            SQLiteConnection.CreateFile(_configurationService.NetGetDatabasePath);
        }

        using var connection = Connection;
        await connection.OpenAsync();

        var sql = @"
            CREATE TABLE IF NOT EXISTS NetGetItems 
            (
                Id TEXT PRIMARY KEY, 
                Publisher TEXT, 
                Name TEXT, 
                Description TEXT, 
                PublisherUrl TEXT, 
                License TEXT, 
                LicenseUrl TEXT
            )";

        await connection.ExecuteAsync(sql);
    }

    /// <summary>
    /// Upserts the NetGet database with the provided WinGet items.
    /// </summary>
    /// <param name="winGetItems">The WinGet items to upsert.</param>
    public async Task UpsertNetGetDatabaseAsync(IEnumerable<WinGetItem> winGetItems)
    {
        using var connection = Connection;
        await connection.OpenAsync();

        var sql = @"
            INSERT OR IGNORE INTO NetGetItems 
            (
                Id, 
                Publisher, 
                Name
            ) 
            VALUES 
            (
                @Id, 
                @Publisher, 
                @Name
            );

            UPDATE NetGetItems 
            SET 
                Publisher = @Publisher, 
                Name = @Name
            WHERE Id = @Id;";

        foreach (var winGetItem in winGetItems)
        {
            await connection.ExecuteAsync(sql, winGetItem);
        }
    }
    
    /// <summary>
    /// Queries the NetGet database and returns a collection of NetGetItems.
    /// </summary>
    public async Task<IEnumerable<NetGetItem>> QueryNetGetItemsAsync()
    {
        using var connection = Connection;
        await connection.OpenAsync();

        var sql = @"
        SELECT *
        FROM NetGetItems
        GROUP BY id 
        ORDER BY name";

        return await connection.QueryAsync<NetGetItem>(sql);
    }
}
