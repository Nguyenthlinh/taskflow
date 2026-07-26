using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalApp
{
    class Bird : Animal
    {
        public Bird(string name) : base(name)
        {
        }
        public override void Speak()
        {
            Console.WriteLine($"{Name} says: Tweet!");
        }
        public override void Eat()
        {
            Console.WriteLine($"{Name} is pecking at seeds.");
        }
    }
}
