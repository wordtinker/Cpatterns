using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeExample
{
    public abstract class Item
    {
        public virtual string Description { get; private set; }
        public virtual int Cost { get; private set; }

        public Item(string description, int cost)
        {
            Description = description;
            Cost = cost;
        }

        public abstract void AddItem(Item item);
        public abstract void RemoveItem(Item item);
        public abstract Item[] Items { get; }

        public override string ToString()
        {
            return $"{Description} cost: {Cost}";
        }
    }

    public class Part : Item
    {
        public Part(string description, int cost) : base(description, cost) { }
        // Empty implementations for unit parts...
        public override void AddItem(Item item) { }
        public override void RemoveItem(Item item) { }
        public override Item[] Items
        {
            get
            {
                return new Item[0];
            }
        }
    }

    public class Assembly : Item
    {
        private IList<Item> items = new List<Item>();

        public Assembly(string description) : base(description, 0) { }

        public override Item[] Items
        {
            get
            {
                return items.ToArray();
            }
        }

        public override void AddItem(Item item)
        {
            items.Add(item);
        }

        public override void RemoveItem(Item item)
        {
            items.Remove(item);
        }

        // Also have to override getCost() to accumulate cost of all items in list
        public override int Cost
        {
            get
            {
                return items.Sum(item => item.Cost);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Item nut = new Part("Nut", 5);
            Item bolt = new Part("Bolt", 9);
            Item panel = new Part("Panel", 35);
            Item gizmo = new Assembly("Gizmo");
            gizmo.AddItem(panel);
            gizmo.AddItem(nut);
            gizmo.AddItem(bolt);

            Item widget = new Assembly("Widget");
            widget.AddItem(gizmo);
            widget.AddItem(nut);

            Console.WriteLine(nut);
            Console.WriteLine(bolt);
            Console.WriteLine(panel);
            Console.WriteLine(gizmo);
            Console.WriteLine(widget);
        }
    }
}
