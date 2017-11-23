using System;

namespace DecoratorExample
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
                return 7000;
            }
        }
    }

    // Decorator hierarchy

    public abstract class AbstractVehicleOption : AbstractVehicle
    {
        protected internal IVehicle decoratedVehicle;

        public AbstractVehicleOption(IVehicle vehicle) : base(vehicle.Engine)
        {
            decoratedVehicle = vehicle;
        }
        public override string ToString()
        {
            return $"{this.GetType().Name} ({Engine}) :: {Price}" + "\r\n" + decoratedVehicle.ToString();
        }
    }
    public class AlloyWheeledVehicle : AbstractVehicleOption
    {
        public AlloyWheeledVehicle(IVehicle vehicle) : base(vehicle)
        {
        }
        public override decimal Price
        {
            get
            {
                return decoratedVehicle.Price + 250M;
            }
        }
    }
    public class AirConditionedVehicle : AbstractVehicleOption
    {
        public AirConditionedVehicle(IVehicle vehicle)
        : base(vehicle)
        {
        }
        public override decimal Price
        {
            get
            {
                return decoratedVehicle.Price + 600;
            }
        }
        public virtual int Temperature
        {
            set
            {
                // code to set the temperature...
            }
        }
    }

    /// <summary>
    /// The Decorator pattern is a good example of preferring object composition over inheritance. Had we
    /// attempted to use inheritance for the various vehicle options we would have needed to create many
    /// different combinations of subclasses to model each combination of selectable options.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            IVehicle saloon = new Saloon(new StandardEngine(1300));
            IVehicle coupe = new Coupe(new StandardEngine(1400));
            saloon = new AirConditionedVehicle(saloon);
            saloon = new AlloyWheeledVehicle(saloon);
            coupe = new AlloyWheeledVehicle(coupe);
            Console.WriteLine(saloon);
            Console.WriteLine();
            Console.WriteLine(coupe);
        }
    }
}
