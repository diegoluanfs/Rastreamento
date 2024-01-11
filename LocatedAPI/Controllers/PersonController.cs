using LocatedAPI.Models;
using LocatedAPI.Services;
using LocatedAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LocatedAPI.Models.DTO;
using System.Security.Claims;

[Route("api")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly IJWTAuthenticationManager jWTAuthenticationManager;
    private readonly IPersonService personService;

    public PersonController(IJWTAuthenticationManager jWTAuthenticationManager, IPersonService personService)
    {
        this.jWTAuthenticationManager = jWTAuthenticationManager;
        this.personService = personService;
    }

    [Authorize]
    [HttpGet("persons")]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var personIdentify = new PersonIdentifyReq { UserId = userId };

            var persons = await personService.GetAllPersonsAsync(personIdentify);

            return Ok(new
            {
                StatusCode = 200,
                Message = "Consulta realizada com sucesso.",
                Data = persons
            });
        }
        catch (Exception ex)
        {
            var errorResponse = new ApiResp
            {
                StatusCode = 500,
                Message = ex.Message
            };

            return StatusCode(500, errorResponse);
        }
    }

    [Authorize]
    [HttpGet("persons/{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        try
        {
            var person = await personService.GetPersonByIdAsync(id);

            if (person == null)
            {
                var notFoundResponse = new ApiResp
                {
                    StatusCode = 404,
                    Message = "Person não encontrada!"
                };

                return NotFound(notFoundResponse);
            }

            return Ok(new
            {
                StatusCode = 200,
                Message = "Consulta realizada com sucesso.",
                Data = person
            });
        }
        catch (Exception ex)
        {
            var errorResponse = new ApiResp
            {
                StatusCode = 500,
                Message = ex.Message
            };

            return StatusCode(500, errorResponse);
        }
    }

    [AllowAnonymous]
    [HttpPost("person")]
    public async Task<IActionResult> PostAsync([FromBody] PersonSignUpReq person)
    {
        try
        {
            var createdPersonId = await personService.CreatePersonAsync(person);

            var response = new ApiResp
            {
                StatusCode = 201,
                Message = "Registro criado com sucesso."
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorResponse = new ApiResp
            {
                StatusCode = 400,
                Message = ex.Message
            };

            return BadRequest(errorResponse);
        }
    }

    [Authorize]
    [HttpPut("persons")]
    public async Task<IActionResult> PutAsync([FromBody] PerfilReq perfil)
    {
        try
        {
            var success = await personService.UpdatePersonAsync(perfil);

            if (!success)
            {
                var notFoundResponse = new ApiResp
                {
                    StatusCode = 404,
                    Message = "Perfil não atualizado!"
                };

                return NotFound(notFoundResponse);
            }

            return Ok(new
            {
                StatusCode = 200,
                Message = "Perfil atualizado.",
                Data = success
            });
        }
        catch (Exception ex)
        {
            var errorResponse = new ApiResp
            {
                StatusCode = 500,
                Message = ex.Message
            };

            return StatusCode(500, errorResponse);
        }
    }

    [Authorize]
    [HttpDelete("persons/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var success = await personService.DeletePersonAsync(id);

            if (!success)
            {
                var badRequestResponse = new ApiResp
                {
                    StatusCode = 400,
                    Message = "Exclusão não realizada."
                };

                return BadRequest(badRequestResponse);
            }

            var response = new ApiResp
            {
                StatusCode = 200,
                Message = "Exclusão realizada com sucesso."
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorResponse = new ApiResp
            {
                StatusCode = 500,
                Message = ex.Message
            };

            return StatusCode(500, errorResponse);
        }
    }

    [AllowAnonymous]
    [HttpPost("autenticar")]
    public async Task<IActionResult> AuthenticateAsync([FromBody] PersonSignInReq personSignInReq)
    {
        try
        {
            var token = await personService.AuthenticateAsync(personSignInReq);

            var response = new ApiResp
            {
                Token = token,
                StatusCode = 200,
                Message = "Login realizado com sucesso."
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return Unauthorized(new ApiResp
            {
                StatusCode = 401,
                Message = ex.Message
            });
        }
    }
}
