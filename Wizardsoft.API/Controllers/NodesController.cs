using Microsoft.AspNetCore.Mvc;
using Wizardsoft.API.Models;
using Wizardsoft.DAL.Contracts;
using Wizardsoft.DAL.Models;

namespace Wizardsoft.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NodesController : ControllerBase
    {
        private readonly INodeRepository _repository;

        public NodesController(INodeRepository repository)
        {
            _repository = repository;
        }


        /// <summary>
        /// Получить все узлы
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Node>> Get() => Ok(_repository.GetAll());


        /// <summary>
        /// Получить узел по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Node> Get(Guid id)
        {
            var node = _repository.GetById(id);
            if (node == null)
            {
                return NotFound();
            }

            return Ok(node);
        }


        /// <summary>
        /// Создать новый узел
        /// </summary>
        /// <param name="inputNode"></param>
        /// <returns>id узла</returns>
        [HttpPost]
        public ActionResult<int> Post([FromBody] CreateNodeRequest inputNode)
        {
            if (inputNode == null)
            {
                return BadRequest("Узел не был создан");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Node node = inputNode.ToNode();
            Guid id = _repository.Create(node);
            return Ok(id);
        }


        /// <summary>
        /// Обновить узел
        /// </summary>
        /// <param name="inputNode"></param>
        /// <returns></returns>
        [HttpPut]
        public ActionResult<NodeResponse> Put([FromBody] Node inputNode)
        {
            if (inputNode == null)
            {
                return BadRequest("Узел не был изменен");
            }
            else
            {
                var result = _repository.Update(inputNode);
                if (result.IsSuccess)
                {
                    return Ok(result.Value.ToNodeResponse());
                }
                else
                {
                    return BadRequest(result.Error);
                }
            }

        }


        /// <summary>
        /// Удалить узел по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult<int> Delete(Guid id)
        {
            bool isDeleted = _repository.Delete(id);
            if (isDeleted)
            {
                return Ok(id);
            }
            else
            {
                return NotFound();
            }
        }
    }

}
