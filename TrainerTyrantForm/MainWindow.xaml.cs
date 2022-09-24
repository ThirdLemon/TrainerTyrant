using System;
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
using Microsoft.WindowsAPICodePack.Dialogs;

namespace TrainerTyrantForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string openJSONFileDialogFilter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
        private static readonly string getPokemonFile = "Open Pokemon JSON";
        private static readonly string getTrainerSlotFile = "Open Trainer JSON";
        private static readonly string getMovesFile = "Open Moves JSON";
        private static readonly string getItemsFile = "Open Items JSON";
        private static readonly string getTRDataFolder = "Open TRData Folder";
        private static readonly string getTRPokeFolder = "Open TRPoke Folder";

        private string jsonDialogDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private string narcDialogDirectory = AppDomain.CurrentDomain.BaseDirectory;

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
                Filter = openJSONFileDialogFilter,
                Title = getPokemonFile,
                InitialDirectory = jsonDialogDirectory
            };

            if (openFileDialog.ShowDialog() == true)
            {
                //update the dialogDirectory so the next file dialog opens in the same location as this ends
                jsonDialogDirectory = new FileInfo(openFileDialog.FileName).DirectoryName;

                bool success = _appData.LoadPokemonData(openFileDialog.FileName, out IList<string> errors);

                if (!success)
                    MessageBox.Show(errors[0], "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnOpenTrainerFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = openJSONFileDialogFilter,
                Title = getTrainerSlotFile,
                InitialDirectory = jsonDialogDirectory
            };

            if (openFileDialog.ShowDialog() == true)
            {
                //update the dialogDirectory so the next file dialog opens in the same location as this ends
                jsonDialogDirectory = new FileInfo(openFileDialog.FileName).DirectoryName;

                bool success = _appData.LoadSlotData(openFileDialog.FileName, out IList<string> errors);

                if (!success)
                    MessageBox.Show(errors[0], "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnOpenMovesFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = openJSONFileDialogFilter,
                Title = getMovesFile,
                InitialDirectory = jsonDialogDirectory
            };

            if (openFileDialog.ShowDialog() == true)
            {
                //update the dialogDirectory so the next file dialog opens in the same location as this ends
                jsonDialogDirectory = new FileInfo(openFileDialog.FileName).DirectoryName;

                bool success = _appData.LoadMoveData(openFileDialog.FileName, out IList<string> errors);

                if (!success)
                    MessageBox.Show(errors[0], "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnOpenItemsFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = openJSONFileDialogFilter,
                Title = getItemsFile,
                InitialDirectory = jsonDialogDirectory
            };

            if (openFileDialog.ShowDialog() == true)
            {
                //update the dialogDirectory so the next file dialog opens in the same location as this ends
                jsonDialogDirectory = new FileInfo(openFileDialog.FileName).DirectoryName;

                bool success = _appData.LoadItemData(openFileDialog.FileName, out IList<string> errors);

                if (!success)
                    MessageBox.Show(errors[0], "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnLoadTRData_Click(object sender, RoutedEventArgs e)
        {
            //If the operating system does not support the file dialog, show an error and exit out. In the future this will be updated to just use a support, worse folder picker.
            if(!CommonFileDialog.IsPlatformSupported)
            {
                MessageBox.Show("Operating system does not support browsing for folders.", "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = getTRDataFolder,
                InitialDirectory = narcDialogDirectory
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                //Have to use full name here to get the desired effect. Why? Unsure.
                narcDialogDirectory = new DirectoryInfo(dialog.FileName).Parent.FullName;

                _appData.LoadTRData(dialog.FileName);

                if (_appData.CanDecompNARCs)
                    btnDecompileNarcs.IsEnabled = true;
            }
        }

        private void btnLoadTRPoke_Click(object sender, RoutedEventArgs e)
        {
            //If the operating system does not support the file dialog, show an error and exit out. In the future this will be updated to just use a support, worse folder picker.
            if (!CommonFileDialog.IsPlatformSupported)
            {
                MessageBox.Show("Operating system does not support browsing for folders.", "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = getTRPokeFolder,
                InitialDirectory = narcDialogDirectory
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                //Have to use full name here to get the desired effect. Why? Unsure.
                narcDialogDirectory = new DirectoryInfo(dialog.FileName).Parent.FullName;

                _appData.LoadTRPoke(dialog.FileName);

                if (_appData.CanDecompNARCs)
                    btnDecompileNarcs.IsEnabled = true;
            }
        }

        private void btnDecompileNarcs_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
