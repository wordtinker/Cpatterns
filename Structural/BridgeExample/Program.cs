using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeExample
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

    // Controls hierarchy
    public class AbstractDriverControls
    {
        private IEngine engine;

        public AbstractDriverControls(IEngine engine)
        {
            this.engine = engine;
        }

        public virtual void IgnitionOn()
        {
            engine.Start();
        }

        public virtual void IgnitionOff()
        {
            engine.Stop();
        }

        public virtual void Accelerate()
        {
            engine.IncreasePower();
        }

        public virtual void Brake()
        {
            engine.DecreasePower();
        }
    }

    public class StandardControls : AbstractDriverControls
    {
        public StandardControls(IEngine engine) : base(engine) { }
        // No extra functions
    }

    public class SportControls : AbstractDriverControls
    {
        public SportControls(IEngine engine) : base(engine) { }
        public virtual void AccelerateHard()
        {
            // class sees no engine here, just "one tower of the bridge"
            Accelerate();
            Accelerate();
        }
    }
    class Program
    {
        // The Bridge pattern addresses this requirement by separating the 'abstraction' from the
        // 'implementation' into two separate but connected hierarchies such that each can vary independently of
        // the other.
        static void Main(string[] args)
        {
            // 
            IEngine engine = new StandardEngine(1300);
            StandardControls controls1 = new StandardControls(engine);
            controls1.IgnitionOn();
            controls1.Accelerate();
            controls1.Brake();
            controls1.IgnitionOff();
            // Now use sport controls
            SportControls controls2 = new SportControls(engine);
            controls2.IgnitionOn();
            controls2.Accelerate();
            controls2.AccelerateHard();
            controls2.Brake();
            controls2.IgnitionOff();
        }
    }
}
