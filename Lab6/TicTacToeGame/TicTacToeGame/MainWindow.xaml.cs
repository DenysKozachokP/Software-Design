﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using TicTacToeDataAccess;
using TicTacToeGame.DataAccess;

namespace TicTacToeGame
{
    public partial class MainWindow : Window
    {
        private const string DatabaseFileName = "TicTacToeGame.db";
        private DatabaseManager dbManager;
        private PlayerManager playerManager;
        private string loggedInUsername;

        public MainWindow()
        {
            InitializeComponent();
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFileName);
            dbManager = new DatabaseManager(dbPath);
            playerManager = new PlayerManager(dbPath);
            BoardSizeComboBox.SelectedIndex = 0; // Встановлення стандартного розміру 3x3
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog() == true)
            {
                loggedInUsername = loginWindow.LoggedInUsername;
                WelcomeTextBlock.Text = $"Welcome, {loggedInUsername}!";
            }
        }

        private void PlayWithAI_Click(object sender, RoutedEventArgs e)
        {
            int boardSize = GetSelectedBoardSize();
            GameWindow gameWindow = new GameWindow(true, loggedInUsername, boardSize, dbManager, playerManager);
            if (!string.IsNullOrEmpty(loggedInUsername))
            {
                gameWindow.SetPlayerUsername(loggedInUsername);
            }
            gameWindow.Show();
            this.Close();
        }

        private void PlayWithFriend_Click(object sender, RoutedEventArgs e)
        {
            int boardSize = GetSelectedBoardSize();
            GameWindow gameWindow = new GameWindow(false, loggedInUsername, boardSize, dbManager, playerManager);
            if (!string.IsNullOrEmpty(loggedInUsername))
            {
                gameWindow.SetPlayerUsername(loggedInUsername);
            }
            gameWindow.Show();
            this.Close();
        }

        private int GetSelectedBoardSize()
        {
            ComboBoxItem selectedItem = (ComboBoxItem)BoardSizeComboBox.SelectedItem;
            switch (selectedItem.Content.ToString())
            {
                case "4x4":
                    return 4;
                case "5x5":
                    return 5;
                default:
                    return 3;
            }
        }

        private void OpenGameHistoryWindow(object sender, RoutedEventArgs e)
        {
            GameHistoryWindow gameHistoryWindow = new GameHistoryWindow(dbManager);
            gameHistoryWindow.Show();
        }

        private void OpenLeaderboardWindow(object sender, RoutedEventArgs e)
        {
            LeaderboardWindow leaderboardWindow = new LeaderboardWindow(dbManager);
            leaderboardWindow.Show();
        }
    }
}
