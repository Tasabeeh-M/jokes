namespace MywebApp.Models
{
    public class Jokes
    {
        public int Id { get; set; }
        public string JokesQuestion { get; set; } = string.Empty;
        public string JokesAnswer { get; set; } = string.Empty;

        // Navigation property for related comments
        public List<Comment> Comments { get; set; } = [];
    }
}