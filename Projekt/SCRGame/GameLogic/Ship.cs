using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace SCRGame
{
    public class Ship : APartOfSomethingGreater
    {
        public Mutex Mutex = new Mutex();
        public Cannon Cannon;
        public Shield Shield;
        public double HitPoints = 10000;
        public int ChosenEnemy;
        public int WhichPlasmaGenerator;
        public int WhichEnergyGenerator;
        public bool Deafeated = false;
        public string colorShip;
        public List<Ship> enemiesShipList;        
        public List<EnergyGenerator> energyGeneratorsShipList = new List<EnergyGenerator>();
        public List<PlasmaGenerator> plasmaGeneratorsShipList = new List<PlasmaGenerator>();
        public override bool HasFinished { get; set; }

        public Ship(double cannonPower, double shieldWorkingSpeed, double shootingSpeed, List<EnergyGenerator> energyGenerators, List<PlasmaGenerator> plasmaGenerators, List<Ship> enemies, string color)
        {
            Cannon = new Cannon(cannonPower, shootingSpeed, energyGenerators, plasmaGenerators);
            Shield = new Shield(shieldWorkingSpeed, energyGenerators);
            energyGeneratorsShipList = energyGenerators;
            plasmaGeneratorsShipList = plasmaGenerators;
            Deafeated = false;
            enemiesShipList = enemies;
            colorShip = color;
        }
        public void Shoot(Ship ship, int whichEnergyGenerator, int whichPlasmaGenerator)
        {
            Cannon.Shoot(10, 10, ship, whichEnergyGenerator, whichPlasmaGenerator);
        }
        public void GenerateShield()
        {
            if (energyGeneratorsShipList[0].Level > 10)
            {
                Shield.RenewShield(10, 0);
            }
            else if (energyGeneratorsShipList[1].Level > 10)
            {
                Shield.RenewShield(10, 1);
            }
        }
        public override void Update()
        {
            if (Deafeated == true)
            {
                HasFinished = true;
                return;
            }
            if (Deafeated == false)
            {
                if (energyGeneratorsShipList[0].Level > 20)
                {
                    WhichEnergyGenerator = 0;
                }
                else if (energyGeneratorsShipList[1].Level > 20)
                {
                    WhichEnergyGenerator = 1;
                }
                if (plasmaGeneratorsShipList[0].Level > 10)
                {
                    WhichPlasmaGenerator = 0;
                }
                else if (plasmaGeneratorsShipList[1].Level > 10)
                {
                    WhichPlasmaGenerator = 1;
                }
                if (Shield.Level < 9000)
                {
                    GenerateShield();
                }
                try
                {
                    if (enemiesShipList[ChosenEnemy].Deafeated == false)
                    {
                        Shoot(enemiesShipList[ChosenEnemy], WhichEnergyGenerator, WhichPlasmaGenerator);
                    }
                }
                catch
                {
                    MessageBoxResult wrongResult = MessageBox.Show("Błąd w aktualizacji stanu statku!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Thread.Sleep(4000);
                }
            }
        }
    }
}
