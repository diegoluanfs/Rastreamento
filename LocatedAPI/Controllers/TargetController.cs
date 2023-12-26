using LocatedAPI.Models;
using LocatedAPI.Services;
using LocatedAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using LocatedAPI;

[Route("api")]
[ApiController]
public class TargetController : ControllerBase
{
    private readonly IJWTAuthenticationManager jWTAuthenticationManager;
    private readonly ITargetService targetService;

    public TargetController(IJWTAuthenticationManager jWTAuthenticationManager, ITargetService targetService)
    {
        this.jWTAuthenticationManager = jWTAuthenticationManager;
        this.targetService = targetService;
    }

    [Authorize]
    [HttpGet("targets")]
    public async Task<ActionResult<List<Target>>> GetAllTargetsAsync()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var personIdentify = new PersonIdentifyReq { UserId = userId };

            var targets = await targetService.GetAllTargetsAsync(personIdentify);

            return Ok(targets);
        }
        catch (Exception ex)
        {
            // Trate a exceção conforme necessário
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [Authorize]
    [HttpGet("target/{id}")]
    public async Task<ActionResult<Target>> GetTargetByIdAsync(int id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var personIdentify = new PersonIdentifyReq { UserId = userId };

            var target = await targetService.GetTargetByIdAsync(id, personIdentify);

            if (target == null)
            {
                return NotFound(); // ou outra resposta adequada para quando o alvo não for encontrado
            }

            return Ok(target);
        }
        catch (Exception ex)
        {
            // Trate a exceção conforme necessário
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [Authorize]
    [HttpPost("target")]
    public async Task<ActionResult<Target>> CreateTargetAsync()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Não foi possível obter o ID do usuário autenticado.");
            }

            var personIdentify = new PersonIdentifyReq { UserId = userId };

            var createdTargetId = await targetService.CreateTargetAsync(personIdentify);

            if (createdTargetId <= 0)
            {
                return StatusCode(500, "Erro ao criar o target.");
            }

            var createdTarget = await targetService.GetTargetByIdAsync(createdTargetId, personIdentify);

            if (createdTarget == null)
            {
                return StatusCode(500, "Erro ao obter o target recém-criado.");
            }

            return Ok(createdTarget);
        }
        catch (Exception ex)
        {
            // Logue a exceção para análise
            Console.WriteLine(ex);

            // Trate a exceção conforme necessário
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [Authorize]
    [HttpDelete("target/{id}")]
    public async Task<IActionResult> DelTargetByIdAsync(int id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var personIdentify = new PersonIdentifyReq { UserId = userId };

            var isDeleted = await targetService.DelTargetByIdAsync(id, personIdentify);

            if (!isDeleted)
            {
                return NotFound(new ApiResp { StatusCode = 404, Message = "Recurso não encontrado." });
            }

            return Ok(new ApiResp { StatusCode = 200, Message = "Exclusão bem-sucedida." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResp { StatusCode = 500, Message = "Erro interno do servidor." });
        }
    }

}
