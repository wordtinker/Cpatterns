using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterExample
{
    // In house classes
    interface IEngine
    {
        int Size { get; }
        bool Turbo { get; }
    }

    public abstract class AbstractEngine : IEngine
    {
        private int size;
        private bool turbo;

        public virtual int Size
        {
            get
            {
                return size;
            }
        }

        public virtual bool Turbo
        {
            get
            {
                return turbo;
            }
        }

        public override string ToString()
        {
            return $"{GetType().Name} ({Size})";
        }

        public AbstractEngine(int size, bool turbo)
        {
            this.size = size;
            this.turbo = turbo;
        }
    }

    class StandardEngine : AbstractEngine
    {
        public StandardEngine(int size) : base(size, false) { }
        public StandardEngine(int size, bool turbo) : base(size, false) { }
    }

    class TurboEngine : AbstractEngine
    {
        public TurboEngine(int size) : base(size, true) { }
        public TurboEngine(int size, bool turbo) : base(size, true) { }
    }

    // 3rd party code. Can't change that
    public class SuperGreenEngine // extends object
    {
        // This engine is never turbocharged
        public int EngineSize { get; private set; }

        public SuperGreenEngine(int engineSize)
        {
            EngineSize = engineSize;
        }

        public override string ToString()
        {
            return $"SUPER ENGINE {EngineSize}";
        }
    }

    // Adapter
    public class SuperEngineAdapter : AbstractEngine
    {
        public SuperEngineAdapter(SuperGreenEngine greenEngine) : base(greenEngine.EngineSize, false) { }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IList<IEngine> engines = new List<IEngine>
            {
                new StandardEngine(1300),
                new StandardEngine(2000),
                new TurboEngine(2000),
                new SuperEngineAdapter(new SuperGreenEngine(1100))
            };

            foreach (IEngine engine in engines)
            {
                Console.WriteLine(engine);
            }
        }
    }
}
