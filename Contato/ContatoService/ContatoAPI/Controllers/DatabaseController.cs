using Contatos.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Core.Entidades.Usuario;

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

    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    [HttpPost("ExecutarMigracaoBanco")]
    [Authorize(Roles = Roles.Administrador)]
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
