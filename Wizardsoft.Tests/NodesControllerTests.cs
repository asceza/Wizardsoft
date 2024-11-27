using NUnit.Framework;
using Moq;
using Wizardsoft.API.Controllers;
using Wizardsoft.DAL.Contracts;
using Wizardsoft.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Wizardsoft.API.Models;
using Wizardsoft.DAL;
using System.Xml.Linq;
using NUnit.Framework.Interfaces;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Http;

namespace Wizardsoft.Tests
{
    [TestFixture]
    public class NodesControllerTests
    {
        private NodesController _controller;
        private Mock<INodeRepository> _mockRepository;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<INodeRepository>();
            _controller = new NodesController(_mockRepository.Object);
        }

        [Test]
        public void Get_ReturnsAllNodes()
        {
            // Arrange
            var nodes = new List<Node>
            {
                new Node("Node1", new List<Node>()),
                new Node("Node2", new List<Node> {new Node("Node22", null)}),
                new Node("Node3", new List<Node> {new Node("Node33", null)}),
            };

            // Настройка мока
            _mockRepository.Setup(repo => repo.GetAllNodes()).Returns(nodes);

            // Act
            var result = _controller.Get();

            // Assert
            var okResult = result.Result as OkObjectResult; // код 200
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOf<IEnumerable<Node>>(okResult.Value);

            // количество узлов
            Assert.AreEqual(nodes.Count, ((IEnumerable<Node>)okResult.Value).Count());
        }

        [Test]
        public void Get_WithExistingId_ReturnsNode()
        {
            // Arrange
            var id = Guid.NewGuid();
            var node = new Node(id, "Node1", new List<Node>());

            // Настройка мока для возврата узла при запросе по id
            _mockRepository.Setup(repo => repo.GetById(id)).Returns(node);

            // Act
            var result = _controller.Get(id);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(node, okResult.Value);
        }

        [Test] // Тестирование метода Get по несуществующему Id
        public void Get_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Настройка мока на возврат null при запросе по несуществующему id
            _mockRepository.Setup(repo => repo.GetById(id)).Returns((Node)null);

            // Act
            var result = _controller.Get(id);

            // Assert
            // Проверка, что результат является NotFoundResult
            Assert.IsInstanceOf<ActionResult<Node>>(result);
            var notFoundResult = result.Result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
        }

        [Test] // Тестирование метода Post с действительным узлом
        public void Post_ValidNode_ReturnsId()
        {
            // Arrange
            var inputNode = new CreateNodeRequest { Name = "Node1" };
            var id = Guid.NewGuid();
            var node = new Node(id, "Node1", null);

            // Настройка мока, чтобы возвращать id узла при его создании
            _mockRepository.Setup(repo => repo.Create(It.IsAny<Node>())).Returns(node.Id);

            // Act
            var result = _controller.Post(inputNode);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(node.Id, okResult.Value);
        }

        [Test] // Тестирование метода Post с недействительной моделью
        public void Post_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            // Недействительный запрос без имени (обязательное поле)
            var inputNode = new CreateNodeRequest();

            // Добавление ошибки в модель для проверки
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = _controller.Post(inputNode);

            // Assert
            // Проверка, что результат является BadRequestObjectResult
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test] // Тестирование метода Put с существующим узлом
        public void Put_ExistingNode_ReturnsUpdatedNode()
        {
            // Arrange
            var id = Guid.NewGuid();
            var node = new Node(id, "Node1", new List<Node>());

            // Настройка мока для возврата обновленного узла
            _mockRepository.Setup(repo => repo.Update(node)).Returns(Result<Node>.Success(node));

            // Act
            var result = _controller.Put(node);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            // Сравниваются ссылки -> не годится
            //Assert.AreEqual(node.ToNodeResponse(), okResult.Value);

            Assert.AreEqual(node.ToNodeResponse().Id, ((NodeResponse)okResult.Value).Id);
            Assert.AreEqual(node.ToNodeResponse().Name, ((NodeResponse)okResult.Value).Name);
        }

        [Test] // Тестирование метода Put с несуществующим узлом
        public void Put_NonExistingNode_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var node = new Node(id, "Node1", null);
            _mockRepository.Setup(repo => repo.Update(node)).Returns(Result<Node>.Failure("Узел с таким id не найден")); // Настройка мока для возврата ошибки

            // Act
            var result = _controller.Put(node); // Вызов метода Put с узлом, который не существует

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult; // Приведение результата к BadRequestObjectResult
            Assert.IsNotNull(badRequestResult); // Проверка, что результат не равен null
            Assert.AreEqual("Узел с таким id не найден", badRequestResult.Value); // Проверка, что ошибка совпадает с ожидаемой
        }

        [Test] // Тестирование метода Delete с существующим узлом
        public void Delete_ExistingNode_ReturnsId()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Настройка мока на успешное удаление
            _mockRepository.Setup(repo => repo.Delete(id)).Returns(true);

            // Act
            var result = _controller.Delete(id);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(id, okResult.Value);
        }

        [Test] // Тестирование метода Delete с несуществующим узлом
        public void Delete_NonExistingNode_ReturnsNotFound()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            // Настройка мока на неуспешное удаление
            _mockRepository.Setup(repo => repo.Delete(id)).Returns(false);

            // Act
            var result = _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
    }
}