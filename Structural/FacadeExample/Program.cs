using System;

namespace FacadeExample
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
        // Extar methods
        void CleanInterior();
        void CleanExteriorBody();
        void PolishWindows();
        void TakeForTestDrive();

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
        public virtual void CleanInterior()
        {
            Console.WriteLine("Cleaning interior...");
        }
        public virtual void CleanExteriorBody()
        {
            Console.WriteLine("Cleaning exterior body...");
        }
        public virtual void PolishWindows()
        {
            Console.WriteLine("Polishing windows...");
        }
        public virtual void TakeForTestDrive()
        {
            Console.WriteLine("Taking for test drive...");
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

    // Additional classes
    public class Registration
    {
        private IVehicle vehicle;
        public Registration(IVehicle vehicle)
        {
            this.vehicle = vehicle;
        }
        public virtual void AllocateLicensePlate()
        {
            Console.WriteLine("Allocating license plate...");
        }
        public virtual void AllocateVehicleNumber()
        {
            Console.WriteLine("Allocating vehicle number...");
        }
    }

    public class Documentation
    {
        public static void PrintBrochure(IVehicle vehicle)
        {
            Console.WriteLine("Printing brochure...");
        }
    }
    // Facade class

    public class VehicleFacade
    {
        public virtual void PrepareForSale(IVehicle vehicle)
        {
            Registration reg = new Registration(vehicle);
            reg.AllocateVehicleNumber();
            reg.AllocateLicensePlate();
            Documentation.PrintBrochure(vehicle);
            vehicle.CleanInterior();
            vehicle.CleanExteriorBody();
            vehicle.PolishWindows();
            vehicle.TakeForTestDrive();
        }
    }

    /// <summary>
    /// Provide a unified interface to a set of interfaces in a subsystem. Facade defines a higherlevel
    /// interface that makes the subsystem easier to use.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            IVehicle coupe = new Coupe(new StandardEngine(1300));
            VehicleFacade f = new VehicleFacade();
            f.PrepareForSale(coupe);
        }
    }
}
