using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MywebApp.Models;

namespace MywebApp.Data
{
    // Primary constructor: DbContextOptions injected directly
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext(options)
    {
        public DbSet<Jokes> Jokes { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}