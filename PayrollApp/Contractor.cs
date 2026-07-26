namespace PayrollApp
{
    class Contractor : IPayable
    {
        public string Name { get; set; }
        public double ProjectFee { get; set; }
        public int ProjectCount { get; set; }

        public double CalculateSalary()
        {
            return ProjectFee * ProjectCount;
        }

        public void PrintPayslip()
        {
            Console.WriteLine($"[Contractor] {Name}: {ProjectCount} project × {ProjectFee:N0} = {CalculateSalary():N0} VNĐ");
        }
    }
}
