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
        private static readonly string saveJSONFileDialogFilter = "JSON files (*.json)|*.json";
        private static readonly string getPokemonFile = "Open Pokemon JSON";
        private static readonly string getTrainerSlotFile = "Open Trainer JSON";
        private static readonly string getMovesFile = "Open Moves JSON";
        private static readonly string getItemsFile = "Open Items JSON";
        private static readonly string getTRDataFolder = "Open TRData Folder";
        private static readonly string getTRPokeFolder = "Open TRPoke Folder";
        private static readonly string saveDecompedJSON = "Save JSON File";
        private static readonly string getDecompedJSON = "Open JSON File";

        private string jsonDialogDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private string narcDialogDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private string saveDialogDirectory = AppDomain.CurrentDomain.BaseDirectory;

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

            CommonOpenFileDialog openFolderDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = getTRDataFolder,
                InitialDirectory = narcDialogDirectory
            };

            if (openFolderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                //Have to use full name here to get the desired effect. Why? Unsure.
                narcDialogDirectory = new DirectoryInfo(openFolderDialog.FileName).Parent.FullName;

                _appData.LoadTRData(openFolderDialog.FileName);

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

            CommonOpenFileDialog openFolderDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = getTRPokeFolder,
                InitialDirectory = narcDialogDirectory
            };

            if (openFolderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                //Have to use full name here to get the desired effect. Why? Unsure.
                narcDialogDirectory = new DirectoryInfo(openFolderDialog.FileName).Parent.FullName;

                _appData.LoadTRPoke(openFolderDialog.FileName);

                if (_appData.CanDecompNARCs)
                    btnDecompileNarcs.IsEnabled = true;
            }
        }

        private void btnDecompileNarcs_Click(object sender, RoutedEventArgs e)
        {
            //Check that all external data is loaded. 
            if(!_appData.ValidateExternalData(out string error))
            {
                MessageBox.Show(error, "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //Check that the narc folders are valid.
            if (!_appData.ValidateNarcFolders(out error))
            {
                MessageBox.Show(error, "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = saveDecompedJSON,
                Filter = saveJSONFileDialogFilter,
                InitialDirectory = saveDialogDirectory
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                _appData.DecompileNarcFolders(saveFileDialog.FileName);
            }
        }

        private void btnCompileNarcs_Click(object sender, RoutedEventArgs e)
        {
            //Check that all external data is loaded. 
            if (!_appData.ValidateExternalData(out string error))
            {
                MessageBox.Show(error, "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = getDecompedJSON,
                Filter = openJSONFileDialogFilter,
                InitialDirectory = jsonDialogDirectory
            };

            if (openFileDialog.ShowDialog() == true)
            {
                jsonDialogDirectory = new FileInfo(openFileDialog.FileName).DirectoryName;

                bool validJSON = _appData.ValidateTrainerJSON(openFileDialog.FileName, out IList<string> errors);

                //if it does not validate, show an error message and exit out early
                if (validJSON == false)
                {
                    if (errors.Count == 1)
                    {
                        MessageBox.Show("Provided JSON did not validate.\n" + errors[0], "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        string errorMessage = "Provided Json did not validate.\n" + errors.Count + " errors occurred. Check the log file.";
                        File.WriteAllLines("Log.txt", errors);
                        
                        MessageBox.Show(errorMessage, "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    return;
                }
                else
                {
                    bool success = _appData.CompileNarcFolder(openFileDialog.FileName);

                    if (success == false)
                    {
                        MessageBox.Show("An error occurred while compiling the narc.", "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }
    }
}
