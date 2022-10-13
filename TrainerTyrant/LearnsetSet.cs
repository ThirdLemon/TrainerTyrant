using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NARCLord;

namespace TrainerTyrant
{
    /**
     * <summary>A complete representation of every pokemon's learnset.</summary>
     */
    public class LearnsetSet
    {
        private Dictionary<string, List<LevelUpMove>> _data;
        public bool Initialized { get; private set; }

        public LearnsetSet()
        {
            _data = null;
            Initialized = false;
        }

        public void InitializeWithJSON(string JSON)
        {
            //Don't initialize if already initialized.
            if (Initialized)
                return;

            //specifically only initialize if the json validates
            if (JSONValidatorLearnsetSet.ValidateLearnsetSetJSON(JSON))
            {
                _data = JsonConvert.DeserializeObject<Dictionary<string, List<LevelUpMove>>>(JSON);
                Initialized = true;
            }
        }

        public void InitializeWithJSON(string JSON, out IList<string> errors)
        {
            //don't initialize if already initialized
            if (Initialized)
            {
                errors = new List<string> { "LearnsetSet has already been initialized." };
                return;
            }

            if (JSONValidatorLearnsetSet.ValidateLearnsetSetJSON(JSON, out errors))
            {
                _data = JsonConvert.DeserializeObject<Dictionary<string, List<LevelUpMove>>>(JSON);
                Initialized = true;
            }
        }

        public void InitializeWithNarc(string narcFile, ExternalMoveList moves, ExternalPokemonList pokemon)
        {
            //Don't initialize when already filled
            if (Initialized)
                return;

            NARC learnsetNarc;

            try
            {
                learnsetNarc = NARC.Build(narcFile);
            }
            catch (Exception e)
            {
                throw new ArgumentException("An error occured while parsing the narc.", e);
            }

            _data = new Dictionary<string, List<LevelUpMove>>();

            if (pokemon.PokemonData.Count != learnsetNarc.Length)
                throw new ArgumentException("The narc file and external pokemon list have differing pokemon counts. " + pokemon.PokemonData.Count + " vs " + learnsetNarc.Length);

            for (int monNum = 1; monNum < learnsetNarc.Length; monNum++)
            {
                _data.Add(pokemon.GetPokemon(monNum), BuildLearnsetFromFile(learnsetNarc[monNum], moves));
            }

            Initialized = true;
        }

        public string SerializeJSON()
        {
            //TODO: make it so that the level up moves only take up one line of space
            string JSON = JsonConvert.SerializeObject(_data, Formatting.Indented);

            return JSON.Replace("{\r\n      \"Level", "{ \"Level").Replace(",\r\n      \"Move", ", \"Move").Replace("\"\r\n    }", "\" }");
        }

        /**
         * <summary>Returns true if all pokemon in the ExternalPokemonList are found in the learnsetset</summary>
         */
        public bool ValidateAllSlotsUsed(ExternalPokemonList pokemon)
        {
            if (pokemon.PokemonData.Count != _data.Count + 1)
                return false;

            for (int mon = 1; mon < pokemon.PokemonData.Count; mon++) 
                if (!_data.ContainsKey(pokemon.PokemonData[mon]))
                    return false;

            return true;
        }

        public byte[] GetNarcData(ExternalMoveList moves, ExternalPokemonList pokemon)
        {
            //if it does not validate, don't pass
            if (!ValidateAllSlotsUsed(pokemon))
            {
                throw new ArgumentException("The file does not sync with the given pokemon definition file.");
            }

            //Start
            NARC outputNarc = new NARC();

            //add the placeholder bytes for the 'zeroth' pokemon.
            outputNarc.Add(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF });
            //go through each pokemon in the pokemon list, and add its moves
            for(int monName = 1; monName < pokemon.PokemonData.Count; monName++)
            {
                outputNarc.Add(GetLearnsetByteRepresentation(_data[pokemon.PokemonData[monName]], moves));
            }

            return outputNarc.Compile();
        }

        private static byte[] GetLearnsetByteRepresentation(List<LevelUpMove> learnset, ExternalMoveList moves)
        {
            //each levelupmove is represented by 2 bytes for the level, and 2 bytes for the move. at the end of the file is a terminating indication of four FFs.
            byte[] to_return = new byte[4 * (learnset.Count + 1)];

            for(int move = 0; move < learnset.Count; move++)
            {
                int moveID = moves.GetIndexOfMove(learnset[move].Move);
                to_return[(move * 4)] = (byte)(moveID - moveID / 256 * 256);
                to_return[(move * 4) + 1] = (byte)(moveID / 256);
                //As level can never be above 100, the fourth byte is never used.
                to_return[(move * 4) + 2] = (byte)learnset[move].Level;
                to_return[(move * 4) + 3] = 0;
            }
            //final four bytes are FF
            for (int i = 0; i < 4; i++)
                to_return[learnset.Count * 4 + i] = 0xFF;

            return to_return;
        }

        private static List<LevelUpMove> BuildLearnsetFromFile(byte[] file, ExternalMoveList moves)
        {
            List<LevelUpMove> toReturn = new List<LevelUpMove>();

            for(int move = 0; move < (file.Length / 4) - 1; move++)
            {
                LevelUpMove newMove = new LevelUpMove
                {
                    Move = moves.GetMove(file[move * 4] + file[move * 4 + 1] * 256),
                    Level = file[move * 4 + 2]
                };
                toReturn.Add(newMove);
            }

            return toReturn;
        }

        /**
         * <returns>An array of up to 4 moves that the pokemon would know at the given level. Returns null if the pokemon is not found.</returns>
         */
        public List<string> GetMovesetAtLevel(string pokemon, int level)
        {
            if (!_data.ContainsKey(pokemon))
                return null;

            List<string> toReturn = new List<string>();

            for(int i = 0; i < _data[pokemon].Count; i++)
            {

                //We assume that every learnset is ordered from lowest level to highest
                if (_data[pokemon][i].Level > level)
                    break;

                toReturn.Add(_data[pokemon][i].Move);
                if (toReturn.Count > 4)
                    toReturn.RemoveAt(0);
            }

            return toReturn;
        }
    }

    /**
     * <summary>The level and move learned at a specific level</summary>
     */
    class LevelUpMove
    {
        public int Level { get; set; }
        public string Move { get; set; }
    }
}
