using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using System.Collections.Generic;

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
  }
}
