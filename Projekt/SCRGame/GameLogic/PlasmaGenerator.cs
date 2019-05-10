using System.Threading;

namespace SCRGame
{
    public class PlasmaGenerator : APartOfSomethingGreater
    {

        public Mutex Mutex = new Mutex();
        public PlasmaGenerator(double speed)
        {
            Level = 50;
            WorkingSpeed = speed;
        }

        public override bool HasFinished { get; set; }

        void Generate()
        {

            while (Level < 100)
            {
                Mutex.WaitOne();
                Level += WorkingSpeed;
                Mutex.ReleaseMutex();
                Thread.Sleep(100);
            }

        }

        public override void Update()
        {
            Generate();
        }
    }
}
