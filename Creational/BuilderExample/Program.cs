using System;

namespace BuilderExample
{

    /* Builders */
    // This is an interface which is used to define all the steps to create a product */
    public interface IBuilder
    {
        void BuildPart1();
        void BuildPart2();
        void BuildPart3();
        Product GetProduct();
    }

    // This is a class which implements the Builder interface to create complex product.
    public class ConcreteBuilder : IBuilder
    {
        private Product product = new Product();

        public void BuildPart1()
        {
            product.Part1 = "Part 1";
        }

        public void BuildPart2()
        {
            product.Part2 = "Part 2";
        }

        public void BuildPart3()
        {
            product.Part3 = "Part 3";
        }

        public Product GetProduct()
        {
            return product;
        }
    }

    /* Product */
    public class Product
    {
        public string Part1 { get; set; }
        public string Part2 { get; set; }
        public string Part3 { get; set; }

        public void Show()
        {
            Console.WriteLine(Part1);
            Console.WriteLine(Part2);
            Console.WriteLine(Part3);
        }
    }

    /* Director */
    // This is a class which is used to construct an object using the Builder interface.
    public class Director
    {
        private IBuilder builder;

        public Director(IBuilder builder)
        {
            this.builder = builder;
        }

        public void Construct()
        {
            builder.BuildPart1();
            builder.BuildPart2();
            builder.BuildPart3();
        }
    }

    /* Fluent builder approach */
    class FluentBuilder
    {
        private Product product;

        public FluentBuilder Begin()
        {
            product = new Product();
            // Note this !
            return this;
        }

        public FluentBuilder BuildPart1()
        {
            product.Part1 = "Part 1";
            return this;
        }

        public FluentBuilder BuildPart2()
        {
            product.Part2 = "Part 2";
            return this;
        }

        public FluentBuilder BuildPart3()
        {
            product.Part3 = "Part 3";
            return this;
        }

        public Product Construct()
        {
            return product;
        }

        // implicit cast
        public static implicit operator Product(FluentBuilder fb)
        {
            return fb.product;
        }
    }

    class Program
    {
        /// <summary>
        /// The builder pattern is an object creation software design pattern.
        /// Unlike the abstract factory pattern and the factory method pattern whose
        /// intention is to enable polymorphism, the intention of the builder pattern is
        /// to find a solution to the telescoping constructor anti-pattern[citation needed].
        /// The telescoping constructor anti-pattern occurs when the increase of object
        /// constructor parameter combination leads to an exponential list of
        /// constructors. Instead of using numerous constructors, the builder pattern
        /// uses another object, a builder, that receives each initialization parameter
        /// step by step and then returns the resulting constructed object at once.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // client code
            ConcreteBuilder builder = new ConcreteBuilder();
            Director director = new Director(builder);
            director.Construct();
            Product product = builder.GetProduct();
            product.Show();

            // with fluent builder
            FluentBuilder fb = new FluentBuilder();
            Product prod = fb.Begin().BuildPart1().BuildPart2().BuildPart3().Construct();

            // fluent with cast
            FluentBuilder fb2 = new FluentBuilder();
            Product prod2 = fb2.Begin().BuildPart1().BuildPart2().BuildPart3();


        }
    }
}
