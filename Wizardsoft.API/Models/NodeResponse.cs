using System.ComponentModel.DataAnnotations;
using Wizardsoft.DAL.Models;

namespace Wizardsoft.API.Models
{
    public class NodeResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //public int? ParentId { get; set; }

        public List<Node> Children { get; set; } = new List<Node>();
    }
}
