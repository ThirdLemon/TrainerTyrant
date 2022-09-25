using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TrainerTyrant
{
    public enum BattleType:byte { Single, Double, Triple, Rotation }
    public enum Gender:byte { Random, Male, Female }

    public class TrainerRepresentation
    {
        public TrainerRepresentation()
        {
            TrainerData = new TrainerData();
            PokemonData = null;
        }

        [JsonProperty(PropertyName = "Trainer Data")]
        public TrainerData TrainerData { get; set; }
        [JsonProperty(PropertyName = "Pokemon Data")]
        public PokemonData[] PokemonData { get; set; }

        [JsonIgnore]
        public int PokemonCount { get { return PokemonData.Length; } }

        public static TrainerRepresentation DeserializeJSON(string JSON)
        {
            //Validate the JSON
            if (TrainerJSONValidator.ValidateTrainerJSON(JSON))
                return JsonConvert.DeserializeObject<TrainerRepresentation>(JSON);

            return null;
        }

        public static TrainerRepresentation DeserializeJSON(string JSON, out IList<string> errors)
        {
            if (TrainerJSONValidator.ValidateTrainerJSON(JSON, out errors))
                return JsonConvert.DeserializeObject<TrainerRepresentation>(JSON);

            return null;
        }

        public static List<TrainerRepresentation> DeserializeListJSON(string JSON)
        {
            if (TrainerJSONValidator.ValidateTrainerListJSON(JSON))
                return JsonConvert.DeserializeObject<List<TrainerRepresentation>>(JSON);

            return null;
        }

        public static List<TrainerRepresentation> DeserializeListJSON(string JSON, out IList<string> errors)
        {
            bool valid = TrainerJSONValidator.ValidateTrainerListJSON(JSON, out errors);

            if (valid)
                return JsonConvert.DeserializeObject<List<TrainerRepresentation>>(JSON);

            return null;
        }

        public static TrainerRepresentation BuildFromBytes(byte[] TRData, byte[] TRPoke, int slotID, ExternalItemList items, ExternalMoveList moves, ExternalPokemonList pokemon, ExternalTrainerSlotList trainers)
        {
            TrainerRepresentation to_return = new TrainerRepresentation();

            //The first byte of TRData stores the format data
            byte format = TRData[0];
            if ((format & 2) == 2)
                to_return.TrainerData.Format.Items = true;
            if ((format & 1) == 1)
                to_return.TrainerData.Format.Moves = true;
            //The second byte of TRData stores trainer class
            to_return.TrainerData.TrainerClass.NumberID = TRData[1];
            //The thrid byte of TRData stores the battle type
            to_return.TrainerData.BattleType = (BattleType)TRData[2];
            //The fourth byte of TRData stores the pokemon count, which doesn't need to be stored
            //Item id's are stored from fifth to twelth bytes
            for(int itemNumber = 0; itemNumber < 4; itemNumber++)
            {
                int fullID = TRData[4 + itemNumber * 2] + TRData[5 + itemNumber * 2] * 256;
                to_return.TrainerData.Items[itemNumber] = items.GetItem(fullID);
            }
            //The thirteenth byte stores the ai value
            to_return.TrainerData.AIFlags.Bitmap = TRData[12];
            //The use of the fourteenth, fifteenth, and sixteenth bytes is currently unknown
            //The seventeenth byte stores the healer data
            to_return.TrainerData.Healer = Convert.ToBoolean(TRData[16]);
            //The eigteenth byte stored base money
            to_return.TrainerData.BaseMoney = TRData[17];
            //The use of the nineteenth and twentieth bytes are currently unknown

            //Go onto the TRPoke data
            //store segment length
            int segmentLength = 8 + Convert.ToInt32(to_return.TrainerData.Format.Items) * 2 + Convert.ToInt32(to_return.TrainerData.Format.Moves) * 8;
            //create space for the data. null filled initially.
            to_return.PokemonData = new PokemonData[TRData[3]];
            //loop for each pokemon
            for(int pokemonNum = 0; pokemonNum < TRData[3]; pokemonNum++)
            {
                //initialize the pokemon data
                to_return.PokemonData[pokemonNum] = new PokemonData();

                //Set the reader to start at the beginning of the pokemon
                int monStart = pokemonNum * segmentLength;

                //Start of general data
                //The first byte stores difficulty
                to_return.PokemonData[pokemonNum].Difficulty = TRPoke[monStart + 0];
                //The second byte stores miscellaneous
                to_return.PokemonData[pokemonNum].Miscellaneous.Byte = TRPoke[monStart + 1];
                //The third byte stores level
                to_return.PokemonData[pokemonNum].Level = TRPoke[monStart + 2];
                //The fourth byte stores nothing(probably)
                //The fifth and sixth bytes store pokemon ID
                int fullID = TRPoke[monStart + 4] + TRPoke[monStart + 5] * 256;
                to_return.PokemonData[pokemonNum].Pokemon = pokemon.GetPokemon(fullID);
                //The seventh byte stores form number
                to_return.PokemonData[pokemonNum].Form = TRPoke[monStart + 6];
                //The eight byte stores nothing(probably)

                //push monstart past the general data
                monStart += 8;
                //Item data
                if (to_return.TrainerData.Format.Items)
                {
                    //the two bytes here store the item
                    fullID = TRPoke[monStart + 0] + TRPoke[monStart + 1] * 256;
                    to_return.PokemonData[pokemonNum].Item = items.GetItem(fullID);
                    //push monstart after the item data
                    monStart += 2;
                }
                //Move data
                if (to_return.TrainerData.Format.Moves)
                {
                    //the eight bytes stored here store the four moves id
                    for (int moveNum = 0; moveNum < 4; moveNum++)
                    {
                        fullID = TRPoke[monStart + 0] + TRPoke[monStart + 1] * 256;
                        to_return.PokemonData[pokemonNum].Moves[moveNum] = moves.GetMove(fullID);
                        //move monstart after each data
                        monStart += 2;
                    }
                }
            }

            //Store trainer slot data
            to_return.TrainerData.Identification.NumberID = slotID;
            TrainerSlotData tSlotData = trainers.GetSlot(slotID);
            if (tSlotData == null)
                throw new Exception("Trainer slot " + slotID + " did not compute.");
            to_return.TrainerData.Identification.NameID = new NameID();
            to_return.TrainerData.Identification.NameID.Name = tSlotData.Name;
            to_return.TrainerData.Identification.NameID.Variation = tSlotData.Variation;

            return to_return;
        }

        //The zeroth trainer slot is filled with this data.
        public static void GetPlaceholderBytes(out byte[] TRData, out byte[] TRPoke)
        {
            TRData = new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            TRPoke = new byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        }

        public byte[] GetTrainerBytes(ExternalItemList items)
        {
            byte[] to_return = new byte[20];

            //write the format to the first byte
            to_return[0] = TrainerData.Format.BitFlag;
            //Write the selected trainer class to the second byte
            to_return[1] = (byte)TrainerData.TrainerClass.NumberID;
            //write the battle type to the third byte
            to_return[2] = (byte)TrainerData.BattleType;
            //Write the pokemon count to the fourth byte
            to_return[3] = (byte)PokemonCount;
            //Write item 1's id to the fifth and sixth bytes
            int id = TrainerData.ItemIndex(items, 0);
            to_return[4] = (byte)(id - id / 256 * 256);
            to_return[5] = (byte)(id / 256);
            //Write item 2's id to the seventh and eight bytes
            id = TrainerData.ItemIndex(items, 1);
            to_return[6] = (byte)(id - id / 256 * 256);
            to_return[7] = (byte)(id / 256);
            //Write item 3's id to the ninth and tenth bytes
            id = TrainerData.ItemIndex(items, 2);
            to_return[8] = (byte)(id - id / 256 * 256);
            to_return[9] = (byte)(id / 256);
            //Write item 4's id to the eleventh and twelth bytes
            id = TrainerData.ItemIndex(items, 3);
            to_return[10] = (byte)(id - id / 256 * 256);
            to_return[11] = (byte)(id / 256);
            //Write the ai value to the thirteenth byte
            to_return[12] = TrainerData.AIFlags.Bitmap;
            //Write nothing to the fourteenth, fifteenth, and sixteenth byte
            //Write healer to the seventeeth byte
            to_return[16] = Convert.ToByte(TrainerData.Healer);
            //Write basemoney to the eigtheenth byte
            to_return[17] = (byte)TrainerData.BaseMoney;
            //Write nothing to the nineteenth and twentieth bytes

            return to_return;
        }

        public byte[] GetPokemonBytes(ExternalItemList items, ExternalMoveList moves, ExternalPokemonList pokemon)
        {
            //8 bytes are given to general data. 2 bytes given to items, and 8 to moves.
            int segmentLength = 8 + Convert.ToInt32(TrainerData.Format.Items) * 2 + Convert.ToInt32(TrainerData.Format.Moves) * 8;
            byte[] to_return = new byte[PokemonCount * segmentLength];

            //Write for each pokemon
            for(int mon = 0; mon < PokemonCount; mon++)
            {
                //The first byte in the byte array allocated to the mon
                int monStart = mon * segmentLength;
                //General data
                //Write the difficulty to the first byte
                to_return[monStart] = (byte)PokemonData[mon].Difficulty;
                //Write miscellaneous to the second byte
                to_return[monStart + 1] = PokemonData[mon].Miscellaneous.Byte;
                //Write level to the third byte
                to_return[monStart + 2] = (byte)PokemonData[mon].Level;
                //Write nothing to the fourth byte
                //Write pokemon index to the fifth and sixth bytes
                int id = pokemon.GetIndexOfPokemon(PokemonData[mon].Pokemon);
                //Currently no tracking for whether valid pokemon is given here. The invalid Pokemon "0" may be put here.
                to_return[monStart + 4] = (byte)(id - id / 256 * 256);
                to_return[monStart + 5] = (byte)(id / 256);
                //Write form number to the seventh byte
                to_return[monStart + 6] = (byte)(PokemonData[mon].Form);
                //Write nothing to the eight byte
                //end of general data.

                //update monstart to start of next section for convenience
                monStart += 8;

                //Items are written before moves
                if (TrainerData.Format.Items)
                {
                    id = items.GetIndexOfItem(PokemonData[mon].Item);
                    //Write the item id to the first and second bytes
                    to_return[monStart] = (byte)(id - id / 256 * 256);
                    to_return[monStart + 1] = (byte)(id / 256);
                    //End of item data

                    //update monstart so that its after item data incase move data is also written
                    monStart += 2;
                }
                //Write movedata
                if (TrainerData.Format.Moves)
                {
                    //Run the same code for each move
                    for (int i = 0; i < 4; i++)
                    {
                        //Write the move data into the two byte pairs through each of the eight bytes allocated
                        id = moves.GetIndexOfMove(PokemonData[mon].Moves[i]);
                        to_return[monStart + i * 2] = (byte)(id - id / 256 * 256);
                        to_return[monStart + i * 2 + 1] = (byte)(id / 256);
                    }
                }
            }

            return to_return;
        }

        //Not necessarily the slot in the list, as the list is missing the "null" item 0.
        public int GetSlotID(ExternalTrainerSlotList slotList)
        {
            //If you have a number ID, automatically use it.
            if (TrainerData.Identification.NumberID >= 0)
                return TrainerData.Identification.NumberID;
            return slotList.GetIndexOfSlot(TrainerData.Identification.NameID.Name, TrainerData.Identification.NameID.Variation);
        }
    }

    public class TrainerData
    {
        public TrainerData()
        {
            Identification = new Identification();
            TrainerClass = new TrainerClass();
            BattleType = BattleType.Single;
            Format = new Format();
            BaseMoney = 0;
            Items = new string[] { null, null, null, null };
            AIFlags = new AIFlags();
            Healer = false;
        }

        public Identification Identification { get; set; }
        [JsonProperty(PropertyName = "Trainer Class")]
        public TrainerClass TrainerClass { get; set; }
        [JsonProperty(PropertyName = "Battle Type"), JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public BattleType BattleType { get; set; }
        public Format Format { get; set; }
        [JsonProperty(PropertyName = "Base Money")]
        public int BaseMoney { get; set; }
        public string[] Items { get; set; }
        [JsonProperty(PropertyName = "AI Flags")]
        public AIFlags AIFlags { get; set; }
        public bool Healer { get; set; }

        //This function controls the serailization of the Items. Don't serialize them when the array only contains null to save space.
        public bool ShouldSerializeItems()
        {
            return !(Items[0] == null && Items[1] == null && Items[2] == null && Items[3] == null);
        }

        //This function controls the serialization of Healer.
        public bool ShouldSerializeHealer()
        {
            return Healer;
        }

        public int ItemIndex(ExternalItemList itemList, int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= 4)
                return 0;
            return itemList.GetIndexOfItem(Items[itemIndex]);
        }
    }

    public class Identification
    {
        public Identification()
        {
            NumberID = -1;
            NameID = null;
        }

        [JsonProperty(PropertyName = "Number ID")]
        public int NumberID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Name ID")]
        public NameID NameID { get; set; }

        //This function controls ther serialization of NumberID
        public bool ShouldSerializeNumberID()
        {
            return NameID == null;
        }
    }

    public class NameID
    {
        public string Name { get; set; }
        public int Variation { get; set; }
    }

    public class TrainerClass
    {
        public TrainerClass()
        {
            //Set it to the first value.
            NumberID = 0;
        }

        [JsonProperty(PropertyName = "Number ID")]
        public int NumberID { get; set; }
    }

    public class Format
    {
        public Format()
        {
            Moves = false;
            Items = false;
        }

        public bool Moves { get; set; }
        public bool Items { get; set; }

        [JsonIgnore]
        public byte BitFlag { get { return (byte)(Convert.ToByte(Moves) + (Convert.ToByte(Items) * 2));  } }
    }

    public class AIFlags
    {
        public AIFlags()
        {
            CheckBadMoves = false;
            EvaluateAttacks = false;
            Expert = false;
            SetupFirstTurn = false;
            Risky = false;
            PreferStrongMoves = false;
            PreferBatonPass = false;
            DoubleBattle = false;
        }

        [JsonProperty(PropertyName = "Check Bad Moves")]
        public bool CheckBadMoves { get; set; }
        [JsonProperty(PropertyName = "Evaluate Attacks")]
        public bool EvaluateAttacks { get; set; }
        public bool Expert { get; set; }
        [JsonProperty(PropertyName = "Setup First Turn")]
        public bool SetupFirstTurn { get; set; }
        public bool Risky { get; set; }
        [JsonProperty(PropertyName = "Prefer Strong Moves")]
        public bool PreferStrongMoves { get; set; }
        [JsonProperty(PropertyName = "Prefer Baton Pass")]
        public bool PreferBatonPass { get; set; }
        [JsonProperty(PropertyName = "Double Battle")]
        public bool DoubleBattle { get; set; }

        [JsonIgnore]
        public byte Bitmap 
        {
            get
            {
                return (byte)(Convert.ToInt32(DoubleBattle) << 7 | Convert.ToInt32(PreferBatonPass) << 6 | Convert.ToInt32(PreferStrongMoves) << 5 | Convert.ToInt32(Risky) << 4 | Convert.ToInt32(SetupFirstTurn) << 3 | 
                              Convert.ToInt32(Expert) << 2 | Convert.ToInt32(EvaluateAttacks) << 1 | Convert.ToInt32(CheckBadMoves));
            }
            set
            {
                CheckBadMoves       =   (value & 0x01) == 0x01;
                EvaluateAttacks     =   (value & 0x02) == 0x02;
                Expert              =   (value & 0x04) == 0x04;    
                SetupFirstTurn      =   (value & 0x08) == 0x08;
                Risky               =   (value & 0x10) == 0x10;
                PreferStrongMoves   =   (value & 0x20) == 0x20;
                PreferBatonPass     =   (value & 0x40) == 0x40;
                DoubleBattle        =   (value & 0x80) == 0x80;
            }
        }
    }

    public class PokemonData
    {
        public PokemonData()
        {
            Pokemon = "Bulbasaur";
            Form = 0;
            Level = 5;
            Difficulty = 0;
            Miscellaneous = new Miscellaneous();
            Moves = new string[] { null, null, null, null };
            Item = null;
        }

        public string Pokemon { get; set; }
        public int Form { get; set; }
        public int Level { get; set; }
        public int Difficulty { get; set; }
        public Miscellaneous Miscellaneous { get; set; }
        public string[] Moves { get; set; }
        public string Item { get; set; }

        //dont' serialize moves when unnecessary. This hinges on the idea that if moves are enabled, the pokemon will always have moves in their data.
        public bool ShouldSerializeMoves()
        {
            return !(Moves[0] == null && Moves[1] == null && Moves[2] == null && Moves[3] == null);
        }
    }

    public class Miscellaneous
    {
        public Miscellaneous()
        {
            Gender = Gender.Random;
            Ability = 0;
        }

        //Gender should be one of 'Random', 'Female', or 'Male'.
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public Gender Gender { get; set; }
        //Ability should always fall within the range of 0-2.
        public int Ability { get; set; }

        [JsonIgnore]
        public byte Byte { get { return (byte)((int)Gender + (16 * Ability));  } set { Ability = (value / 16); Gender = (Gender)(value - value / 16 * 16); } }
    }


}
