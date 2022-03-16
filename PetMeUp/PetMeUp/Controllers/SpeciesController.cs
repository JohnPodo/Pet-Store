using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetMeUp.Dtos;
using PetMeUp.Handlers;
using PetMeUp.Models.Models;
using PetMeUp.Models.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetMeUp.Controllers
{ 
    public class SpeciesController : SuperController
    {
        private readonly SpeciesHandler _handler;
        public SpeciesController(IConfiguration config) : base(config)
        {
            _handler = new SpeciesHandler(_conString, _dbtype, _LogHandler);
        }
         
        [HttpGet]
        public async Task<ActionResult<DataResponse<List<PetSpecie>>>> GetSpecies()
        {
            try
            {
                await WriteRequestInfoToLog<object?>(null);
                var result = await _handler.GetSpecies();
                await WriteResponseInfoToLog(result);
                return result.Success ?  Ok(result) : BadRequest(result) ;
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on GetSpecies with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            }
        }

        // GET api/<GroupController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DataResponse<PetSpecie>>> GetSpecie(int id)
        {
            try
            {
                await WriteRequestInfoToLog(id);
                var result = await _handler.GetSpecie(id);
                await WriteResponseInfoToLog(result);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on GetGroup with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            }
        }
        [HttpGet("{pageNo}/{pageSize}")]
        public async Task<ActionResult<DataCountResponse<PetSpecie>>> GetSpeciesPage(int pageNo,int pageSize)
        {
            try
            {
                await WriteRequestInfoToLog(new { pageNo = pageNo , pageSize = pageSize });
                var result = await _handler.GetSpecies(pageNo,pageSize);
                await WriteResponseInfoToLog(result);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on GetSpeciesPage with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            }
        } 
        // POST api/<GroupController>
        [HttpPost, Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult<BaseResponse>> AddSpecie([FromBody] SpecieDto dto)
        {
            try
            {
                await WriteRequestInfoToLog(dto);
                var result = await _handler.AddSpecie(dto);
                await WriteResponseInfoToLog(result);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on AddSpecie with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            }
        }

        // PUT api/<GroupController>/5
        [HttpPut("{id}"), Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult<BaseResponse>> UpdateSpecie(int id, [FromBody] SpecieDto dto)
        {
            try
            {
                await WriteRequestInfoToLog(id);
                var result = await _handler.UpdateSpecie(id, dto);
                await WriteResponseInfoToLog(result);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on UpdateSpecie with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            }
        }

        // DELETE api/<GroupController>/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult<BaseResponse>> DeleteSpecie(int id)
        {
            try
            {
                await WriteRequestInfoToLog(id);
                var result = await _handler.DeleteSpecie(id);
                await WriteResponseInfoToLog(result);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                await _LogHandler.WriteToLog($"Exception on DeleteSpecie with message --> {ex.Message}", Models.Severity.Exception);
                return StatusCode(500);
            }
        }

        protected override void DisposeLocal()
        {
            _handler.Dispose();
        }
    }
}
