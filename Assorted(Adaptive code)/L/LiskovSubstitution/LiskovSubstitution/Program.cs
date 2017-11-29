using System;

namespace LiskovSubstitution
{
    public enum RegionInfo
    {
        None,
        CurrentRegion,
        OtherRegion
    }
    public class Size<T>
    {
        public T X { get; set; }
        public T Y { get; set; }
    }
    public interface IShippingStrategy
    {
        decimal FlatRate { get; set; }
        decimal CalculateShippingCost(
            float packageWeightInKilograms,
            Size<float> packageDimensionsInInches,
            RegionInfo destination);
    }
    public class ShippingStrategy : IShippingStrategy
    {
        protected decimal flatRate;
        public ShippingStrategy(decimal flatRate)
        {
            FlatRate = flatRate;
        }
        public virtual decimal FlatRate
        {
            get => flatRate;
            set
            {
                // That keeps data invariant
                if (value <= decimal.Zero)
                    throw new ArgumentOutOfRangeException("value", "Flat rate must be positive and non zero");

                flatRate = value;
            }
        }
        public virtual decimal CalculateShippingCost(
            float packageWeightInKilograms,
            Size<float> packageDimensionsInInches,
            RegionInfo destination)
        {
            // Preconditions
            if (packageWeightInKilograms <= 0f) throw new ArgumentOutOfRangeException(
                "packageWeightInKilograms",
                "Package weight must be positive and nonzero");
            if (packageDimensionsInInches.X <= 0f || packageDimensionsInInches.Y <= 0f)
                throw new ArgumentOutOfRangeException(
                    "packageDimensionsInInches",
            "Package dimensions must be positive and nonzero");

            var shippingCost = FlatRate;
            // STUB calculate shipping cost

            // PostConditions
            if (shippingCost <= decimal.Zero)
                throw new ArgumentOutOfRangeException("return",
                    "The return value is out of range");

            return shippingCost;
        }
    }
    public class WorldWideShippingStrategy : ShippingStrategy
    {
        public WorldWideShippingStrategy(decimal flatRate) : base(flatRate) { }
        public override decimal FlatRate
        {
            get => flatRate;
            set => flatRate = value;
        }
        public override decimal CalculateShippingCost(
            float packageWeightInKilograms,
            Size<float> packageDimensionsInInches,
            RegionInfo destination)
        {
            // Strengthed precondition
            if (destination == RegionInfo.None)
                throw new ArgumentNullException("destination", "Destination must be provided");
            decimal shippingCost = base.CalculateShippingCost(packageWeightInKilograms, packageDimensionsInInches, destination);
            // Weakening the postcondition.
            if (destination == RegionInfo.CurrentRegion)
            {
                shippingCost = decimal.Zero;
            }
            return shippingCost;
        }
    }

    class Program
    {
        /// <summary>
        /// If S is a subtype of T, then objects of type T may be
        /// replaced with objects of type S,
        /// without breaking the program.
        /// Contract rules:
        /// 1. Preconditions cannot be strengthened in a subtype.
        /// 2. Postconditions cannot be weakened in a subtype.
        /// 3. Invariants of the supertype—conditions that must remain true—must be preserved in a subtype.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Client code
            // Parent class is behaving ok
            IShippingStrategy domesticStrategy = new ShippingStrategy(1);
            var cost = domesticStrategy.CalculateShippingCost(10, new Size<float> { X = 1, Y = 1}, RegionInfo.None);
            var currRegionCost = domesticStrategy.CalculateShippingCost(10, new Size<float> { X = 1, Y = 1 }, RegionInfo.CurrentRegion);
            Console.WriteLine("Shipping cost of {0} is {1}", RegionInfo.None, cost);
            Console.WriteLine("Shipping cost of {0} is {1}", RegionInfo.CurrentRegion, currRegionCost);
            // Subclass breaks all the rules of LSP given same call params
            // First rule
            IShippingStrategy wwstrategy = new WorldWideShippingStrategy(1);
            try
            {
                cost = wwstrategy.CalculateShippingCost(10, new Size<float> { X = 1, Y = 1 }, RegionInfo.None);
            }
            catch (Exception)
            {
                Console.WriteLine("LSP first condition is broken");
            }

            // Second rule
            try
            {
                cost = wwstrategy.CalculateShippingCost(10, new Size<float> { X = 1, Y = 1 }, RegionInfo.CurrentRegion);
                Console.WriteLine("Division by shipping cost {0}", 1 / cost );
            }
            catch (Exception)
            {
                Console.WriteLine("LSP second condition is broken. 0 was not expected");
            }
            // Third rule
            try
            {
                // break invariant
                wwstrategy.FlatRate = -1;
                cost = wwstrategy.CalculateShippingCost(10, new Size<float> { X = 1, Y = 1 }, RegionInfo.OtherRegion);
            }
            catch (Exception e)
            {
                Console.WriteLine("LSP third condition is broken.");
                Console.WriteLine(e.Message);
            }
        }
    }
}
