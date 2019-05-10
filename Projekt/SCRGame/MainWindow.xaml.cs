using System;
using System.Threading;
using System.Windows;

namespace SCRGame
{
    public partial class MainWindow : Window
    {
        GameManager gameManager;

        public MainWindow()
        {
            InitializeComponent();
            gameManager = new GameManager();
            this.Width = SystemParameters.PrimaryScreenWidth;
            var autoEvent = new AutoResetEvent(false);
            gameManager.timer = new Timer(Callabelack, autoEvent, 1000, 100);
        }

        public void Callabelack(Object state)
        {
            int invokeCount = 0;
            int maxCount = 10;
            AutoResetEvent autoEvent = (AutoResetEvent)state;
            ++invokeCount;
            gameManager.UpdateAll();
            if (invokeCount == maxCount)
            {
                invokeCount = 0;
                autoEvent.Set();
            }
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            gameManager.RunGame();
        }
    }
}
