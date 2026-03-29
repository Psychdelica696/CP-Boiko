using SQLite;

namespace CP_Boiko.Models
{
    public class TodoItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool Completed { get; set; }
    }
}