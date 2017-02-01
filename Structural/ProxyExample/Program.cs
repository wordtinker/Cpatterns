using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxyExample
{
    // Engine hierarchy
    public interface IEngine
    {
        int Size { get; }
        bool Turbo { get; }

        // Methods having extrinsic (i.e. unshared) state
        void Diagnose(IDiagnosticTool tool);
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

        public virtual void Diagnose(IDiagnosticTool tool)
        {
            tool.RunDiagnosis(this);
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

    //
    public interface IDiagnosticTool
    {
        void RunDiagnosis(object obj);
    }
    public class EngineDiagnosticTool : IDiagnosticTool
    {
        public virtual void RunDiagnosis(object obj)
        {
            Console.WriteLine("Starting engine diagnostic tool for " + obj);
            Console.WriteLine($"Running on: {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(5000);
            Console.WriteLine("Engine diagnosis complete");
        }
    }

    // 
    public class EngineProxy : IEngine
    {
        private IEngine engine;
        public EngineProxy(int size, bool turbo)
        {
            if (turbo)
            {
                engine = new TurboEngine(size);
            }
            else
            {
                engine = new StandardEngine(size);
            }
        }
        public virtual int Size
        {
            get
            {
                return engine.Size;
            }
        }
        public virtual bool Turbo
        {
            get
            {
                return engine.Turbo;
            }
        }
        // TODO
        // This method is time-consuming...
        public virtual async void Diagnose(IDiagnosticTool tool)
        {
            Console.WriteLine("(Running tool on proxy)");
            await Task.Run(() =>
            {
                tool.RunDiagnosis(this);
            });
            Console.WriteLine("EngineProxy diagnose() method finished");
        }
    }

    /// <summary>
    /// Some methods can be time-consuming, such as those that load complex graphical components or need
    /// network connections.In these instances, the Proxy pattern provides a 'stand-in' object until such time
    /// that the time-consuming resource is complete, allowing the rest of your application to load.    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Running on: {Thread.CurrentThread.ManagedThreadId}");
            IEngine engine = new StandardEngine(1300);
            IDiagnosticTool tool = new EngineDiagnosticTool();
            engine.Diagnose(tool);
            Console.ReadLine();

            // Running on separate thread
            IEngine proxy = new EngineProxy(1300, false);
            proxy.Diagnose(tool);
            Console.ReadLine();
        }
    }
}
