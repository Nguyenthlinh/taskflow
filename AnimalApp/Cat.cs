using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalApp
{
     class Cat : Animal
    {
        public Cat(string name) : base(name)
        {
        }
        public override void Speak()
        {
            Console.WriteLine($"{Name} says: Meow!");
        }
        public void Purr()
        {
            Console.WriteLine($"{Name} is purring!");
        }
    }
}
