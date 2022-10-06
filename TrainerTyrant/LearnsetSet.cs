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
        private bool _initialized;
        public bool Initialized { get { return _initialized; } }

        public LearnsetSet()
        {
            _data = null;
            _initialized = false;
        }

        public void InitializeWithJSON(string JSON)
        {
            //Don't initialize if already initialized.
            if (Initialized)
                return;

            //specifically only initialize if the json validates
            if (LearnsetSetJSONValidator.ValidateLearnsetSetJSON(JSON))
            {
                _data = JsonConvert.DeserializeObject<Dictionary<string, List<LevelUpMove>>>(JSON);
                _initialized = true;
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

            if (LearnsetSetJSONValidator.ValidateLearnsetSetJSON(JSON, out errors))
            {
                _data = JsonConvert.DeserializeObject<Dictionary<string, List<LevelUpMove>>>(JSON);
                _initialized = true;
            }
        }

        public string SerializeJSON()
        {
            //TODO: make it so that the level up moves only take up one line of space
            return JsonConvert.SerializeObject(_data, Formatting.Indented);
        }

        /**
         * <summary>Returns true if all pokemon in the ExternalPokemonList are found in the learnsetset</summary>
         */
        public bool ValidateAllSlotsUsed(ExternalPokemonList pokemon)
        {
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
                return null;
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
