using LocatedAPI.Models;
using LocatedAPI.Services;
using LocatedAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LocatedAPI.Models.DTO;

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
    public async Task<ActionResult<List<Person>>> GetAllAsync()
    {
        var persons = await personService.GetAllPersonsAsync();
        return Ok(persons);
    }

    [Authorize]
    [HttpGet("persons/{id}")]
    public async Task<ActionResult<PersonResp>> GetByIdAsync(int id)
    {
        var person = await personService.GetPersonByIdAsync(id);
        if (person == null)
            return NotFound("Person não encontrada!");

        return Ok(person);
    }

    [AllowAnonymous]
    [HttpPost("persons")]
    public async Task<ActionResult<Person>> PostAsync([FromBody] PersonSignUpReq person)
    {
        try
        {
            var createdPersonId = await personService.CreatePersonAsync(person);
            return Created($"api/persons/{createdPersonId}", person);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
    
    [Authorize]
    [HttpPut("persons")]
    public async Task<ActionResult<Person>> PutAsync([FromBody] PersonReq person)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var success = await personService.UpdatePersonAsync(person);

        if (!success)
            return NotFound("Person não encontrada!");

        return Ok(person);
    }

    [Authorize]
    [HttpDelete("persons/{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var success = await personService.DeletePersonAsync(id);

        if (!success)
            return BadRequest("Person não encontrada");

        return Ok();
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
                Message = "Registro criado com sucesso."
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
