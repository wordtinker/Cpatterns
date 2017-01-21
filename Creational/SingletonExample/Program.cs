
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
        }
    }
}
