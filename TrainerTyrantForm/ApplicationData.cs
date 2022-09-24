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

    }
}
