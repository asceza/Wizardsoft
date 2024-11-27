using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wizardsoft.DAL.Models
{
    public class Node
    {
        public Guid Id { get; private set; }

        [Required(ErrorMessage = "Поле /Имя/ обязательно для заполнения.")]
        [StringLength(100, ErrorMessage = "Длина поля /Имя/ не может превышать 100 символов.")]
        public string Name { get; set; }

        //public int? ParentId { get; set; }

        public ICollection<Node> Children { get; private set; } = new List<Node>();

        public Node()
        { 
            Id = Guid.NewGuid();
        }

        public Node(string name, ICollection<Node> children)
        {
            Id = Guid.NewGuid();
            Name = name;
            Children = children;
        }

        // конструктор для тестирования
        public Node(Guid id, string name, ICollection<Node> children)
        {
            Id = id;
            Name = name;
            Children = children;
        }
    }
}
