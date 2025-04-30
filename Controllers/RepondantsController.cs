using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QS.Models;

namespace QS.Controllers
{
    // Route de base pour le contrôleur
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

            if (repondantDto.Id == 0)
            {
                var repondant = new Repondant
                {
                    Nom = repondantDto.Nom,
                    CategorieId = repondantDto.Categorie
                };

                _context.Repondants.Add(repondant);
                await _context.SaveChangesAsync();

                // ✅ Renvoie ici l'objet créé avec son ID
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
                repondant.CategorieId = repondantDto.Categorie;

                await _context.SaveChangesAsync();

                // Optionnel : ici aussi on peut renvoyer l'objet complet
                return Ok(repondant);
            }
        }

        // PUT: api/MedicalService/repondants/5
        [HttpPut("repondants/{id}")]
        public async Task<IActionResult> PutRepondant(int id, Repondant repondant)
        {
            if (id != repondant.Id)
            {
                return BadRequest();
            }

            // Validation des relations
            if (!_context.Categories.Any(c => c.Id == repondant.CategorieId))
            {
                return BadRequest("CategorieId invalide.");
            }

            if (!_context.MedicalServices.Any(s => s.Id == repondant.ServiceId))
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

        // DELETE: api/MedicalService/repondants/5
        [HttpDelete("repondants/{id}")]
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
