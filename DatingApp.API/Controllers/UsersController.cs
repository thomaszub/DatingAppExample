using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DatingApp.API.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly IDatingRepository _repository;
    private readonly IMapper _mapper;

    public UsersController(IDatingRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
      var users = await _repository.GetUsers();
      var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
      return Ok(usersToReturn);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
      var user = await _repository.GetUser(id);
      var userToReturn = _mapper.Map<UserForDetailedDto>(user);
      return Ok(userToReturn);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> updateUser(int id, UserForUpdateDto userForUpdateDto)
    {
      if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        return Unauthorized();

      var userFromRepo = await _repository.GetUser(id);
      _mapper.Map(userForUpdateDto, userFromRepo);

      if (await _repository.SaveAll())
        return NoContent();

      throw new Exception($"Updating user {id} failed on save");
    }
  }
}
