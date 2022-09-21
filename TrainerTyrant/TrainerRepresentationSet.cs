using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TrainerTyrant
{
    public class TrainerRepresentationSet
    {
        private List<TrainerRepresentation> _data;
        private bool _initialized;
        public bool Initialized { get { return _initialized; } }

        public TrainerRepresentationSet()
        {
            _data = null;
            _initialized = false;
        }

        public void InitializeWithJSON(string JSON)
        {
            //Don't initialize when already filled
            if (Initialized)
                return;

            if (TrainerJSONValidator.ValidateTrainerListJSON(JSON))
            {
                _data = JsonConvert.DeserializeObject<List<TrainerRepresentation>>(JSON);
                _initialized = true;
            }
        }

        public void InitializeWithJSON(string JSON, out IList<string> errors)
        {
            //Don't initialize when already filled
            if (Initialized)
            {
                errors = new List<string> { "TrainerRepresentationSet has already been initialized." };
                return;
            }

            if (TrainerJSONValidator.ValidateTrainerListJSON(JSON, out errors))
            {
                _data = JsonConvert.DeserializeObject<List<TrainerRepresentation>>(JSON);
                _initialized = true;
            }
        }

        public void InitializeWithExtractedFiles(byte[][] TRData, byte[][] TRPoke, ExternalItemList itemList, ExternalMoveList moveList, ExternalPokemonList monList, ExternalTrainerSlotList slotList)
        {
            //Don't initialize when already filled
            if (Initialized)
                return;

            if (TRData.Length != TRPoke.Length)
                throw new ArgumentException("TRData and TRPoke must be the same length.");
            if (TRData.Length < 1)
                throw new ArgumentException("TRData and TRPoke must have more than 1 file.");

            for (int trainer = 1; trainer < TRData.Length; trainer++)
            {
                TrainerRepresentation tr = TrainerRepresentation.BuildFromBytes(TRData[trainer], TRPoke[trainer], trainer, itemList, moveList, monList, slotList);
                _data.Add(tr);
            }

            _initialized = true;
        }

        public string SerializeJSON()
        {
            return JsonConvert.SerializeObject(_data, Formatting.Indented);
        }

        //Checks if there are any trainer's stored inside that overlap, returns true if there are no overlaps. also returns false if the data doesn't show up in the list.
        public bool ValidateNoDuplicates(ExternalTrainerSlotList slots)
        {
            int[] occupiedSlots = new int[_data.Count];

            for (int trainer = 0; trainer < _data.Count; trainer++)
            {
                int slotNum = _data[trainer].GetSlotID(slots);

                if (occupiedSlots.Contains(slotNum))
                    return false;

                occupiedSlots[trainer] = slotNum;
            }

            return true;
        }

        public bool ValidateAllSlotsUsed(ExternalTrainerSlotList slots)
        {
            //If there is a mismatch, neither should be used.
            if (_data.Count != slots.SlotData.Count)
                return false;

            for (int num = 0; num < _data.Count; num++)
            {
                bool found = false;
                for (int trainer = 0; trainer < _data.Count; trainer++)
                    if (_data[trainer].GetSlotID(slots) == num+1)
                    {
                        found = true;
                        break;
                    }
                if (!found)
                    return false;
            }

            return true;  
        }

        //Sorts the data
        private void SortData(ExternalTrainerSlotList slots)
        {
            _data.Sort((x, y) => x.GetSlotID(slots).CompareTo(y.GetSlotID(slots)));
        }

        /**
         * The set must pass both validation checks (ValidateNoDuplicates and ValidateAllSlotsUsed) for this to execute. It will simply return null if it doesn't pass these.
         */
        public void GetByteData(out byte[][] TRData, out byte[][] TRPoke, ExternalItemList items, ExternalMoveList moves, ExternalPokemonList pokemon, ExternalTrainerSlotList slots)
        {
            //If it doesn't pass these validation checks, then the output would be nonsense.
            if (!ValidateAllSlotsUsed(slots) || !ValidateNoDuplicates(slots))
            {
                TRData = null;
                TRPoke = null;
                return;
            }

            TRData = new byte[_data.Count + 1][];
            TRPoke = new byte[_data.Count + 1][];

            //Sort the data.
            SortData(slots);

            //Fill the placeholder zeroth slot with the placeholder data
            TrainerRepresentation.GetPlaceholderBytes(out TRData[0], out TRPoke[0]);

            //fill in the rest of the data
            for (int trainer = 0; trainer < _data.Count; trainer++)
            {
                TRData[trainer + 1] = _data[trainer].GetTrainerBytes(items);
                TRPoke[trainer + 1] = _data[trainer].GetPokemonBytes(items, moves, pokemon);
            }
        }
    }
}
