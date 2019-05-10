/*
    Before you run this project remember to copy Umg folder to: ~/Projekt/SCRGame/bin/Debug
*/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SCRGame
{
    class GameManager
    {
        public Timer timer;

        List<Image> spaceshipsList;
        List<Image> shieldsList;
        List<Image> destroyedList;
        List<Image> generatorsPicturesRedList;
        List<Image> generatorsPicturesBlueList;
        List<ProgressBar> lifeBarsList;
        List<ProgressBar> shieldBarsList;
        List<Button> shooterList;
        List<Label> labelsList;
        List<ProgressBar> generatorBarsRedList;
        List<ProgressBar> generatorBarsBlueList;
        List<Ship> fightersRedList;
        List<Ship> fightersBlueList;
        List<IRunnable> gamersList;
        List<PlasmaGenerator> plasmageneratorsPicturesBlueList;
        List<EnergyGenerator> energygeneratorsPicturesBlueList;
        List<PlasmaGenerator> plasmageneratorsPicturesRedList;
        List<EnergyGenerator> energygeneratorsPicturesRedList;
        Image picTemp;
        MainWindow window;
        int numberOfShips;
        double shieldWidth = 330;       //This is objects property which is a base for sizing other objects
        public bool endOfGame = false;
        bool start = true;
        string path;
        int i;

        public GameManager()
        {
            window = Application.Current.Windows[0] as MainWindow;
        }

        public void RunGame()
        {
            ClearAllAtTheEnd();
            PrepareForBattle();
            PrepareShips("Red");
            UpdateTeam("Red");
            ClearLists();
            PrepareShips("Blue");
            UpdateTeam("Blue");
            ClearLists();
            start = false;
            LetsStartTheBattle();
            window.ExecuteButton.Content = "Reset";
        }

        public void LetsStartTheBattle()
        {
            if (gamersList != null)
            {
                RunThreads(gamersList);
            }
            else
            {
                MessageBoxResult wrongResult = MessageBox.Show("Najpierw wybierz liczbę graczy", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public static void RunThreads(List<IRunnable> elements)
        {
            int id = 0;
            List<Thread> threadList = new List<Thread>();
            foreach (IRunnable element in elements)
            {
                threadList.Add(new Thread(new ThreadStart(element.Run)));
                id++;
            }

            foreach (Thread thread in threadList)
            {
                thread.IsBackground = true;
                thread.Start();
            }
        }

        public void PrepareForBattle()
        {
            start = true;
            path = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace(@"\", "/");
            numberOfShips = window.ShipsNumberComboBox.SelectedIndex + 2;
            gamersList = new List<IRunnable>();
            plasmageneratorsPicturesBlueList = new List<PlasmaGenerator>();
            energygeneratorsPicturesBlueList = new List<EnergyGenerator>();
            plasmageneratorsPicturesRedList = new List<PlasmaGenerator>();
            energygeneratorsPicturesRedList = new List<EnergyGenerator>();
            spaceshipsList = new List<Image>();
            shieldsList = new List<Image>();
            destroyedList = new List<Image>();
            generatorsPicturesBlueList = new List<Image>();
            generatorsPicturesRedList = new List<Image>();
            lifeBarsList = new List<ProgressBar>();
            shieldBarsList = new List<ProgressBar>();
            generatorBarsRedList = new List<ProgressBar>();
            generatorBarsBlueList = new List<ProgressBar>();
            fightersRedList = new List<Ship>();
            fightersBlueList = new List<Ship>();
            plasmageneratorsPicturesBlueList.Add(new PlasmaGenerator(0.7));
            energygeneratorsPicturesBlueList.Add(new EnergyGenerator(0.7));
            plasmageneratorsPicturesBlueList.Add(new PlasmaGenerator(0.5));
            energygeneratorsPicturesBlueList.Add(new EnergyGenerator(0.5));
            plasmageneratorsPicturesRedList.Add(new PlasmaGenerator(0.6));
            energygeneratorsPicturesRedList.Add(new EnergyGenerator(0.6));
            plasmageneratorsPicturesRedList.Add(new PlasmaGenerator(0.6));
            energygeneratorsPicturesRedList.Add(new EnergyGenerator(0.6));

            foreach (EnergyGenerator genEn in energygeneratorsPicturesBlueList)
            {
                gamersList.Add(genEn);
            }
            foreach (PlasmaGenerator genPl in plasmageneratorsPicturesBlueList)
            {
                gamersList.Add(genPl);
            }
            foreach (EnergyGenerator genEn in energygeneratorsPicturesRedList)
            {
                gamersList.Add(genEn);
            }
            foreach (PlasmaGenerator genPl in plasmageneratorsPicturesRedList)
            {
                gamersList.Add(genPl);
            }
            for (int a = 0; a < 2; a++)
            {
                picTemp = new Image();
                picTemp.Source = new BitmapImage(new Uri(path + "/Img/plazma.jpg"));
                generatorsPicturesRedList.Add(picTemp);
                picTemp = new Image();
                picTemp.Source = new BitmapImage(new Uri(path + "/Img/plazma.jpg"));
                generatorsPicturesBlueList.Add(picTemp);
                picTemp = new Image();
                picTemp.Source = new BitmapImage(new Uri(path + "/Img/energy.jpg"));
                generatorsPicturesRedList.Add(picTemp);
                picTemp = new Image();
                picTemp.Source = new BitmapImage(new Uri(path + "/Img/energy.jpg"));
                generatorsPicturesBlueList.Add(picTemp);
            }
            foreach (Image img in generatorsPicturesRedList)
            {
                img.Margin = new Thickness(18, 0, 18, 0);
            }
            foreach (Image img in generatorsPicturesBlueList)
            {
                img.Margin = new Thickness(18, 0, 18, 0);
            }
            window.ShieldPictureRedItems1.ItemsSource = generatorsPicturesRedList;
            window.ShieldPictureRedItems2.ItemsSource = generatorsPicturesBlueList;
            foreach (Image img in generatorsPicturesRedList)
            {
                generatorBarsBlueList.Add(new ProgressBar());
                generatorBarsRedList.Add(new ProgressBar());
            }
            UpdateGenerators();
        }

        public void PrepareShips(string team)
        {
            for (int i = 0; i < numberOfShips; i++)
            {
                picTemp = new Image();
                picTemp.Source = new BitmapImage(new Uri(path + "/Img/spaceship.png"));
                spaceshipsList.Add(picTemp);
                picTemp = new Image();
                picTemp.Source = new BitmapImage(new Uri(path + "/Img/sphere.png"));
                shieldsList.Add(picTemp);
            }
            i = 1;
            foreach (Image img in shieldsList)
            {
                img.Width = shieldWidth;
                img.Name = "Shield" + i;
                img.Visibility = Visibility.Visible;
                img.Margin = new Thickness(7, 0, 7, 0);
                img.Opacity = 0.3;
                i++;
            }
            i = 1;
            foreach (Image img in spaceshipsList)
            {
                img.Width = 0.4 * shieldWidth;
                img.Name = "Spaceship" + i;
                img.Visibility = Visibility.Visible;
                if (team == "Red")
                {
                    fightersRedList.Add(new Ship(1, 10, 10, energygeneratorsPicturesRedList, plasmageneratorsPicturesRedList, fightersBlueList, "Red" + i));
                    img.Margin = new Thickness(0.3 * shieldWidth + 7, 0, 0.3 * shieldWidth + 7, 0);
                }
                else if (team == "Blue")
                {
                    fightersBlueList.Add(new Ship(1, 10, 10, energygeneratorsPicturesBlueList, plasmageneratorsPicturesBlueList, fightersRedList, "Blue" + i));
                    img.Margin = new Thickness(0.3 * shieldWidth + 5, 0, 0.3 * shieldWidth + 5, 0);
                }
                i++;
            }
            if (team == "Red")
            {
                window.ShieldPictureRedItems.ItemsSource = shieldsList;
                window.SpaceshipPictureRedItems.ItemsSource = spaceshipsList;
            }
            else if (team == "Blue")
            {
                shooterList = new List<Button>();
                foreach (Image spaceship in spaceshipsList)
                {
                    shooterList.Add(new Button());
                }
                int buttonOffset = 0;
                foreach (Button button in shooterList)
                {

                    button.Name = "BlueSpaceshipBut" + buttonOffset;

                    button.Margin = new Thickness(0, 0, 0, 0);
                    button.Background = Brushes.Transparent;
                    button.BorderBrush = Brushes.Transparent;
                    button.Content = spaceshipsList[buttonOffset];
                    button.Click += (s, a) =>
                    {
                        try
                        {
                            string content = (s as Button).Name.ToString();
                            content = content.Replace("BlueSpaceshipBut", "");
                            foreach (Ship fighter in fightersRedList)
                            {
                                fighter.ChosenEnemy = int.Parse(content);
                            }
                        }
                        catch
                        {
                            MessageBoxResult wrongResult = MessageBox.Show("Błąd ataku. Spróbuj ponownie za chwilę.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    };
                    buttonOffset++;
                }
                window.ShieldPictureBlueItems.ItemsSource = shieldsList;
                window.SpaceshipPictureBlueItems.ItemsSource = shooterList;
            }
        }

        public void UpdateAll()
        {
            if (start == false)
            {
                window.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {

                    try
                    {
                        UpdateTeam("Red");
                        ClearLists();
                        UpdateTeam("Blue");
                        ClearLists();
                        UpdateGenerators();

                        if (fightersRedList.FindAll(x => x.Deafeated == true).Count == numberOfShips)
                        {
                            if (endOfGame == false)
                            {
                                endOfGame = true;
                                MessageBoxResult result = MessageBox.Show("Gratulacje! Starcie wygrała drużyna NIEBIESKICH! Aby rozpocząć grę ponownie kliknij OK i wpisz liczbę statków.", "WOW", MessageBoxButton.OK, MessageBoxImage.Error);
                                ClearAllAtTheEnd();
                            }
                            timer.Change(Timeout.Infinite, Timeout.Infinite);
                        }
                        if (fightersBlueList.FindAll(x => x.Deafeated == true).Count == numberOfShips)
                        {
                            if (endOfGame == false)
                            {
                                endOfGame = true;
                                MessageBoxResult result = MessageBox.Show("Gratulacje! Starcie wygrała drużyna CZERWONYCH! Aby rozpocząć grę ponownie kliknij OK i wpisz liczbę statków.", "WOW", MessageBoxButton.OK, MessageBoxImage.Error);
                                ClearAllAtTheEnd();
                            }
                            timer.Change(Timeout.Infinite, Timeout.Infinite);
                        }
                    }
                    catch
                    {
                        MessageBoxResult result = MessageBox.Show("Błąd aktualizowania bitwy!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }

        public void UpdateTeam(string team)
        {
            lifeBarsList = new List<ProgressBar>();
            shieldBarsList = new List<ProgressBar>();
            destroyedList = new List<Image>();
            labelsList = new List<Label>();
            foreach (var fighter in fightersRedList)
            {
                lifeBarsList.Add(new ProgressBar());
            }
            i = 1;
            foreach (ProgressBar progressBar in lifeBarsList)
            {
                if (team == "Red")
                {
                    progressBar.Name = "RedSpaceshipLifeBar" + i;
                    progressBar.Value = fightersRedList[i - 1].HitPoints;
                }
                else if (team == "Blue")
                {
                    progressBar.Name = "BlueSpaceshipLifeBar" + i;
                    progressBar.Value = fightersBlueList[i - 1].HitPoints;
                }
                progressBar.Width = 0.4 * shieldWidth;
                progressBar.Minimum = 0;
                progressBar.Maximum = 10000;
                progressBar.Foreground = new SolidColorBrush(Colors.Red);
                progressBar.Margin = new Thickness(0.3 * shieldWidth + 5, 0, 0.3 * shieldWidth + 5, 0);
                progressBar.Visibility = Visibility.Visible;
                i++;
            }
            foreach (var fighter in fightersRedList)
            {
                shieldBarsList.Add(new ProgressBar());
                picTemp = new Image();
                picTemp.Source = new BitmapImage(new Uri(path + "/Img/x.png"));
                destroyedList.Add(picTemp);
            }
            i = 1;
            foreach (ProgressBar progressBar in shieldBarsList)
            {
                if (team == "Red")
                {
                    progressBar.Name = "RedspaceshipsListhieldBar" + i;
                    progressBar.Value = fightersRedList[i - 1].Shield.Level;
                }
                else if (team == "Blue")
                {
                    progressBar.Name = "BluespaceshipsListhieldBar" + i;
                    progressBar.Value = fightersBlueList[i - 1].Shield.Level;
                }
                progressBar.Width = 0.4 * shieldWidth;
                progressBar.Minimum = 0;
                progressBar.Maximum = 10000;
                progressBar.Foreground = new SolidColorBrush(Colors.Blue);
                progressBar.Margin = new Thickness(0.3 * shieldWidth + 5, 0, 0.3 * shieldWidth + 5, 0);
                progressBar.Visibility = Visibility.Visible;
                i++;
            }
            i = 1;
            foreach (Image img in destroyedList)
            {
                if (team == "Red")
                {
                    img.Name = "RedX" + i;
                    if (fightersRedList[i - 1].HitPoints <= 0)
                    {
                        img.Visibility = Visibility.Visible;
                        fightersRedList[i - 1].Deafeated = true;
                    }
                    else
                    {
                        img.Visibility = Visibility.Hidden;
                    }
                }
                else if (team == "Blue")
                {
                    img.Name = "BlueX" + i;
                    if (fightersBlueList[i - 1].HitPoints <= 0)
                    {
                        img.Visibility = Visibility.Visible;
                        fightersBlueList[i - 1].Deafeated = true;
                    }
                    else
                    {
                        img.Visibility = Visibility.Hidden;
                    }
                }
                img.Width = 0.4 * shieldWidth;
                img.Margin = new Thickness(0.3 * shieldWidth + 7, 0, 0.3 * shieldWidth + 7, 0);
                i++;
            }
            foreach (Ship ship in fightersBlueList)
            {
                labelsList.Add(new Label());
            }
            i = 0;
            foreach (Label label in labelsList)
            {
                label.Width = 0.4 * shieldWidth;
                label.FontWeight = FontWeights.Bold;
                label.Height = 30;
                label.Margin = new Thickness(0.3 * shieldWidth + 5, 0, 0.3 * shieldWidth + 5, 0);
                label.Content = (fightersBlueList[i].WhichPlasmaGenerator == 0 ? "L" : "P") + " plazma " + (fightersBlueList[i].WhichPlasmaGenerator == 0 ? "L" : "P") + " energia";
                i++;
            }
            if (team == "Red")
            {
                if (start == true)
                {
                    foreach (Ship fighter in fightersRedList)
                    {
                        gamersList.Add(fighter);
                    }
                }               
                window.LifeBarsRedItems.ItemsSource = lifeBarsList;
                window.DestroyedRedItems.ItemsSource = destroyedList;
                window.ShieldBarsRedItems.ItemsSource = shieldBarsList;
                window.LabelsRedItems.ItemsSource = labelsList;
            }
            if (team == "Blue")
            {
                Random rd = new Random();
                int randomShipNumber = rd.Next() % numberOfShips;
                foreach (Ship fighter in fightersBlueList)
                {
                    fighter.ChosenEnemy = randomShipNumber;
                }
                if (start == true)
                {
                    foreach (Ship fighter in fightersBlueList)
                    {
                        gamersList.Add(fighter);
                    }
                }                
                window.LifeBarsBlueItems.ItemsSource = lifeBarsList;
                window.DestroyedBlueItems.ItemsSource = destroyedList;
                window.ShieldBarsBlueItems.ItemsSource = shieldBarsList;
                window.LabelsBlueItems.ItemsSource = labelsList;
            }
        }

        public void UpdateGenerators()
        {
            i = 1;
            foreach (ProgressBar progressBar in generatorBarsBlueList)
            {
                progressBar.Name = "BlueGeneratorBar" + i;
                progressBar.Width = 100;
                progressBar.Minimum = 0;
                progressBar.Maximum = 100;                
                if (i <= 2)
                {
                    progressBar.Value = plasmageneratorsPicturesBlueList[i - 1].Level;
                }
                else
                {
                    progressBar.Value = energygeneratorsPicturesBlueList[i - 3].Level;
                }
                progressBar.Margin = new Thickness(5, 0, 5, 0);
                progressBar.Visibility = Visibility.Visible;
                i++;
            }
            window.GeneratorBarsBlueItems.ItemsSource = generatorBarsBlueList;
            i = 1;
            foreach (ProgressBar progressBar in generatorBarsRedList)
            {
                progressBar.Name = "RedGeneratorBar" + i;
                progressBar.Width = 100;
                progressBar.Minimum = 0;
                progressBar.Maximum = 100;
                if (i <= 2)
                {
                    progressBar.Value = plasmageneratorsPicturesRedList[i - 1].Level;
                }
                else
                {
                    progressBar.Value = energygeneratorsPicturesRedList[i - 3].Level;
                }
                progressBar.Margin = new Thickness(5, 0, 5, 0);
                progressBar.Visibility = Visibility.Visible;
                i++;
            }
            window.GeneratorBarsRedItems.ItemsSource = generatorBarsRedList;
        }

        public void ClearLists()
        {
            if (labelsList != null)
            {
                labelsList.Clear();
            }
            if (labelsList != null)
            {
                labelsList.Clear();
            }
            if (lifeBarsList != null)
            {
                lifeBarsList.Clear();
            }
            if (destroyedList != null)
            {
                destroyedList.Clear();
            }
            if (shieldsList != null)
            {
                shieldsList.Clear();
            }
            if (spaceshipsList != null)
            {
                spaceshipsList.Clear();
            }
            if (shieldBarsList != null)
            {
                shieldBarsList.Clear();
            }
            if (shooterList != null)
            {
                shooterList.Clear();
            }
        }

        public void ClearAllAtTheEnd()
        {
            ClearLists();
            start = true;
            if (gamersList != null)
            {
                foreach (IRunnable gamer in gamersList)
                {
                    gamer.HasFinished = true;
                }
            }
            if (generatorsPicturesBlueList != null)
            {
                generatorsPicturesBlueList.Clear();
            }
            if (generatorsPicturesRedList != null)
            {
                generatorsPicturesRedList.Clear();
            }
            if (generatorBarsRedList != null)
            {
                generatorBarsRedList.Clear();
            }
            if (generatorBarsBlueList != null)
            {
                generatorBarsBlueList.Clear();
            }
            if (energygeneratorsPicturesBlueList != null)
            {
                energygeneratorsPicturesBlueList.Clear();
            }
            if (energygeneratorsPicturesRedList != null)
            {
                energygeneratorsPicturesRedList.Clear();
            }
            if (plasmageneratorsPicturesBlueList != null)
            {
                plasmageneratorsPicturesBlueList.Clear();
            }
            if (plasmageneratorsPicturesRedList != null)
            {
                plasmageneratorsPicturesRedList.Clear();
            }
            if (fightersRedList != null)
            {
                fightersRedList.Clear();
            }
            if (fightersBlueList != null)
            {
                fightersBlueList.Clear();
            }
            window.ExecuteButton.Content = "Rozpocznij";
        }

    }
}
