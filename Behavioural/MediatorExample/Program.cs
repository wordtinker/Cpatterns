using System;

namespace MediatorExample
{
    public class Ignition
    {
        private EngineManagementSystem mediator;
        // Constructor accepts mediator as an argument
        public Ignition(EngineManagementSystem mediator)
        {
            this.mediator = mediator;
            On = false;
            // Register back with the mediator;
            mediator.RegisterIgnition(this);
        }
        public virtual void Start()
        {
            On = true;
            mediator.IgnitionTurnedOn();
            Console.WriteLine("Ignition turned on");
        }
        public virtual void Stop()
        {
            On = false;
            mediator.IgnitionTurnedOff();
            Console.WriteLine("Ignition turned off");
        }
        public virtual bool On { get; private set; }
    }

    public class Gearbox
    {
        private EngineManagementSystem mediator;
        private bool enabled;
        private Gear currentGear;
        // Constructor accepts mediator as an argument
        public Gearbox(EngineManagementSystem mediator)
        {
            this.mediator = mediator;
            enabled = false;
            currentGear = Gear.Neutral;
            mediator.RegisterGearbox(this);
        }
        public virtual void Enable()
        {
            enabled = true;
            mediator.GearboxEnabled();
            Console.WriteLine("Gearbox enabled");
        }
        public virtual void Disable()
        {
            enabled = false;
            mediator.GearboxDisabled();
            Console.WriteLine("Gearbox disabled");
        }
        public virtual bool Enabled
        {
            get
            {
                return enabled;
            }
        }
        public virtual Gear Gear
        {
            set
            {
                if ((Enabled) && (Gear != value))
                {
                    currentGear = value;
                    mediator.GearChanged();
                    Console.WriteLine("Now in " + Gear + " gear");
                }
            }
            get
            {
                return currentGear;
            }
        }
    }

    public enum Gear
    {
        Neutral,
        First,
        Second,
        Third,
        Fourth,
        Fifth,
        Reverse
    }

    // MEdiator
    public class EngineManagementSystem
    {
        private Ignition ignition;
        private Gearbox gearbox;
        private int currentSpeed;
        public EngineManagementSystem()
        {
            currentSpeed = 0;
        }
        // Methods that enable registration with this mediator...
        public virtual void RegisterIgnition(Ignition ignition)
        {
            this.ignition = ignition;
        }
        public virtual void RegisterGearbox(Gearbox gearbox)
        {
            this.gearbox = gearbox;
        }
        // Methods that handle object interactions...
        public virtual void IgnitionTurnedOn()
        {
            gearbox.Enable();
        }
        public virtual void IgnitionTurnedOff()
        {
            gearbox.Disable();
        }
        public virtual void GearboxEnabled()
        {
            Console.WriteLine("EMS now controlling the gearbox");
        }
        public virtual void GearboxDisabled()
        {
            Console.WriteLine("EMS no longer controlling the gearbox");
        }
        public virtual void GearChanged()
        {
            Console.WriteLine("EMS disengaging revs while gear changing");
        }
        public virtual void AcceleratorEnabled()
        {
            Console.WriteLine("EMS now controlling the accelerator");
        }
        public virtual void AcceleratorDisabled()
        {
            Console.WriteLine("EMS no longer controlling the accelerator");
        }
        public virtual void BrakeEnabled()
        {
            Console.WriteLine("EMS now controlling the brake");
        }
        public virtual void BrakeDisabled()
        {
            Console.WriteLine("EMS no longer controlling the brake");
        }
        public virtual void BrakePressed()
        {
            currentSpeed = 0;
        }
        public virtual void BrakeReleased()
        {
            gearbox.Gear = Gear.First;
        }
    }

    /// <summary>
    /// The Mediator pattern helps to solve this through the definition of a separate class (the mediator) that
    /// knows about the individual component classes and takes responsibility for managing their interaction.
    /// The component classes also each know about the mediator class, but this is the only coupling they
    /// have.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            EngineManagementSystem mediator = new EngineManagementSystem();
            Ignition ignition = new Ignition(mediator);
            Gearbox gearbox = new Gearbox(mediator);
            ignition.Start();
            ignition.Stop();
        }
    }
}
