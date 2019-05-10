using System.Collections.Generic;
using System.Threading;

namespace SCRGame
{
    public class Cannon : APartOfSomethingGreater
    {
        double cannonPower;
        double inflictedDamage;
        double energyConsumed;
        double plasmaConsumed;
        public int WhichPlasmaGenerator;
        public int WhichEnergyGenerator;
        public List<EnergyGenerator> energyGeneratorsCannonList = new List<EnergyGenerator>();
        public List<PlasmaGenerator> plasmaGeneratorsCannonList = new List<PlasmaGenerator>();

        public override bool HasFinished { get; set; }

        public Cannon(double power, double speed, List<EnergyGenerator> energyGenerators, List<PlasmaGenerator> plasmaGenerators)
        {
            WorkingSpeed = speed;
            cannonPower = power;
            energyGeneratorsCannonList = energyGenerators;
            plasmaGeneratorsCannonList = plasmaGenerators;
        }
        public void Shoot(double energyConsumption, double plasmaConsumption, Ship ship, int whichEnergyGenerator, int whichPlasmaGenerator)
        {

            energyConsumed = energyConsumption;
            plasmaConsumed = plasmaConsumption;
            if (energyGeneratorsCannonList[whichEnergyGenerator].Level > energyConsumed && plasmaGeneratorsCannonList[whichPlasmaGenerator].Level > plasmaConsumed)
            {
                energyGeneratorsCannonList[whichEnergyGenerator].Mutex.WaitOne();
                energyGeneratorsCannonList[whichEnergyGenerator].Level -= energyConsumed;
                energyGeneratorsCannonList[whichEnergyGenerator].Mutex.ReleaseMutex();
                plasmaGeneratorsCannonList[whichPlasmaGenerator].Mutex.WaitOne();
                plasmaGeneratorsCannonList[whichPlasmaGenerator].Level -= plasmaConsumed;
                plasmaGeneratorsCannonList[whichPlasmaGenerator].Mutex.ReleaseMutex();

                inflictedDamage = 4 * cannonPower * plasmaConsumed * WorkingSpeed;
                ship.Mutex.WaitOne();

                if (ship.Shield.Level >= inflictedDamage)
                {
                    ship.Shield.Level -= inflictedDamage;
                }
                else if (ship.Shield.Level < inflictedDamage)
                {
                    inflictedDamage -= ship.Shield.Level;
                    if (ship.HitPoints > inflictedDamage)
                    {
                        ship.HitPoints -= inflictedDamage;
                        ship.Shield.Level = 0;
                    }
                    else
                    {
                        ship.Shield.Level = 0;
                        ship.HitPoints = 0;
                        ship.Deafeated = true;
                    }

                }

                ship.Mutex.ReleaseMutex();
            }
            Thread.Sleep(100);
        }

        public override void Update()
        {

        }
    }
}
