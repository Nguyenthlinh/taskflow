using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalApp
{
     class Dog : Animal
    {
        public Dog(string name) : base(name)
        {
        }
        public override void Speak()
        {
            Console.WriteLine($"{Name} says: Woof!");
        }
      public void Fetch()
        {
            Console.WriteLine($"{Name} is fetching the ball!");
        }

    }
}
