using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MementoExample
{
    public class Speedometer
    {
        // Normal private visibility but has accessor method...
        private int currentSpeed;
        // internal visibility and no accessor method...
        internal int previousSpeed;
        public Speedometer()
        {
            currentSpeed = 0;
            previousSpeed = 0;
        }
        public virtual int CurrentSpeed
        {
            set
            {
                previousSpeed = currentSpeed;
                currentSpeed = value;
            }
            get
            {
                return currentSpeed;
            }
        }
    }

    public class SpeedometerMemento
    {
        private Speedometer speedometer;
        private int copyOfCurrentSpeed;
        private int copyOfPreviousSpeed;
        public SpeedometerMemento(Speedometer speedometer)
        {
            this.speedometer = speedometer;
            copyOfCurrentSpeed = speedometer.CurrentSpeed;
            copyOfPreviousSpeed = speedometer.previousSpeed;
        }
        public virtual void RestoreState()
        {
            speedometer.CurrentSpeed = copyOfCurrentSpeed;
            speedometer.previousSpeed = copyOfPreviousSpeed;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Speedometer speedo = new Speedometer();
            speedo.CurrentSpeed = 50;
            speedo.CurrentSpeed = 100;
            Console.WriteLine("Current speed: " + speedo.CurrentSpeed);
            Console.WriteLine("Previous speed: " + speedo.previousSpeed);
            // Save the state of 'speedo'...
            SpeedometerMemento memento = new SpeedometerMemento(speedo);
            // Change the state of 'speedo'...
            speedo.CurrentSpeed = 80;
            Console.WriteLine("After setting to 80...");
            Console.WriteLine("Current speed: " + speedo.CurrentSpeed);
            Console.WriteLine("Previous speed: " + speedo.previousSpeed);
            // Restore the state of 'speedo'...
            Console.WriteLine("Now restoring state...");
            memento.RestoreState();
            Console.WriteLine("Current speed: " + speedo.CurrentSpeed);
            Console.WriteLine("Previous speed: " + speedo.previousSpeed);
            Console.Read();
        }
    }
}
