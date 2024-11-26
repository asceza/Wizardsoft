using Wizardsoft.DAL.Models;

namespace Wizardsoft.API.Models
{
    public class CreateNodeRequest
    {
        public string Name { get; set; }
        //public int? ParentId { get; set; }
        public List<CreateNodeRequest> Children { get; set; } = new List<CreateNodeRequest>();
    }
}
