namespace Dotabuff_2._0.Models
{
    public class ProfileViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
