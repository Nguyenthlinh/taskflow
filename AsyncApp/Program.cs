
class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine(" đang kết nối đến server...");
        //gọi hàm và đợi kết quả trả về
        List<string> danhSach = await FetchProductsAsync();
        Console.WriteLine("Đã lấy xong! Danh sách ");
        //hiển thị kết quả
        foreach (var item in danhSach)
        {
            Console.WriteLine(item);
        }

    }
    // Hàm phụ nằm ngoài Main nhưng vẫn trong class Program
    static async Task<List<string>> FetchProductsAsync()
    {
        //giả lập việc lấy dữ liệu từ server mất 3 giây
        await Task.Delay(3000);
        //trả về một danh sách sản phẩm
        return new List<string> { "Laptop", "Chuột", "Bàn Phím" };
    }


}