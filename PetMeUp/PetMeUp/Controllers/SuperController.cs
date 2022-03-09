using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PetMeUp.Handlers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetMeUp
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public abstract class SuperController : ControllerBase,IDisposable
    {
        protected readonly IConfiguration config;
        protected readonly LogHandler _LogHandler;
        protected readonly string _conString;
        protected readonly string _dbtype;

        public SuperController(IConfiguration config)
        {
            this.config = config;
            _conString = config.GetConnectionString("ConString");
            _dbtype = config.GetConnectionString("DbType");
            _LogHandler = new LogHandler(_conString, _dbtype);
        }

        public void Dispose()
        {
            _LogHandler.Dispose();
            DisposeLocal();
        }

        protected abstract void DisposeLocal();

        protected async Task WriteRequestInfoToLog<T>(T model)
        {
            await _LogHandler.WriteToLog("Received Request", Models.Severity.Information);
            await _LogHandler.WriteToLog($"Request Path --> {JsonConvert.SerializeObject(this.Request.Path, Formatting.Indented)}", Models.Severity.Information);
            await _LogHandler.WriteToLog($"Request Headers --> {JsonConvert.SerializeObject(this.Request.Headers, Formatting.Indented)}", Models.Severity.Information);
            await _LogHandler.WriteToLog($"Request Body --> {JsonConvert.SerializeObject(model, Formatting.Indented)}", Models.Severity.Information);
            await _LogHandler.WriteToLog($"Session User --> {JsonConvert.SerializeObject(this.User.Identity.Name, Formatting.Indented)}", Models.Severity.Information);
        }

        protected async Task WriteResponseInfoToLog<T>(T response)
        {
            await _LogHandler.WriteToLog($"Response --> {JsonConvert.SerializeObject(response, Formatting.Indented)}", Models.Severity.Information);
            await _LogHandler.WriteToLog("Request was answered in success", Models.Severity.Information);
        }
    }
}
