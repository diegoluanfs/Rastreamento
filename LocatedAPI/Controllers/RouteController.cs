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
public class RouteController : ControllerBase
{
    private readonly IJWTAuthenticationManager jWTAuthenticationManager;
    private readonly IRouteService routeService;

    public RouteController(IJWTAuthenticationManager jWTAuthenticationManager, IRouteService routeService)
    {
        this.jWTAuthenticationManager = jWTAuthenticationManager;
        this.routeService = routeService;
    }

    [Authorize]
    [HttpGet("routes")]
    public async Task<ActionResult<List<TargetRoute>>> GetAllRoutesAsync()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var personIdentify = new PersonIdentifyReq { UserId = userId };

            var routes = await routeService.GetAllRoutesAsync(personIdentify);

            return Ok(routes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [Authorize]
    [HttpGet("route/{idTarget}")]
    public async Task<ActionResult<List<TargetRoute>>> GetRouteByIdAsync(int idTarget)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var personIdentify = new PersonIdentifyReq { UserId = userId };

            var route = await routeService.GetRouteByIdAsync(idTarget, personIdentify);

            if (route == null)
            {
                return NotFound(); // ou outra resposta adequada para quando o alvo não for encontrado
            }

            return Ok(route);
        }
        catch (Exception ex)
        {
            // Trate a exceção conforme necessário
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [Authorize]
    [HttpPost("route")]
    public async Task<ActionResult<Route>> CreateRouteAsync(RouteTargetReq routeTargetReq)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Não foi possível obter o ID do usuário autenticado.");
            }

            var personIdentify = new PersonIdentifyReq { UserId = userId };

            var createdRouteId = await routeService.SaveRouteAsync(personIdentify, routeTargetReq);

            return Ok(createdRouteId);
        }
        catch (Exception ex)
        {
            // Logue a exceção para análise
            Console.WriteLine(ex);

            // Trate a exceção conforme necessário
            return StatusCode(500, "Erro interno do servidor");
        }
    }

}
