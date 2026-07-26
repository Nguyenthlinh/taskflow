Console.OutputEncoding = System.Text.Encoding.UTF8;

List<(string Ten, double DiemTB, string XepLoai)> danhSach = new();

while (true)
{
    Console.Write("Tên sinh viên (exit để thoát): ");
    string ten = Console.ReadLine() ?? "";
    if (ten.ToLower() == "exit") break;

    // ??? Nhập điểm 3 môn (tự viết, không cần tách method)

    Console.Write("Điểm Toán: ");
    double toan = double.Parse(Console.ReadLine() ?? "0");
    //            ↑ chuyển string → số thập phân
    //                              ↑ nếu null thì dùng "0" thay thế
    Console.Write("Điểm Lý: ");
    double ly = double.Parse(Console.ReadLine() ?? "0");
    Console.Write("Điểm Hóa: ");
    double hoa = double.Parse(Console.ReadLine() ?? "0");


    // ??? Tính điểm TB
    double diemTB = (toan + ly + hoa) / 3;

    // ??? Xếp loại bằng if/else
    string xepLoai;
    
     if (diemTB >= 8.5)
    {
        xepLoai = "Giỏi";
    }
    else if (diemTB >= 7.0)
    {
        xepLoai = "Khá";
    }
    else if (diemTB >= 5.0)
    {
        xepLoai = "Trung bình";
    }
    else
    {
        xepLoai = "Yếu";
    }

    // ??? In kết quả ra màn hình
    Console.WriteLine($"Sinh viên: {ten}, Điểm TB: {diemTB:F2}, Xếp loại: {xepLoai}");

    // ??? Add vào danhSach
    danhSach.Add((ten, diemTB, xepLoai));
}

// ??? Sort và in bảng xếp hạng


// Nếu không nhập ai mà gõ exit ngay → tránh in bảng rỗng
if (danhSach.Count == 0)
{
    Console.WriteLine("Chưa có sinh viên nào!");
}
else
{
    var sorted = danhSach.OrderByDescending(sv => sv.DiemTB).ToList();
    int rank = 1;
    foreach (var sv in sorted)
    {
        Console.WriteLine($"#{rank} {sv.Ten} - Điểm TB: {sv.DiemTB:F2}, Xếp loại: {sv.XepLoai}");
        rank++;
    }
}
Console.WriteLine("\nNhấn Enter để đóng...");
Console.ReadLine();
