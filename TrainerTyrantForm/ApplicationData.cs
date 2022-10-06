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
            if (File.Exists(dirLoc))
            {
                _trDataLoc = dirLoc;
                return true;
            }
            return false;
        }

        public bool LoadTRPoke(string dirLoc)
        {
            if(File.Exists(dirLoc))
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

        public bool ValidateTrainerJSON(string jsonFileLoc, out IList<string> errors)
        {
            return TrainerJSONValidator.ValidateTrainerListJSON(File.ReadAllText(jsonFileLoc), out errors);
        }

        /**
         * <summary>When the app data has been given its Narc locations, this function takes them and decompiles them to a JSON file.</summary>
         * <exception cref="InvalidDataException">Thrown when the narcs are not formatted correctly.</exception>
         * <param name="saveloc">The location for the decompiled JSON to be written to.</param>
         */
        public bool DecompileNarcs(string saveloc)
        {
            TrainerRepresentationSet export = new TrainerRepresentationSet();
            try
            {
                export.InitializeWithNarc(_trDataLoc, _trPokeLoc, _itemData, _moveData, _pokemonData, _slotData);
            }
            catch (Exception e)
            {
                throw new InvalidDataException("An error occurred when parsing the given NARCs.", e);
            }

            File.WriteAllText(saveloc, export.SerializeJSON());
            return true;
        }

        /**
         * <summary>When the app data has been given its Narc locations, this function takes them and decompiles them to a JSON file.</summary>
         * <exception cref="InvalidDataException">Thrown when the narcs are not formatted correctly.</exception>
         * <param name="saveloc">The location for the decompiled JSON to be written to.</param>
         * <param name="trDataLoc">The location of the TRData narc</param>
         * <param name="trPokeLoc">The location of the TRPoke narc</param>
         */
        public bool DecompileNarcs(string saveloc, string trDataLoc, string trPokeLoc)
        {
            TrainerRepresentationSet export = new TrainerRepresentationSet();
            try
            {
                export.InitializeWithNarc(trDataLoc, trPokeLoc, _itemData, _moveData, _pokemonData, _slotData);
            }
            catch (Exception e)
            {
                throw new InvalidDataException("An error occurred when parsing the given NARCs.", e);
            }

            File.WriteAllText(saveloc, export.SerializeJSON());
            return true;
        }

        /**
         * <summary>Take a decompiled JSON file and compile it to a NARC file.</summary>
         * <param name="fileLoc">The JSON file to compile.</param>
         */
        public bool CompileNarcs(string fileLoc)
        {
            //simple safeguard
            if (!File.Exists(fileLoc))
                return false;

            TrainerRepresentationSet export = new TrainerRepresentationSet();
            export.InitializeWithJSON(File.ReadAllText(fileLoc));
            if (export == null)
                return false;

            export.GetNarc(out byte[] TRData, out byte[] TRPoke, _itemData, _moveData, _pokemonData, _slotData);

            File.WriteAllBytes(Path.GetDirectoryName(fileLoc) + "/" + Path.GetFileNameWithoutExtension(fileLoc) + "_TRData.narc", TRData);
            File.WriteAllBytes(Path.GetDirectoryName(fileLoc) + "/" + Path.GetFileNameWithoutExtension(fileLoc) + "_TRPoke.narc", TRPoke);

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
