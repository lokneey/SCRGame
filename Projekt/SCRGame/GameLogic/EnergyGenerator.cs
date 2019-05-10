using System.Threading;

namespace SCRGame
{
    public class EnergyGenerator : APartOfSomethingGreater
    {

        public Mutex Mutex = new Mutex();
        public EnergyGenerator(double x)      //x rzędu 0,01
        {
            Level = 50;
            WorkingSpeed = x;
        }

        public override bool HasFinished { get; set; }

        void Generate()
        {

            while (Level < 100)
            {
                Mutex.WaitOne();
                Level += WorkingSpeed;
                Mutex.ReleaseMutex();
                Thread.Sleep(50);
            }

        }

        public override void Update()
        {
            Generate();
        }
    }
}
