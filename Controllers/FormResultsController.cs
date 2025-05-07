using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QS.Models;
using Microsoft.Extensions.Logging;

namespace QS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormResultsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<FormResultsController> _logger;

        public FormResultsController(AppDbContext context, ILogger<FormResultsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST: api/FormResults
        [HttpPost]
        public async Task<IActionResult> PostFormResult([FromBody] FormResultDto formResult)
        {
            if (formResult == null)
                return BadRequest("Données invalides");

            if (!_context.Categories.Any(c => c.Id == formResult.Repondant.Categorie))
                return BadRequest("Categorie invalide.");

            if (!_context.MedicalServices.Any(s => s.Id == formResult.Service))
                return BadRequest("Service invalide.");

            var repondant = new Repondant
            {
                Nom = formResult.Repondant.Nom,
                CategorieId = formResult.Repondant.Categorie,
                ServiceId = formResult.Service,
                 Genre = formResult.Repondant.Genre, // Ajout du genre
               TrancheAge = formResult.Repondant.TrancheAge  // Assure-toi que la tranche d'âge est envoyée
            };

            _context.Repondants.Add(repondant);
            await _context.SaveChangesAsync();

            var formResultEntity = new FormResultEntity
            {
                ServiceId = formResult.Service,
                RepondantId = repondant.Id,
                Reponses = JsonConvert.SerializeObject(formResult.Reponses)
            };

            _context.FormResultEntities.Add(formResultEntity);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Formulaire enregistré avec succès ✅" });
        }

        // GET: api/FormResults
        [HttpGet]
public async Task<IActionResult> GetFormResults()
{
    try
    {
        _logger.LogInformation("Récupération des résultats de formulaire.");

        var results = await _context.FormResultEntities
            .Include(r => r.Repondant)
            .ToListAsync();

        var resultDtos = results.Select(r => new
        {
            Service = r.ServiceId,
            Repondant = new
            {
                Nom = r.Repondant.Nom,
                Categorie = r.Repondant.CategorieId,
                Genre = r.Repondant.Genre, // Ajout du genre
               TrancheAge = r.Repondant.TrancheAge  // Ajout de la tranche d'âge
            },
            Reponses = string.IsNullOrEmpty(r.Reponses)
                ? new List<ReponseDto>()
                : JsonConvert.DeserializeObject<List<ReponseDto>>(r.Reponses)
        }).ToList();

        _logger.LogInformation("Résultats de formulaire récupérés : {Count} résultats", resultDtos.Count);

        return Ok(resultDtos);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erreur lors de la récupération des résultats de formulaire.");
        return StatusCode(500, $"Erreur interne du serveur: {ex.Message}");
    }
}


        // DELETE: api/FormResults
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            var allResults = await _context.FormResultEntities.ToListAsync();

            if (allResults.Count == 0)
                return NoContent();

            // Suppression des répondants liés (facultatif, décommente si tu veux cette logique)
            var repondantIds = allResults.Select(r => r.RepondantId).ToList();
            var repondants = await _context.Repondants
                .Where(r => repondantIds.Contains(r.Id))
                .ToListAsync();

            _context.FormResultEntities.RemoveRange(allResults);
            _context.Repondants.RemoveRange(repondants); // facultatif

            await _context.SaveChangesAsync();

            return Ok(new { message = "Tous les résultats et répondants liés ont été supprimés." });
        }
        [HttpDelete("partial-delete")]
public async Task<IActionResult> DeleteReponsesEtRepondants()
{
    // Supprime d'abord les réponses
    var reponses = await _context.Reponses.ToListAsync();
    _context.Reponses.RemoveRange(reponses);

    // Supprime ensuite les répondants
    var repondants = await _context.Repondants.ToListAsync();
    _context.Repondants.RemoveRange(repondants);

    await _context.SaveChangesAsync();

    return NoContent();
}

    }
    
}
