namespace aspProject.Models
{
    public class DbInitializer
    {
        public static void ClassInitialize(ApplicationDbContext context)
        {
            var classesSetup = new List<CharacterClass>()
            {
                new CharacterClass() {Name = "Воин", Agility = 2, Strength = 5, Intelligence = 1 },
                new CharacterClass() {Name = "Следопыт", Agility = 5, Strength = 2, Intelligence = 1 },
                new CharacterClass() {Name = "Маг", Agility = 2, Strength = 1, Intelligence = 5 }
            };

            if (!context.Classes.Any())
            {
                context.Classes.AddRange(classesSetup);
                context.SaveChanges();
            }
        }
    }
}
