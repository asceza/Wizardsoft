using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wizardsoft.DAL.Models
{
    public class Node
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле /Имя/ обязательно для заполнения.")]
        [StringLength(100, ErrorMessage = "Длина поля /Имя/ не может превышать 100 символов.")]
        public string Name { get; set; }

        //public int? ParentId { get; set; }

        public List<Node> Children { get; set; } = new List<Node>();
    }
}
