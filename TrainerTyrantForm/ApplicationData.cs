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

        //The main data;
        private TrainerRepresentationSet _mainData;

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

            _mainData = null;
        }

    }
}
