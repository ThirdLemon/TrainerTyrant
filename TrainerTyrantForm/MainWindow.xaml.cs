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

namespace TrainerTyrantForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string openJSONFileDialogFilter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
        private static readonly string openNARCFileDialogFilter = "NARC files (*.narc)|*.narc|All files (*.*)|*.*";
        private static readonly string openTxtFileDialogFilter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
        private static readonly string saveJSONFileDialogFilter = "JSON files (*.json)|*.json";
        private static readonly string getPokemonFile = "Open Pokemon JSON";
        private static readonly string getTrainerSlotFile = "Open Trainer JSON";
        private static readonly string getMovesFile = "Open Moves JSON";
        private static readonly string getItemsFile = "Open Items JSON";
        private static readonly string getTRDataNARC = "Open TRData NARC";
        private static readonly string getTRPokeNARC = "Open TRPoke NARC";
        private static readonly string getLearnsetNARC = "Open Learnset NARC";
        private static readonly string saveDecompedJSON = "Save JSON File";
        private static readonly string getDecompedJSON = "Open JSON File";
        private static readonly string getSourceJSON = "Open Base JSON File";
        private static readonly string getSecondaryJSON = "Open JSON File to be Merged";
        private static readonly string getTemplateTextFile = "Open Text File";

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

        private void btnDecompileNarcs_Click(object sender, RoutedEventArgs e)
        {
            //Check that all external data is loaded. 
            if (!_appData.ValidateExternalData(out string error))
            {
                MessageBox.Show(error, "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            OpenFileDialog openTRDataDialog = new OpenFileDialog
            {
                Title = getTRDataNARC,
                Filter = openNARCFileDialogFilter,
                InitialDirectory = narcDialogDirectory
            };

            if (openTRDataDialog.ShowDialog() == true)
            {
                OpenFileDialog openTRPokeDialog = new OpenFileDialog
                {
                    Title = getTRPokeNARC,
                    Filter = openNARCFileDialogFilter,
                    InitialDirectory = narcDialogDirectory
                };

                if (openTRPokeDialog.ShowDialog() == true)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Title = saveDecompedJSON,
                        Filter = saveJSONFileDialogFilter,
                        InitialDirectory = saveDialogDirectory
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        try
                        {
                            _appData.DecompileNarcs(saveFileDialog.FileName, openTRDataDialog.FileName, openTRPokeDialog.FileName);
                        }
                        catch (InvalidDataException err)
                        {
                            MessageBox.Show(err.Message, "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
                            File.WriteAllText("Log.txt", err.ToString());
                        }
                    }
                }
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
                    try
                    {
                        _appData.CompileNarcs(openFileDialog.FileName);
                    }
                    catch(Exception err)
                    {
                        MessageBox.Show(err.Message + "\nCheck the log file.", "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
                        File.WriteAllText("Log.txt", err.ToString());
                    }
                }
            }
        }

        private void btnAlterJSON_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = getSourceJSON,
                Filter = openJSONFileDialogFilter,
                InitialDirectory = jsonDialogDirectory
            };

            if (openFileDialog.ShowDialog() == true)
            {
                OpenFileDialog secondaryFileDialog = new OpenFileDialog
                {
                    Title = getSecondaryJSON,
                    Filter = openJSONFileDialogFilter,
                    InitialDirectory = jsonDialogDirectory
                };

                if (secondaryFileDialog.ShowDialog() == true)
                {
                    bool validJSON = _appData.ValidateTrainerJSON(openFileDialog.FileName, out IList<string> errors);

                    //if it does not validate, show an error message and exit out early
                    if (validJSON == false)
                    {
                        if (errors.Count == 1)
                        {
                            MessageBox.Show("Provided source JSON did not validate.\n" + errors[0], "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            string errorMessage = "Provided source Json did not validate.\n" + errors.Count + " errors occurred. Check the log file.";
                            File.WriteAllLines("Log.txt", errors);

                            MessageBox.Show(errorMessage, "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        return;
                    }
                    else
                    {
                        bool success = _appData.AlterJSON(openFileDialog.FileName, secondaryFileDialog.FileName);

                        if (success == false)
                        {
                            MessageBox.Show("An error occurred while altering the JSON.", "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
        }

        private void btnDecompileLearnset_Click(object sender, RoutedEventArgs e)
        {
            //Check that all external data is loaded. 
            if (!_appData.ValidateExternalData(out string error))
            {
                MessageBox.Show(error, "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            OpenFileDialog openNarcDialog = new OpenFileDialog
            {
                Title = getLearnsetNARC,
                Filter = openNARCFileDialogFilter,
                InitialDirectory = narcDialogDirectory
            };

            if (openNarcDialog.ShowDialog() == true)
            {
                SaveFileDialog saveJSONDialog = new SaveFileDialog
                {
                    Title = saveDecompedJSON,
                    Filter = saveJSONFileDialogFilter,
                    InitialDirectory = saveDialogDirectory
                };

                if (saveJSONDialog.ShowDialog() == true)
                {
                    try
                    {
                        _appData.DecompileLearnsets(saveJSONDialog.FileName, openNarcDialog.FileName);
                    }
                    catch (InvalidDataException err)
                    {
                        MessageBox.Show(err.Message, "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
                        File.WriteAllText("Log.txt", err.ToString());
                    }
                }
            }
        }

        private void btnCompileLearnset_Click(object sender, RoutedEventArgs e)
        {
            //Check that all external data is loaded. 
            if (!_appData.ValidateExternalData(out string error))
            {
                MessageBox.Show(error, "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            OpenFileDialog openJSONDialog = new OpenFileDialog
            {
                Title = getDecompedJSON,
                Filter = openJSONFileDialogFilter,
                InitialDirectory = jsonDialogDirectory
            };

            if (openJSONDialog.ShowDialog() == true)
            {
                jsonDialogDirectory = new FileInfo(openJSONDialog.FileName).DirectoryName;

                bool validJSON = _appData.ValidateLearnsetJSON(openJSONDialog.FileName, out IList<string> errors);

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
                    try
                    {
                        _appData.CompileLearnsets(openJSONDialog.FileName);
                    }
                    catch(Exception err)
                    {
                        MessageBox.Show(err.Message + "\nCheck the log file.", "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
                        File.WriteAllText("Log.txt", err.ToString());
                    }
                }
            }
        }

        private void btnExportDocumentation_Click(object sender, RoutedEventArgs e)
        {
            //Check that all external data is loaded. 
            if (!_appData.ValidateExternalData(out string error))
            {
                MessageBox.Show(error, "TrainerTyrant", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            OpenFileDialog openJSONDialog = new OpenFileDialog
            {
                Title = getDecompedJSON,
                Filter = openJSONFileDialogFilter,
                InitialDirectory = jsonDialogDirectory
            };

            if (openJSONDialog.ShowDialog() == true)
            {
                OpenFileDialog openTemplateDialog = new OpenFileDialog
                {
                    Title = getTemplateTextFile,
                    Filter = openTxtFileDialogFilter
                };

                if (openTemplateDialog.ShowDialog() == true)
                {
                    _appData.ProduceTrainerDataDocumentation(openJSONDialog.FileName, openTemplateDialog.FileName);
                }
            }
        }
    }
}
