using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QS.Models;

namespace QS.Controllers
{
    [Route("api/repondants")]
    [ApiController]
    public class RepondantController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RepondantController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/repondants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Repondant>>> GetRepondants()
        {
            return await _context.Repondants
                .Include(r => r.Categorie)
                .Include(r => r.Service)
                .ToListAsync();
        }

        // GET: api/repondants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Repondant>> GetRepondant(int id)
        {
            var repondant = await _context.Repondants
                .Include(r => r.Categorie)
                .Include(r => r.Service)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (repondant == null)
            {
                return NotFound();
            }

            return repondant;
        }

        // POST: api/repondants
        [HttpPost]
        public async Task<IActionResult> PostRepondant([FromBody] RepondantDto repondantDto)
        {
            if (repondantDto == null)
            {
                return BadRequest("Données invalides");
            }

            if (!_context.Categories.Any(c => c.Id == repondantDto.Categorie))
            {
                return BadRequest("CategorieId invalide.");
            }

            if (repondantDto.Service.HasValue && !_context.MedicalServices.Any(s => s.Id == repondantDto.Service.Value))
            {
                return BadRequest("ServiceId invalide.");
            }

            if (repondantDto.Id == 0)
            {
                var repondant = new Repondant
                {
                    Nom = repondantDto.Nom,
                    Genre = repondantDto.Genre,
                    TrancheAge = repondantDto.TrancheAge,
                    CategorieId = repondantDto.Categorie,
                    ServiceId = repondantDto.Service
                };

                _context.Repondants.Add(repondant);
                await _context.SaveChangesAsync();

                return Ok(repondant);
            }
            else
            {
                var repondant = await _context.Repondants.FindAsync(repondantDto.Id);
                if (repondant == null)
                {
                    return NotFound("Répondant introuvable.");
                }

                repondant.Nom = repondantDto.Nom;
                repondant.Genre = repondantDto.Genre;
                repondant.TrancheAge = repondantDto.TrancheAge;
                repondant.CategorieId = repondantDto.Categorie;
                repondant.ServiceId = repondantDto.Service;

                await _context.SaveChangesAsync();

                return Ok(repondant);
            }
        }

        // PUT: api/repondants/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRepondant(int id, [FromBody] Repondant repondant)
        {
            if (id != repondant.Id)
            {
                return BadRequest();
            }

            if (!_context.Categories.Any(c => c.Id == repondant.CategorieId))
            {
                return BadRequest("CategorieId invalide.");
            }

            if (repondant.ServiceId.HasValue && !_context.MedicalServices.Any(s => s.Id == repondant.ServiceId.Value))
            {
                return BadRequest("ServiceId invalide.");
            }

            _context.Entry(repondant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RepondantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/repondants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepondant(int id)
        {
            var repondant = await _context.Repondants.FindAsync(id);
            if (repondant == null)
            {
                return NotFound();
            }

            _context.Repondants.Remove(repondant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RepondantExists(int id)
        {
            return _context.Repondants.Any(e => e.Id == id);
        }
    }
}
