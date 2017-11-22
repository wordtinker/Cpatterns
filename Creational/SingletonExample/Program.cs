
namespace SingletonExample
{
    public sealed class Singleton
    {
        private static readonly Singleton instance = new Singleton();

        private Singleton() { }

        public static Singleton Instance
        {
            get
            {
                return instance;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // In software engineering, the singleton pattern is a software design pattern
            // that restricts the instantiation of a class to one object. This is useful
            // when exactly one object is needed to coordinate actions across the system.
            // The concept is sometimes generalized to systems that operate more efficiently
            // when only one object exists, or that restrict the instantiation to a certain number of objects.
        }
    }
}
