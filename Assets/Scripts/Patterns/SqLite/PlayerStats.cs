using SQLite;

public class PlayerStats
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    [Indexed]
    public string Username { get; set; }
    
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int HighScore { get; set; }
    public string PreferredTheme { get; set; }
}