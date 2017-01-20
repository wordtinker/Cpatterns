using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractFactoryExample
{
    /*********** Factories ***********/

    /// <summary>
    /// Abstract factory.
    /// </summary>
    interface IBiome
    {
        ICarnivore CreateCarnivore();
        IHerbivore CreateHerbivore();
    }

    /// <summary>
    /// Concrete factory I
    /// </summary>
    class Tundra : IBiome
    {
        public ICarnivore CreateCarnivore()
        {
            return new Wolf();
        }

        public IHerbivore CreateHerbivore()
        {
            return new Goat();
        }
    }

    /// <summary>
    /// Concrete factory II
    /// </summary>
    class Africa : IBiome
    {
        public ICarnivore CreateCarnivore()
        {
            return new Lion();
        }

        public IHerbivore CreateHerbivore()
        {
            return new Elephant(); 
        }
    }

    /*********** Products  ***********/

    /// <summary>
    /// Abstract product I.
    /// </summary>
    interface ICarnivore
    {
        void Eat(IHerbivore herbie);
    }

    /// <summary>
    /// Abstract product II.
    /// </summary>
    interface IHerbivore { }

    // Concrete Products

    class Wolf : ICarnivore
    {
        public void Eat(IHerbivore herbie)
        {
            Console.WriteLine("{0} eats {1}", GetType().Name, herbie.GetType().Name);
        }
    }
    class Goat : IHerbivore { }
    class Lion : ICarnivore
    {
        public void Eat(IHerbivore herbie)
        {
            Console.WriteLine("{0} bites {1}", GetType().Name, herbie.GetType().Name);
        }
    }
    class Elephant : IHerbivore { }

    /*********** Client    ***********/
    class Zoo
    {
        // The client code has no knowledge whatsoever of the concrete type,
        // not needing to include any header files or class declarations related to it.
        // The client code deals only with the abstract type. Objects of a concrete type
        // are indeed created by the factory, but the client code accesses such objects only
        // through their abstract interface.
        // Adding new concrete types is done by modifying the client code to use
        // a different factory, a modification that is typically one line in one file.
        // The different factory then creates objects of a different concrete type,
        // but still returns a pointer of the same abstract type as before —
        // thus insulating the client code from change.This is significantly
        // easier than modifying the client code to instantiate a new type,
        // which would require changing every location in the code where
        // a new object is created(as well as making sure that all such code
        // locations also have knowledge of the new concrete type,
        // by including for instance a concrete class header file).
        // If all factory objects are stored globally in a singleton object,
        // and all client code goes through the singleton to access the proper
        // factory for object creation, then changing factories is as easy as changing the singleton object.

        // note usage of interfaces in that class
        private ICarnivore carnivore;
        private IHerbivore herbivore;

        public Zoo(IBiome factory)
        {
            carnivore = factory.CreateCarnivore();
            herbivore = factory.CreateHerbivore();
        }

        public void FeedAnimals()
        {
            carnivore.Eat(herbivore);
        }
    }
    
    class GenericZoo<TFactory> where TFactory : IBiome, new()
    {
        private ICarnivore carnivore;
        private IHerbivore herbivore;

        public GenericZoo()
        {
            IBiome factory = new TFactory();
            carnivore = factory.CreateCarnivore();
            herbivore = factory.CreateHerbivore();
        }

        public void FeedAnimals()
        {
            carnivore.Eat(herbivore);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            // Create concrete factory
            IBiome biome = new Tundra();
            Zoo firstZoo = new Zoo(biome);
            firstZoo.FeedAnimals();

            // Create concrete factory
            IBiome biome2 = new Africa();
            Zoo secondZoo = new Zoo(biome2);
            secondZoo.FeedAnimals();

            // Same with generic
            GenericZoo<Tundra> genericZoo = new GenericZoo<Tundra>();
            firstZoo.FeedAnimals();

        }
    }
}
