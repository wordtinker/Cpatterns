using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeExample
{
    /* Vehicles */
    public interface IVehicle : ICloneable
    {
        VehicleColor Color { get; }
        void Paint(VehicleColor color);
    }

    public abstract class AbstractVehicle : IVehicle
    {
        public VehicleColor Color { get; private set; }
        public void Paint(VehicleColor color)
        {
            Color = color;
        }

        public override string ToString()
        {
            return $"{GetType().Name}::Color->{Color}";
        }

        public object Clone()
        {
            // It's easier to clone that using prototype 
            // than use ctor.
            Console.WriteLine($"{GetType().Name} has been cloned");
            return this.MemberwiseClone();
        }

        public AbstractVehicle()
        {
            Console.WriteLine($"{GetType().Name} has been created");
            // Lots of time/resouce consuming stuff here.
        }
    }

    public abstract class AbstractVan : AbstractVehicle { }
    public abstract class AbstractCar : AbstractVehicle { }

    public class Saloon : AbstractCar { }
    public class Coupe : AbstractCar { }
    public class Sport : AbstractCar { }

    public class Pickup : AbstractVan { }
    public class BoxVan : AbstractVan { }

    public enum VehicleColor
    {
        Unpainted, Blue, Black, Green,
        Red, Silver, White, Yellow
    }

    /* Dispatcher of prototypes */
    // If there are several prototypes in the system.

    public class PrototypeDispatcher
    {
        // note Lazy init here
        private Lazy<IVehicle> saloon, coupe, sport, pickup, boxVan;

        public PrototypeDispatcher()
        {
            saloon = new Lazy<IVehicle>(() => new Saloon());
            coupe = new Lazy<IVehicle>(() => new Coupe());
            sport = new Lazy<IVehicle>(() => new Sport());
            pickup = new Lazy<IVehicle>(() => new Pickup());
            boxVan = new Lazy<IVehicle>(() => new BoxVan());
        }

        public IVehicle CreateSaloon()
        {
            return (IVehicle)saloon.Value.Clone();
        }

        public IVehicle CreateCoupe()
        {
            return (IVehicle)coupe.Value.Clone();
        }

        public IVehicle CreateSport()
        {
            return (IVehicle)sport.Value.Clone();
        }

        public IVehicle CreatePickup()
        {
            return (IVehicle)pickup.Value.Clone();
        }

        public IVehicle CreateBoxVan()
        {
            return (IVehicle)boxVan.Value.Clone();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PrototypeDispatcher manager = new PrototypeDispatcher();
            IVehicle saloon1 = manager.CreateSaloon();
            IVehicle saloon2 = manager.CreateSaloon();
            IVehicle pickup = manager.CreatePickup();
        }
    }
}
