using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QS.Models;

namespace QS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReponsesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ReponseValidator _validator;

        public ReponsesController(AppDbContext context, ReponseValidator validator)
        {
            _context = context;
            _validator = validator;
        }
// GET: api/Reponses
[HttpGet]
public async Task<ActionResult<IEnumerable<object>>> GetReponses(string genre = null)
{
    var query = _context.Reponses
        .Include(r => r.Question)
        .Include(r => r.Repondant)
        .Select(r => new
        {
            r.Id,
            r.QuestionId,
            QuestionType = r.Question.Type,
            r.RepondantId,
            RepondantNom = r.Repondant.Nom,
            RepondantCategorie = r.Repondant.Categorie,
            RepondantGenre = r.Repondant.Genre,  // Ajout du genre
            r.NumericValue,
            r.BooleanValue,
            r.TextValue
        });

    // Si un genre est passé, appliquer un filtre
    if (!string.IsNullOrEmpty(genre))
    {
        genre = genre.ToLower();  // rendre le genre insensible à la casse
        query = query.Where(r => r.RepondantGenre.ToLower() == genre);
    }

    var reponses = await query.ToListAsync();
    return Ok(reponses);
}


        // GET: api/Reponses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetReponse(int id)
        {
            var reponse = await _context.Reponses
                .Include(r => r.Question)
                .Include(r => r.Repondant)
                .Select(r => new
                {
                    r.Id,
                    r.QuestionId,
                    QuestionType = r.Question.Type,
                    r.RepondantId,
                    RepondantNom = r.Repondant.Nom,
                    RepondantCategorie = r.Repondant.Categorie,
                    r.NumericValue,
                    r.BooleanValue,
                    r.TextValue
                })
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reponse == null)
            {
                return NotFound();
            }

            return Ok(reponse);
        }

        // POST: api/Reponses
        [HttpPost]
        public async Task<ActionResult<Reponse>> PostReponse(Reponse reponse)
        {
            // Validation du type de question
            if (!_validator.ValidateQuestionType(reponse.Question.Type))
            {
                return BadRequest("Le type de question est invalide.");
            }

            // Validation de la question existante
            if (!_context.Questions.Any(q => q.Id == reponse.QuestionId))
            {
                return BadRequest("QuestionId invalide.");
            }

            // Validation du répondant existant
            if (!_context.Repondants.Any(r => r.Id == reponse.RepondantId))
            {
                return BadRequest("RepondantId invalide.");
            }

            _context.Reponses.Add(reponse);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReponse), new { id = reponse.Id }, reponse);
        }

        // PUT: api/Reponses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReponse(int id, Reponse reponse)
        {
            if (id != reponse.Id)
            {
                return BadRequest();
            }

            // Validation
            if (!_context.Questions.Any(q => q.Id == reponse.QuestionId))
            {
                return BadRequest("QuestionId invalide.");
            }

            if (!_context.Repondants.Any(r => r.Id == reponse.RepondantId))
            {
                return BadRequest("RepondantId invalide.");
            }

            _context.Entry(reponse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReponseExists(id))
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

        // DELETE: api/Reponses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReponse(int id)
        {
            var reponse = await _context.Reponses.FindAsync(id);
            if (reponse == null)
            {
                return NotFound();
            }

            _context.Reponses.Remove(reponse);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReponseExists(int id)
        {
            return _context.Reponses.Any(e => e.Id == id);
        }
    }
}
