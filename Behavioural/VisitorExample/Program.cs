using System;

namespace VisitorExample
{
    // Visitor interfaces

    public interface IEngineVisitor
    {
        void Visit(Camshaft camshaft);
        void Visit(IEngine engine);
        void Visit(Piston piston);
        void Visit(SparkPlug sparkPlug);
    }

    public interface IVisitable
    {
        void AcceptEngineVisitor(IEngineVisitor visitor);
    }

    public class EngineDiagnostics : IEngineVisitor
    {
        public virtual void Visit(Camshaft camshaft)
        {
            Console.WriteLine("Diagnosing the camshaft");
        }
        public virtual void Visit(IEngine engine)
        {
            Console.WriteLine("Diagnosing the engine");
        }
        public virtual void Visit(Piston piston)
        {
            Console.WriteLine("Diagnosing the piston");
        }
        public virtual void Visit(SparkPlug sparkPlug)
        {
            Console.WriteLine("Diagnosing a single spark plug");
        }
    }

    // Engine hierarchy

    public interface IEngine : IVisitable
    {
        int Size { get; }
        bool Turbo { get; }
    }

    public abstract class AbstractEngine : IEngine
    {
        private Camshaft camshaft;
        private Piston piston;
        private SparkPlug[] sparkPlugs;

        public AbstractEngine(int size, bool turbo)
        {
            camshaft = new Camshaft();
            piston = new Piston();
            sparkPlugs = new SparkPlug[] { new SparkPlug(), new SparkPlug(), new SparkPlug(), new SparkPlug() };
        }

        public int Size { get; private set; }
        public bool Turbo { get; private set; }
        public void AcceptEngineVisitor(IEngineVisitor visitor)
        {
            // Visit each component first...
            camshaft.AcceptEngineVisitor(visitor);
            piston.AcceptEngineVisitor(visitor);
            foreach (SparkPlug eachSparkPlug in sparkPlugs)
            {
                eachSparkPlug.AcceptEngineVisitor(visitor);
            }
            // Now visit the receiver...
            visitor.Visit(this);
        }
    }

    public class StandardEngine : AbstractEngine
    {
        public StandardEngine(int size) : base(size, false)
        {
           
        }
    }

    public class Camshaft : IVisitable
    {
        public void AcceptEngineVisitor(IEngineVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
    public class Piston : IVisitable
    {
        public void AcceptEngineVisitor(IEngineVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
    public class SparkPlug : IVisitable
    {
        public void AcceptEngineVisitor(IEngineVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create an engine...
            IEngine engine = new StandardEngine(1300);
            // Run diagnostics on the engine...
            engine.AcceptEngineVisitor(new EngineDiagnostics());
        }
    }
}
