using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyExample
{
    // Car classes
    public interface IEngine
    {
        int Size { get; }
        bool Turbo { get; }
    }

    public class StandardEngine : IEngine
    {
        private int size;
        private bool turbo;

        public StandardEngine(int size)
        {
            this.size = size;
            this.turbo = false;
        }
       
        public int Size
        {
            get
            {
                return size;
            }
        }

        public bool Turbo
        {
            get
            {
                return turbo;
            }
        }
    }

    public interface IVehicle
    {

    }

    public abstract class AbstractVehicle : IVehicle
    {
        protected IEngine engine;

        public AbstractVehicle(IEngine engine)
        {
            this.engine = engine;
        }

        public AbstractVehicle()
        {
            Console.WriteLine($"{GetType().Name} has been created");
        }
    }

    public abstract class AbstractCar : AbstractVehicle
    {
        private IGearboxStrategy gearboxStrategy;

        public AbstractCar(IEngine engine)
        : base(engine)
        {
            // Starts in standard gearbox mode (more economical)
            gearboxStrategy = new StandardGearboxStrategy();
        }
        // Allow the gearbox strategy to be changed...
        public virtual IGearboxStrategy IGearboxStrategy
        {
            set
            {
                gearboxStrategy = value;
            }
            get
            {
                return gearboxStrategy;
            }
        }
        public virtual int Speed
        {
            set
            {
                // Delegate to strategy in effect...
                gearboxStrategy.EnsureCorrectGear(engine, value);
            }
        }
    }

    public class Sport : AbstractCar
    {
        public Sport(IEngine engine) : base(engine)
        {
        }
    }

    // Strategy
    public interface IGearboxStrategy
    {
        void EnsureCorrectGear(IEngine engine, int speed);
    }

    public class StandardGearboxStrategy : IGearboxStrategy
    {
        public virtual void EnsureCorrectGear(IEngine engine, int speed)
        {
            int engineSize = engine.Size;
            bool turbo = engine.Turbo;
            // Some complicated code to determine correct gear
            // setting based on engineSize, turbo & speed, etc.
            // ... omitted ...
            Console.WriteLine($"Working out correct gear at {speed} mph for a STANDARD gearbox");
        }
    }

    public class SportGearboxStrategy : IGearboxStrategy
    {
        public virtual void EnsureCorrectGear(IEngine engine, int speed)
        {
            int engineSize = engine.Size;
            bool turbo = engine.Turbo;
            // Some complicated code to determine correct gear
            // setting based on engineSize, turbo & speed, etc.
            // ... omitted ...
            Console.WriteLine($"Working out correct gear at {speed} mph for a SPORT gearbox");
        }
    }

    /// <summary>
    ///  Define a family of algorithms, encapsulate each one, and make them interchangeable.
    /// Strategy lets the algorithm vary independently from clients that use it.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            AbstractCar myCar = new Sport(new StandardEngine(2000));
            myCar.Speed = 20;
            myCar.Speed = 40;

            Console.WriteLine("Switching on sports mode gearbox...");
            myCar.IGearboxStrategy = new SportGearboxStrategy();
            myCar.Speed = 20;
            myCar.Speed = 40;
        }
    }
}
