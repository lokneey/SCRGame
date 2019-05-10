using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace SCRGame
{
    public class Shield : APartOfSomethingGreater
    {
        public Mutex Mutex = new Mutex();
        public double energyConsumed;
        List<EnergyGenerator> energyShileldGeneratorsList = new List<EnergyGenerator>();
        public Shield(double workingSpeed, List<EnergyGenerator> energyGeneratosList)
        {
            WorkingSpeed = workingSpeed;
            Level = 10000;
            energyShileldGeneratorsList = energyGeneratosList;
        }

        public override bool HasFinished { get; set; }

        public void RenewShield(double neededEnergy, int whichGenerator)
        {
            energyConsumed = neededEnergy;
            energyShileldGeneratorsList[whichGenerator].Mutex.WaitOne();
            if (energyShileldGeneratorsList[whichGenerator].Level > energyConsumed)
            {

                energyShileldGeneratorsList[whichGenerator].Level -= energyConsumed;


                Mutex.WaitOne();
                Level += (WorkingSpeed * energyConsumed);
                Mutex.ReleaseMutex();
                Thread.Sleep(100);
            }
            try
            {
                energyShileldGeneratorsList[whichGenerator].Mutex.ReleaseMutex();
            }
            catch
            {
                MessageBoxResult wrongResult = MessageBox.Show("Wątek nie zakończył jeszcze pracy. Proszę zaczekać!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Thread.Sleep(4000);
            }
        }

        public override void Update()
        {

        }
    }
}
