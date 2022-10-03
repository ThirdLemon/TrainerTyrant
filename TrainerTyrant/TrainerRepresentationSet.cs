using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NARCLord;

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
                throw new ArgumentException("TRData and TRPoke must have the same amount of files.");
            if (TRData.Length < 1)
                throw new ArgumentException("TRData and TRPoke must have more than 1 file.");

            _data = new List<TrainerRepresentation>();

            for (int trainer = 1; trainer < TRData.Length; trainer++)
            {
                TrainerRepresentation tr = TrainerRepresentation.BuildFromBytes(TRData[trainer], TRPoke[trainer], trainer, itemList, moveList, monList, slotList);
                _data.Add(tr);
            }

            _initialized = true;
        }

        public void InitializeWithNarc(string TRDataFile, string TRPokeFile, ExternalItemList itemList, ExternalMoveList moveList, ExternalPokemonList pokemonList, ExternalTrainerSlotList slotList)
        {
            //Don't initialize when already filled
            if (Initialized)
                return;

            NARC trDataNarc, trPokeNarc;

            try
            {
                trDataNarc = NARC.Build(TRDataFile);
                trPokeNarc = NARC.Build(TRPokeFile);
            }
            catch (Exception e)
            {
                throw new ArgumentException("An error occured while parsing the narc.", e);
            }

            if (trDataNarc.Length != trPokeNarc.Length)
                throw new ArgumentException("TRData and TRPoke must have the same amount of files.");
            if (trDataNarc.Length < 1)
                throw new ArgumentException("TRData and TRPoke must have more than 1 file.");

            _data = new List<TrainerRepresentation>();

            for (int trainer = 1; trainer < trDataNarc.Length; trainer++)
            {
                TrainerRepresentation tr = TrainerRepresentation.BuildFromBytes(trDataNarc[trainer], trPokeNarc[trainer], trainer, itemList, moveList, pokemonList, slotList);
                _data.Add(tr);
            }

            _initialized = true;
        }

        public bool AlterWithJSON(string JSON)
        {
            //Doesn't work when there is not pre-existing data
            if (!Initialized)
                return false;

            if (TrainerJSONValidator.ValidateTrainerListJSON(JSON))
            {
                List<TrainerRepresentation> newData = JsonConvert.DeserializeObject<List<TrainerRepresentation>>(JSON);
                foreach(TrainerRepresentation trainer in newData)
                {
                    int matchIndex;
                    if (trainer.TrainerData.Identification.NumberID != -1)
                        matchIndex = _data.FindIndex(i => i.TrainerData.Identification.NumberID == trainer.TrainerData.Identification.NumberID);
                    // if the trainer has a number id of -1, it must have a nameid, otherwise it wouldn't pass the validation
                    else
                        //Assumably the check that its not null will prevent the other checks from running and throwing null exception errors due to the nature of && 
                        matchIndex = _data.FindIndex(i => i.TrainerData.Identification.NameID != null && i.TrainerData.Identification.NameID.Name.Equals(trainer.TrainerData.Identification.NameID.Name) &&
                            i.TrainerData.Identification.NameID.Variation == trainer.TrainerData.Identification.NameID.Variation);

                    //if no copy is found, simply add the trainer to the list
                    if (matchIndex == -1)
                        _data.Append(trainer);
                    else
                        //if it was found, overwrite the given trainer.
                        _data[matchIndex] = trainer;
                   
                }
                return true;
            }
            return false;
        }

        public bool AlterWithJSON(string JSON, out IList<string> errors)
        {
            //Doesn't work when there is not pre-existing data
            if (!Initialized)
            {
                errors = new List<string> { "The set is not yet initialized." };
                return false;
            }

            if (TrainerJSONValidator.ValidateTrainerListJSON(JSON, out errors))
            {
                List<TrainerRepresentation> newData = JsonConvert.DeserializeObject<List<TrainerRepresentation>>(JSON);
                foreach (TrainerRepresentation trainer in newData)
                {
                    int matchIndex;
                    if (trainer.TrainerData.Identification.NumberID != -1)
                        matchIndex = _data.FindIndex(i => i.TrainerData.Identification.NumberID == trainer.TrainerData.Identification.NumberID);
                    // if the trainer has a number id of -1, it must have a nameid, otherwise it wouldn't pass the validation
                    else
                        //Assumably the check that its not null will prevent the other checks from running and throwing null exception errors due to the nature of && 
                        matchIndex = _data.FindIndex(i => i.TrainerData.Identification.NameID != null && i.TrainerData.Identification.NameID.Name.Equals(trainer.TrainerData.Identification.NameID.Name) &&
                            i.TrainerData.Identification.NameID.Variation == trainer.TrainerData.Identification.NameID.Variation);

                    //if no copy is found, simply add the trainer to the list
                    if (matchIndex == -1)
                        _data.Append(trainer);
                    else
                        //if it was found, overwrite the given trainer.
                        _data[matchIndex] = trainer;

                }
                return true;
            }
            return false;
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
            //If the data count is smaller than the slot count, it logically cannot use up all slots
            if (_data.Count < slots.SlotData.Count)
                return false;

            for (int num = 0; num < slots.SlotData.Count; num++)
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

        public void GetNarc(out byte[] TRData, out byte[] TRPoke, ExternalItemList items, ExternalMoveList moves, ExternalPokemonList pokemon, ExternalTrainerSlotList slots)
        {
            //If it doesn't pass these validation checks, then the output would be nonsense.
            if (!ValidateAllSlotsUsed(slots) || !ValidateNoDuplicates(slots))
            {
                TRData = null;
                TRPoke = null;
                return;
            }

            //start
            NARC trdataNarc = new NARC();
            NARC trpokeNarc = new NARC();

            //sort the data so that its 'filed' in correct order
            SortData(slots);

            //first, fill the placeholder zeroth slot with the placeholder data
            TrainerRepresentation.GetPlaceholderBytes(out byte[] placeholderTRData, out byte[] placeholderTRPoke);
            trdataNarc.Add(placeholderTRData);
            trpokeNarc.Add(placeholderTRPoke);

            //fill in the rest of the data
            for (int trainer = 0; trainer < _data.Count; trainer++)
            {
                trdataNarc.Add(_data[trainer].GetTrainerBytes(items));
                trpokeNarc.Add(_data[trainer].GetPokemonBytes(items, moves, pokemon));
            }

            TRData = trdataNarc.Compile();
            TRPoke = trpokeNarc.Compile();
        }
    }
}
