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

        //Should only run when confirmed valid
        private void ConvertNarcFolders(out byte[][] TRData, out byte[][]TRPoke)
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
    }
}
