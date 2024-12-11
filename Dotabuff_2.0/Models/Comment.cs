namespace Dotabuff_2._0.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string MatchId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
