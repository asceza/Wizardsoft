using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wizardsoft.DAL.Models;

namespace Wizardsoft.DAL.Contracts
{
    public interface INodeRepository
    {
        public IEnumerable<Node> GetAll();
        public Node GetById(Guid id);
        public Guid Create(Node node);
        public Result<Node> Update(Node node);
        public bool Delete(Guid id);
    }
}
