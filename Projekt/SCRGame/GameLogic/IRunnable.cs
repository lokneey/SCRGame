namespace SCRGame
{
    public interface IRunnable
    {
        bool HasFinished { get; set; }
        void Update();
        void Run();
    }
}
