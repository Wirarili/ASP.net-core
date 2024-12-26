using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace aspProject.Models
{
    public class Character
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя персонажа обязательно")]
        public string Name { get; set; } = string.Empty;

        [Range(1, 65, ErrorMessage = "Уровень должен быть от 1 до 65")]
        public int Level { get; set; }

        public int ClassId { get; set; }

        public CharacterClass CharacterClass { get; set; }
        //public int CharacterClassId { get; set; } вот так надо

        [StringLength(50,MinimumLength =3, ErrorMessage ="Длина фракции должна быть от 3 до 50 символов")]
        public string Fraction { get; set; } = string.Empty;
    }
}
