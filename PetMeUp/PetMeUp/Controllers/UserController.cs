using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetMeUp.Dtos;
using PetMeUp.Handlers;
using PetMeUp.Models.Models;
using PetMeUp.Models.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetMeUp.Controllers
{
    public class UserController : SuperController
    {
        private readonly UserHandler _handler;
        public UserController(IConfiguration config) : base(config)
        {
            _handler = new UserHandler(_conString, _dbtype, _LogHandler);
        }

        // GET: api/<UserController>
        [HttpGet, Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult<DataResponse<List<User>>>> GetAllUsers()
        {
            try
            {
                await WriteRequestInfoToLog<object?>(null);
                var result = await _handler.GetUsers();
                await WriteResponseInfoToLog(result);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on GetAllUsers with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            }

        }

        // GET api/<UserController>/5
        [HttpGet("{id}"), Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult<DataResponse<User>>> GetUserFromId(int id)
        {
            try
            {
                await WriteRequestInfoToLog(id);
                var result = await _handler.GetUser(id);
                await WriteResponseInfoToLog(result);
                return result.Data is null ? BadRequest() : Ok(result);
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on GetUserFromId with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            }
        }
        [HttpGet("{username}"), Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult<DataResponse<User>>> GetUserFromUsername(string username)
        {
            try
            {
                await WriteRequestInfoToLog(username);
                var result = await _handler.GetUser(username);
                await WriteResponseInfoToLog(result);
                return result.Data is null ? BadRequest() : Ok(result);
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on GetUserFromUsername with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            } 
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> Register([FromBody] UserDto dto)
        {
            try
            {
                await WriteRequestInfoToLog(dto);
                var result = await _handler.AddUser(dto, true);
                await WriteResponseInfoToLog(result);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on Register with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            } 
        }

        [HttpPost]
        public async Task<ActionResult<DataResponse<string>>> Login([FromBody] UserDto dto)
        {
            try
            {
                var secret = config.GetSection("SecretKey").Value;
                await WriteRequestInfoToLog(dto);
                var result = await _handler.Login(dto, secret);
                await WriteResponseInfoToLog(result);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on Login with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            }
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResponse>> RegisterEmployee([FromBody] UserDto dto)
        {
            try
            {
                await WriteRequestInfoToLog(dto);
                var result = await _handler.AddUser(dto, false);
                await WriteResponseInfoToLog(result);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on RegisterEmployee with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            }
            
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResponse>> UpdateUser(int id, [FromBody] UserDto dto)
        {
            try
            {
                await WriteRequestInfoToLog(dto);
                var result = await _handler.UpdateUser(dto, id);
                await WriteResponseInfoToLog(result);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on UpdateUser with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResponse>> DeleteUser(int id)
        {
            try
            {
                await WriteRequestInfoToLog(id);
                var result = await _handler.DeleteUser(id);
                await WriteResponseInfoToLog(result);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on DeleteUser with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            }
        }

        protected override void DisposeLocal()
        {
            _handler.Dispose();
        }
    }
}
