using System;

namespace CommandExample
{
    public class Radio
    {
        public const int MinVolume = 0;
        public const int MaxVolume = 10;
        public const int DefaultVolume = 5;

        public Radio()
        {
            On = false;
            Volume = DefaultVolume;
        }
        public virtual bool On { get; private set; }
        public virtual int Volume { get; private set; }
        
        public virtual void SwitchOn()
        {
            On = true;
            Console.WriteLine("Radio now on, volume level " + Volume);
        }
        public virtual void SwitchOff()
        {
            On = false;
            Console.WriteLine("Radio now off");
        }
        public virtual void VolumeUp()
        {
            if (On)
            {
                if (Volume < MaxVolume)
                {
                    Volume++;
                    Console.WriteLine("Volume turned up to level " + Volume);
                }
            }
        }
        public virtual void VolumeDown()
        {
            if (On)
            {
                if (Volume > MinVolume)
                {
                    Volume--;
                    Console.WriteLine("Volume turned down to level " + Volume);
                }
            }
        }
    }

    public class ElectricWindow
    {
        public ElectricWindow()
        {
            Open = false;
            Console.WriteLine("Window is closed");
        }
        public virtual bool Open { get; private set; }
        public virtual bool Closed
        {
            get
            {
                return (!Open);
            }
        }
        public virtual void OpenWindow()
        {
            if (Closed)
            {
                Open = true;
                Console.WriteLine("Window is now open");
            }
        }
        public virtual void CloseWindow()
        {
            if (Open)
            {
                Open = false;
                Console.WriteLine("Window is now closed");
            }
        }
    }

    public interface IVoiceCommand
    {
        void Execute();
    }

    public class VolumeUpCommand : IVoiceCommand
    {
        private Radio radio;
        public VolumeUpCommand(Radio radio)
        {
            this.radio = radio;
        }
        public virtual void Execute()
        {
            radio.VolumeUp();
        }
    }
    public class VolumeDownCommand : IVoiceCommand
    {
        private Radio radio;
        public VolumeDownCommand(Radio radio)
        {
            this.radio = radio;
        }
        public virtual void Execute()
        {
            radio.VolumeDown();
        }
    }
    public class WindowUpCommand : IVoiceCommand
    {
        private ElectricWindow window;
        public WindowUpCommand(ElectricWindow window)
        {
            this.window = window;
        }
        public virtual void Execute()
        {
            window.CloseWindow();
        }
    }
    public class WindowDownCommand : IVoiceCommand
    {
        private ElectricWindow window;
        public WindowDownCommand(ElectricWindow window)
        {
            this.window = window;
        }
        public virtual void Execute()
        {
            window.OpenWindow();
        }
    }

    public class SpeechRecogniser
    {
        private IVoiceCommand upCommand, downCommand;
        public virtual void SetCommands(IVoiceCommand upCommand, IVoiceCommand downCommand)
        {
            this.upCommand = upCommand;
            this.downCommand = downCommand;
        }
        public virtual void HearUpSpoken()
        {
            upCommand.Execute();
        }
        public virtual void HearDownSpoken()
        {
            downCommand.Execute();
        }
    }

    /// <summary>
    /// Commande decouples controller from the object it controls. 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Create a radio and its up/down command objects
            Radio radio = new Radio();
            radio.SwitchOn();
            IVoiceCommand volumeUpCommand = new VolumeUpCommand(radio);
            IVoiceCommand volumeDownCommand = new VolumeDownCommand(radio);
            // Create an electric window and its up/down command objects
            ElectricWindow window = new ElectricWindow();
            IVoiceCommand windowUpCommand = new WindowUpCommand(window);
            IVoiceCommand windowDownCommand = new WindowDownCommand(window);

            // Create a speech recognition object
            SpeechRecogniser speechRecogniser = new SpeechRecogniser();
            // Control the radio
            speechRecogniser.SetCommands(volumeUpCommand, volumeDownCommand);
            Console.WriteLine("Speech recognition controlling the radio");
            speechRecogniser.HearUpSpoken();
            speechRecogniser.HearUpSpoken();
            speechRecogniser.HearUpSpoken();
            speechRecogniser.HearDownSpoken();
            // Control the electric window
            speechRecogniser.SetCommands(windowUpCommand, windowDownCommand);
            Console.WriteLine("Speech recognition will now control the window");
            speechRecogniser.HearDownSpoken();
            speechRecogniser.HearUpSpoken();
        }
    }
}
