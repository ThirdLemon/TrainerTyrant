using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TrainerTyrant;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace TrainerTyrantTest
{
    [TestClass]
    public class SerializationTest
    {
        private string shauntalJSON;
        private string shauntalweakJSON;
        private string strangeJSON;
        private string incompleteJSON;
        private string multiJSON;
        private string multi2JSON;
        private string multimovelessJSON;
        private string biancaJSON;
        private string hugh9JSON;
        private string fletcherJSON;
        private string customelenaJSON;
        private string customelenadanJSON;
        private string customelenadan2JSON;
        private string custom3JSON;
        private string movelistJSON;
        private string pokemonlistJSON;
        private string bulbapokemonlistJSON;
        private string itemlistJSON;
        private string slotlistJSON;
        private string smallslotlistJSON;
        private string smallslotlist2JSON;
        private string emptyJSON;

        private string bulbalineJSON;
        private string bulbalinewrongJSON;
        private string halfe4JSON;

        private string shorte4Doc;
        private string docOutput;

        private string simpleTRPoke;
        private string simpleTRData;
        private string simpleLearnset;

        [TestInitialize]
        public void TestInit()
        {
            shauntalJSON = File.ReadAllText("../../../SampleJSON/sampleJSON1.json");

            shauntalweakJSON = File.ReadAllText("../../../SampleJSON/sampleJSON1b.json");

            strangeJSON = File.ReadAllText("../../../SampleJSON/sampleJSON2.json");

            incompleteJSON = File.ReadAllText("../../../SampleJSON/sampleJSON3.json");

            multiJSON = File.ReadAllText("../../../SampleJSON/sampleJSON4.json");

            multi2JSON = File.ReadAllText("../../../SampleJSON/sampleJSON4b.json");

            multimovelessJSON = File.ReadAllText("../../../SampleJSON/sampleJSON4c.json");

            biancaJSON = File.ReadAllText("../../../SampleJSON/sampleJSON5.json");

            hugh9JSON = File.ReadAllText("../../../SampleJSON/sampleJSON6.json");

            fletcherJSON = File.ReadAllText("../../../SampleJSON/sampleJSON7.json");

            customelenaJSON = File.ReadAllText("../../../SampleJSON/sampleJSON8.json");

            customelenadanJSON = File.ReadAllText("../../../SampleJSON/sampleJSON8b.json");

            customelenadan2JSON = File.ReadAllText("../../../SampleJSON/sampleJSON8c.json");

            custom3JSON = File.ReadAllText("../../../SampleJSON/sampleJSON8d.json");

            movelistJSON = File.ReadAllText("../../../SampleJSON/External/MoveList1.json");

            pokemonlistJSON = File.ReadAllText("../../../SampleJSON/External/PokemonList1.json");

            bulbapokemonlistJSON = File.ReadAllText("../../../SampleJSON/External/PokemonList2.json");

            itemlistJSON = File.ReadAllText("../../../SampleJSON/External/ItemList1.json");

            slotlistJSON = File.ReadAllText("../../../SampleJSON/External/SlotList1.json");

            smallslotlistJSON = File.ReadAllText("../../../SampleJSON/External/SlotList2.json");

            smallslotlist2JSON = File.ReadAllText("../../../SampleJSON/External/SlotList3.json");

            emptyJSON = File.ReadAllText("../../../SampleJSON/External/Empty.json");

            bulbalineJSON = File.ReadAllText("../../../SampleJSON/SampleJSON9.json");

            bulbalinewrongJSON = File.ReadAllText("../../../SampleJSON/SampleJSON9b.json");

            halfe4JSON = File.ReadAllText("../../../SampleJSON/SampleJSON10.json");

            shorte4Doc = "../../../SampleDocs/SampleDoc1.txt";

            docOutput = "../../../SampleDocs/output.txt";

            simpleTRData = "../../../SampleNARCs/TRData1.narc";

            simpleTRPoke = "../../../SampleNARCs/TRPoke1.narc";

            simpleLearnset = "../../../SampleNARCs/Learnset1.narc";
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(docOutput))
                File.Delete(docOutput);
        }

        [TestMethod]
        public void BaseDeserializeNoErrors()
        {
            TrainerRepresentation shauntal = JsonConvert.DeserializeObject<TrainerRepresentation>(shauntalJSON);

            Assert.AreEqual(shauntal.TrainerData.Identification.NumberID, 38);
            Assert.AreEqual(shauntal.TrainerData.Identification.NameID.Name, "Shauntal");
            Assert.AreEqual(shauntal.TrainerData.BattleType, BattleType.Single);
            Assert.IsTrue(shauntal.TrainerData.Format.Items);
            Assert.IsTrue(shauntal.TrainerData.Items.Length == 4);
            Assert.IsTrue(shauntal.TrainerData.Items[0] == "Full Restore");
            Assert.IsTrue(shauntal.TrainerData.AIFlags.CheckBadMoves);
            Assert.IsTrue(shauntal.PokemonData.Length == 4);
            Assert.AreEqual(shauntal.PokemonCount, shauntal.PokemonData.Length);
        }

        [TestMethod]
        public void Deserialize()
        {
            TrainerRepresentation shauntal = TrainerRepresentation.DeserializeJSON(shauntalJSON);
            Assert.AreEqual(38, shauntal.TrainerData.Identification.NumberID);
            Assert.AreEqual("Shauntal", shauntal.TrainerData.Identification.NameID.Name);
            Assert.AreEqual(0, shauntal.TrainerData.Identification.NameID.Variation);
            Assert.AreEqual(78, shauntal.TrainerData.TrainerClass.NumberID);
            Assert.AreEqual(BattleType.Single, shauntal.TrainerData.BattleType);
            Assert.IsTrue(shauntal.TrainerData.Format.Moves);
            Assert.IsTrue(shauntal.TrainerData.Format.Items);
            Assert.AreEqual(30, shauntal.TrainerData.BaseMoney);
            Assert.AreEqual(4, shauntal.TrainerData.Items.Length);
            Assert.AreEqual("Full Restore", shauntal.TrainerData.Items[0]);
            for(int i = 1; i < 4; i++)
                Assert.AreEqual(null, shauntal.TrainerData.Items[i]);
            Assert.AreEqual(7, shauntal.TrainerData.AIFlags.Bitmap);
            Assert.IsFalse(shauntal.TrainerData.Healer);
            Assert.AreEqual(4, shauntal.PokemonCount);
            Assert.AreEqual(4, shauntal.PokemonData.Length);
            string[] monnames = { "Cofagrigus", "Drifblim", "Golurk", "Chandelure" };
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(monnames[i], shauntal.PokemonData[i].Pokemon);
                Assert.AreEqual(0, shauntal.PokemonData[i].Form);
                Assert.AreEqual(Gender.Random, shauntal.PokemonData[i].Miscellaneous.Gender);
            }
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(56, shauntal.PokemonData[i].Level);
                Assert.AreEqual(200, shauntal.PokemonData[i].Difficulty);
                Assert.AreEqual(1, shauntal.PokemonData[i].Miscellaneous.Ability);
                Assert.AreEqual(null, shauntal.PokemonData[i].Item);
            }
            Assert.AreEqual(58, shauntal.PokemonData[3].Level);
            Assert.AreEqual(250, shauntal.PokemonData[3].Difficulty);
            Assert.AreEqual(2, shauntal.PokemonData[3].Miscellaneous.Ability);
            Assert.AreEqual("Sitrus Berry", shauntal.PokemonData[3].Item);
            string[,] monmoves = { { "Will-O-Wisp", "Grass Knot", "Psychic", "Shadow Ball" }, 
                                    { "Psychic", "Thunderbolt", "Acrobatics", "Shadow Ball" }, 
                                    { "Heavy Slam", "Earthquake", "Brick Break", "Shadow Punch" },
                                    { "Energy Ball", "Fire Blast", "Psychic", "Shadow Ball"} };
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    Assert.AreEqual(monmoves[i,j], shauntal.PokemonData[i].Moves[j]);
        }

        [TestMethod]
        public void DeserializeWOut()
        {
            TrainerRepresentation shauntal = TrainerRepresentation.DeserializeJSON(shauntalJSON, out IList<string> errors);
            Assert.AreEqual(0, errors.Count);
            Assert.AreEqual(38, shauntal.TrainerData.Identification.NumberID);
            Assert.AreEqual("Shauntal", shauntal.TrainerData.Identification.NameID.Name);
            Assert.AreEqual(0, shauntal.TrainerData.Identification.NameID.Variation);
            Assert.AreEqual(78, shauntal.TrainerData.TrainerClass.NumberID);
            Assert.AreEqual(BattleType.Single, shauntal.TrainerData.BattleType);
            Assert.IsTrue(shauntal.TrainerData.Format.Moves);
            Assert.IsTrue(shauntal.TrainerData.Format.Items);
            Assert.AreEqual(30, shauntal.TrainerData.BaseMoney);
            Assert.AreEqual(4, shauntal.TrainerData.Items.Length);
            Assert.AreEqual("Full Restore", shauntal.TrainerData.Items[0]);
            for (int i = 1; i < 4; i++)
                Assert.AreEqual(null, shauntal.TrainerData.Items[i]);
            Assert.AreEqual(7, shauntal.TrainerData.AIFlags.Bitmap);
            Assert.IsFalse(shauntal.TrainerData.Healer);
            Assert.AreEqual(4, shauntal.PokemonCount);
            Assert.AreEqual(4, shauntal.PokemonData.Length);
            string[] monnames = { "Cofagrigus", "Drifblim", "Golurk", "Chandelure" };
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(monnames[i], shauntal.PokemonData[i].Pokemon);
                Assert.AreEqual(0, shauntal.PokemonData[i].Form);
                Assert.AreEqual(Gender.Random, shauntal.PokemonData[i].Miscellaneous.Gender);
            }
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(56, shauntal.PokemonData[i].Level);
                Assert.AreEqual(200, shauntal.PokemonData[i].Difficulty);
                Assert.AreEqual(1, shauntal.PokemonData[i].Miscellaneous.Ability);
                Assert.AreEqual(null, shauntal.PokemonData[i].Item);
            }
            Assert.AreEqual(58, shauntal.PokemonData[3].Level);
            Assert.AreEqual(250, shauntal.PokemonData[3].Difficulty);
            Assert.AreEqual(2, shauntal.PokemonData[3].Miscellaneous.Ability);
            Assert.AreEqual("Sitrus Berry", shauntal.PokemonData[3].Item);
            string[,] monmoves = { { "Will-O-Wisp", "Grass Knot", "Psychic", "Shadow Ball" },
                                    { "Psychic", "Thunderbolt", "Acrobatics", "Shadow Ball" },
                                    { "Heavy Slam", "Earthquake", "Brick Break", "Shadow Punch" },
                                    { "Energy Ball", "Fire Blast", "Psychic", "Shadow Ball"} };
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    Assert.AreEqual(monmoves[i, j], shauntal.PokemonData[i].Moves[j]);
        }

        [TestMethod]
        public void DeserializeList()
        {
            List<TrainerRepresentation> data = TrainerRepresentation.DeserializeListJSON(multiJSON);
            Assert.AreEqual(2, data.Count);
            TrainerRepresentation shauntal = data[0];
            Assert.AreEqual(38, shauntal.TrainerData.Identification.NumberID);
            Assert.AreEqual("Shauntal", shauntal.TrainerData.Identification.NameID.Name);
            Assert.AreEqual(0, shauntal.TrainerData.Identification.NameID.Variation);
            Assert.AreEqual(78, shauntal.TrainerData.TrainerClass.NumberID);
            Assert.AreEqual(BattleType.Single, shauntal.TrainerData.BattleType);
            Assert.IsTrue(shauntal.TrainerData.Format.Moves);
            Assert.IsTrue(shauntal.TrainerData.Format.Items);
            Assert.AreEqual(30, shauntal.TrainerData.BaseMoney);
            Assert.AreEqual(4, shauntal.TrainerData.Items.Length);
            Assert.AreEqual("Full Restore", shauntal.TrainerData.Items[0]);
            for (int i = 1; i < 4; i++)
                Assert.AreEqual(null, shauntal.TrainerData.Items[i]);
            Assert.AreEqual(7, shauntal.TrainerData.AIFlags.Bitmap);
            Assert.IsFalse(shauntal.TrainerData.Healer);
            Assert.AreEqual(4, shauntal.PokemonCount);
            Assert.AreEqual(4, shauntal.PokemonData.Length);
            string[] monnames = { "Cofagrigus", "Drifblim", "Golurk", "Chandelure" };
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(monnames[i], shauntal.PokemonData[i].Pokemon);
                Assert.AreEqual(0, shauntal.PokemonData[i].Form);
                Assert.AreEqual(Gender.Random, shauntal.PokemonData[i].Miscellaneous.Gender);
            }
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(56, shauntal.PokemonData[i].Level);
                Assert.AreEqual(200, shauntal.PokemonData[i].Difficulty);
                Assert.AreEqual(1, shauntal.PokemonData[i].Miscellaneous.Ability);
                Assert.AreEqual(null, shauntal.PokemonData[i].Item);
            }
            Assert.AreEqual(58, shauntal.PokemonData[3].Level);
            Assert.AreEqual(250, shauntal.PokemonData[3].Difficulty);
            Assert.AreEqual(2, shauntal.PokemonData[3].Miscellaneous.Ability);
            Assert.AreEqual("Sitrus Berry", shauntal.PokemonData[3].Item);
            string[,] monmoves = { { "Will-O-Wisp", "Grass Knot", "Psychic", "Shadow Ball" },
                                    { "Psychic", "Thunderbolt", "Acrobatics", "Shadow Ball" },
                                    { "Heavy Slam", "Earthquake", "Brick Break", "Shadow Punch" },
                                    { "Energy Ball", "Fire Blast", "Psychic", "Shadow Ball"} };
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    Assert.AreEqual(monmoves[i, j], shauntal.PokemonData[i].Moves[j]);

            TrainerRepresentation marshal = data[1];
            Assert.AreEqual(39, marshal.TrainerData.Identification.NumberID);
            Assert.AreEqual("Marshal", marshal.TrainerData.Identification.NameID.Name);
            Assert.AreEqual(0, marshal.TrainerData.Identification.NameID.Variation);
            Assert.AreEqual(79, marshal.TrainerData.TrainerClass.NumberID);
            Assert.AreEqual(BattleType.Single, marshal.TrainerData.BattleType);
            Assert.IsTrue(marshal.TrainerData.Format.Moves);
            Assert.IsTrue(marshal.TrainerData.Format.Items);
            Assert.AreEqual(30, marshal.TrainerData.BaseMoney);
            Assert.AreEqual(4, marshal.TrainerData.Items.Length);
            Assert.AreEqual("Full Restore", marshal.TrainerData.Items[0]);
            for (int i = 1; i < 4; i++)
                Assert.AreEqual(null, marshal.TrainerData.Items[i]);
            Assert.AreEqual(7, marshal.TrainerData.AIFlags.Bitmap);
            Assert.IsFalse(marshal.TrainerData.Healer);
            Assert.AreEqual(4, marshal.PokemonCount);
            Assert.AreEqual(4, marshal.PokemonData.Length);
            string[] marshalmonnames = { "Throh", "Sawk", "Mienshao", "Conkeldurr" };
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(marshalmonnames[i], marshal.PokemonData[i].Pokemon);
                Assert.AreEqual(0, marshal.PokemonData[i].Form);
                Assert.AreEqual(Gender.Random, marshal.PokemonData[i].Miscellaneous.Gender);
            }
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(56, marshal.PokemonData[i].Level);
                Assert.AreEqual(200, marshal.PokemonData[i].Difficulty);
                Assert.AreEqual(null, marshal.PokemonData[i].Item);
            }
            Assert.AreEqual(1, marshal.PokemonData[0].Miscellaneous.Ability);
            Assert.AreEqual(1, marshal.PokemonData[1].Miscellaneous.Ability);
            Assert.AreEqual(2, marshal.PokemonData[2].Miscellaneous.Ability);
            Assert.AreEqual(1, marshal.PokemonData[3].Miscellaneous.Ability);
            Assert.AreEqual(58, marshal.PokemonData[3].Level);
            Assert.AreEqual(250, marshal.PokemonData[3].Difficulty);
            Assert.AreEqual("Sitrus Berry", marshal.PokemonData[3].Item);
            string[,] marshalmonmoves = { { "Storm Throw", "Bulldoze", "Rock Tomb", "Payback" },
                                    { "Brick Break", "Retaliate", "Rock Slide", "Payback" },
                                    { "Hi Jump Kick", "U-Turn", "Bounce", "Retaliate" },
                                    { "Hammer Arm", "Bulk Up", "Stone Edge", "Retaliate"} };
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    Assert.AreEqual(marshalmonmoves[i, j], marshal.PokemonData[i].Moves[j]);
        }

        [TestMethod]
        public void BaseSerializeNoErrors()
        {
            TrainerRepresentation shauntal = new();
            shauntal.TrainerData.Identification.NumberID = 38;
            shauntal.TrainerData.TrainerClass.NumberID = 78;
            shauntal.TrainerData.BattleType = BattleType.Single;
            shauntal.TrainerData.Format.Moves = true;
            shauntal.TrainerData.Format.Items = true;
            shauntal.TrainerData.BaseMoney = 30;
            shauntal.TrainerData.Items = new string[] { "Full Restore", null, null, null };
            shauntal.TrainerData.AIFlags.CheckBadMoves = true;
            shauntal.TrainerData.AIFlags.EvaluateAttacks = true;
            shauntal.TrainerData.AIFlags.Expert = true;
            shauntal.PokemonData = new PokemonData[] {new PokemonData() };
            shauntal.PokemonData[0].Pokemon = "Cofagrigus";
            shauntal.PokemonData[0].Form = 0;
            shauntal.PokemonData[0].Level = 56;
            shauntal.PokemonData[0].Difficulty = 200;
            shauntal.PokemonData[0].Miscellaneous.Gender = Gender.Random;
            shauntal.PokemonData[0].Miscellaneous.Ability = 0;
            shauntal.PokemonData[0].Moves = new string[] { "Will-O-Wisp", "Grass Knot", "Psychic", "Shadow Ball" };
            shauntal.PokemonData[0].Item = null;

            string json = JsonConvert.SerializeObject(shauntal, Formatting.Indented);

            Console.WriteLine(json);
        }

        [TestMethod]
        public void MultiBaseSerializeNoErrors()
        {
            TrainerRepresentation shauntal = new();
            shauntal.TrainerData.Identification.NumberID = 38;
            shauntal.TrainerData.TrainerClass.NumberID = 78;
            shauntal.TrainerData.BattleType = BattleType.Single;
            shauntal.TrainerData.Format.Moves = true;
            shauntal.TrainerData.Format.Items = true;
            shauntal.TrainerData.BaseMoney = 30;
            shauntal.TrainerData.Items = new string[] { "Full Restore", null, null, null };
            shauntal.TrainerData.AIFlags.CheckBadMoves = true;
            shauntal.TrainerData.AIFlags.EvaluateAttacks = true;
            shauntal.TrainerData.AIFlags.Expert = true;
            shauntal.PokemonData = new PokemonData[] { new PokemonData() };
            shauntal.PokemonData[0].Pokemon = "Cofagrigus";
            shauntal.PokemonData[0].Form = 0;
            shauntal.PokemonData[0].Level = 56;
            shauntal.PokemonData[0].Difficulty = 200;
            shauntal.PokemonData[0].Miscellaneous.Gender = Gender.Random;
            shauntal.PokemonData[0].Miscellaneous.Ability = 0;
            shauntal.PokemonData[0].Moves = new string[] { "Will-O-Wisp", "Grass Knot", "Psychic", "Shadow Ball" };
            shauntal.PokemonData[0].Item = null;

            TrainerRepresentation marshal = new();
            marshal.TrainerData.Identification.NumberID = 39;
            marshal.TrainerData.TrainerClass.NumberID = 79;
            marshal.TrainerData.BattleType = BattleType.Single;
            marshal.TrainerData.Format.Moves = true;
            marshal.TrainerData.Format.Items = true;
            marshal.TrainerData.BaseMoney = 30;
            marshal.TrainerData.Items = new string[] { null, null, null, null };
            marshal.TrainerData.AIFlags.CheckBadMoves = true;
            marshal.TrainerData.AIFlags.EvaluateAttacks = true;
            marshal.TrainerData.AIFlags.Expert = true;
            marshal.PokemonData = new PokemonData[] { new PokemonData() };
            marshal.PokemonData[0].Pokemon = "Throh";
            marshal.PokemonData[0].Form = 0;
            marshal.PokemonData[0].Level = 56;
            marshal.PokemonData[0].Difficulty = 200;
            marshal.PokemonData[0].Miscellaneous.Gender = Gender.Random;
            marshal.PokemonData[0].Miscellaneous.Ability = 0;
            marshal.PokemonData[0].Moves = new string[] { "Storm Throw", "Rock Slide", "Bulk Up", "Earthquake" };
            marshal.PokemonData[0].Item = null;

            TrainerRepresentation[] trainers = new TrainerRepresentation[] { shauntal, marshal };

            string json = JsonConvert.SerializeObject(trainers, Formatting.Indented);

            Console.WriteLine(json);
        }

        [TestMethod]
        public void Serialize()
        {
            TrainerRepresentation shauntal = TrainerRepresentation.DeserializeJSON(shauntalJSON);

            string serialized = JsonConvert.SerializeObject(shauntal, Formatting.Indented);

            TrainerRepresentation shauntal2 = TrainerRepresentation.DeserializeJSON(serialized, out IList<string> errors);

            foreach (string l in errors)
                Console.WriteLine(l);

            Assert.IsNotNull(shauntal2);
            //When NameID is set, numberID shouldn't be serialized.
            Assert.AreEqual(-1, shauntal2.TrainerData.Identification.NumberID);
            Assert.AreEqual("Shauntal", shauntal2.TrainerData.Identification.NameID.Name);
            Assert.AreEqual(0, shauntal2.TrainerData.Identification.NameID.Variation);
            Assert.AreEqual(78, shauntal2.TrainerData.TrainerClass.NumberID);
            Assert.AreEqual(BattleType.Single, shauntal2.TrainerData.BattleType);
            Assert.IsTrue(shauntal2.TrainerData.Format.Moves);
            Assert.IsTrue(shauntal2.TrainerData.Format.Items);
            Assert.AreEqual(30, shauntal2.TrainerData.BaseMoney);
            Assert.AreEqual(4, shauntal2.TrainerData.Items.Length);
            Assert.AreEqual("Full Restore", shauntal2.TrainerData.Items[0]);
            for (int i = 1; i < 4; i++)
                Assert.AreEqual(null, shauntal2.TrainerData.Items[i]);
            Assert.AreEqual(7, shauntal2.TrainerData.AIFlags.Bitmap);
            Assert.IsFalse(shauntal2.TrainerData.Healer);
            Assert.AreEqual(4, shauntal2.PokemonCount);
            Assert.AreEqual(4, shauntal2.PokemonData.Length);
            string[] monnames = { "Cofagrigus", "Drifblim", "Golurk", "Chandelure" };
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(monnames[i], shauntal2.PokemonData[i].Pokemon);
                Assert.AreEqual(0, shauntal2.PokemonData[i].Form);
                Assert.AreEqual(Gender.Random, shauntal2.PokemonData[i].Miscellaneous.Gender);
            }
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(56, shauntal2.PokemonData[i].Level);
                Assert.AreEqual(200, shauntal2.PokemonData[i].Difficulty);
                Assert.AreEqual(1, shauntal2.PokemonData[i].Miscellaneous.Ability);
                Assert.AreEqual(null, shauntal2.PokemonData[i].Item);
            }
            Assert.AreEqual(58, shauntal2.PokemonData[3].Level);
            Assert.AreEqual(250, shauntal2.PokemonData[3].Difficulty);
            Assert.AreEqual(2, shauntal2.PokemonData[3].Miscellaneous.Ability);
            Assert.AreEqual("Sitrus Berry", shauntal2.PokemonData[3].Item);
            string[,] monmoves = { { "Will-O-Wisp", "Grass Knot", "Psychic", "Shadow Ball" },
                                    { "Psychic", "Thunderbolt", "Acrobatics", "Shadow Ball" },
                                    { "Heavy Slam", "Earthquake", "Brick Break", "Shadow Punch" },
                                    { "Energy Ball", "Fire Blast", "Psychic", "Shadow Ball"} };
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    Assert.AreEqual(monmoves[i, j], shauntal2.PokemonData[i].Moves[j]);
        }

        [TestMethod]
        public void BaseDeserializeWithStrangeProperties()
        {
            TrainerRepresentation shauntal = JsonConvert.DeserializeObject<TrainerRepresentation>(strangeJSON);

            Console.WriteLine(shauntal.TrainerData.Identification.NameID.Name);
            Console.WriteLine(shauntal.TrainerData.Identification.NameID.Variation);
        }

        [TestMethod]
        public void ValidateCorrectJSON()
        {
            Assert.IsTrue(JSONValidatorTrainer.ValidateTrainerListJSON(multiJSON));
            Assert.IsTrue(JSONValidatorTrainer.ValidateTrainerJSON(shauntalJSON));
            Assert.IsTrue(JSONValidatorTrainer.ValidateTrainerJSON(shauntalJSON, out IList<string> errors));
            Assert.AreEqual(errors.Count, 0);
            Assert.IsTrue(JSONValidatorTrainer.ValidateTrainerListJSON(multiJSON, out errors));
            Assert.AreEqual(errors.Count, 0);
        }

        [TestMethod]
        public void FailIncorrectJSON()
        {
            Assert.IsFalse(JSONValidatorTrainer.ValidateTrainerJSON(multiJSON));
            Assert.IsFalse(JSONValidatorTrainer.ValidateTrainerListJSON(shauntalJSON));
            Assert.IsFalse(JSONValidatorTrainer.ValidateTrainerJSON(multiJSON, out IList<string> errors));
            Assert.AreEqual(errors.Count, 1);
            Assert.IsFalse(JSONValidatorTrainer.ValidateTrainerListJSON(shauntalJSON, out errors));
            Assert.AreEqual(errors.Count, 1);
        }

        [TestMethod]
        public void ValidateCorrectButStrangeJSON()
        {
            Assert.IsTrue(JSONValidatorTrainer.ValidateTrainerJSON(strangeJSON));
        }

        [TestMethod]
        public void FailToValidateIncompleteJSON()
        {
            Assert.IsFalse(JSONValidatorTrainer.ValidateTrainerJSON(incompleteJSON));
            Assert.IsFalse(JSONValidatorTrainer.ValidateTrainerJSON(incompleteJSON, out IList<string> errors));
            Console.WriteLine(errors.Count);
            Console.WriteLine(errors[0]);
            // There are more mistakes with the json than 1 mistake, but only 1 error is written to the IList. Why is this? Is this correct behaviour?
            Assert.IsTrue(errors.Count > 0);
        }

        [TestMethod]
        public void ValidateCorrectExternalDataJSON()
        {
            Assert.IsTrue(JSONValidatorExternalData.ValidateItemListJSON(itemlistJSON));
            Assert.IsTrue(JSONValidatorExternalData.ValidateMoveListJSON(movelistJSON));
            Assert.IsTrue(JSONValidatorExternalData.ValidatePokemonListJSON(pokemonlistJSON));
            Assert.IsTrue(JSONValidatorExternalData.ValidateSlotListJSON(slotlistJSON));

            Assert.IsTrue(JSONValidatorExternalData.ValidateItemListJSON(itemlistJSON, out IList<string> errors));
            Assert.AreEqual(0, errors.Count);
            Assert.IsTrue(JSONValidatorExternalData.ValidateMoveListJSON(movelistJSON, out errors));
            Assert.AreEqual(0, errors.Count);
            Assert.IsTrue(JSONValidatorExternalData.ValidatePokemonListJSON(pokemonlistJSON, out errors));
            Assert.AreEqual(0, errors.Count);
            Assert.IsTrue(JSONValidatorExternalData.ValidateSlotListJSON(slotlistJSON, out errors));
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void FailIncorrectExternalDataJSON()
        {
            Assert.IsFalse(JSONValidatorExternalData.ValidateItemListJSON(emptyJSON));
            Assert.IsFalse(JSONValidatorExternalData.ValidateMoveListJSON(emptyJSON));
            Assert.IsFalse(JSONValidatorExternalData.ValidatePokemonListJSON(emptyJSON));
            Assert.IsFalse(JSONValidatorExternalData.ValidateSlotListJSON(emptyJSON));

            Assert.IsFalse(JSONValidatorExternalData.ValidateItemListJSON(emptyJSON, out IList<string> errors));
            Assert.IsFalse(JSONValidatorExternalData.ValidateMoveListJSON(emptyJSON, out errors));
            Assert.IsFalse(JSONValidatorExternalData.ValidatePokemonListJSON(emptyJSON, out errors));
            Assert.IsFalse(JSONValidatorExternalData.ValidateSlotListJSON(emptyJSON, out errors));
        }

        [TestMethod]
        public void DeserializeExternalData()
        {
            ExternalItemList itemList = ExternalItemList.DeserializeJSON(itemlistJSON);
            Assert.IsNotNull(itemList);
            Assert.AreEqual(627, itemList.ItemData.Count);
            Assert.AreEqual("None", itemList.ItemData[0]);
            Assert.AreEqual("Master Ball", itemList.ItemData[1]);
            Assert.AreEqual("Xtransceiver", itemList.ItemData[626]);
            Assert.AreEqual(0, itemList.GetIndexOfItem(null));
            Assert.AreEqual(0, itemList.GetIndexOfItem("Garbage Data"));
            Assert.AreEqual(1, itemList.GetIndexOfItem("Master Ball"));
            Assert.AreEqual(610, itemList.GetIndexOfItem("X Accuracy 6"));

            ExternalMoveList moveList = ExternalMoveList.DeserializeJSON(movelistJSON);
            Assert.IsNotNull(moveList);
            Assert.AreEqual(560, moveList.MoveData.Count);
            Assert.AreEqual("-----", moveList.MoveData[0]);
            Assert.AreEqual("Pound", moveList.MoveData[1]);
            Assert.AreEqual("V-create", moveList.MoveData[557]);
            Assert.AreEqual(0, moveList.GetIndexOfMove(null));
            Assert.AreEqual(0, moveList.GetIndexOfMove("Garbage Data"));
            Assert.AreEqual(1, moveList.GetIndexOfMove("Pound"));
            Assert.AreEqual(557, moveList.GetIndexOfMove("V-Create"));

            ExternalPokemonList monList = ExternalPokemonList.DeserializeJSON(pokemonlistJSON);
            Assert.IsNotNull(monList);
            Assert.AreEqual(650, monList.PokemonData.Count);
            Assert.AreEqual("-----", monList.PokemonData[0]);
            Assert.AreEqual("Bulbasaur", monList.PokemonData[1]);
            Assert.AreEqual("Victini", monList.PokemonData[494]);
            Assert.AreEqual("Genesect", monList.PokemonData[649]);
            Assert.AreEqual(0, monList.GetIndexOfPokemon(null));
            Assert.AreEqual(0, monList.GetIndexOfPokemon("Garbage Data"));
            Assert.AreEqual(1, monList.GetIndexOfPokemon("Bulbasaur"));
            Assert.AreEqual(649, monList.GetIndexOfPokemon("Genesect"));

            ExternalTrainerSlotList trainerList = ExternalTrainerSlotList.DeserializeJSON(slotlistJSON);
            Assert.IsNotNull(trainerList);
            Assert.AreEqual(813, trainerList.SlotData.Count);
            Assert.AreEqual("Elena", trainerList.SlotData[0].Name);
            Assert.AreEqual(0, trainerList.SlotData[0].Variation);
            Assert.AreEqual("Elena - 1", trainerList.SlotData[0].ExportName);
            Assert.AreEqual("Iris", trainerList.SlotData[535].Name);
            Assert.AreEqual(1, trainerList.SlotData[535].Variation);
            Assert.AreEqual("Iris - Rematch (Normal Mode)", trainerList.SlotData[535].ExportName);
            Assert.AreEqual(0, trainerList.GetIndexOfSlot(null, 0));
            Assert.AreEqual(0, trainerList.GetIndexOfSlot("Garbage Data", 0));
            Assert.AreEqual(0, trainerList.GetIndexOfSlot("Iris", 222));
            Assert.AreEqual(3, trainerList.GetIndexOfSlot("Aspen", 0));
            Assert.AreEqual(763, trainerList.GetIndexOfSlot("Grunt", 47));
        }

        [TestMethod]
        public void CheckAIFlagsBitmap()
        {
            TrainerRepresentation sample = new();

            Assert.IsTrue(sample.TrainerData.AIFlags.Bitmap == 0);
            sample.TrainerData.AIFlags.CheckBadMoves = true;
            Assert.IsTrue(sample.TrainerData.AIFlags.Bitmap == 1);
            sample.TrainerData.AIFlags.Risky = true;
            Assert.IsTrue(sample.TrainerData.AIFlags.Bitmap == 17);
            sample.TrainerData.AIFlags.Expert = true;
            Assert.IsTrue(sample.TrainerData.AIFlags.Bitmap == 21);
            sample.TrainerData.AIFlags.EvaluateAttacks = true;
            sample.TrainerData.AIFlags.DoubleBattle = true;
            Assert.IsTrue(sample.TrainerData.AIFlags.Bitmap == 151);
            sample.TrainerData.AIFlags.Expert = false;
            Assert.IsTrue(sample.TrainerData.AIFlags.Bitmap == 147);

            sample.TrainerData.AIFlags.Bitmap = 7;
            Assert.IsTrue(sample.TrainerData.AIFlags.CheckBadMoves);
            Assert.IsTrue(sample.TrainerData.AIFlags.EvaluateAttacks);
            Assert.IsTrue(sample.TrainerData.AIFlags.Expert);
            Assert.IsFalse(sample.TrainerData.AIFlags.SetupFirstTurn);
            Assert.IsFalse(sample.TrainerData.AIFlags.Risky);
            Assert.IsFalse(sample.TrainerData.AIFlags.PreferStrongMoves);
            Assert.IsFalse(sample.TrainerData.AIFlags.PreferBatonPass);
            Assert.IsFalse(sample.TrainerData.AIFlags.DoubleBattle);
        }

        [TestMethod]
        public void CheckTrainerDataByteRepresentation()
        {
            TrainerRepresentation shauntal = TrainerRepresentation.DeserializeJSON(shauntalJSON);
            ExternalItemList itemsList = ExternalItemList.DeserializeJSON(itemlistJSON);
            //Target is the exact bytes used on file for shauntal.
            byte[] target = new byte[] { 0x03, 0x4E, 0x00, 0x04, 0x17, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x1E, 0x00, 0x00 };

            byte[] byteRepresentation = shauntal.GetTrainerBytes(itemsList);

            Assert.IsNotNull(byteRepresentation);
            Assert.AreEqual(target.Length, byteRepresentation.Length);
            for (int i = 0; i < target.Length; i++)
                Assert.AreEqual(target[i], byteRepresentation[i]);

        }

        [TestMethod]
        public void CheckPokemonDataByteRepresentation()
        {
            TrainerRepresentation shauntal = TrainerRepresentation.DeserializeJSON(shauntalJSON);
            ExternalItemList itemList = ExternalItemList.DeserializeJSON(itemlistJSON);
            ExternalMoveList moveList = ExternalMoveList.DeserializeJSON(movelistJSON);
            ExternalPokemonList monList = ExternalPokemonList.DeserializeJSON(pokemonlistJSON);

            //Target is the exact bytes used on file for shauntal
            byte[] target = new byte[] 
            {
                0xC8, 0x10, 0x38, 0x00, 0x33, 0x02, 0x00, 0x00, 0x00, 0x00, 0x05, 0x01, 0xBF, 0x01, 0x5E, 0x00, 0xF7, 0x00,
                0xC8, 0x10, 0x38, 0x00, 0xAA, 0x01, 0x00, 0x00, 0x00, 0x00, 0x5E, 0x00, 0x55, 0x00, 0x00, 0x02, 0xF7, 0x00,
                0xC8, 0x10, 0x38, 0x00, 0x6F, 0x02, 0x00, 0x00, 0x00, 0x00, 0xE4, 0x01, 0x59, 0x00, 0x18, 0x01, 0x45, 0x01,
                0xFA, 0x20, 0x3A, 0x00, 0x61, 0x02, 0x00, 0x00, 0x9E, 0x00, 0x9C, 0x01, 0x7E, 0x00, 0x5E, 0x00, 0xF7, 0x00 
            };

            byte[] byteRepresenation = shauntal.GetPokemonBytes(itemList, moveList, monList);

            Assert.IsNotNull(byteRepresenation);
            Assert.AreEqual(target.Length, byteRepresenation.Length);
            for (int i = 0; i < target.Length; i++)
                Assert.AreEqual(target[i], byteRepresenation[i]);

            //Run checks for trainers of all types of formats.
            //Bianca has no items but has moves.
            TrainerRepresentation bianca = TrainerRepresentation.DeserializeJSON(biancaJSON);

            target = new byte[]
            {
                0xFA, 0x10, 0x28, 0x00, 0x06, 0x02, 0x00, 0x00, 0x5E, 0x00, 0x9C, 0x01, 0x5F, 0x00, 0xF7, 0x00,
                0xFA, 0x10, 0x26, 0x00, 0xFC, 0x01, 0x00, 0x00, 0xA7, 0x01, 0xA8, 0x01, 0xA6, 0x01, 0xD8, 0x00,
                0xFA, 0x10, 0x26, 0x00, 0x6B, 0x02, 0x00, 0x00, 0x99, 0x01, 0xFC, 0x00, 0x71, 0x01, 0x00, 0x02
            };

            byteRepresenation = bianca.GetPokemonBytes(itemList, moveList, monList);

            Assert.IsNotNull(byteRepresenation);
            Assert.AreEqual(target.Length, byteRepresenation.Length);
            for (int i = 0; i < target.Length; i++)
                Assert.AreEqual(target[i], byteRepresenation[i]);

            //Fletcher has no items or moves
            TrainerRepresentation fletcher = TrainerRepresentation.DeserializeJSON(fletcherJSON);

            target = new byte[]
            {
                0x00, 0x00, 0x3C, 0x00, 0x06, 0x01, 0x00, 0x00,
                0x00, 0x00, 0x3C, 0x00, 0xB3, 0x01, 0x00, 0x00,
                0x00, 0x00, 0x3C, 0x00, 0x56, 0x01, 0x00, 0x00
            };

            byteRepresenation = fletcher.GetPokemonBytes(itemList, moveList, monList);

            Assert.IsNotNull(byteRepresenation);
            Assert.AreEqual(target.Length, byteRepresenation.Length);
            for (int i = 0; i < target.Length; i++)
                Assert.AreEqual(target[i], byteRepresenation[i]);

            //Hugh has no moves but has items
            TrainerRepresentation hugh = TrainerRepresentation.DeserializeJSON(hugh9JSON);

            target = new byte[]
            {
                0x64, 0x10, 0x27, 0x00, 0x09, 0x02, 0x00, 0x00, 0xE8, 0x00,
                0x64, 0x00, 0x27, 0x00, 0x04, 0x02, 0x00, 0x00, 0xF3, 0x00,
                0x96, 0x00, 0x29, 0x00, 0xF4, 0x01, 0x00, 0x00, 0xF9, 0x00
            };

            byteRepresenation = hugh.GetPokemonBytes(itemList, moveList, monList);

            Assert.IsNotNull(byteRepresenation);
            Assert.AreEqual(target.Length, byteRepresenation.Length);
            for (int i = 0; i < target.Length; i++)
                Assert.AreEqual(target[i], byteRepresenation[i]);
        }

        [TestMethod]
        public void CheckByteUnpacking()
        {
            ExternalItemList items = ExternalItemList.DeserializeJSON(itemlistJSON);
            ExternalMoveList moves = ExternalMoveList.DeserializeJSON(movelistJSON);
            ExternalPokemonList mons = ExternalPokemonList.DeserializeJSON(pokemonlistJSON);
            ExternalTrainerSlotList slots = ExternalTrainerSlotList.DeserializeJSON(slotlistJSON);

            //the exact bytes used on file for shauntal.
            byte[] shauntalTRData = new byte[] { 0x03, 0x4E, 0x00, 0x04, 0x17, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x1E, 0x00, 0x00 };
            //the exact bytes used on file for shauntal
            byte[] shauntalTRPoke = new byte[]
            {
                0xC8, 0x10, 0x38, 0x00, 0x33, 0x02, 0x00, 0x00, 0x00, 0x00, 0x05, 0x01, 0xBF, 0x01, 0x5E, 0x00, 0xF7, 0x00,
                0xC8, 0x10, 0x38, 0x00, 0xAA, 0x01, 0x00, 0x00, 0x00, 0x00, 0x5E, 0x00, 0x55, 0x00, 0x00, 0x02, 0xF7, 0x00,
                0xC8, 0x10, 0x38, 0x00, 0x6F, 0x02, 0x00, 0x00, 0x00, 0x00, 0xE4, 0x01, 0x59, 0x00, 0x18, 0x01, 0x45, 0x01,
                0xFA, 0x20, 0x3A, 0x00, 0x61, 0x02, 0x00, 0x00, 0x9E, 0x00, 0x9C, 0x01, 0x7E, 0x00, 0x5E, 0x00, 0xF7, 0x00
            };

            TrainerRepresentation shauntal = TrainerRepresentation.BuildFromBytes(shauntalTRData, shauntalTRPoke, 38, items, moves, mons, slots);

            Assert.IsNotNull(shauntal);
            Assert.AreEqual(38, shauntal.TrainerData.Identification.NumberID);
            Assert.AreEqual("Shauntal", shauntal.TrainerData.Identification.NameID.Name);
            Assert.AreEqual(0, shauntal.TrainerData.Identification.NameID.Variation);
            Assert.AreEqual(78, shauntal.TrainerData.TrainerClass.NumberID);
            Assert.AreEqual(BattleType.Single, shauntal.TrainerData.BattleType);
            Assert.IsTrue(shauntal.TrainerData.Format.Moves);
            Assert.IsTrue(shauntal.TrainerData.Format.Items);
            Assert.AreEqual(30, shauntal.TrainerData.BaseMoney);
            Assert.AreEqual(4, shauntal.TrainerData.Items.Length);
            Assert.AreEqual("Full Restore", shauntal.TrainerData.Items[0]);
            for (int i = 1; i < 4; i++)
                Assert.AreEqual(null, shauntal.TrainerData.Items[i]);
            Assert.AreEqual(7, shauntal.TrainerData.AIFlags.Bitmap);
            Assert.IsFalse(shauntal.TrainerData.Healer);
            Assert.AreEqual(4, shauntal.PokemonCount);
            Assert.AreEqual(4, shauntal.PokemonData.Length);
            string[] monnames = { "Cofagrigus", "Drifblim", "Golurk", "Chandelure" };
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(monnames[i], shauntal.PokemonData[i].Pokemon);
                Assert.AreEqual(0, shauntal.PokemonData[i].Form);
                Assert.AreEqual(Gender.Random, shauntal.PokemonData[i].Miscellaneous.Gender);
            }
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(56, shauntal.PokemonData[i].Level);
                Assert.AreEqual(200, shauntal.PokemonData[i].Difficulty);
                Assert.AreEqual(1, shauntal.PokemonData[i].Miscellaneous.Ability);
                Assert.AreEqual(null, shauntal.PokemonData[i].Item);
            }
            Assert.AreEqual(58, shauntal.PokemonData[3].Level);
            Assert.AreEqual(250, shauntal.PokemonData[3].Difficulty);
            Assert.AreEqual(2, shauntal.PokemonData[3].Miscellaneous.Ability);
            Assert.AreEqual("Sitrus Berry", shauntal.PokemonData[3].Item);
            string[,] monmoves = { { "Will-O-Wisp", "Grass Knot", "Psychic", "Shadow Ball" },
                                    { "Psychic", "Thunderbolt", "Acrobatics", "Shadow Ball" },
                                    { "Heavy Slam", "Earthquake", "Brick Break", "Shadow Punch" },
                                    { "Energy Ball", "Fire Blast", "Psychic", "Shadow Ball"} };
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    Assert.AreEqual(monmoves[i, j], shauntal.PokemonData[i].Moves[j]);
        }

        [TestMethod]
        public void CheckSlotIDCalculation()
        {
            ExternalTrainerSlotList slotList = ExternalTrainerSlotList.DeserializeJSON(slotlistJSON);

            TrainerRepresentation shauntal = TrainerRepresentation.DeserializeJSON(shauntalJSON);
            TrainerRepresentation hugh = TrainerRepresentation.DeserializeJSON(hugh9JSON);

            Assert.AreEqual(38, shauntal.GetSlotID(slotList));
            Assert.AreEqual(378, hugh.GetSlotID(slotList));
        }

        [TestMethod]
        public void CheckTrainerRepresentationSetDeserialization()
        {
            TrainerRepresentationSet set = new();

            set.InitializeWithJSON(multiJSON);

            List<TrainerRepresentation> fluxed = TrainerRepresentation.DeserializeListJSON(set.SerializeJSON());

            Assert.IsNotNull(fluxed);
            Assert.AreEqual(2, fluxed.Count);
            TrainerRepresentation shauntal = fluxed[0];
            Assert.AreEqual(-1, shauntal.TrainerData.Identification.NumberID);
            Assert.AreEqual("Shauntal", shauntal.TrainerData.Identification.NameID.Name);
            Assert.AreEqual(0, shauntal.TrainerData.Identification.NameID.Variation);
            Assert.AreEqual(78, shauntal.TrainerData.TrainerClass.NumberID);
            Assert.AreEqual(BattleType.Single, shauntal.TrainerData.BattleType);
            Assert.IsTrue(shauntal.TrainerData.Format.Moves);
            Assert.IsTrue(shauntal.TrainerData.Format.Items);
            Assert.AreEqual(30, shauntal.TrainerData.BaseMoney);
            Assert.AreEqual(4, shauntal.TrainerData.Items.Length);
            Assert.AreEqual("Full Restore", shauntal.TrainerData.Items[0]);
            for (int i = 1; i < 4; i++)
                Assert.AreEqual(null, shauntal.TrainerData.Items[i]);
            Assert.AreEqual(7, shauntal.TrainerData.AIFlags.Bitmap);
            Assert.IsFalse(shauntal.TrainerData.Healer);
            Assert.AreEqual(4, shauntal.PokemonCount);
            Assert.AreEqual(4, shauntal.PokemonData.Length);
            string[] monnames = { "Cofagrigus", "Drifblim", "Golurk", "Chandelure" };
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(monnames[i], shauntal.PokemonData[i].Pokemon);
                Assert.AreEqual(0, shauntal.PokemonData[i].Form);
                Assert.AreEqual(Gender.Random, shauntal.PokemonData[i].Miscellaneous.Gender);
            }
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(56, shauntal.PokemonData[i].Level);
                Assert.AreEqual(200, shauntal.PokemonData[i].Difficulty);
                Assert.AreEqual(1, shauntal.PokemonData[i].Miscellaneous.Ability);
                Assert.AreEqual(null, shauntal.PokemonData[i].Item);
            }
            Assert.AreEqual(58, shauntal.PokemonData[3].Level);
            Assert.AreEqual(250, shauntal.PokemonData[3].Difficulty);
            Assert.AreEqual(2, shauntal.PokemonData[3].Miscellaneous.Ability);
            Assert.AreEqual("Sitrus Berry", shauntal.PokemonData[3].Item);
            string[,] monmoves = { { "Will-O-Wisp", "Grass Knot", "Psychic", "Shadow Ball" },
                                    { "Psychic", "Thunderbolt", "Acrobatics", "Shadow Ball" },
                                    { "Heavy Slam", "Earthquake", "Brick Break", "Shadow Punch" },
                                    { "Energy Ball", "Fire Blast", "Psychic", "Shadow Ball"} };
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    Assert.AreEqual(monmoves[i, j], shauntal.PokemonData[i].Moves[j]);

            TrainerRepresentation marshal = fluxed[1];
            Assert.AreEqual(-1, marshal.TrainerData.Identification.NumberID);
            Assert.AreEqual("Marshal", marshal.TrainerData.Identification.NameID.Name);
            Assert.AreEqual(0, marshal.TrainerData.Identification.NameID.Variation);
            Assert.AreEqual(79, marshal.TrainerData.TrainerClass.NumberID);
            Assert.AreEqual(BattleType.Single, marshal.TrainerData.BattleType);
            Assert.IsTrue(marshal.TrainerData.Format.Moves);
            Assert.IsTrue(marshal.TrainerData.Format.Items);
            Assert.AreEqual(30, marshal.TrainerData.BaseMoney);
            Assert.AreEqual(4, marshal.TrainerData.Items.Length);
            Assert.AreEqual("Full Restore", marshal.TrainerData.Items[0]);
            for (int i = 1; i < 4; i++)
                Assert.AreEqual(null, marshal.TrainerData.Items[i]);
            Assert.AreEqual(7, marshal.TrainerData.AIFlags.Bitmap);
            Assert.IsFalse(marshal.TrainerData.Healer);
            Assert.AreEqual(4, marshal.PokemonCount);
            Assert.AreEqual(4, marshal.PokemonData.Length);
            string[] marshalmonnames = { "Throh", "Sawk", "Mienshao", "Conkeldurr" };
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(marshalmonnames[i], marshal.PokemonData[i].Pokemon);
                Assert.AreEqual(0, marshal.PokemonData[i].Form);
                Assert.AreEqual(Gender.Random, marshal.PokemonData[i].Miscellaneous.Gender);
            }
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(56, marshal.PokemonData[i].Level);
                Assert.AreEqual(200, marshal.PokemonData[i].Difficulty);
                Assert.AreEqual(null, marshal.PokemonData[i].Item);
            }
            Assert.AreEqual(1, marshal.PokemonData[0].Miscellaneous.Ability);
            Assert.AreEqual(1, marshal.PokemonData[1].Miscellaneous.Ability);
            Assert.AreEqual(2, marshal.PokemonData[2].Miscellaneous.Ability);
            Assert.AreEqual(1, marshal.PokemonData[3].Miscellaneous.Ability);
            Assert.AreEqual(58, marshal.PokemonData[3].Level);
            Assert.AreEqual(250, marshal.PokemonData[3].Difficulty);
            Assert.AreEqual("Sitrus Berry", marshal.PokemonData[3].Item);
            string[,] marshalmonmoves = { { "Storm Throw", "Bulldoze", "Rock Tomb", "Payback" },
                                    { "Brick Break", "Retaliate", "Rock Slide", "Payback" },
                                    { "Hi Jump Kick", "U-Turn", "Bounce", "Retaliate" },
                                    { "Hammer Arm", "Bulk Up", "Stone Edge", "Retaliate"} };
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    Assert.AreEqual(marshalmonmoves[i, j], marshal.PokemonData[i].Moves[j]);
        }

        [TestMethod]
        public void CheckTrainerSetValidation()
        {
            ExternalTrainerSlotList smallSlots = ExternalTrainerSlotList.DeserializeJSON(smallslotlistJSON);

            TrainerRepresentationSet elenaOnly = new();
            TrainerRepresentationSet elenaDan = new();
            TrainerRepresentationSet elenaDanDan = new();
            TrainerRepresentationSet elenaShauntalDan = new();
            TrainerRepresentationSet shauntalMarshal = new();

            elenaOnly.InitializeWithJSON(customelenaJSON);
            elenaDan.InitializeWithJSON(customelenadanJSON);
            elenaDanDan.InitializeWithJSON(customelenadan2JSON);
            elenaShauntalDan.InitializeWithJSON(custom3JSON);
            shauntalMarshal.InitializeWithJSON(multiJSON);

            Assert.IsNotNull(elenaOnly);
            Assert.IsNotNull(elenaDan);
            Assert.IsNotNull(elenaDanDan);
            Assert.IsNotNull(elenaShauntalDan);
            Assert.IsNotNull(shauntalMarshal);

            Assert.IsTrue(elenaOnly.ValidateNoDuplicates(smallSlots));
            Assert.IsTrue(elenaDan.ValidateNoDuplicates(smallSlots));
            Assert.IsFalse(elenaDanDan.ValidateNoDuplicates(smallSlots));
            Assert.IsTrue(elenaShauntalDan.ValidateNoDuplicates(smallSlots));
            Assert.IsTrue(shauntalMarshal.ValidateNoDuplicates(smallSlots));

            Assert.IsFalse(elenaOnly.ValidateAllSlotsUsed(smallSlots));
            Assert.IsTrue(elenaDan.ValidateAllSlotsUsed(smallSlots));
            Assert.IsTrue(elenaDanDan.ValidateAllSlotsUsed(smallSlots));
            Assert.IsTrue(elenaShauntalDan.ValidateAllSlotsUsed(smallSlots));
            Assert.IsFalse(elenaOnly.ValidateAllSlotsUsed(smallSlots));
        }

        [TestMethod]
        public void CheckGetByteRepresentation()
        {
            ExternalTrainerSlotList slots = ExternalTrainerSlotList.DeserializeJSON(smallslotlist2JSON);

            ExternalItemList items = ExternalItemList.DeserializeJSON(itemlistJSON);
            ExternalMoveList moves = ExternalMoveList.DeserializeJSON(movelistJSON);
            ExternalPokemonList pokemon = ExternalPokemonList.DeserializeJSON(pokemonlistJSON);

            TrainerRepresentationSet set = new();

            set.InitializeWithJSON(multi2JSON);

            Assert.IsNotNull(set);

            Assert.IsTrue(set.ValidateAllSlotsUsed(slots));
            Assert.IsTrue(set.ValidateNoDuplicates(slots));

            set.GetByteData(out byte[][] TRData, out byte[][] TRPoke, items, moves, pokemon, slots);

            Assert.IsNotNull(TRData);
            Assert.IsNotNull(TRPoke);

            //Check that the zeroth items are placeholders and properly only filled with zeros
            foreach (byte b in TRData[0])
                Assert.AreEqual(0x00, b);
            foreach (byte b in TRPoke[0])
                Assert.AreEqual(0x00, b);

            //Check that the data matches the data taken from the files
            //the exact bytes used on file for shauntal.
            byte[] shauntalTRData = new byte[] { 0x03, 0x4E, 0x00, 0x04, 0x17, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x1E, 0x00, 0x00 };
            //the exact bytes used on file for marshal
            byte[] shauntalTRPoke = new byte[]
            {
                0xC8, 0x10, 0x38, 0x00, 0x33, 0x02, 0x00, 0x00, 0x00, 0x00, 0x05, 0x01, 0xBF, 0x01, 0x5E, 0x00, 0xF7, 0x00,
                0xC8, 0x10, 0x38, 0x00, 0xAA, 0x01, 0x00, 0x00, 0x00, 0x00, 0x5E, 0x00, 0x55, 0x00, 0x00, 0x02, 0xF7, 0x00,
                0xC8, 0x10, 0x38, 0x00, 0x6F, 0x02, 0x00, 0x00, 0x00, 0x00, 0xE4, 0x01, 0x59, 0x00, 0x18, 0x01, 0x45, 0x01,
                0xFA, 0x20, 0x3A, 0x00, 0x61, 0x02, 0x00, 0x00, 0x9E, 0x00, 0x9C, 0x01, 0x7E, 0x00, 0x5E, 0x00, 0xF7, 0x00
            };
            byte[] marshalTRData = new byte[] { 0x03, 0x4F, 0x00, 0x04, 0x17, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x1E, 0x00, 0x00 };
            //the exact bytes used on file for marshal
            byte[] marshalTRPoke = new byte[]
            {
                0xC8, 0x10, 0x38, 0x00, 0x1A, 0x02, 0x00, 0x00, 0x00, 0x00, 0xE0, 0x01, 0x0B, 0x02, 0x3D, 0x01, 0x73, 0x01,
                0xC8, 0x10, 0x38, 0x00, 0x1B, 0x02, 0x00, 0x00, 0x00, 0x00, 0x18, 0x01, 0x02, 0x02, 0x9D, 0x00, 0x73, 0x01,
                0xC8, 0x20, 0x38, 0x00, 0x6C, 0x02, 0x00, 0x00, 0x00, 0x00, 0x88, 0x00, 0x71, 0x01, 0x54, 0x01, 0x02, 0x02,
                0xFA, 0x10, 0x3A, 0x00, 0x16, 0x02, 0x00, 0x00, 0x9E, 0x00, 0x67, 0x01, 0x53, 0x01, 0xBC, 0x01, 0x02, 0x02
            };

            Assert.AreEqual(shauntalTRData.Length, TRData[1].Length);
            Assert.AreEqual(shauntalTRPoke.Length, TRPoke[1].Length);
            Assert.AreEqual(marshalTRData.Length, TRData[2].Length);
            Assert.AreEqual(marshalTRPoke.Length, TRPoke[2].Length);

            for (int i = 0; i < shauntalTRData.Length; i++)
                Assert.AreEqual(shauntalTRData[i], TRData[1][i]);
            for (int i = 0; i < shauntalTRPoke.Length; i++)
                Assert.AreEqual(shauntalTRPoke[i], TRPoke[1][i]);
            for (int i = 0; i < marshalTRData.Length; i++)
                Assert.AreEqual(marshalTRData[i], TRData[2][i]);
            for (int i = 0; i < marshalTRPoke.Length; i++)
                Assert.AreEqual(marshalTRPoke[i], TRPoke[2][i]);
        }

        [TestMethod]
        public void AlterTrainerRepresentationSet()
        {
            TrainerRepresentationSet set = new();

            set.InitializeWithJSON(multi2JSON);

            set.AlterWithJSON(shauntalweakJSON);

            List<TrainerRepresentation> listSet = TrainerRepresentation.DeserializeListJSON(set.SerializeJSON());

            Assert.IsNotNull(listSet);
            Assert.AreEqual(2, listSet.Count);

            Assert.AreEqual(1, listSet[0].PokemonCount);
            Assert.AreEqual(1, listSet[0].PokemonData.Length);
            Assert.AreEqual("Duskull", listSet[0].PokemonData[0].Pokemon);
            Assert.AreEqual(5, listSet[0].PokemonData[0].Level);
            Assert.AreEqual(0, listSet[0].PokemonData[0].Difficulty);
            Assert.IsFalse(listSet[0].TrainerData.Format.Items);
        }

        [TestMethod]
        public void CheckNarcCompiling()
        {
            ExternalTrainerSlotList slots = ExternalTrainerSlotList.DeserializeJSON(smallslotlist2JSON);

            ExternalItemList items = ExternalItemList.DeserializeJSON(itemlistJSON);
            ExternalMoveList moves = ExternalMoveList.DeserializeJSON(movelistJSON);
            ExternalPokemonList pokemon = ExternalPokemonList.DeserializeJSON(pokemonlistJSON);

            TrainerRepresentationSet set = new();

            set.InitializeWithJSON(multi2JSON);

            set.GetNarc(out byte[] TRData, out byte[] TRPoke, items, moves, pokemon, slots);

            Console.WriteLine(BitConverter.ToString(TRPoke));

            byte[] desired = File.ReadAllBytes(simpleTRData);

            Assert.AreEqual(desired.Length, TRData.Length, "Generated bytes does not match size.");

            for (int i = 0; i < desired.Length; i++)
                Assert.AreEqual(desired[i], TRData[i], "Incorrect at byte " + i);
        }

        [TestMethod]
        public void CheckNarcDecompile()
        {
            ExternalTrainerSlotList slots = ExternalTrainerSlotList.DeserializeJSON(smallslotlist2JSON);

            ExternalItemList items = ExternalItemList.DeserializeJSON(itemlistJSON);
            ExternalMoveList moves = ExternalMoveList.DeserializeJSON(movelistJSON);
            ExternalPokemonList pokemon = ExternalPokemonList.DeserializeJSON(pokemonlistJSON);

            TrainerRepresentationSet set = new();

            set.InitializeWithNarc(simpleTRData, simpleTRPoke, items, moves, pokemon, slots);

            List<TrainerRepresentation> fluxed = TrainerRepresentation.DeserializeListJSON(set.SerializeJSON());

            Assert.IsNotNull(fluxed);
            Assert.AreEqual(2, fluxed.Count);
            TrainerRepresentation shauntal = fluxed[0];
            Assert.AreEqual(-1, shauntal.TrainerData.Identification.NumberID);
            Assert.AreEqual("Shauntal", shauntal.TrainerData.Identification.NameID.Name);
            Assert.AreEqual(0, shauntal.TrainerData.Identification.NameID.Variation);
            Assert.AreEqual(78, shauntal.TrainerData.TrainerClass.NumberID);
            Assert.AreEqual(BattleType.Single, shauntal.TrainerData.BattleType);
            Assert.IsTrue(shauntal.TrainerData.Format.Moves);
            Assert.IsTrue(shauntal.TrainerData.Format.Items);
            Assert.AreEqual(30, shauntal.TrainerData.BaseMoney);
            Assert.AreEqual(4, shauntal.TrainerData.Items.Length);
            Assert.AreEqual("Full Restore", shauntal.TrainerData.Items[0]);
            for (int i = 1; i < 4; i++)
                Assert.AreEqual(null, shauntal.TrainerData.Items[i]);
            Assert.AreEqual(7, shauntal.TrainerData.AIFlags.Bitmap);
            Assert.IsFalse(shauntal.TrainerData.Healer);
            Assert.AreEqual(4, shauntal.PokemonCount);
            Assert.AreEqual(4, shauntal.PokemonData.Length);
            string[] monnames = { "Cofagrigus", "Drifblim", "Golurk", "Chandelure" };
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(monnames[i], shauntal.PokemonData[i].Pokemon);
                Assert.AreEqual(0, shauntal.PokemonData[i].Form);
                Assert.AreEqual(Gender.Random, shauntal.PokemonData[i].Miscellaneous.Gender);
            }
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(56, shauntal.PokemonData[i].Level);
                Assert.AreEqual(200, shauntal.PokemonData[i].Difficulty);
                Assert.AreEqual(1, shauntal.PokemonData[i].Miscellaneous.Ability);
                Assert.AreEqual(null, shauntal.PokemonData[i].Item);
            }
            Assert.AreEqual(58, shauntal.PokemonData[3].Level);
            Assert.AreEqual(250, shauntal.PokemonData[3].Difficulty);
            Assert.AreEqual(2, shauntal.PokemonData[3].Miscellaneous.Ability);
            Assert.AreEqual("Sitrus Berry", shauntal.PokemonData[3].Item);
            string[,] monmoves = { { "Will-O-Wisp", "Grass Knot", "Psychic", "Shadow Ball" },
                                    { "Psychic", "Thunderbolt", "Acrobatics", "Shadow Ball" },
                                    { "Heavy Slam", "Earthquake", "Brick Break", "Shadow Punch" },
                                    { "Energy Ball", "Fire Blast", "Psychic", "Shadow Ball"} };
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    Assert.AreEqual(monmoves[i, j], shauntal.PokemonData[i].Moves[j]);

            TrainerRepresentation marshal = fluxed[1];
            Assert.AreEqual(-1, marshal.TrainerData.Identification.NumberID);
            Assert.AreEqual("Marshal", marshal.TrainerData.Identification.NameID.Name);
            Assert.AreEqual(0, marshal.TrainerData.Identification.NameID.Variation);
            Assert.AreEqual(79, marshal.TrainerData.TrainerClass.NumberID);
            Assert.AreEqual(BattleType.Single, marshal.TrainerData.BattleType);
            Assert.IsTrue(marshal.TrainerData.Format.Moves);
            Assert.IsTrue(marshal.TrainerData.Format.Items);
            Assert.AreEqual(30, marshal.TrainerData.BaseMoney);
            Assert.AreEqual(4, marshal.TrainerData.Items.Length);
            Assert.AreEqual("Full Restore", marshal.TrainerData.Items[0]);
            for (int i = 1; i < 4; i++)
                Assert.AreEqual(null, marshal.TrainerData.Items[i]);
            Assert.AreEqual(7, marshal.TrainerData.AIFlags.Bitmap);
            Assert.IsFalse(marshal.TrainerData.Healer);
            Assert.AreEqual(4, marshal.PokemonCount);
            Assert.AreEqual(4, marshal.PokemonData.Length);
            string[] marshalmonnames = { "Throh", "Sawk", "Mienshao", "Conkeldurr" };
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(marshalmonnames[i], marshal.PokemonData[i].Pokemon);
                Assert.AreEqual(0, marshal.PokemonData[i].Form);
                Assert.AreEqual(Gender.Random, marshal.PokemonData[i].Miscellaneous.Gender);
            }
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(56, marshal.PokemonData[i].Level);
                Assert.AreEqual(200, marshal.PokemonData[i].Difficulty);
                Assert.AreEqual(null, marshal.PokemonData[i].Item);
            }
            Assert.AreEqual(1, marshal.PokemonData[0].Miscellaneous.Ability);
            Assert.AreEqual(1, marshal.PokemonData[1].Miscellaneous.Ability);
            Assert.AreEqual(2, marshal.PokemonData[2].Miscellaneous.Ability);
            Assert.AreEqual(1, marshal.PokemonData[3].Miscellaneous.Ability);
            Assert.AreEqual(58, marshal.PokemonData[3].Level);
            Assert.AreEqual(250, marshal.PokemonData[3].Difficulty);
            Assert.AreEqual("Sitrus Berry", marshal.PokemonData[3].Item);
            string[,] marshalmonmoves = { { "Storm Throw", "Bulldoze", "Rock Tomb", "Payback" },
                                    { "Brick Break", "Retaliate", "Rock Slide", "Payback" },
                                    { "Hi Jump Kick", "U-turn", "Bounce", "Retaliate" },
                                    { "Hammer Arm", "Bulk Up", "Stone Edge", "Retaliate"} };
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    Assert.AreEqual(marshalmonmoves[i, j], marshal.PokemonData[i].Moves[j]);
        }

        [TestMethod]
        public void CheckLearnsetSetValidation()
        {
            Assert.IsTrue(JSONValidatorLearnsetSet.ValidateLearnsetSetJSON(bulbalineJSON));
            Assert.IsFalse(JSONValidatorLearnsetSet.ValidateLearnsetSetJSON(bulbalinewrongJSON));

            Assert.IsTrue(JSONValidatorLearnsetSet.ValidateLearnsetSetJSON(bulbalineJSON, out IList<string> errors));
            Assert.IsFalse(JSONValidatorLearnsetSet.ValidateLearnsetSetJSON(bulbalinewrongJSON, out errors));
            Assert.AreEqual(1, errors.Count);
        }

        [TestMethod]
        public void CheckLearnsetCompiling()
        {
            ExternalPokemonList pokemon = ExternalPokemonList.DeserializeJSON(bulbapokemonlistJSON);
            ExternalMoveList moves = ExternalMoveList.DeserializeJSON(movelistJSON);

            LearnsetSet set = new LearnsetSet();

            set.InitializeWithJSON(bulbalineJSON);

            Assert.IsNotNull(set);

            Assert.IsTrue(set.ValidateAllSlotsUsed(pokemon));

            byte[] narcData = set.GetNarcData(moves, pokemon);

            Assert.IsNotNull(narcData);

            byte[] desired = File.ReadAllBytes(simpleLearnset);

            Assert.AreEqual(desired.Length, narcData.Length, "The length of the produced file does not match with the desired file.");

            for (int i = 0; i < desired.Length; i++)
                Assert.AreEqual(desired[i], narcData[i], "Byte " + i + " does not match between desired and produced.");
        }

        [TestMethod]
        public void CheckLearnsetSerialization()
        {
            LearnsetSet set = new LearnsetSet();

            set.InitializeWithJSON(bulbalineJSON);

            Assert.IsNotNull(set);

            string returned = set.SerializeJSON();

            //Compare the two strings while ignoring capitalization and whitespace
            Assert.IsTrue(String.Compare(bulbalineJSON, returned, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.CompareOptions.IgnoreCase | System.Globalization.CompareOptions.IgnoreSymbols) == 0);
        }

        [TestMethod]
        public void CheckLearnsetDecompiling()
        {
            ExternalPokemonList pokemon = ExternalPokemonList.DeserializeJSON(bulbapokemonlistJSON);
            ExternalMoveList moves = ExternalMoveList.DeserializeJSON(movelistJSON);

            LearnsetSet set = new LearnsetSet();

            set.InitializeWithNarc(simpleLearnset, moves, pokemon);

            Assert.IsTrue(set.Initialized);

            string returned = set.SerializeJSON();

            //Compare the two strings while ignoring capitalization and whitespace
            Assert.IsTrue(String.Compare(bulbalineJSON, returned, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.CompareOptions.IgnoreCase | System.Globalization.CompareOptions.IgnoreSymbols) == 0);

            byte[] narcData = set.GetNarcData(moves, pokemon);

            byte[] desired = File.ReadAllBytes(simpleLearnset);

            Assert.AreEqual(desired.Length, narcData.Length, "The length of the produced file does not match with the desired file.");

            for (int i = 0; i < desired.Length; i++)
                Assert.AreEqual(desired[i], narcData[i], "Byte " + i + " does not match between desired and produced.");
        }

        [TestMethod]
        public void CheckProduceDocumentation()
        {
            ExternalTrainerSlotList slots = ExternalTrainerSlotList.DeserializeJSON(slotlistJSON);

            TrainerRepresentationSet set = new TrainerRepresentationSet();

            set.InitializeWithJSON(multi2JSON);

            FileStream input = new FileStream(shorte4Doc, FileMode.Open, FileAccess.Read);

            set.ProduceDocumentation(slots, input, docOutput);

            Assert.IsTrue(File.Exists(docOutput));

            Console.WriteLine(File.ReadAllText(docOutput));
        }

        [TestMethod]
        public void CheckProduceDocumentationWLearnsets()
        {
            ExternalTrainerSlotList slots = ExternalTrainerSlotList.DeserializeJSON(slotlistJSON);

            TrainerRepresentationSet set = new TrainerRepresentationSet();

            set.InitializeWithJSON(multimovelessJSON);

            Assert.IsTrue(set.Initialized);

            LearnsetSet learnsets = new LearnsetSet();

            learnsets.InitializeWithJSON(halfe4JSON);

            Assert.IsTrue(learnsets.Initialized);

            FileStream input = new FileStream(shorte4Doc, FileMode.Open, FileAccess.Read);

            set.ProduceDocumentation(slots, input, docOutput, learnsets);

            Assert.IsTrue(File.Exists(docOutput));

            Console.WriteLine(File.ReadAllText(docOutput));
        }
    }
}
