using System.ComponentModel.DataAnnotations;
using Wizardsoft.DAL.Models;

namespace Wizardsoft.API.Models
{
    public class CreateNodeRequest
    {
        [Required(ErrorMessage = "Поле /Имя/ обязательно для заполнения.")]
        public string Name { get; set; }

        public List<CreateNodeRequest> Children { get; set; } = new List<CreateNodeRequest>();
    }
}
