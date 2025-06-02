using System.ComponentModel.DataAnnotations;

namespace Library.Model
{
    public class Food
    {
        [Required]
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }
}
