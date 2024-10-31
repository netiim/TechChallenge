using Contatos.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContatosService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly IDatabaseService _databaseService;

    public DatabaseController(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [HttpPost("ExecutarMigracaoBanco")]
    public IActionResult Migrate()
    {
        try
        {
            _databaseService.MigrateDatabase();
            return Ok("Database migration applied successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error applying migration: {ex.Message}");
        }
    }
}
