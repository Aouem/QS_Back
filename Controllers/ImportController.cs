using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QS.Models;
using System.Threading.Tasks;
using System;

namespace QS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ImportController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportData([FromBody] ExportDto exportData)
        {
            if (exportData == null)
            {
                return BadRequest("Données manquantes.");
            }

            try
            {
                // Ajouter ou mettre à jour les questions
                foreach (var question in exportData.Questions)
                {
                    var existingQuestion = await _context.Questions
                        .FirstOrDefaultAsync(q => q.Id == question.Id);
                    if (existingQuestion != null)
                    {
                        _context.Entry(existingQuestion).CurrentValues.SetValues(question);
                    }
                    else
                    {
                        _context.Questions.Add(question);
                    }
                }

                // Ajouter ou mettre à jour les répondants
                foreach (var repondant in exportData.Repondants)
                {
                    var existingRepondant = await _context.Repondants
                        .FirstOrDefaultAsync(r => r.Id == repondant.Id);
                    if (existingRepondant != null)
                    {
                        _context.Entry(existingRepondant).CurrentValues.SetValues(repondant);
                    }
                    else
                    {
                        _context.Repondants.Add(repondant);
                    }
                }

                // Ajouter ou mettre à jour les services médicaux
                foreach (var service in exportData.MedicalServices)
                {
                    var existingService = await _context.MedicalServices
                        .FirstOrDefaultAsync(s => s.Id == service.Id);
                    if (existingService != null)
                    {
                        _context.Entry(existingService).CurrentValues.SetValues(service);
                    }
                    else
                    {
                        _context.MedicalServices.Add(service);
                    }
                }

                // Ajouter ou mettre à jour les catégories
                foreach (var category in exportData.Categories)
                {
                    var existingCategory = await _context.Categories
                        .FirstOrDefaultAsync(c => c.Id == category.Id);
                    if (existingCategory != null)
                    {
                        _context.Entry(existingCategory).CurrentValues.SetValues(category);
                    }
                    else
                    {
                        _context.Categories.Add(category);
                    }
                }

                // Ajouter ou mettre à jour les réponses
                foreach (var reponseDto in exportData.Reponses)
                {
                    var question = await _context.Questions.FindAsync(reponseDto.QuestionId);
                    var repondant = await _context.Repondants.FindAsync(reponseDto.Repondant.Id);

                    if (question == null || repondant == null)
                    {
                        return BadRequest("Question ou Répondant non trouvé.");
                    }

                    var reponse = new Reponse
                    {
                        Question = question,
                        Repondant = repondant,
                        NumericValue = reponseDto.NumericValue,
                        BooleanValue = reponseDto.BooleanValue,
                        TextValue = reponseDto.TextValue,
                        RepondantId = repondant.Id,
                        QuestionId = question.Id
                    };

                    var existingReponse = await _context.Reponses
                        .FirstOrDefaultAsync(r => r.QuestionId == reponseDto.QuestionId && r.RepondantId == reponseDto.Repondant.Id);

                    if (existingReponse != null)
                    {
                        _context.Entry(existingReponse).CurrentValues.SetValues(reponse);
                    }
                    else
                    {
                        _context.Reponses.Add(reponse);
                    }
                }

                await _context.SaveChangesAsync();

                return Ok("Importation réussie !");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de l'importation des données: {ex.Message}");
            }
        }
    }
}
