using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wizardsoft.DAL.Models
{
    public class Node
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Поле /Имя/ обязательно для заполнения.")]
        [StringLength(100, ErrorMessage = "Длина поля /Имя/ не может превышать 100 символов.")]
        public string Name { get; set; }

        //public int? ParentId { get; set; }

        public ICollection<Node> Children { get; private set; } = new List<Node>();

        public Node()
        { 
            Id = Guid.NewGuid();
        }
    }
}
