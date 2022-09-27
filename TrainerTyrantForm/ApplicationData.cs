using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TrainerTyrant;

namespace TrainerTyrantForm
{
    class ApplicationData
    {
        private ExternalPokemonList _pokemonData;
        private string _pokemonDataLoc = "DefaultJSON/DefaultPokemon.json";
        private ExternalMoveList _moveData;
        private string _moveDataLoc = "DefaultJSON/DefaultMoves.json";
        private ExternalItemList _itemData;
        private string _itemDataLoc = "DefaultJSON/DefaultItems.json";
        private ExternalTrainerSlotList _slotData;
        private string _slotDataLoc = "DefaultJSON/DefaultSlots.json";

        //The location of the folder containing the extracted TRData narc, set by the user
        private string _trDataLoc = null;
        //The location of the folder containing the extracted TRPoke narc, set by the user
        private string _trPokeLoc = null;

        public bool CanDecompNARCs { get { return _trDataLoc != null && _trPokeLoc != null; } }

        public ApplicationData()
        {
            if (File.Exists(_pokemonDataLoc))
            {
                _pokemonData = ExternalPokemonList.DeserializeJSON(File.ReadAllText(_pokemonDataLoc));
            }
            else
            {
                _pokemonData = null;
                _pokemonDataLoc = null;
            }
            if (File.Exists(_moveDataLoc))
            {
                _moveData = ExternalMoveList.DeserializeJSON(File.ReadAllText(_moveDataLoc));
            }
            else
            {
                _moveData = null;
                _moveDataLoc = null;
            }
            if (File.Exists(_itemDataLoc))
            {
                _itemData = ExternalItemList.DeserializeJSON(File.ReadAllText(_itemDataLoc));
            }
            else
            {
                _itemData = null;
                _itemDataLoc = null;
            }
            if (File.Exists(_slotDataLoc))
            {
                _slotData = ExternalTrainerSlotList.DeserializeJSON(File.ReadAllText(_slotDataLoc));
            }
            else
            {
                _slotData = null;
                _slotDataLoc = null;
            }
            _trDataLoc = _trPokeLoc = null;
        }

        /**
         * Returns true if successful, if it returns false, it will also return errors.
         */
        public bool LoadPokemonData(string fileLoc, out IList<string> errors)
        {
            //if the file exists
            if (File.Exists(fileLoc))
            {
                ExternalPokemonList newData = ExternalPokemonList.DeserializeJSON(File.ReadAllText(fileLoc), out errors);
                //If the file did not deserialize properly, stop loading new data and pre-emptively return false.
                if (newData == null)
                    return false;
                //If execution reached here, set the values to match the new data.
                _pokemonData = newData;
                _pokemonDataLoc = fileLoc;
                return true;
            }
            else
            {
                //if the file does not exist, return an error.
                errors = new List<string> { "File does not exist." };
                return false;
            }
        }

        /**
         * Returns true if successful, if it returns false, it will also return errors.
         */
        public bool LoadItemData(string fileLoc, out IList<string> errors)
        {
            //if the file exists
            if (File.Exists(fileLoc))
            {
                ExternalItemList newData = ExternalItemList.DeserializeJSON(File.ReadAllText(fileLoc), out errors);
                //If the file did not deserialize properly, stop loading new data and pre-emptively return false.
                if (newData == null)
                    return false;
                //If execution reached here, set the values to match the new data.
                _itemData = newData;
                _itemDataLoc = fileLoc;
                return true;
            }
            else
            {
                //if the file does not exist, return an error.
                errors = new List<string> { "File does not exist." };
                return false;
            }
        }

        /**
         * Returns true if successful, if it returns false, it will also return errors.
         */
        public bool LoadMoveData(string fileLoc, out IList<string> errors)
        {
            //if the file exists
            if (File.Exists(fileLoc))
            {
                ExternalMoveList newData = ExternalMoveList.DeserializeJSON(File.ReadAllText(fileLoc), out errors);
                //If the file did not deserialize properly, stop loading new data and pre-emptively return false.
                if (newData == null)
                    return false;
                //If execution reached here, set the values to match the new data.
                _moveData = newData;
                _moveDataLoc = fileLoc;
                return true;
            }
            else
            {
                //if the file does not exist, return an error.
                errors = new List<string> { "File does not exist." };
                return false;
            }
        }

        /**
         * Returns true if successful, if it returns false, it will also return errors.
         */
        public bool LoadSlotData(string fileLoc, out IList<string> errors)
        {
            //if the file exists
            if (File.Exists(fileLoc))
            {
                ExternalTrainerSlotList newData = ExternalTrainerSlotList.DeserializeJSON(File.ReadAllText(fileLoc), out errors);
                //If the file did not deserialize properly, stop loading new data and pre-emptively return false.
                if (newData == null)
                    return false;
                //If execution reached here, set the values to match the new data.
                _slotData = newData;
                _slotDataLoc = fileLoc;
                return true;
            }
            else
            {
                //if the file does not exist, return an error.
                errors = new List<string> { "File does not exist." };
                return false;
            }
        }

        public bool LoadTRData(string dirLoc)
        {
            if (Directory.Exists(dirLoc))
            {
                _trDataLoc = dirLoc;
                return true;
            }
            return false;
        }

        public bool LoadTRPoke(string dirLoc)
        {
            if(Directory.Exists(dirLoc))
            {
                _trPokeLoc = dirLoc;
                return true;
            }
            return false;
        }

        public bool ValidateExternalData(out string error)
        {

            //If any of the definitions don't exist, don't pass
            if (_itemData == null || _moveData == null || _pokemonData == null || _slotData == null)
            {
                if (_itemData == null)
                    error = "There is no item data loaded.";
                else if (_moveData == null)
                    error = "There is no move data loaded.";
                else if (_pokemonData == null)
                    error = "There is no pokemon data loaded.";
                else
                    error = "There is no trainer data loaded.";
                return false;
            }
            error = null;
            return true;
        }

        public bool ValidateNarcFolders(out string error)
        {
            //If either does not exist, don't pass
            if (!Directory.Exists(_trDataLoc) || !Directory.Exists(_trPokeLoc))
            {
                error = "One of the specified folders do not exist.";
                return false;
            }

            FileInfo[] TRDataFiles = new DirectoryInfo(_trDataLoc).GetFiles();
            FileInfo[] TRPokeFiles = new DirectoryInfo(_trPokeLoc).GetFiles();

            //If the given folders do not contain enough data to match the slot data, do not pass
            if ((TRDataFiles.Length != _slotData.SlotData.Count + 1) || (TRPokeFiles.Length != _slotData.SlotData.Count + 1))
            {
                error = "The amount of files within the given folders does not sync with the trainer JSON.";
                return false;
            }

            //Otherwise, valid.
            error = null;
            return true;
        }

        public bool ValidateTrainerJSON(string jsonFileLoc, out IList<string> errors)
        {
            return TrainerJSONValidator.ValidateTrainerListJSON(File.ReadAllText(jsonFileLoc), out errors);
        }

        //Should only run when confirmed valid
        private void ConvertNarcFoldersToByte(out byte[][] TRData, out byte[][]TRPoke)
        {
            FileInfo[] TRDataFiles = new DirectoryInfo(_trDataLoc).GetFiles();
            FileInfo[] TRPokeFiles = new DirectoryInfo(_trPokeLoc).GetFiles();

            //set these to fill the length of the folders.
            TRData = new byte[TRDataFiles.Length][];
            TRPoke = new byte[TRPokeFiles.Length][];

            //pull out data from every file
            for(int fileNum = 0; fileNum < TRDataFiles.Length; fileNum++)
            {
                byte[] temp = File.ReadAllBytes(TRDataFiles[fileNum].FullName);
                //in order to save memory, keep only the first 20 bytes of the file, as that is all that will ever be used.
                TRData[fileNum] = temp.Take(20).ToArray();
                //repeat the same thing with trpoke, but this time keeping up to 6 * 18 bytes(108) to match the max trpoke will hit.
                temp = File.ReadAllBytes(TRPokeFiles[fileNum].FullName);
                TRPoke[fileNum] = temp.Take(108).ToArray();
            }
        }

        public bool DecompileNarcFolders(string saveLoc)
        {
            ConvertNarcFoldersToByte(out byte[][] TRData, out byte[][] TRPoke);

            TrainerRepresentationSet export = new TrainerRepresentationSet();
            export.InitializeWithExtractedFiles(TRData, TRPoke, _itemData, _moveData, _pokemonData, _slotData);

            File.WriteAllText(saveLoc, export.SerializeJSON());

            return true;
        }

        public bool CompileNarcFolder(string fileLoc)
        {
            //simple safeguard
            if (!File.Exists(fileLoc))
                return false;

            TrainerRepresentationSet export = new TrainerRepresentationSet();
            export.InitializeWithJSON(File.ReadAllText(fileLoc));
            if (export == null)
                return false;

            export.GetByteData(out byte[][] TRData, out byte[][] TRPoke, _itemData, _moveData, _pokemonData, _slotData);

            //don't rewrite this a million times.
            string strippedName = Path.GetFileNameWithoutExtension(fileLoc);
            //get folder names for TRData and TRPoke
            string TRDataFolder = Path.GetDirectoryName(fileLoc) + "/" + strippedName + "_TRData";
            string TRPokeFolder = Path.GetDirectoryName(fileLoc) + "/" + strippedName + "_TRPoke";
            //create the directory
            if (!Directory.Exists(TRDataFolder))
                Directory.CreateDirectory(TRDataFolder);
            if (!Directory.Exists(TRPokeFolder))
                Directory.CreateDirectory(TRPokeFolder);
            //get the amount of zeroes that need to be padded
            int maxNumLength = TRData.Length.ToString().Length;

            for (int dataNum = 0; dataNum < TRData.Length; dataNum++)
            {
                File.WriteAllBytes(TRDataFolder + "/" + strippedName + "_" + dataNum.ToString().PadLeft(maxNumLength, '0'), TRData[dataNum]);
                File.WriteAllBytes(TRPokeFolder + "/" + strippedName + "_" + dataNum.ToString().PadLeft(maxNumLength, '0'), TRPoke[dataNum]);
            }

            return true;
        }

        public bool AlterJSON(string sourceFile, string addedFile)
        {
            //simple safeguard
            if (!File.Exists(sourceFile) || !File.Exists(addedFile))
                return false;

            TrainerRepresentationSet source = new TrainerRepresentationSet();
            source.InitializeWithJSON(File.ReadAllText(sourceFile));
            if (source == null)
                return false;

            bool success = source.AlterWithJSON(File.ReadAllText(addedFile));

            if (success == false)
                return false;

            File.WriteAllText(sourceFile, source.SerializeJSON());
            return true;
        }
    }
}
