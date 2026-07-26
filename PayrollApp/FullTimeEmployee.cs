namespace PayrollApp
{
    class FullTimeEmployee : IPayable
    {
        public string Name { get; set; }
        public double MonthlySalary { get; set; }

        public double CalculateSalary()
        {
            return MonthlySalary;
        }

        public void PrintPayslip()
        {
            Console.WriteLine($"[Full-time] {Name}: {CalculateSalary():N0} VNĐ/tháng");
        }
    }
}
