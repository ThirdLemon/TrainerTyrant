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
        private string strangeJSON;
        private string incompleteJSON;
        private string multiJSON;

        [TestInitialize]
        public void TestInit()
        {
            shauntalJSON = File.ReadAllText("../../../SampleJSON/sampleJSON1.json");

            strangeJSON = File.ReadAllText("../../../SampleJSON/sampleJSON2.json");

            incompleteJSON = File.ReadAllText("../../../SampleJSON/sampleJSON3.json");

            multiJSON = File.ReadAllText("../../../SampleJSON/sampleJSON4.json");
        }

        [TestMethod]
        public void DeserializeNoErrors()
        {
            

            TrainerRepresentation shauntal = Newtonsoft.Json.JsonConvert.DeserializeObject<TrainerRepresentation>(shauntalJSON);

            Assert.AreEqual(shauntal.TrainerData.Identification.NumberID, 38);
            Assert.AreEqual(shauntal.TrainerData.Identification.NameID.Name, "Shauntal");
            Assert.AreEqual(shauntal.TrainerData.BattleType, "Single");
            Assert.IsTrue(shauntal.TrainerData.Format.Items);
            Assert.IsTrue(shauntal.TrainerData.Items.Length == 4);
            Assert.IsTrue(shauntal.TrainerData.Items[0] == "Full Restore");
            Assert.IsTrue(shauntal.TrainerData.AIFlags.CheckBadMoves);
            Assert.IsTrue(shauntal.PokemonData.Length == 4);
            Assert.AreEqual(shauntal.PokemonCount, shauntal.PokemonData.Length);
        }

        [TestMethod]
        public void SerializeNoErrors()
        {
            TrainerRepresentation shauntal = new();
            shauntal.TrainerData.Identification.NumberID = 38;
            shauntal.TrainerData.TrainerClass.NumberID = 78;
            shauntal.TrainerData.BattleType = "Single";
            shauntal.TrainerData.Format.Moves = true;
            shauntal.TrainerData.Format.Items = true;
            shauntal.TrainerData.BaseMoney = 1200;
            shauntal.TrainerData.Items = new string[] { "Full Restore", null, null, null };
            shauntal.TrainerData.AIFlags.CheckBadMoves = true;
            shauntal.TrainerData.AIFlags.EvaluateAttacks = true;
            shauntal.TrainerData.AIFlags.Expert = true;
            shauntal.PokemonData = new PokemonData[] {new PokemonData() };
            shauntal.PokemonData[0].Pokemon = "Cofagrigus";
            shauntal.PokemonData[0].Form = 0;
            shauntal.PokemonData[0].Level = 56;
            shauntal.PokemonData[0].Difficulty = 200;
            shauntal.PokemonData[0].Miscellaneous.Gender = "Random";
            shauntal.PokemonData[0].Miscellaneous.Ability = 0;
            shauntal.PokemonData[0].Moves = new string[] { "Will-O-Wisp", "Grass Knot", "Psychic", "Shadow Ball" };
            shauntal.PokemonData[0].Item = null;

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(shauntal, Newtonsoft.Json.Formatting.Indented);

            Console.WriteLine(json);
        }

        [TestMethod]
        public void MultiSerializeNoErrors()
        {
            TrainerRepresentation shauntal = new();
            shauntal.TrainerData.Identification.NumberID = 38;
            shauntal.TrainerData.TrainerClass.NumberID = 78;
            shauntal.TrainerData.BattleType = "Single";
            shauntal.TrainerData.Format.Moves = true;
            shauntal.TrainerData.Format.Items = true;
            shauntal.TrainerData.BaseMoney = 1200;
            shauntal.TrainerData.Items = new string[] { "Full Restore", null, null, null };
            shauntal.TrainerData.AIFlags.CheckBadMoves = true;
            shauntal.TrainerData.AIFlags.EvaluateAttacks = true;
            shauntal.TrainerData.AIFlags.Expert = true;
            shauntal.PokemonData = new PokemonData[] { new PokemonData() };
            shauntal.PokemonData[0].Pokemon = "Cofagrigus";
            shauntal.PokemonData[0].Form = 0;
            shauntal.PokemonData[0].Level = 56;
            shauntal.PokemonData[0].Difficulty = 200;
            shauntal.PokemonData[0].Miscellaneous.Gender = "Random";
            shauntal.PokemonData[0].Miscellaneous.Ability = 0;
            shauntal.PokemonData[0].Moves = new string[] { "Will-O-Wisp", "Grass Knot", "Psychic", "Shadow Ball" };
            shauntal.PokemonData[0].Item = null;

            TrainerRepresentation marshal = new();
            marshal.TrainerData.Identification.NumberID = 39;
            marshal.TrainerData.TrainerClass.NumberID = 79;
            marshal.TrainerData.BattleType = "Single";
            marshal.TrainerData.Format.Moves = true;
            marshal.TrainerData.Format.Items = true;
            marshal.TrainerData.BaseMoney = 1200;
            marshal.TrainerData.Items = new string[] { null, null, null, null };
            marshal.TrainerData.AIFlags.CheckBadMoves = true;
            marshal.TrainerData.AIFlags.EvaluateAttacks = true;
            marshal.TrainerData.AIFlags.Expert = true;
            marshal.PokemonData = new PokemonData[] { new PokemonData() };
            marshal.PokemonData[0].Pokemon = "Throh";
            marshal.PokemonData[0].Form = 0;
            marshal.PokemonData[0].Level = 56;
            marshal.PokemonData[0].Difficulty = 200;
            marshal.PokemonData[0].Miscellaneous.Gender = "Random";
            marshal.PokemonData[0].Miscellaneous.Ability = 0;
            marshal.PokemonData[0].Moves = new string[] { "Storm Throw", "Rock Slide", "Bulk Up", "Earthquake" };
            marshal.PokemonData[0].Item = null;

            TrainerRepresentation[] trainers = new TrainerRepresentation[] { shauntal, marshal };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(trainers, Newtonsoft.Json.Formatting.Indented);

            Console.WriteLine(json);
        }

        [TestMethod]
        public void DeserializeWithStrangeProperties()
        {
            TrainerRepresentation shauntal = Newtonsoft.Json.JsonConvert.DeserializeObject<TrainerRepresentation>(strangeJSON);

            Console.WriteLine(shauntal.TrainerData.Identification.NameID.Name);
            Console.WriteLine(shauntal.TrainerData.Identification.NameID.Variation);
        }

        [TestMethod]
        public void ValidateCorrectJSON()
        {
            Assert.IsTrue(TrainerJSONValidator.ValidateTrainerListJSON(multiJSON));
            Assert.IsTrue(TrainerJSONValidator.ValidateTrainerJSON(shauntalJSON));
            Assert.IsTrue(TrainerJSONValidator.ValidateTrainerJSON(shauntalJSON, out IList<string> errors));
            Assert.AreEqual(errors.Count, 0);
            Assert.IsTrue(TrainerJSONValidator.ValidateTrainerListJSON(multiJSON, out errors));
            Assert.AreEqual(errors.Count, 0);
        }

        [TestMethod]
        public void WrongInputJSONValidationFails()
        {
            Assert.IsFalse(TrainerJSONValidator.ValidateTrainerJSON(multiJSON));
            Assert.IsFalse(TrainerJSONValidator.ValidateTrainerListJSON(shauntalJSON));
            Assert.IsFalse(TrainerJSONValidator.ValidateTrainerJSON(multiJSON, out IList<string> errors));
            Assert.AreEqual(errors.Count, 1);
            Assert.IsFalse(TrainerJSONValidator.ValidateTrainerListJSON(shauntalJSON, out errors));
            Assert.AreEqual(errors.Count, 1);
        }

        [TestMethod]
        public void ValidateCorrectButStrangeJSON()
        {
            Assert.IsTrue(TrainerJSONValidator.ValidateTrainerJSON(strangeJSON));
        }

        [TestMethod]
        public void FailToValidateIncompleteJSON()
        {
            Assert.IsFalse(TrainerJSONValidator.ValidateTrainerJSON(incompleteJSON));
            Assert.IsFalse(TrainerJSONValidator.ValidateTrainerJSON(incompleteJSON, out IList<string> errors));
            Console.WriteLine(errors.Count);
            Console.WriteLine(errors[0]);
            // There are more mistakes with the json than 1 mistake, but only 1 error is written to the IList. Why is this? Is this correct behaviour?
            Assert.IsTrue(errors.Count > 0);
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
    }
}
