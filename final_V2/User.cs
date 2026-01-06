using SQLite;

namespace final_V2;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Bio { get; set; } = "Henüz bir bio eklenmemiş.";
    public DateTime JoinDate { get; set; } = DateTime.Now;
}
