using System.Text;


using AnimalApp;

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        List<Animal> zoo = new ()
        {
            new Dog("Buddy"),
            new Cat("Whiskers"),
            new Bird("Tweety")
        };
        Console.WriteLine("=== Tất cả động vật lên tiếng ===");
        foreach (var a in zoo)
        {
            a.Speak();  // Polymorphism
        }
        Console.WriteLine("\n=== Tất cả ăn ===");
        foreach (var a in zoo)
        {
            a.Eat();
        }

    }
}
