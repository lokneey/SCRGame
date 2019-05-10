using System.Threading;

namespace SCRGame
{
    public abstract class APartOfSomethingGreater : IRunnable
    {

        public double Level;
        public double WorkingSpeed;

        public abstract bool HasFinished { get; set; }

        public void Run()
        {
            Thread.Sleep(4000);
            while (!HasFinished)
            {
                Update();
                Thread.Sleep(1000);
            }

        }

        public abstract void Update();


    }
}
