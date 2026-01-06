using SQLite;

namespace final_V2;

public class Item
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public int? Rating { get; set; }
    public string? Name { get; set; }
    public string? Link { get; set; }
    public string? Author { get; set; }
    public string? Description { get; set; }
    public string? Comment { get; set; }
    public string? Username { get; set; }

    [Ignore]
    public string ThumbnailUrl 
    {
        get 
        {
            if (string.IsNullOrEmpty(Link)) return "";
            string thumb = Link;
            if (!thumb.EndsWith("/")) thumb += "/";
            if (thumb.Contains("?")) thumb = thumb.Split('?')[0] + "/";
            return thumb + "media/?size=l";
        }
    }
}
