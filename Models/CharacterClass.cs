using System.ComponentModel.DataAnnotations;

namespace aspProject.Models
{
    public class CharacterClass
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Название")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Название должно быть длинной от 3 до 30 символов")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Сила")]
        [Range(0.01,5, ErrorMessage = "Прирост силы должен быть в пределах от 0.01 до 5")]
        public double Strength { get; set; }

        [Display(Name = "Ловкость")]

        [Range(0.01, 5, ErrorMessage = "Прирост ловкости должен быть в пределах от 0.01 до 5")]
        public double Agility { get; set; }

        [Display(Name = "Интеллект")]

        [Range(0.01, 5, ErrorMessage = "Прирост интеллекта должен быть в пределах от 0.01 до 5")]
        public double Intelligence { get; set; }
        public ICollection<Character> Character { get; set; } = new List<Character>();
    }
}
