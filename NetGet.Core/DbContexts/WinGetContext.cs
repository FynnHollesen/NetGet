using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using Dapper;
using NetGet.Core.Contracts.DbContexts;
using NetGet.Core.Contracts.Services;
using NetGet.Core.Models;

namespace NetGet.Core.DbContexts;

public class WinGetContext : IWinGetContext
{
    private readonly IConfigurationService _configurationService;

    public DbConnection Connection => new SQLiteConnection($"Data Source={_configurationService.WinGetDatabasePath};");

    public WinGetContext(IConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    /// <summary>
    /// Queries the WinGetItems from the database.
    /// </summary>
    public async Task<IEnumerable<WinGetItem>> QueryWinGetItemsAsync()

    {
        await using var connection = Connection;
        await connection.OpenAsync();

        var sql = @"
        SELECT id, name, publisher
        FROM (
          SELECT ids.id, versions.version, names.name, norm_publishers.norm_publisher AS publisher 
          FROM manifest 
          JOIN ids on ids.rowid = manifest.id 
          JOIN names ON names.rowid = manifest.name 
          JOIN norm_publishers_map ON norm_publishers_map.manifest = manifest.rowid 
          JOIN norm_publishers ON norm_publishers.rowid = norm_publishers_map.norm_publisher 
          JOIN versions ON versions.rowid = manifest.version 
          ORDER BY versions.version DESC
        ) 
        GROUP BY id 
        ORDER BY name";

        var winGetItems = await connection.QueryAsync<WinGetItem>(sql);
        return winGetItems;
    }

    /// <summary>
    /// Queries the versions of a WinGetItem.
    /// </summary>
    /// <param name="winGetItem">The WinGetItem to query versions for.</param>
    public async Task<IEnumerable<string>> QueryWinGetItemVersionsAsync(WinGetItem winGetItem)
    {
        await using var connection = Connection;
        await connection.OpenAsync();

        var sql = @"
        SELECT versions.version
        FROM manifest
        JOIN ids on ids.rowid = manifest.id 
        JOIN versions ON versions.rowid = manifest.version
        WHERE ids.id = @Id
        ORDER BY versions.version DESC";

        var newVersions = await connection.QueryAsync<string>(sql, new { winGetItem.Id });
        return newVersions;
    }
}
