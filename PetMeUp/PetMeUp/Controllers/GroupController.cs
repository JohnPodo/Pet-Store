using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetMeUp.Dtos;
using PetMeUp.Handlers;
using PetMeUp.Models.Models;
using PetMeUp.Models.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetMeUp.Controllers
{ 
    public class GroupController : SuperController
    {
        private readonly GroupHandler _handler;
        public GroupController(IConfiguration config) : base(config)
        {
            _handler = new GroupHandler(_conString, _dbtype, _LogHandler);
        }
         
        [HttpGet, Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult<DataResponse<List<PetGroup>>>> GetGroups()
        {
            try
            {
                await WriteRequestInfoToLog<object?>(null);
                var result = await _handler.GetGroups();
                await WriteResponseInfoToLog(result);
                return result.Success ?  Ok(result) : BadRequest(result) ;
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on GetGroups with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            }
        }

        // GET api/<GroupController>/5
        [HttpGet("{id}"), Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult<DataResponse<PetGroup>>> GetGroup(int id)
        {
            try
            {
                await WriteRequestInfoToLog(id);
                var result = await _handler.GetGroup(id);
                await WriteResponseInfoToLog(result);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on GetGroup with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            }
        }
        [HttpGet("{pageNo}/{pageSize}"), Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult<DataResponse<PetGroup>>> GetGroupPage(int pageNo,int pageSize)
        {
            try
            {
                await WriteRequestInfoToLog(new { pageNo = pageNo , pageSize = pageSize });
                var result = await _handler.GetGroups(pageNo,pageSize);
                await WriteResponseInfoToLog(result);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on GetGroupPage with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            }
        }

        // POST api/<GroupController>
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddGroup([FromBody] GroupDto dto)
        {
            try
            {
                await WriteRequestInfoToLog(dto);
                var result = await _handler.AddGroup(dto);
                await WriteResponseInfoToLog(result);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on GetGroupPage with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            }
        }

        // PUT api/<GroupController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GroupController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        protected override void DisposeLocal()
        {
            _handler.Dispose();
        }
    }
}
