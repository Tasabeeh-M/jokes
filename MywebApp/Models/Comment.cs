namespace MywebApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Foreign key
        public int JokeId { get; set; }

        // Navigation property (nullable)
        public Jokes? Joke { get; set; }
    }
}