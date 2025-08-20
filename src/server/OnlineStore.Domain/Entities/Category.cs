namespace OnlineStore.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public List<Category> SubCategories { get; set; } = new();
    public List<Product> Products { get; set; } = new();

    public Category() { }

    public Category(string name, Guid? parentCategoryId = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        ParentCategoryId = parentCategoryId;
        SubCategories = new List<Category>();
        Products = new List<Product>();
    }
}
