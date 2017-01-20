using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryMethodExample
{
    /* Factories */
    public abstract class VehicleFactory
    {
        public enum Category
        {
            Car, Van
        }

        public enum DrivingStyle
        {
            Economical, Midrange, Powerful
        }

        // Method for static creation
        public static IVehicle Make(Category category, DrivingStyle style, VehicleColor color)
        {
            VehicleFactory factory = null;
            if (category == Category.Car)
            {
                factory = new CarFactory();
            }
            else
            {
                factory = new VanFactory();
            }
            return factory.Build(style, color);
        }

        public virtual IVehicle Build(DrivingStyle style, VehicleColor color)
        {
            IVehicle v = SelectVehicle(style);
            v.Paint(color);
            return v;
        }

        // This is the factory method
        protected internal abstract IVehicle SelectVehicle(DrivingStyle style);
    }

    public class CarFactory : VehicleFactory
    {
        protected internal override IVehicle SelectVehicle(DrivingStyle style)
        {
            if (style == DrivingStyle.Economical)
            {
                return new Saloon();
            }
            else if (style == DrivingStyle.Midrange)
            {
                return new Coupe();
            }
            else
            {
                return new Sport();
            }
        }
    }

    public class VanFactory : VehicleFactory
    {
        protected internal override IVehicle SelectVehicle(DrivingStyle style)
        {
            if ((style == DrivingStyle.Economical) || (style == DrivingStyle.Midrange))
            {
                return new Pickup();
            }
            else
            {
                return new BoxVan();
            }
        }
    }
    /* Vehicles */
    public interface IVehicle
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


    class Program
    {
        // Define an interface for creating an object, but let subclasses decide which class to
        // instantiate.        static void Main(string[] args)
        {
            // Client code
            // I want an economical car, coloured blue...
            VehicleFactory carFactory = new CarFactory();
            IVehicle car = carFactory.Build(VehicleFactory.DrivingStyle.Economical, VehicleColor.Blue);
            Console.WriteLine(car);

            // I am a "white van man"...
            VehicleFactory vanFactory = new VanFactory();
            IVehicle van = vanFactory.Build(VehicleFactory.DrivingStyle.Powerful, VehicleColor.White);
            Console.WriteLine(van);

            // Or using static method
            IVehicle newCar = VehicleFactory.Make(VehicleFactory.Category.Car, VehicleFactory.DrivingStyle.Powerful, VehicleColor.Green);
            Console.WriteLine(newCar);

        }
    }
}
