using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wizardsoft.DAL.Models;

namespace Wizardsoft.DAL.Contracts
{
    public interface ITreeNodeRepository
    {
        IEnumerable<TreeNode> GetAll();
        TreeNode GetById(int id);
        void Create(TreeNode node);
        void Update(TreeNode node);
        void Delete(int id);
    }
}
