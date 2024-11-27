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

            // ��������� ����
            _mockRepository.Setup(repo => repo.GetAllNodes()).Returns(nodes);

            // Act
            var result = _controller.Get();

            // Assert
            var okResult = result.Result as OkObjectResult; // ��� 200
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOf<IEnumerable<Node>>(okResult.Value);

            // ���������� �����
            Assert.AreEqual(nodes.Count, ((IEnumerable<Node>)okResult.Value).Count());
        }

        [Test]
        public void Get_WithExistingId_ReturnsNode()
        {
            // Arrange
            var id = Guid.NewGuid();
            var node = new Node(id, "Node1", new List<Node>());

            // ��������� ���� ��� �������� ���� ��� ������� �� id
            _mockRepository.Setup(repo => repo.GetById(id)).Returns(node);

            // Act
            var result = _controller.Get(id);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(node, okResult.Value);
        }

        [Test] // ������������ ������ Get �� ��������������� Id
        public void Get_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            // ��������� ���� �� ������� null ��� ������� �� ��������������� id
            _mockRepository.Setup(repo => repo.GetById(id)).Returns((Node)null);

            // Act
            var result = _controller.Get(id);

            // Assert
            // ��������, ��� ��������� �������� NotFoundResult
            Assert.IsInstanceOf<ActionResult<Node>>(result);
            var notFoundResult = result.Result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
        }

        [Test] // ������������ ������ Post � �������������� �����
        public void Post_ValidNode_ReturnsId()
        {
            // Arrange
            var inputNode = new CreateNodeRequest { Name = "Node1" };
            var id = Guid.NewGuid();
            var node = new Node(id, "Node1", null);

            // ��������� ����, ����� ���������� id ���� ��� ��� ��������
            _mockRepository.Setup(repo => repo.Create(It.IsAny<Node>())).Returns(node.Id);

            // Act
            var result = _controller.Post(inputNode);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(node.Id, okResult.Value);
        }

        [Test] // ������������ ������ Post � ���������������� �������
        public void Post_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            // ���������������� ������ ��� ����� (������������ ����)
            var inputNode = new CreateNodeRequest();

            // ���������� ������ � ������ ��� ��������
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = _controller.Post(inputNode);

            // Assert
            // ��������, ��� ��������� �������� BadRequestObjectResult
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test] // ������������ ������ Put � ������������ �����
        public void Put_ExistingNode_ReturnsUpdatedNode()
        {
            // Arrange
            var id = Guid.NewGuid();
            var node = new Node(id, "Node1", new List<Node>());

            // ��������� ���� ��� �������� ������������ ����
            _mockRepository.Setup(repo => repo.Update(node)).Returns(Result<Node>.Success(node));

            // Act
            var result = _controller.Put(node);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            // ������������ ������ -> �� �������
            //Assert.AreEqual(node.ToNodeResponse(), okResult.Value);

            Assert.AreEqual(node.ToNodeResponse().Id, ((NodeResponse)okResult.Value).Id);
            Assert.AreEqual(node.ToNodeResponse().Name, ((NodeResponse)okResult.Value).Name);
        }

        [Test] // ������������ ������ Put � �������������� �����
        public void Put_NonExistingNode_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var node = new Node(id, "Node1", null);
            _mockRepository.Setup(repo => repo.Update(node)).Returns(Result<Node>.Failure("���� � ����� id �� ������")); // ��������� ���� ��� �������� ������

            // Act
            var result = _controller.Put(node); // ����� ������ Put � �����, ������� �� ����������

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult; // ���������� ���������� � BadRequestObjectResult
            Assert.IsNotNull(badRequestResult); // ��������, ��� ��������� �� ����� null
            Assert.AreEqual("���� � ����� id �� ������", badRequestResult.Value); // ��������, ��� ������ ��������� � ���������
        }

        [Test] // ������������ ������ Delete � ������������ �����
        public void Delete_ExistingNode_ReturnsId()
        {
            // Arrange
            var id = Guid.NewGuid();

            // ��������� ���� �� �������� ��������
            _mockRepository.Setup(repo => repo.Delete(id)).Returns(true);

            // Act
            var result = _controller.Delete(id);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(id, okResult.Value);
        }

        [Test] // ������������ ������ Delete � �������������� �����
        public void Delete_NonExistingNode_ReturnsNotFound()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            // ��������� ���� �� ���������� ��������
            _mockRepository.Setup(repo => repo.Delete(id)).Returns(false);

            // Act
            var result = _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
    }
}