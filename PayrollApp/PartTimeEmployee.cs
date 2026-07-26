namespace PayrollApp
{
    class PartTimeEmployee : IPayable
    {
        public string Name { get; set; }
        public double HourlyRate { get; set; }
        public int HoursWorked { get; set; }

        public double CalculateSalary()
        {
            return HourlyRate * HoursWorked;
        }

        public void PrintPayslip()
        {
            Console.WriteLine($"[Part-time] {Name}: {HoursWorked}h × {HourlyRate:N0} = {CalculateSalary():N0} VNĐ");
        }
    }
}
