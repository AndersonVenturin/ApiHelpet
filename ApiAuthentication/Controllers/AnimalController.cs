using ApiAuthentication.Data;
using ApiAuthentication.DTO;
using ApiAuthentication.Entities;
using ApiHelpet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiHelpet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnimalController(DataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Animal
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimals()
        {
            var currentUserId = await GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var userAnimals = await _context.Animals
                                            .Where(a => a.UserCode == currentUserId)
                                            .ToListAsync();

            return userAnimals;
        }

        // GET: api/Animal/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<Animal>> GetAnimal(int id)
        {
            var currentUserId = await GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var animal = await _context.Animals.FindAsync(id);

            if (animal == null)
            {
                return NotFound();
            }

            if (animal.UserCode != currentUserId)
            {
                return Forbid();
            }

            return animal;
        }

        // POST: api/Animal
        [HttpPost, Authorize]
        public async Task<ActionResult<Animal>> PostAnimal(AnimalViewModel animalViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var currentUserId = await GetCurrentUserId();

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var animal = new Animal
            {
                UserCode = currentUserId,
                Description = animalViewModel.Description,
                Size = animalViewModel.Size,
                Color = animalViewModel.Color,
                Age = animalViewModel.Age,
                Race = animalViewModel.Race,
                Type = animalViewModel.Type
            };

            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, animal);
        }

        // PUT: api/Animal/5
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutAnimal(int id, AnimalViewModel animalViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var currentUserId = await GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var existingAnimal = await _context.Animals.FindAsync(id);
            if (existingAnimal == null)
            {
                return NotFound();
            }

            if (existingAnimal.UserCode != currentUserId)
            {
                return Forbid();
            }

            existingAnimal.Description = animalViewModel.Description;
            existingAnimal.Size = animalViewModel.Size;
            existingAnimal.Color = animalViewModel.Color;
            existingAnimal.Age = animalViewModel.Age;
            existingAnimal.Race = animalViewModel.Race;
            existingAnimal.Type = animalViewModel.Type;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimalExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(existingAnimal);
        }

        // DELETE: api/Animal/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var currentUserId = await GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            if (animal.UserCode != currentUserId)
            {
                return Forbid(); 
            }

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Animal com ID {id} excluído com sucesso" });
        }

        private bool AnimalExists(int id)
        {
            return _context.Animals.Any(e => e.Id == id);
        }

        private async Task<string> GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userIdClaim);

            if (user != null)
            {
                return user.Id;
            }
            else
            {
                return null; 
            }
        }

    }
}

