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
    public async Task<ActionResult<List<Microsoft.AspNetCore.Routing.Route>>> GetAllRoutesAsync()
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
    [HttpGet("route/{id}")]
    public async Task<ActionResult<Microsoft.AspNetCore.Routing.Route>> GetRouteByIdAsync(int id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var personIdentify = new PersonIdentifyReq { UserId = userId };

            var route = await routeService.GetRouteByIdAsync(id, personIdentify);

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

}
