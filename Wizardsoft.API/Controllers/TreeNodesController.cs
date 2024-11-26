using Microsoft.AspNetCore.Mvc;
using Wizardsoft.DAL.Contracts;
using Wizardsoft.DAL.Models;

namespace Wizardsoft.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TreeNodesController : ControllerBase
    {
        private readonly ITreeNodeRepository _repository;

        public TreeNodesController(ITreeNodeRepository repository)
        {
            _repository = repository;
        }


        /// <summary>
        /// Получить все узлы
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<TreeNode>> Get() => Ok(_repository.GetAll());


        /// <summary>
        /// Получить узел по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<TreeNode> Get(int id)
        {
            var node = _repository.GetById(id);
            if (node == null) return NotFound();
            return Ok(node);
        }


        /// <summary>
        /// Создать новый узел
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<TreeNode> Post([FromBody] TreeNode node)
        {
            if (node == null)
            {
                return BadRequest("Node cannot be null.");
            }

            // Дополнительная проверка на обязательные поля
            if (string.IsNullOrEmpty(node.Name))
            {
                return BadRequest("The Name field is required.");
            }

            _repository.Create(node);
            return CreatedAtAction(nameof(Get), new { id = node.Id }, node);
        }



        /// <summary>
        /// Обновить узел
        /// </summary>
        /// <param name="id"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] TreeNode node)
        {
            if (id != node.Id) return BadRequest();

            _repository.Update(node);
            return NoContent();
        }


        /// <summary>
        /// Удалить узел по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _repository.Delete(id);
            return NoContent();
        }
    }

}
