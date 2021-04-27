using Aws.Todo.Core;
using Aws.Todo.Model;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aws.Api.UnitTest
{



    public class TodoProcessor_Tests
    {
        Mock<IRepository> repo;
        ITodoProcessor processor;

        [SetUp]
        public void Setup()
        {
            var logger = new Mock<ILogger<ITodoProcessor>>();
            repo = new Mock<IRepository>();

            repo.Setup(p => p.TryAdd(It.IsAny<ITodoItem>())).Verifiable();
            repo.Setup(p => p.Delete(It.IsAny<string>())).Verifiable();
            repo.Setup(p => p.Update(It.IsAny<string>(), It.IsAny<ITodoItem>())).Verifiable();
            //repo.Setup(p => p.Get()).Returns<IEnumerable<ITodoItem>> (p => new List<ITodoItem>());

            processor = new TodoProcessor(repo.Object, logger.Object);
        }

        [Test]
        public void Test_Add_Happy_Flow()
        {

            var todoResponse = processor.Handle(new TodoCommand()
            {
                Operation = EnumOperation.Add,
                Title = "some task"
            });

            Assert.IsTrue(todoResponse.IsSuccess);
        }



        [Test]
        public void Test_Add_Fail_Empty_Title()
        {
            var todoResponse = processor.Handle(new TodoCommand()
            {
                Operation = EnumOperation.Add
            });

            Assert.IsFalse(todoResponse.IsSuccess);
            Assert.AreEqual(todoResponse.ErrorMessage, "Cannot perform operation. Title is empty");
        }

        [Test]
        public void Test_Update_Happy_Flow()
        {
            string TASK_TO_RENAME = "some task";
            repo.Setup(p => p.Get(It.IsAny<Func<ITodoItem, bool>>())).Returns<Func<ITodoItem, bool>>(f => new List<ITodoItem>() { new TodoItem() { Title = TASK_TO_RENAME } });

            var todoResponse = processor.Handle(new TodoCommand()
            {
                Operation = EnumOperation.Update,
                Title = TASK_TO_RENAME,
                NewTitle = "another task"
            });

            Assert.IsTrue(todoResponse.IsSuccess);             
        }

        [Test]
        public void Test_Update_Task_Not_Exist()
        {
            string TASK_TO_RENAME = "some task";
            repo.Setup(p => p.Get(It.IsAny<Func<ITodoItem, bool>>())).Returns<Func<ITodoItem, bool>>(f => new List<ITodoItem>());

            var todoResponse = processor.Handle(new TodoCommand()
            {
                Operation = EnumOperation.Update,
                Title = TASK_TO_RENAME,
                NewTitle = "another task"
            });

            Assert.IsFalse(todoResponse.IsSuccess);
        }

        [Test]
        public void Test_Update_New_Task_Title_Empty()
        {
            string TASK_TO_RENAME = "some task";
            repo.Setup(p => p.Get(It.IsAny<Func<ITodoItem, bool>>())).Returns<Func<ITodoItem, bool>>(f => new List<ITodoItem>() { new TodoItem() { Title = TASK_TO_RENAME } });

            var todoResponse = processor.Handle(new TodoCommand()
            {
                Operation = EnumOperation.Update,
                Title = TASK_TO_RENAME,
                NewTitle = null
            });

            Assert.IsFalse(todoResponse.IsSuccess);
        }


        [Test]
        public void Test_Delete_Happy_Flow()
        {
            string TASK_TO_DELETE = "some task";
            
            var todoResponse = processor.Handle(new TodoCommand()
            {
                Operation = EnumOperation.Delete,
                Title = TASK_TO_DELETE                
            });

            Assert.IsTrue(todoResponse.IsSuccess);
        }


        [Test]
        public void Test_Complete_Happy_Flow()
        {
             
            string TASK_TITLE = "some task";
            repo.Setup(p => p.Get(It.IsAny<Func<ITodoItem, bool>>())).Returns<Func<ITodoItem, bool>>(f => new List<ITodoItem>() { new TodoItem() { Title = TASK_TITLE, IsCompleted = false } });

            var todoResponse = processor.Handle(new TodoCommand()
            {
                Operation = EnumOperation.Complete,
                Title = TASK_TITLE                
            });

            Assert.IsTrue(todoResponse.IsSuccess);
        }

        [Test]
        public void Test_Complete_Fail_Already_Completed()
        {

            string TASK_TITLE = "some task";
            repo.Setup(p => p.Get(It.IsAny<Func<ITodoItem, bool>>())).Returns<Func<ITodoItem, bool>>(f => new List<ITodoItem>() { new TodoItem() { Title = TASK_TITLE, IsCompleted = true } });

            var todoResponse = processor.Handle(new TodoCommand()
            {
                Operation = EnumOperation.Complete,
                Title = TASK_TITLE
            });

            Assert.IsFalse(todoResponse.IsSuccess);
        }

        [Test]
        public void Test_Undo_Happy_Flow()
        {

            string TASK_TITLE = "some task";
            repo.Setup(p => p.Get(It.IsAny<Func<ITodoItem, bool>>())).Returns<Func<ITodoItem, bool>>(f => new List<ITodoItem>() { new TodoItem() { Title = TASK_TITLE, IsCompleted = true } });

            var todoResponse = processor.Handle(new TodoCommand()
            {
                Operation = EnumOperation.Undo,
                Title = TASK_TITLE
            });

            Assert.IsTrue(todoResponse.IsSuccess);
        }


        [Test]
        public void Test_Undo_Fail_Already_Completed()
        {

            string TASK_TITLE = "some task";
            repo.Setup(p => p.Get(It.IsAny<Func<ITodoItem, bool>>())).Returns<Func<ITodoItem, bool>>(f => new List<ITodoItem>() { new TodoItem() { Title = TASK_TITLE, IsCompleted = false } });

            var todoResponse = processor.Handle(new TodoCommand()
            {
                Operation = EnumOperation.Undo,
                Title = TASK_TITLE
            });

            Assert.IsFalse(todoResponse.IsSuccess);
        }


    }
}