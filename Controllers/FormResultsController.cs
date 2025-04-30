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
            _logger = logger; // Injecter le service de logs
        }

        // POST: api/FormResults
     [HttpPost]
public async Task<IActionResult> PostFormResult([FromBody] FormResultDto formResult)
{
    if (formResult == null)
    {
        return BadRequest("Données invalides");
    }

    if (!_context.Categories.Any(c => c.Id == formResult.Repondant.Categorie))
    {
        return BadRequest("Categorie invalide.");
    }

    if (!_context.MedicalServices.Any(s => s.Id == formResult.Service))
    {
        return BadRequest("Service invalide.");
    }

    var repondant = new Repondant
    {
        Nom = formResult.Repondant.Nom,
        CategorieId = formResult.Repondant.Categorie,
        ServiceId = formResult.Service
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
                        Categorie = r.Repondant.CategorieId
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
    }
}
