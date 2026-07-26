
using System.Text;
using PayrollApp;
public  class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        List<IPayable> employees = new List<IPayable>
        {
            new FullTimeEmployee { Name = "Alice", MonthlySalary = 15000000 },
            new PartTimeEmployee { Name = "Bob", HourlyRate = 100000, HoursWorked = 80 },
            new Contractor { Name = "Charlie", ProjectFee = 5000000, ProjectCount = 3 }

        };
        Console.WriteLine("=== BẢNG LƯƠNG THÁNG 5/2025 ===");
        foreach (var emp in employees)
        {
            emp.PrintPayslip();
        }
        double totalPayroll = employees.Sum(e => e.CalculateSalary());
        Console.WriteLine($"\nTổng chi phí lương: {totalPayroll:N0} VNĐ");
    }
}
