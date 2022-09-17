using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TrainerTyrant
{
    class ExternalPokemonList
    {
        [JsonProperty(PropertyName = "Pokemon")]
        public List<string> PokemonData { get; set; }

        /**
         * Returns -1 if name is not found in the list.
         */
        public int GetIndexOfPokemon(string pokemonname)
        {
            return PokemonData.FindIndex(a => a.Equals(pokemonname, StringComparison.OrdinalIgnoreCase));
        }

        public static ExternalPokemonList GenerateFromJSON(string JSON)
        {


            return null;
        }
    }

    
    class ExternalMoveList
    {
        [JsonProperty(PropertyName = "Moves")]
        public List<string> MoveData { get; set; }

        /**
         * Returns 0 if name is not found in the list. Returns 0 if movename is null.
         */
        public int GetIndexOfMove(string movename)
        {
            if (movename == null)
                return 0;

            int r = MoveData.FindIndex(a => a.Equals(movename, StringComparison.OrdinalIgnoreCase));

            return r == -1 ? 0 : r;
        }
    }

    class ExternalItemList
    {
        [JsonProperty(PropertyName = "Items")]
        public List<string> ItemData { get; set; }

        /**
         * Returns 0 if name is not found in the list. Returns 0 if itemname is null.
         */
        public int GetIndexOfItem(string itemname)
        {
            if (itemname == null)
                return 0;

            int r = ItemData.FindIndex(a => a.Equals(itemname, StringComparison.OrdinalIgnoreCase));

            return r == -1 ? 0 : r;
        }
    }

    class ExternalTrainerSlotList
    {
        [JsonProperty(PropertyName = "Slots")]
        public List<TrainerSlotData> SlotData { get; set; }

        /**
         * Returns -1 if trainer is not found.
         */
        public int GetIndexOfSlot(string trainername, int variation)
        {
            return SlotData.FindIndex(a => a.Name.Equals(trainername, StringComparison.OrdinalIgnoreCase) && a.Variation == variation);
        }
    }

    class TrainerSlotData
    {
        public string Name { get; set; }
        public int Variation { get; set; }
        [JsonProperty(PropertyName = "Export Name")]
        public string ExportName { get; set; }
    }
}
