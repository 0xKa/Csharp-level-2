using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Temperature_Change_Event
{
    public class TemperatureChangedEventArgs : EventArgs
    {
        public double OldTemperature { get; }
        public double NewTemperature { get; }
        public double Difference { get; }

        public TemperatureChangedEventArgs(double OldTemperature, double NewTemperature)
        {
            this.OldTemperature = OldTemperature;
            this.NewTemperature = NewTemperature;
            this.Difference = NewTemperature - OldTemperature;
        }
    }

    public class ThermoStat
    {
        public event EventHandler<TemperatureChangedEventArgs> TempChanged;

        private double _OldTemp;
        private double _CurrentTemp;

        public void SetTemperature(double NewTemperature)
        {
            if (NewTemperature != _CurrentTemp)
            {
                _OldTemp = _CurrentTemp;
                _CurrentTemp = NewTemperature;
                OnTempChanged(new TemperatureChangedEventArgs(_OldTemp, _CurrentTemp)); //this func will raise the event
            }
        }

        protected virtual void OnTempChanged(TemperatureChangedEventArgs e)
        {
            TempChanged?.Invoke(this, e); //call all the functions that are subscribed to this event
        }

    }

    public class Display
    {
        public void Subscribe(ThermoStat thermoStat)
        {
            thermoStat.TempChanged += ThermoStat_TempChanged;
        }

        private void ThermoStat_TempChanged(object sender, TemperatureChangedEventArgs e)
        {
            Console.WriteLine("\n\nTemperature Changed!!!");
            Console.WriteLine($"Old Temp: {e.OldTemperature}°");
            Console.WriteLine($"New Temp: {e.NewTemperature} °");
            Console.WriteLine($"diff: {e.Difference} °");

        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            ThermoStat thermoStat = new ThermoStat();
            Display display = new Display();

            //each time the temp changed, the display class will know.
            display.Subscribe(thermoStat);

            thermoStat.SetTemperature(45);

            thermoStat.SetTemperature(20);

            thermoStat.SetTemperature(90);

            //the display class will not be invoked if the temp didn't change.
            thermoStat.SetTemperature(90);

            Console.Read();
        }
    }
}
