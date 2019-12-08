using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Data;
using DatingApp.API.Models;
using DatingApp.API.Dtos;

namespace DatingApp.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly IAuthRepository _repository;
    public AuthController(IAuthRepository repository) {
      _repository = repository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
    {
      // validate request
      userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
      if (await _repository.UserExists(userForRegisterDto.Username))
        return BadRequest("Username already exists");

      var userToCreate = new User {
        Username = userForRegisterDto.Username
      };
      var createdUser = await _repository.Register(userToCreate, userForRegisterDto.Password);
      return StatusCode(201);
      //return CreatedAtRoute();
    }
  }
}
