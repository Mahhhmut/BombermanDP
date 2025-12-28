using System.Collections.Generic;

public interface IPlayerRepository
{
    void AddOrUpdatePlayer(PlayerStats player);
    PlayerStats GetPlayer(string username);
    List<PlayerStats> GetTopScores(int limit);
}