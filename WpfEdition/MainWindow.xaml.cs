﻿using Controller;
using Model;
using System;
using System.Windows;
using DispatcherPriority = System.Windows.Threading.DispatcherPriority;

namespace WpfEdition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CompetitionStatistics _competitionStatistics;
        private CurrentRaceStatistics _currentRaceStatistics;

        public MainWindow()
        {
            InitializeComponent();
            // Start of application
            ImageCache.Initialize();
            Data.Initialize();
            Data.NextRaceEvent += OnNextRaceEvent; // tell data about visualization event.
            Data.NextRace(); // start first race
        }

        private void OnNextRaceEvent(object sender, NextRaceEventArgs e)
        {
            // reinitialize
            ImageCache.ClearCache();
            Visualization.Initialize(e.Race);

            // link event
            e.Race.DriversChanged += OnDriversChanged;

            // Dispatcher is needed for execution of OnDriversChanged. Otherwise thread exceptions will occur.
            this.Dispatcher.Invoke(() =>
            {
                e.Race.DriversChanged += ((MainDataContext)this.DataContext).OnDriversChanged;
            });
        }

        private void OnDriversChanged(object sender, DriversChangedEventArgs e)
        {
            this.TrackScreen.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    this.TrackScreen.Source = null;
                    this.TrackScreen.Source = Visualization.DrawTrack(e.Track);
                }));
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_OpenCurrentRaceStatistics_Click(object sender, RoutedEventArgs e)
        {
            // initialize window
            _currentRaceStatistics = new CurrentRaceStatistics();

            // link next race event
            Data.NextRaceEvent += ((RaceStatisticsDataContext)_currentRaceStatistics.DataContext).OnNextRace;

            // send current race to data context to show data mid race
            ((RaceStatisticsDataContext)_currentRaceStatistics.DataContext).OnNextRace(null, new NextRaceEventArgs() { Race = Data.CurrentRace });

            // show window
            _currentRaceStatistics.Show();
        }

        private void MenuItem_OpenCompetitionStatistics_Click(object sender, RoutedEventArgs e)
        {
            // initialize window
            _competitionStatistics = new CompetitionStatistics();

            // link next race event
            Data.NextRaceEvent += ((CompetitionStatisticsDataContext)_competitionStatistics.DataContext).OnNextRace;

            // send current race to data context to show data mid race
            ((CompetitionStatisticsDataContext)_competitionStatistics.DataContext).OnNextRace(null, new NextRaceEventArgs() { Race = Data.CurrentRace });

            // show window
            _competitionStatistics.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}