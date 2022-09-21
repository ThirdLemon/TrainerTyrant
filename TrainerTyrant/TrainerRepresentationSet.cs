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
        private bool _valid;
        public bool Valid { get { return _valid; } }

        public TrainerRepresentationSet()
        {
            _data = null;
            _valid = false;
        }

        public void InitializeWithJSON(string JSON)
        {
            //Don't initialize when already filled
            if (Valid)
                return;

            if (TrainerJSONValidator.ValidateTrainerListJSON(JSON))
            {
                _data = JsonConvert.DeserializeObject<List<TrainerRepresentation>>(JSON);
                _valid = true;
            }
        }

        public void InitializeWithJSON(string JSON, out IList<string> errors)
        {
            //Don't initialize when already filled
            if (Valid)
            {
                errors = new List<string> { "TrainerRepresentationSet has already been initialized." };
                return;
            }

            if (TrainerJSONValidator.ValidateTrainerListJSON(JSON, out errors))
            {
                _data = JsonConvert.DeserializeObject<List<TrainerRepresentation>>(JSON);
                _valid = true;
            }
        }

        public void InitializeWithExtractedFiles(byte[][] TRData, byte[][] TRPoke, ExternalItemList itemList, ExternalMoveList moveList, ExternalPokemonList monList, ExternalTrainerSlotList slotList)
        {
            //Don't initialize when already filled
            if (Valid)
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

            _valid = true;
        }

        public string SerializeJSON()
        {
            return JsonConvert.SerializeObject(_data, Formatting.Indented);
        }

        //Checks if there are any trainer's stored inside that overlap, returns true if there are no overlaps
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

    }
}
