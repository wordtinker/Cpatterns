using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateExample
{
    public interface IClockSetupState
    {
        void PreviousValue();
        void NextValue();
        void SelectValue();

        string Instructions { get; }
        int SelectedValue { get; }
    }

    public partial class ClockSetup
    {
        //State classes
        private class YearSetupState : IClockSetupState
        {
            private ClockSetup context;
            private int year;

            public YearSetupState(ClockSetup context)
            {
                this.context = context;
                year = DateTime.Now.Year;
            }

            public string Instructions
            {
                get
                {
                    return "Please set the year...";
                }
            }

            public int SelectedValue
            {
                get
                {
                    return year;
                }
            }

            public void NextValue()
            {
                year++;
            }

            public void PreviousValue()
            {
                year--;
            }

            public void SelectValue()
            {
                Console.WriteLine($"Year set to {SelectedValue}");
                // did a little cheating here. Using Property coule be more safe;
                context.State = context.monthState;
            }
        }

        private class MonthSetupState : IClockSetupState
        {
            private ClockSetup context;
            private int month;

            public MonthSetupState(ClockSetup context)
            {
                this.context = context;
                month = DateTime.Now.Month;
            }

            public string Instructions
            {
                get
                {
                    return "Please set the month...";
                }
            }

            public int SelectedValue
            {
                get
                {
                    return month;
                }
            }

            public void NextValue()
            {
                if (month < 12) month++;
            }

            public void PreviousValue()
            {
                if (month > 1) month--;
            }

            public void SelectValue()
            {
                Console.WriteLine($"Month set to {SelectedValue}");
                context.State = context.dayState;
            }
        }

        private class DaySetupState : IClockSetupState
        {
            private ClockSetup context;
            private int day;

            public DaySetupState(ClockSetup context)
            {
                this.context = context;
                day = DateTime.Now.Day;
            }

            public string Instructions
            {
                get
                {
                    return "Please set the day...";
                }
            }

            public int SelectedValue
            {
                get
                {
                    return day;
                }
            }

            public void NextValue()
            {
                if (day < DateTime.DaysInMonth(new DateTime().Year, new DateTime().Month))
                {
                    day++;
                }
            }

            public void PreviousValue()
            {
                if (day > 1) day--;
            }

            public void SelectValue()
            {
                Console.WriteLine($"Day set to {SelectedValue}");
                context.State = context.finishedState;
            }
        }

        private class FinishedSetupState : IClockSetupState
        {
            private ClockSetup context;

            public FinishedSetupState(ClockSetup context)
            {
                this.context = context;
            }
            public virtual void PreviousValue()
            {
                Console.WriteLine("Ignored...");
            }
            public virtual void NextValue()
            {
                Console.WriteLine("Ignored...");
            }
            public virtual void SelectValue()
            {
                Console.WriteLine($"Date set to: {context.SelectedDate}");
            }
            public virtual string Instructions
            {
                get
                {
                    return "Press knob to view selected date...";
                }
            }
            public virtual int SelectedValue
            {
                get
                {
                    throw new System.NotSupportedException("Clock setup finished");
                }
            }
        }


    }


    // Main class
    public partial class ClockSetup
    {
        // states the setup could be in
        private IClockSetupState yearState;
        private IClockSetupState monthState;
        private IClockSetupState dayState;
        private IClockSetupState finishedState;

        // Current state
        private IClockSetupState currentState;

        // ctor
        public ClockSetup()
        {
            yearState = new YearSetupState(this);
            monthState = new MonthSetupState(this);
            dayState = new DaySetupState(this);
            finishedState = new FinishedSetupState(this);
            State = yearState;
        }

        private IClockSetupState State
        {
            set
            {
                currentState = value;
                Console.WriteLine(currentState.Instructions);
            }
        }

        // public methods
        public void RotateKnobLeft()
        {
            currentState.PreviousValue();
        }

        public void RotateKnobRight()
        {
            currentState.NextValue();
        }

        public void PushKnob()
        {
            currentState.SelectValue();
        }

        public DateTime SelectedDate
        {
            get
            {
                return new
                DateTime(yearState.SelectedValue, monthState.SelectedValue,
                dayState.SelectedValue);
            }
        }

    }

    /// <summary>
    /// Purpose: Allow an object to alter its behaviour when its internal state changes. The object will
    /// appear to change its class.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            ClockSetup clockSetup = new ClockSetup();
            // Setup starts in 'year' state
            clockSetup.RotateKnobRight();
            clockSetup.PushKnob(); // 1 year on
            
            // Setup should now be in 'month' state
            clockSetup.RotateKnobRight();
            clockSetup.RotateKnobRight();
            clockSetup.PushKnob(); // 2 months on

            // Setup should now be in 'day' state
            clockSetup.RotateKnobRight();
            clockSetup.RotateKnobRight();
            clockSetup.RotateKnobRight();
            clockSetup.PushKnob(); // 3 days on
            
            // Setup should now be in 'finished' state
            clockSetup.PushKnob(); // to display selected date
        }
    }
}
