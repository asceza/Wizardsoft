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
        public Node GetById(int id);
        public int Create(Node node);
        public Node Update(Node node);
        public void Delete(int id);
    }
}
