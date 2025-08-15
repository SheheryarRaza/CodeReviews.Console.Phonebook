using System.ComponentModel.DataAnnotations;

namespace PhoneBook.SheheryarRaza.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
