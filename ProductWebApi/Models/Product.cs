namespace ProductWebApi.Models
{
    public class Product
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
