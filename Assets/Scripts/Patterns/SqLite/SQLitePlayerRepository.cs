using SQLite;
using System.Collections.Generic;
using System.Linq;

public class SQLitePlayerRepository : IPlayerRepository
{
    private SQLiteConnection _connection;

    public SQLitePlayerRepository(string dbPath)
    {
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        _connection.CreateTable<PlayerStats>(); // Tablo yoksa oluşturur
    }

    public void AddOrUpdatePlayer(PlayerStats player)
    {
        var existing = GetPlayer(player.Username);
        if (existing == null) _connection.Insert(player);
        else _connection.Update(player);
    }

    public PlayerStats GetPlayer(string username)
    {
        return _connection.Table<PlayerStats>().FirstOrDefault(p => p.Username == username);
    }

    public List<PlayerStats> GetTopScores(int limit)
    {
        return _connection.Table<PlayerStats>()
            .OrderByDescending(p => p.HighScore)
            .Take(limit)
            .ToList();
    }
}