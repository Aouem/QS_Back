using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QS.Models;

namespace QS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuestionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Question
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
        {
            var questions = await _context.Questions.Include(q => q.Reponses).ToListAsync();
            return Ok(questions);
        }

        // GET: api/Question/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
            var question = await _context.Questions
                                          .Include(q => q.Reponses)
                                          .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
            {
                return NotFound("La question avec cet ID n'a pas été trouvée.");
            }

            return Ok(question);
        }

        // POST: api/Question
        [HttpPost]
        public async Task<ActionResult<Question>> PostQuestion(Question question)
        {
            // Validation de la requête
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validation des réponses associées à la question
            if (question.Reponses != null && question.Reponses.Any())
            {
                foreach (var reponse in question.Reponses)
                {
                    reponse.QuestionId = question.Id; // Associer la réponse à la question

                    // Vérifier que le RepondantId existe dans la base de données
                    if (!_context.Repondants.Any(r => r.Id == reponse.RepondantId))
                    {
                        return BadRequest("Le RepondantId est invalide dans une des réponses.");
                    }
                }
            }

            question.CreatedDate = DateTime.UtcNow; // Date de création
            _context.Questions.Add(question); // Ajouter la question à la base de données
            await _context.SaveChangesAsync(); // Sauvegarder les changements

            return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, question); // Retourner la question créée
        }

        // PUT: api/Question/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(int id, Question question)
        {
            // Vérification de l'ID
            if (id != question.Id)
            {
                return BadRequest("L'ID de la question ne correspond pas.");
            }

            // Vérification si la question existe avant de la modifier
            var existingQuestion = await _context.Questions.FindAsync(id);
            if (existingQuestion == null)
            {
                return NotFound("Question non trouvée.");
            }

            // Marquer la question comme modifiée
            _context.Entry(question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
                {
                    return NotFound("Question non trouvée.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Retourner une réponse sans contenu (succès)
        }

        // DELETE: api/Question/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound("Question non trouvée.");
            }

            _context.Questions.Remove(question); // Supprimer la question
            await _context.SaveChangesAsync(); // Sauvegarder les changements

            return NoContent(); // Retourner une réponse sans contenu
        }

        // Vérifier si une question existe
        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
