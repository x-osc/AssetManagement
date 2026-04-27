namespace AssetManagement.Models
{
    public class Category
    {
            public int Id { get; set; }
            public string Name { get; set; }

            public ICollection<Asset> Assets { get; set; } = new List<Asset>();
    }
    
}
