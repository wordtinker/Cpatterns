using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMethodExample
{

    public abstract class AbstractBookletPrinter
    {
        protected internal abstract int PageCount { get; }
        protected internal abstract void PrintFrontCover();
        protected internal abstract void PrintToC();
        protected internal abstract void PrintPage(int pageNumber);
        protected internal abstract void PrintBackCover();

        // This is the 'template method'
        public void Print()
        {
            PrintFrontCover();
            PrintToC();
            for (int i = 1; i <= PageCount; i++)
            {
                PrintPage(i);
            }
            PrintBackCover();
        }
    }

    public class SaloonBooklet : AbstractBookletPrinter
    {
        protected internal override int PageCount
        {
            get
            {
                return 100;
            }
        }
        protected internal override void PrintFrontCover()
        {
            Console.WriteLine("Printing front cover for Saloon car booklet");
        }
        protected internal override void PrintToC()
        {
            Console.WriteLine("Printing table of contents for Saloon car booklet");
        }
        protected internal override void PrintPage(int pageNumber)
        {
            Console.WriteLine("Printing page " + pageNumber + " for Saloon car booklet");
        }
        protected internal override void PrintBackCover()
        {
            Console.WriteLine("Printing back cover for Saloon car booklet");
        }
    }

    public class ServiceHistoryBooklet : AbstractBookletPrinter
    {
        protected internal override int PageCount
        {
            get
            {
                return 12;
            }
        }
        protected internal override void PrintFrontCover()
        {
            Console.WriteLine("Printing front cover for service history booklet");
        }
        protected internal override void PrintToC()
        {
            Console.WriteLine("Printing table of contents for service history booklet");
        }
        protected internal override void PrintPage(int pageNumber)
        {
            Console.WriteLine("Printing page " + pageNumber + " for service history booklet");
        }
        protected internal override void PrintBackCover()
        {
            Console.WriteLine("Printing back cover for service history booklet");
        }
    }

    /// <summary>
    /// Define the skeleton of an algorithm in a method, deferring some steps to subclasses.
    /// Template Method lets subclasses redefine certain steps of an algorithm without changing the
    /// algorithm's structure.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("About to print a booklet for Saloon cars");
            AbstractBookletPrinter saloonBooklet = new SaloonBooklet();
            saloonBooklet.Print();
            Console.WriteLine("About to print a service history booklet");
            AbstractBookletPrinter serviceBooklet = new ServiceHistoryBooklet();
            serviceBooklet.Print();
            Console.Read();
        }
    }
}
