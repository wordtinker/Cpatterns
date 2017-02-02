using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainOfResponsibilityExample
{

    public interface IEmailHandler
    {
        IEmailHandler NextHandler { set; }
        void ProcessHandler(string email);
    }

    public abstract class AbstractEmailHandler : IEmailHandler
    {
        public virtual IEmailHandler NextHandler { get; set; }
        public virtual void ProcessHandler(string email)
        {
            bool wordFound = false;
            // If no words to match against then this object can handle
            if (MatchingWords().Length == 0)
            {
                wordFound = true;
            }
            else
            {
                // Look for any of the matching words
                foreach (string word in MatchingWords())
                {
                    if (email.IndexOf(word) >= 0)
                    {
                        wordFound = true;
                        break;
                    }
                }
            }
            // Can we handle email in this object?
            if (wordFound)
            {
                HandleHere(email);
            }
            else
            {
                // Unable to handle here so forward to next in chain
                NextHandler.ProcessHandler(email);
            }        }
        protected internal abstract string[] MatchingWords();
        protected internal abstract void HandleHere(string email);        public static void Handle(string email)
        {
            // Create the handler objects...
            IEmailHandler spam = new SpamEmailHandler();
            IEmailHandler sales = new SalesEmailHandler();
            IEmailHandler service = new ServiceEmailHandler();
            IEmailHandler manager = new ManagerEmailHandler();
            IEmailHandler general = new GeneralEnquiriesEmailHandler();
            // Chain them together...
            spam.NextHandler = sales;
            sales.NextHandler = service;
            service.NextHandler = manager;
            manager.NextHandler = general;
            // Start the ball rolling...
            spam.ProcessHandler(email);
        }    }

    public class SpamEmailHandler : AbstractEmailHandler
    {
        protected internal override string[] MatchingWords()
        {
            return new string[] { "viagra", "pills", "medicines" };
        }
        protected internal override void HandleHere(string email)
        {
            Console.WriteLine("This is a spam email.");
        }
    }
    public class SalesEmailHandler : AbstractEmailHandler
    {
        protected internal override string[] MatchingWords()
        {
            return new string[] { "buy", "purchase" };
        }
        protected internal override void HandleHere(string email)
        {
            Console.WriteLine("Email handled by sales department.");
        }
    }
    public class ServiceEmailHandler : AbstractEmailHandler
    {
        protected internal override string[] MatchingWords()
        {
            return new string[] { "service", "repair" };
        }
        protected internal override void HandleHere(string email)
        {
            Console.WriteLine("Email handled by service department.");
        }
    }
    public class ManagerEmailHandler : AbstractEmailHandler
    {
        protected internal override string[] MatchingWords()
        {
            return new string[] { "complain", "bad" };
        }
        protected internal override void HandleHere(string email)
        {
            Console.WriteLine("Email handled by manager.");
        }
    }
    public class GeneralEnquiriesEmailHandler : AbstractEmailHandler
    {
        protected internal override string[] MatchingWords()
        {
            return new string[0]; // match anything
        }
        protected internal override void HandleHere(string email)
        {
            Console.WriteLine("Email handled by general enquires.");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            String email = "I need my car repaired.";
            AbstractEmailHandler.Handle(email);
        }
    }
}
