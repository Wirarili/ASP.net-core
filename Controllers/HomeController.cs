using aspProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace aspProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ClassesList(string? name)
        {
            var classes = string.IsNullOrWhiteSpace(name) 
                ? _context.Classes.ToList() 
                : _context.Classes.Where(x => x.Name.StartsWith(name)).ToList();

            return View(classes);
        }
        [HttpPost]
        public IActionResult DeleteClass(int id)
        {
            var @class = _context.Classes.Find(id);
            if (@class is not null)
            {
                var history = new CharacterClassHistory() { Action = "Delete", EntityId = @class.Id, Date = DateTime.Now, PropertyName = "Entity" };
                _context.CharacterClassHistories.Add(history);
                _context.SaveChanges();

                _context.Classes.Remove(@class);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false});

        }

        public IActionResult CreateUpdate(int id)
        {
            var model = _context.Classes?.Find(id) ?? new CharacterClass() { Name = string.Empty, Intelligence = 0, Agility = 0, Strength = 0 };

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateUpdate(CharacterClass model)
        {
            if (ModelState.IsValid)
            {
                var @class = _context.Classes.Find(model.Id);

                if (@class is null)
                {
                    _context.Classes.Add(model);
                    _context.SaveChanges();

                    var history = new List<CharacterClassHistory>()
                    {
                        new CharacterClassHistory() { Date = DateTime.Now, EntityId = model.Id, NewValue = model.Name, PropertyName = "Name", Action = "Add" },
                        new CharacterClassHistory() { Date = DateTime.Now, EntityId = model.Id, NewValue = model.Strength.ToString(), PropertyName = "Strength", Action = "Add" },
                        new CharacterClassHistory() { Date = DateTime.Now, EntityId = model.Id, NewValue = model.Agility.ToString(), PropertyName = "Agility", Action = "Add" },
                        new CharacterClassHistory() { Date = DateTime.Now, EntityId = model.Id, NewValue = model.Intelligence.ToString(), PropertyName = "Intelligence", Action = "Add" },
                    };

                    _context.CharacterClassHistories.AddRange(history);
                    _context.SaveChanges();

                    return View("ClassesList", _context.Classes.ToList());
                }
                else
                {
                    var history = new List<CharacterClassHistory>();

                    if (@class.Name != model.Name)
                    {
                        var add = new CharacterClassHistory() { Date = DateTime.Now, EntityId = @class.Id, OldValue = @class.Name, NewValue = model.Name, PropertyName = "Name", Action = "Change" };
                        history.Add(add);
                        @class.Name = model.Name;
                        
                    }

                    if (@class.Strength != model.Strength)
                    {
                        var add = new CharacterClassHistory() { Date = DateTime.Now, EntityId = @class.Id, OldValue = @class.Strength.ToString(), NewValue = model.Strength.ToString(), PropertyName = "Strength", Action = "Change" };
                        history.Add(add);
                        @class.Strength = model.Strength;
                    }

                    if (@class.Agility != model.Agility)
                    {
                        var add = new CharacterClassHistory() { Date = DateTime.Now, EntityId = @class.Id, OldValue = @class.Agility.ToString(), NewValue = model.Agility.ToString(), PropertyName = "Agility", Action = "Change" };
                        history.Add(add);
                        @class.Agility = model.Agility;
                    }

                    if (@class.Intelligence != model.Intelligence)
                    {
                        var add = new CharacterClassHistory() { Date = DateTime.Now, EntityId = @class.Id, OldValue = @class.Intelligence.ToString(), NewValue = model.Intelligence.ToString(), PropertyName = "Intelligence", Action = "Change" };
                        history.Add(add);
                        @class.Intelligence = model.Intelligence;
                    }

                    _context.CharacterClassHistories.AddRange(history);

                    _context.SaveChanges();
                    return View("ClassesList", _context.Classes.ToList());
                }
            }

            return View(model);
        }
        public IActionResult ClassHistoryList(CharacterClass @class)
        {
            var model = _context.CharacterClassHistories.Where(x => x.EntityId == @class.Id).OrderByDescending(x => x.Date).ToList();
            ViewBag.Id = @class.Id;
            ViewBag.Name = _context.Classes.FirstOrDefault(x => x.Id == @class.Id)?.Name ?? "Îøèáêà";
            ViewBag.Propertes = model.Select(x => x.PropertyName).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult ClassHistoryList(int id, string? act, DateTime? date, string? property)
        {
            var model = _context.CharacterClassHistories.Where(x => x.EntityId == id).OrderByDescending(x => x.Date).ToList();
            ViewBag.Id = id;
            ViewBag.Name = _context.Classes.FirstOrDefault(x => x.Id == id)?.Name ?? "Îøèáêà";
            ViewBag.Propertes = model.Select(x => x.PropertyName).ToList();

            if (!string.IsNullOrEmpty(act))
            {
                model = model.Where(x => x.Action == act).ToList();
            }
            if (date is not null)
            {
                model = model.Where(x => x.Date.Date == date.Value.Date).ToList();
            }
            if (!string.IsNullOrEmpty(property))
            {
                model = model.Where(x => x.PropertyName == property).ToList();
            }
            return View(model);
        }

    }
}
