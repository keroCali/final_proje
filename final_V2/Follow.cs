using SQLite;

namespace final_V2;

public class Follow
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string FollowerUsername { get; set; }
    public string TagetUsername { get; set; }
}
