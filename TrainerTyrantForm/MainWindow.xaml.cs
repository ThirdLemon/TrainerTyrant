﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace TrainerTyrantForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string openFileDialogFilter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
        private static readonly string getPokemonFile = "Open Pokemon JSON";
        private static readonly string getTrainerSlotFile = "Open Trainer JSON";
        private static readonly string getMovesFile = "Open Moves JSON";
        private static readonly string getItemsFile = "Open Items JSON";

        private string dialogDirectory = AppDomain.CurrentDomain.BaseDirectory;

        private ApplicationData _appData;

        public MainWindow()
        {
            InitializeComponent();

            _appData = new ApplicationData();
        }

        private void btnOpenPokemonFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = openFileDialogFilter,
                Title = getPokemonFile,
                InitialDirectory = dialogDirectory
            };

            if (openFileDialog.ShowDialog() == true)
            {
                //update the dialogDirectory so the next file dialog opens in the same location as this ends
                dialogDirectory = new FileInfo(openFileDialog.FileName).DirectoryName;

                bool success = _appData.LoadPokemonData(openFileDialog.FileName, out IList<string> errors);

                if (!success)
                    MessageBox.Show(errors[0], "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnOpenTrainerFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = openFileDialogFilter,
                Title = getTrainerSlotFile,
                InitialDirectory = dialogDirectory
            };

            if (openFileDialog.ShowDialog() == true)
            {
                //update the dialogDirectory so the next file dialog opens in the same location as this ends
                dialogDirectory = new FileInfo(openFileDialog.FileName).DirectoryName;

                bool success = _appData.LoadSlotData(openFileDialog.FileName, out IList<string> errors);

                if (!success)
                    MessageBox.Show(errors[0], "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnOpenMovesFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = openFileDialogFilter,
                Title = getMovesFile,
                InitialDirectory = dialogDirectory
            };

            if (openFileDialog.ShowDialog() == true)
            {
                //update the dialogDirectory so the next file dialog opens in the same location as this ends
                dialogDirectory = new FileInfo(openFileDialog.FileName).DirectoryName;

                bool success = _appData.LoadMoveData(openFileDialog.FileName, out IList<string> errors);

                if (!success)
                    MessageBox.Show(errors[0], "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnOpenItemsFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = openFileDialogFilter,
                Title = getItemsFile,
                InitialDirectory = dialogDirectory
            };

            if (openFileDialog.ShowDialog() == true)
            {
                //update the dialogDirectory so the next file dialog opens in the same location as this ends
                dialogDirectory = new FileInfo(openFileDialog.FileName).DirectoryName;

                bool success = _appData.LoadItemData(openFileDialog.FileName, out IList<string> errors);

                if (!success)
                    MessageBox.Show(errors[0], "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
