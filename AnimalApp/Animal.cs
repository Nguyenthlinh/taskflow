using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalApp
{
    abstract class Animal
    {
        public string Name { get; set; }
        public Animal(string name)
        {
            Name = name;
            
        }
        public abstract void Speak();
        virtual public void Eat()
        {
            Console.WriteLine($"{Name} is eating");
        }
    }
}
