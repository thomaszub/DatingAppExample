using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Data;
using DatingApp.API.Models;
using DatingApp.API.Dtos;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;

namespace DatingApp.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly IAuthRepository _repository;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public AuthController(IAuthRepository repository, IConfiguration config, IMapper mapper)
    {
      _mapper = mapper;
      _repository = repository;
      _config = config;
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

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
    {
      var userFromRepo = await _repository.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
      if (userFromRepo == null)
        return Unauthorized();

      var claims = new[]
      {
        new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
        new Claim(ClaimTypes.Name, userFromRepo.Username)
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = creds
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);

      var user = _mapper.Map<UserForListDto>(userFromRepo);

      return Ok(new {
        token = tokenHandler.WriteToken(token), user
      });
    }
  }
}
