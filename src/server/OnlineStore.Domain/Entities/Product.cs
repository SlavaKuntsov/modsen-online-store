namespace OnlineStore.Domain.Entities;

public class Product
{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        // public List<ProductImage> Images { get; set; }

        public Product() { }

        public Product(
                string name,
                string description,
                decimal price,
                int stockQuantity,
                Guid categoryId)
        {
                Id = Guid.NewGuid();
                Name = name;
                Description = description;
                Price = price;
                StockQuantity = stockQuantity;
                CategoryId = categoryId;
        }
}