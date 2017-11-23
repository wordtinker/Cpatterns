using System;
using System.Collections.Generic;
using System.Threading;

namespace FlyWeightExample
{
    // Engine hierarchy
    public interface IEngine
    {
        int Size { get; }
        bool Turbo { get; }

        void Start();
        void Stop();
        void IncreasePower();
        void DecreasePower();

        // Methods having extrinsic (i.e. unshared) state
        void Diagnose(IDiagnosticTool tool);

    }
    public abstract class AbstractEngine : IEngine
    {
        private int size;
        private bool turbo;
        private bool running;
        private int power;

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

        public virtual void Start()
        {
            running = true;
        }

        public virtual void Stop()
        {
            running = false;
            power = 0;
        }

        public virtual void IncreasePower()
        {
            if (running && power < 10) power++;
        }

        public virtual void DecreasePower()
        {
            if (running && power > 0) power--;
        }

        public override string ToString()
        {
            return $"{GetType().Name} ({Size})";
        }

        public AbstractEngine(int size, bool turbo)
        {
            this.size = size;
            this.turbo = turbo;
            running = false;
            power = 0;
        }

        public virtual void Diagnose(IDiagnosticTool tool)
        {
            tool.RunDiagnosis(this);
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

    // Vehicle hierarchy
    public interface IVehicle
    {
        IEngine Engine { get; }
        decimal Price { get; }
    }
    public abstract class AbstractVehicle : IVehicle
    {
        private IEngine engine;

        public AbstractVehicle(IEngine engine)
        {
            this.engine = engine;
        }

        public IEngine Engine
        {
            get
            {
                return engine;
            }
        }

        public abstract decimal Price { get; }
        public override string ToString()
        {
            return $"{this.GetType().Name} ({Engine}) :: {Price}";
        }
    }
    public class Saloon : AbstractVehicle
    {
        public Saloon(IEngine engine) : base(engine) { }

        public override decimal Price
        {
            get
            {
                return 6000M;
            }
        }
    }
    public class Coupe : AbstractVehicle
    {
        public Coupe(IEngine engine) : base(engine) { }
        public override decimal Price
        {
            get
            {
                return 7000M;
            }
        }
    }

    // Flyweight pattern
    public interface IDiagnosticTool
    {
        void RunDiagnosis(object obj);
    }

    public class EngineDiagnosticTool : IDiagnosticTool
    {
        public virtual void RunDiagnosis(object obj)
        {
            Console.WriteLine("Starting engine diagnostic tool for " + obj);
            Thread.Sleep(5000);
            Console.WriteLine("Engine diagnosis complete");
        }
    }

    public class EngineFlyweightFactory
    {
        private IDictionary<int?, IEngine> standardEnginePool;
        private IDictionary<int?, IEngine> turboEnginePool;
        public EngineFlyweightFactory()
        {
            standardEnginePool = new Dictionary<int?, IEngine>();
            turboEnginePool = new Dictionary<int?, IEngine>();
        }
        public virtual IEngine GetStandardEngine(int size)
        {
            IEngine e = null;
            bool found = standardEnginePool.TryGetValue(size, out e);
            if (!found)
            {
                e = new StandardEngine(size);
                standardEnginePool[size] = e;
            }
            return e;
        }
        public virtual IEngine GetTurboEngine(int size)
        {
            IEngine e = null;
            bool found = turboEnginePool.TryGetValue(size, out e);
            if (!found)
            {
                e = new TurboEngine(size);
                turboEnginePool[size] = e;
            }
            return e;
        }
    }


    /// <summary>
    /// The Flyweight pattern allows you to reference a multitude of objects of the same type and having the
    /// same state, but only by instantiating the minimum number of actual objects needed.This is typically
    /// done by allocating a 'pool' of objects which can be shared, and this is determined by a 'flyweight
    /// factory' class. Client programs get access to engines only through the factory
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Create the flyweight factory...
            EngineFlyweightFactory factory = new EngineFlyweightFactory();
            // Create the diagnostic tool
            IDiagnosticTool tool = new EngineDiagnosticTool();
            // Get the flyweights and run diagnostics on them
            IEngine standard1 = factory.GetStandardEngine(1300);
            standard1.Diagnose(tool);
            IEngine standard2 = factory.GetStandardEngine(1300);
            standard2.Diagnose(tool);
            IEngine standard3 = factory.GetStandardEngine(1300);
            standard3.Diagnose(tool);
            IEngine standard4 = factory.GetStandardEngine(1600);
            standard4.Diagnose(tool);
            IEngine standard5 = factory.GetStandardEngine(1600);
            standard5.Diagnose(tool);
            // Show that objects are shared
            Console.WriteLine(standard1.GetHashCode());
            Console.WriteLine(standard2.GetHashCode());
            Console.WriteLine(standard3.GetHashCode());
            Console.WriteLine(standard4.GetHashCode());
            Console.WriteLine(standard5.GetHashCode());

        }
    }
}
