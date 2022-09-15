using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TrainerTyrant
{

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

        public int PokemonCount { get { return PokemonData.Length; } }
    }

    public class TrainerData
    {
        public TrainerData()
        {
            Identification = new Identification();
            TrainerClass = new TrainerClass();
            BattleType = "Single";
            Format = new Format();
            BaseMoney = 0;
            Items = new string[] { null, null, null, null };
            AIFlags = new AIFlags();
            Healer = false;
        }

        public Identification Identification { get; set; }
        [JsonProperty(PropertyName = "Trainer Class")]
        public TrainerClass TrainerClass { get; set; }
        [JsonProperty(PropertyName = "Battle Type")]
        public string BattleType { get; set; }
        public Format Format { get; set; }
        [JsonProperty(PropertyName = "Base Money")]
        public int BaseMoney { get; set; }
        public string[] Items { get; set; }
        [JsonProperty(PropertyName = "AI Flags")]
        public AIFlags AIFlags { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Healer { get; set; }
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
    }

    public class Miscellaneous
    {
        public Miscellaneous()
        {
            Gender = "Random";
            Ability = 0;
        }

        //Gender should be one of 'Random', 'Female', or 'Male'.
        public string Gender { get; set; }
        //Ability should always fall within the range of 0-2.
        public int Ability { get; set; }
    }


}
