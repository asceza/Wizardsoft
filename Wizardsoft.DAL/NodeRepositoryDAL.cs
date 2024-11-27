using Wizardsoft.DAL.Contracts;
using Wizardsoft.DAL.Models;

namespace Wizardsoft.DAL
{
    public class NodeRepositoryDAL : INodeRepository
    {
        private readonly List<Node> _nodes = new();

        /// <summary>
        /// Получить все узлы
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Node> GetAllNodes() => _nodes;


        /// <summary>
        /// Получить узел по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Node GetById(Guid id)
        {
            return FindNodeById(_nodes, id);
        }


        /// <summary>
        /// Рекурсивный метод для поиска узла по id
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private Node FindNodeById(IEnumerable<Node> nodes, Guid id)
        {
            foreach (var node in nodes)
            {
                if (node.Id == id)
                {
                    return node;
                }
                else
                {
                    // в дочерних узлах
                    var foundNode = FindNodeById(node.Children, id);
                    if (foundNode != null)
                    {
                        return foundNode;
                    }
                }
            }

            // если узел не найден
            return null;
        }
        //public Node GetById(int id) => _nodes.FirstOrDefault(n => n.Id == id);


        /// <summary>
        /// Создать узел
        /// </summary>
        /// <param name="node"></param>
        public Guid Create(Node node)
        {
            _nodes.Add(node);
            return node.Id;
        }


        /// <summary>
        /// Обновить узел
        /// </summary>
        /// <param name="node"></param>
        public Result<Node> Update(Node node)
        {
            var existingNode = GetById(node.Id);
            if (existingNode != null)
            {
                existingNode.Name = node.Name;
                return Result<Node>.Success(node);
            }
            else
            {
                return Result<Node>.Failure("Узел с таким id не найден");
            }
        }


        /// <summary>
        /// Удалить узел по id
        /// </summary>
        /// <param name="id"></param>
        public bool Delete(Guid id)
        {
            var node = GetById(id);
            if (node != null)
            {
                bool isRemoved = _nodes.Remove(node);
                return isRemoved;
            }
            else
            {
                return false;
            }
        }
    }

}
