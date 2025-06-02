using System.ComponentModel.DataAnnotations;

namespace Library.Model
{
    public class Order
    {
        public virtual List<Account> Accounts { get; set; } = new List<Account>();
        public virtual List<Food> Foods { get; set; } = new List<Food>();
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
        [Required]
        public Guid AccountID { get; set; }
        [Required]
        public Guid FoodID { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public int Amount => Foods.Sum(f => f.Quantity);
        public decimal TotalCost => Foods.Sum(f => f.Price * f.Quantity);
        public string Status { get; set; } // e.g., "Pending", "Completed", "Cancelled"
    }
}
