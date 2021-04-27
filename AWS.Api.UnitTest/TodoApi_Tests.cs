using Aws.Todo.Api.Controllers;
using Aws.Todo.Core;
using Aws.Todo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aws.Api.UnitTest
{
    public class TodoApi_Tests
    {
        [Test]
        public async Task Test_Post_HappyFlow()
        {
            var logger = new Mock<ILogger<TodoController>>();
            var processor = new Mock<ITodoProcessor>();
            processor.Setup(p => p.Handle(It.IsAny<TodoCommand>())).Returns<TodoCommand>(p => new TodoResponse() { IsSuccess = true });

            var todoController = new TodoController(logger.Object, processor.Object);
            var response =  await todoController.Post(new TodoCommand() { });
             
            var okResponse = response as OkObjectResult;
            Assert.IsNotNull(okResponse);
            var todoResponse = okResponse.Value as TodoResponse;
            Assert.IsNotNull(todoResponse);
            Assert.IsTrue(todoResponse.IsSuccess);                        
        }

        [Test]
        public async Task Test_Post_GetList_HappyFlow()
        {
            var logger = new Mock<ILogger<TodoController>>();
            var processor = new Mock<ITodoProcessor>();
            processor.Setup(p => p.Handle(It.IsAny<TodoCommand>())).Returns<TodoCommand>(p => new TodoResponse() 
            { 
                IsSuccess = true,
                TaskList = new List<TodoItem>()
                {
                     new TodoItem()
                     {
                          Title = "task1"                            
                     }
                }
            });

            var todoController = new TodoController(logger.Object, processor.Object);
            var response = await todoController.Post(new TodoCommand() { });

            var okResponse = response as OkObjectResult;
            Assert.IsNotNull(okResponse);
            var todoResponse = okResponse.Value as TodoResponse;
            Assert.IsNotNull(todoResponse);
            Assert.IsTrue(todoResponse.IsSuccess);
            Assert.IsTrue(todoResponse.TaskList.Count == 1 );
        }

        [Test]
        public async Task Test_Post_Failure()
        {
            string ERROR_MSG = "error message";
            var logger = new Mock<ILogger<TodoController>>();
            var processor = new Mock<ITodoProcessor>();
            processor.Setup(p => p.Handle(It.IsAny<TodoCommand>())).Returns<TodoCommand>(p => new TodoResponse() { IsSuccess = false, ErrorMessage = ERROR_MSG });

            var todoController = new TodoController(logger.Object, processor.Object);
            var response = await todoController.Post(new TodoCommand() { });

            var okResponse = response as ObjectResult;
            Assert.IsNotNull(okResponse);
            var todoResponse = okResponse.Value as TodoResponse;
            Assert.IsNotNull(todoResponse);
            Assert.IsFalse(todoResponse.IsSuccess);
            Assert.AreEqual(todoResponse.ErrorMessage , ERROR_MSG);
        }

    }
}