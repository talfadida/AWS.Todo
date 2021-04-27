using System;
using System.Threading.Tasks;
using Aws.Todo.Core;
using Aws.Todo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Aws.Todo.Api.Controllers
{
    [ApiController]
    public class TodoController : ControllerBase
    {
       
        private readonly ILogger<TodoController> _logger;
        private readonly ITodoProcessor _processor;

        
        public TodoController(ILogger<TodoController> logger, ITodoProcessor processor )
        {     
            
            this._logger = logger;
            this._processor = processor;
        }
 
         
        [Route("api/operation")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TodoCommand command)
        {
            try
            {


                var processorResponse = await Task.Run(()=>_processor.Handle(command)); //wrapping as Task to utilize async/await pattern

                if (processorResponse.IsSuccess)
                {
                    _logger.LogInformation($"Command {command.OperationUUID} Operation:{command.Operation} completed successfully");
                    return Ok(processorResponse);
                }
                else
                {
                    _logger.LogError($"Command {command.OperationUUID} Operation:{command.Operation} completed with error: {processorResponse.ErrorMessage}");
                    return StatusCode(500, processorResponse);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Command {command.OperationUUID} Operation:{command.Operation} completed with error: {ex.ToString()}");
                return StatusCode(500 , command.CreateErrorResponse(ex.Message));
            }

        }

       
    }
}
