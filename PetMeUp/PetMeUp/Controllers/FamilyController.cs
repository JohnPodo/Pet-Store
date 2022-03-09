using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetMeUp.Handlers;
using PetMeUp.Models.Models;
using PetMeUp.Models.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetMeUp.Controllers
{ 
    public class FamilyController : SuperController
    {
        private readonly FamilyHandler _handler;
        public FamilyController(IConfiguration config) : base(config)
        {
            _handler = new FamilyHandler(_conString, _dbtype, _LogHandler);
        }
         
        [HttpGet, Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult<DataResponse<List<PetFamily>>>> GetFamilies()
        {
            try
            {
                await WriteRequestInfoToLog<object?>(null);
                var result = await _handler.GetFamilies();
                await WriteResponseInfoToLog(result);
                return result.Success ?  Ok(result) : BadRequest(result) ;
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on GetFamilies with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            }
        }

        // GET api/<GroupController>/5
        [HttpGet("{id}"), Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult<DataResponse<PetGroup>>> GetFamily(int id)
        {
            try
            {
                await WriteRequestInfoToLog(id);
                var result = await _handler.GetFamily(id);
                await WriteResponseInfoToLog(result);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on GetFamily with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            }
        }

        protected override void DisposeLocal()
        {
            _handler.Dispose();
        }
    }
}
