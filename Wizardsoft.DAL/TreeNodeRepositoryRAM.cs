using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wizardsoft.DAL.Contracts;
using Wizardsoft.DAL.Models;

namespace Wizardsoft.DAL
{
    public class InMemoryTreeNodeRepository : ITreeNodeRepository
    {
        private readonly List<TreeNode> _nodes = new();

        public IEnumerable<TreeNode> GetAll() => _nodes;

        public TreeNode GetById(int id) => _nodes.FirstOrDefault(n => n.Id == id);

        public void Create(TreeNode node)
        {
            node.Id = _nodes.Count + 1;
            _nodes.Add(node);
        }

        public void Update(TreeNode node)
        {
            var existingNode = GetById(node.Id);
            if (existingNode != null)
            {
                existingNode.Name = node.Name;
                existingNode.ParentId = node.ParentId;
            }
        }

        public void Delete(int id)
        {
            var node = GetById(id);
            if (node != null)
            {
                _nodes.Remove(node);
            }
        }
    }

}
