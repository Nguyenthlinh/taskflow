using ProductApp;

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var manager = new ProductManager();
        manager.AddProduct(new Product(1, "Laptop", 25_000_000, "Electronics"));
        manager.AddProduct(new Product(2, "Mouse", 500_000, "Electronics"));
        manager.AddProduct(new Product(3, "Desk", 3_000_000, "Furniture"));
        manager.AddProduct(new Product(4, "Chair", 2_000_000, "Furniture"));
        manager.AddProduct(new Product(5, "Headset", 1_500_000, "Electronics"));
        // Test exception
        Console.WriteLine("=== TEST EXCEPTION ===");
        try
        {
            var p = manager.GetById(99);
        }
        catch (ProductNotFoundException ex)
        {
            Console.WriteLine($"❌ {ex.Message}");
        }
        // Top 3 đắt nhất
        Console.WriteLine("\n=== TOP 3 ĐẮT NHẤT ===");
        manager.GetTopNExpensive(3).ForEach(p => p.Print());
        // Thống kê
        Console.WriteLine("\n=== THỐNG KÊ THEO CATEGORY ===");
        manager.PrintSumPriceByCategory();

    }
}