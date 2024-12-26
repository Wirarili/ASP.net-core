using aspProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspProject.Controllers
{
    public class CharacterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CharacterController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult CharacterList(string? name, string? className)
        {
            ViewBag.Classes = _context.Classes.ToList();
            var character = _context.Characters.Include(c => c.CharacterClass).ToList();
            if (!string.IsNullOrWhiteSpace(name))
            {
                character = character.Where(x => x.Name.StartsWith(name)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(className))
            {
                character = character.Where(x => x.CharacterClass.Name.StartsWith(className)).ToList();
            }


            return View(character);
        }
        [HttpPost]
        public IActionResult DeleteCharacter(int id)
        {
            var character = _context.Characters.Find(id);

            if (character is not null)
            {
                var history = new CharacterHistory() { Action = "Delete", EntityId = character.Id, Date = DateTime.Now, PropertyName = "Entity" };
                _context.CharacterHistories.Add(history);
                _context.SaveChanges();

                _context.Characters.Remove(character);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });

        }

        public IActionResult CreateUpdateCharacter(int id)
        {
            var character = _context.Characters?.Find(id) ?? new Character() { Name = string.Empty, ClassId = 0, Level = 0 };
            ViewBag.Classes = _context.Classes.ToList();

            var model = new CharacterEditModel()
            {
                Id = character.Id,
                Name = character.Name,
                Level = character.Level,
                ClassId = character.ClassId,
                Fraction = character.Fraction
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateUpdateCharacter(CharacterEditModel editModel)
        {
            ViewBag.Classes = _context.Classes.ToList();

            if (ModelState.IsValid)
            {
                if (editModel.Id == 0)
                {
                    var model = new Character()
                    {
                        Level = editModel.Level,
                        Fraction = editModel.Fraction,
                        ClassId = editModel.ClassId,
                        Name = editModel.Name,
                        CharacterClass = _context.Classes.First(x => x.Id == editModel.ClassId),
                        CharacterClassId = editModel.ClassId
                    };

                    _context.Characters.Add(model);
                    _context.SaveChanges();

                    var history = new List<CharacterHistory>()
                    {
                        new CharacterHistory() { Date = DateTime.Now, EntityId = model.Id, NewValue = model.Name, PropertyName = "Name", Action = "Add" },
                        new CharacterHistory() { Date = DateTime.Now, EntityId = model.Id, NewValue = model.Level.ToString(), PropertyName = "Level", Action = "Add" },
                        new CharacterHistory() { Date = DateTime.Now, EntityId = model.Id, NewValue = model.Fraction, PropertyName = "Fraction", Action = "Add" },
                        new CharacterHistory() { Date = DateTime.Now, EntityId = model.Id, NewValue = model.ClassId.ToString(), PropertyName = "ClassId", Action = "Add" },
                    };

                    _context.CharacterHistories.AddRange(history);
                    _context.SaveChanges();

                    return View("CharacterList", _context.Characters.Include(c => c.CharacterClass).ToList());
                }
                else
                {
                    var character = _context.Characters.Find(editModel.Id);
                    if (character == null)
                    {
                        return NotFound();
                    }

                    var history = new List<CharacterHistory>();

                    if (character.Fraction != editModel.Fraction)
                    {
                        var add = new CharacterHistory() { Date = DateTime.Now, EntityId = character.Id, Action = "Change", OldValue = character.Fraction, NewValue = editModel.Fraction, PropertyName = "Fraction" };
                        history.Add(add);
                        character.Fraction = editModel.Fraction;
                    }

                    if (character.Level != editModel.Level)
                    {
                        var add = new CharacterHistory() { Date = DateTime.Now, EntityId = character.Id, Action = "Change", OldValue = character.Level.ToString(), NewValue = editModel.Level.ToString(), PropertyName = "Level" };
                        history.Add(add);
                        character.Level = editModel.Level;
                    }

                    if (character.ClassId != editModel.ClassId)
                    {
                        var add = new CharacterHistory() { Date = DateTime.Now, EntityId = character.Id, Action = "Change", OldValue = character.ClassId.ToString(), NewValue = editModel.ClassId.ToString(), PropertyName = "ClassId" };
                        history.Add(add);
                        character.ClassId = editModel.ClassId;
                        character.CharacterClass = _context.Classes.First(x => x.Id == editModel.ClassId);
                    }

                    if (character.Name != editModel.Name)
                    {
                        var add = new CharacterHistory() { Date = DateTime.Now, EntityId = character.Id, Action = "Change", OldValue = character.Name, NewValue = editModel.Name, PropertyName = "Name" };
                        history.Add(add);
                        character.Name = editModel.Name;
                    }

                    _context.CharacterHistories.AddRange(history);
                    _context.SaveChanges();

                    return View("CharacterList", _context.Characters.Include(c => c.CharacterClass).ToList());
                }
            }

            return View(editModel);
        }

        public IActionResult CharacterHistoryList(Character character)
        {
            var model = _context.CharacterHistories.Where(x => x.EntityId == character.Id).OrderByDescending(x => x.Date).ToList();
            ViewBag.Id = character.Id;
            ViewBag.Name = _context.Characters.FirstOrDefault(x => x.Id == character.Id)?.Name ?? "Ошибка";
            ViewBag.Propertes = _context.CharacterHistories.Where(x => x.EntityId == character.Id).Select(x => x.PropertyName).ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CharacterHistoryList(int id, string? act, DateTime? date, string? property)
        {
            var model = _context.CharacterHistories.Where(x => x.EntityId == id);
            ViewBag.Id = id;
            ViewBag.Name = _context.Characters.FirstOrDefault(x => x.Id == id)?.Name ?? "Ошибка";
            ViewBag.Propertes = model.Select(x => x.PropertyName).ToList();

            if (!string.IsNullOrEmpty(act))
            {
                model = model.Where(x => x.Action == act).AsQueryable();
            }
            if (date is not null)
            {
                model = model.Where(x => x.Date.Date == date.Value.Date).AsQueryable();
            }
            if (!string.IsNullOrEmpty(property))
            {
                model = model.Where(x => x.PropertyName == property).AsQueryable();
            }
            var lol = await model.OrderByDescending(x => x.Date).ToListAsync();
            return View(lol);
        }
    }
}
