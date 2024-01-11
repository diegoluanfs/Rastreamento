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
public class ItemController : ControllerBase
{
    private readonly IJWTAuthenticationManager jWTAuthenticationManager;
    private readonly IItemService itemService;

    public ItemController(IJWTAuthenticationManager jWTAuthenticationManager, IItemService itemService)
    {
        this.jWTAuthenticationManager = jWTAuthenticationManager;
        this.itemService = itemService;
    }

    [Authorize]
    [HttpPost("item")]
    public async Task<ActionResult<Item>> CreateItemAsync(ItemReq itemReq)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Não foi possível obter o ID do usuário autenticado.");
            }

            var personIdentify = new PersonIdentifyReq { UserId = userId };

            var createdItemId = await itemService.CreateItemAsync(itemReq, personIdentify);

            if (createdItemId <= 0)
            {
                return StatusCode(500, "Erro ao criar o item.");
            }

            //var createdItem = await itemService.GetItemByIdAsync(createdItemId, personIdentify);

            //if (createdItem == null)
            //{
            //    return StatusCode(500, "Erro ao obter o item recém-criado.");
            //}

            //return Ok(createdItem);
            return Ok(createdItemId);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno do servidor");
        }
    }

}
