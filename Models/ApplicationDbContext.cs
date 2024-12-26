using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace aspProject.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<CharacterClass> Classes { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<CharacterClassHistory> CharacterClassHistories { get; set; }
        public DbSet<CharacterHistory> CharacterHistories { get; set; }
    }
}
