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
        private string movelistJSON;
        private string pokemonlistJSON;
        private string itemlistJSON;
        private string slotlistJSON;
        private string emptyJSON;

        [TestInitialize]
        public void TestInit()
        {
            shauntalJSON = File.ReadAllText("../../../SampleJSON/sampleJSON1.json");

            strangeJSON = File.ReadAllText("../../../SampleJSON/sampleJSON2.json");

            incompleteJSON = File.ReadAllText("../../../SampleJSON/sampleJSON3.json");

            multiJSON = File.ReadAllText("../../../SampleJSON/sampleJSON4.json");

            movelistJSON = File.ReadAllText("../../../SampleJSON/External/MoveList1.json");

            pokemonlistJSON = File.ReadAllText("../../../SampleJSON/External/PokemonList1.json");

            itemlistJSON = File.ReadAllText("../../../SampleJSON/External/ItemList1.json");

            slotlistJSON = File.ReadAllText("../../../SampleJSON/External/SlotList1.json");

            emptyJSON = File.ReadAllText("../../../SampleJSON/External/Empty.json");
        }

        [TestMethod]
        public void BaseDeserializeNoErrors()
        {
            TrainerRepresentation shauntal = JsonConvert.DeserializeObject<TrainerRepresentation>(shauntalJSON);

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
        public void Deserialize()
        {
            TrainerRepresentation shauntal = TrainerRepresentation.DeserializeJSON(shauntalJSON);
            Assert.AreEqual(38, shauntal.TrainerData.Identification.NumberID);
            Assert.AreEqual("Shauntal", shauntal.TrainerData.Identification.NameID.Name);
            Assert.AreEqual(0, shauntal.TrainerData.Identification.NameID.Variation);
            Assert.AreEqual(78, shauntal.TrainerData.TrainerClass.NumberID);
            Assert.AreEqual("Single", shauntal.TrainerData.BattleType);
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
                Assert.AreEqual("Random", shauntal.PokemonData[i].Miscellaneous.Gender);
            }
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(56, shauntal.PokemonData[i].Level);
                Assert.AreEqual(200, shauntal.PokemonData[i].Difficulty);
                Assert.AreEqual(0, shauntal.PokemonData[i].Miscellaneous.Ability);
                Assert.AreEqual(null, shauntal.PokemonData[i].Item);
            }
            Assert.AreEqual(58, shauntal.PokemonData[3].Level);
            Assert.AreEqual(250, shauntal.PokemonData[3].Difficulty);
            Assert.AreEqual(1, shauntal.PokemonData[3].Miscellaneous.Ability);
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
            Assert.AreEqual("Single", shauntal.TrainerData.BattleType);
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
                Assert.AreEqual("Random", shauntal.PokemonData[i].Miscellaneous.Gender);
            }
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(56, shauntal.PokemonData[i].Level);
                Assert.AreEqual(200, shauntal.PokemonData[i].Difficulty);
                Assert.AreEqual(0, shauntal.PokemonData[i].Miscellaneous.Ability);
                Assert.AreEqual(null, shauntal.PokemonData[i].Item);
            }
            Assert.AreEqual(58, shauntal.PokemonData[3].Level);
            Assert.AreEqual(250, shauntal.PokemonData[3].Difficulty);
            Assert.AreEqual(1, shauntal.PokemonData[3].Miscellaneous.Ability);
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
            Assert.AreEqual("Single", shauntal.TrainerData.BattleType);
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
                Assert.AreEqual("Random", shauntal.PokemonData[i].Miscellaneous.Gender);
            }
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(56, shauntal.PokemonData[i].Level);
                Assert.AreEqual(200, shauntal.PokemonData[i].Difficulty);
                Assert.AreEqual(0, shauntal.PokemonData[i].Miscellaneous.Ability);
                Assert.AreEqual(null, shauntal.PokemonData[i].Item);
            }
            Assert.AreEqual(58, shauntal.PokemonData[3].Level);
            Assert.AreEqual(250, shauntal.PokemonData[3].Difficulty);
            Assert.AreEqual(1, shauntal.PokemonData[3].Miscellaneous.Ability);
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
            Assert.AreEqual("Single", marshal.TrainerData.BattleType);
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
                Assert.AreEqual("Random", marshal.PokemonData[i].Miscellaneous.Gender);
                Assert.AreEqual(0, marshal.PokemonData[i].Miscellaneous.Ability);
            }
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(56, marshal.PokemonData[i].Level);
                Assert.AreEqual(200, marshal.PokemonData[i].Difficulty);
                Assert.AreEqual(null, marshal.PokemonData[i].Item);
            }
            Assert.AreEqual(58, marshal.PokemonData[3].Level);
            Assert.AreEqual(250, marshal.PokemonData[3].Difficulty);
            Assert.AreEqual("Sitrus Berry", marshal.PokemonData[3].Item);
            string[,] marshalmonmoves = { { "Rock Tomb", "Bulldoze", "Storm Throw", "Payback" },
                                    { "Payback", "Rock Slide", "Retaliate", "Brick Break" },
                                    { "Hi Jump Kick", "U-Turn", "Bounce", "Retaliate" },
                                    { "Stone Edge", "Hammer Arm", "Retaliate", "Bulk Up"} };
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
            shauntal.TrainerData.BattleType = "Single";
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
            shauntal.PokemonData[0].Miscellaneous.Gender = "Random";
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
            shauntal.TrainerData.BattleType = "Single";
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
            marshal.PokemonData[0].Miscellaneous.Gender = "Random";
            marshal.PokemonData[0].Miscellaneous.Ability = 0;
            marshal.PokemonData[0].Moves = new string[] { "Storm Throw", "Rock Slide", "Bulk Up", "Earthquake" };
            marshal.PokemonData[0].Item = null;

            TrainerRepresentation[] trainers = new TrainerRepresentation[] { shauntal, marshal };

            string json = JsonConvert.SerializeObject(trainers, Formatting.Indented);

            Console.WriteLine(json);
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
            Assert.IsTrue(TrainerJSONValidator.ValidateTrainerListJSON(multiJSON));
            Assert.IsTrue(TrainerJSONValidator.ValidateTrainerJSON(shauntalJSON));
            Assert.IsTrue(TrainerJSONValidator.ValidateTrainerJSON(shauntalJSON, out IList<string> errors));
            Assert.AreEqual(errors.Count, 0);
            Assert.IsTrue(TrainerJSONValidator.ValidateTrainerListJSON(multiJSON, out errors));
            Assert.AreEqual(errors.Count, 0);
        }

        [TestMethod]
        public void FailIncorrectJSON()
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
        public void ValidateCorrectExternalDataJSON()
        {
            Assert.IsTrue(ExternalDataJSONValidator.ValidateItemListJSON(itemlistJSON));
            Assert.IsTrue(ExternalDataJSONValidator.ValidateMoveListJSON(movelistJSON));
            Assert.IsTrue(ExternalDataJSONValidator.ValidatePokemonListJSON(pokemonlistJSON));
            Assert.IsTrue(ExternalDataJSONValidator.ValidateSlotListJSON(slotlistJSON));

            Assert.IsTrue(ExternalDataJSONValidator.ValidateItemListJSON(itemlistJSON, out IList<string> errors));
            Assert.AreEqual(0, errors.Count);
            Assert.IsTrue(ExternalDataJSONValidator.ValidateMoveListJSON(movelistJSON, out errors));
            Assert.AreEqual(0, errors.Count);
            Assert.IsTrue(ExternalDataJSONValidator.ValidatePokemonListJSON(pokemonlistJSON, out errors));
            Assert.AreEqual(0, errors.Count);
            Assert.IsTrue(ExternalDataJSONValidator.ValidateSlotListJSON(slotlistJSON, out errors));
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void FailIncorrectExternalDataJSON()
        {
            Assert.IsFalse(ExternalDataJSONValidator.ValidateItemListJSON(emptyJSON));
            Assert.IsFalse(ExternalDataJSONValidator.ValidateMoveListJSON(emptyJSON));
            Assert.IsFalse(ExternalDataJSONValidator.ValidatePokemonListJSON(emptyJSON));
            Assert.IsFalse(ExternalDataJSONValidator.ValidateSlotListJSON(emptyJSON));

            Assert.IsFalse(ExternalDataJSONValidator.ValidateItemListJSON(emptyJSON, out IList<string> errors));
            Assert.IsFalse(ExternalDataJSONValidator.ValidateMoveListJSON(emptyJSON, out errors));
            Assert.IsFalse(ExternalDataJSONValidator.ValidatePokemonListJSON(emptyJSON, out errors));
            Assert.IsFalse(ExternalDataJSONValidator.ValidateSlotListJSON(emptyJSON, out errors));
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
            Assert.AreEqual(-1, trainerList.GetIndexOfSlot(null, 0));
            Assert.AreEqual(-1, trainerList.GetIndexOfSlot("Garbage Data", 0));
            Assert.AreEqual(-1, trainerList.GetIndexOfSlot("Iris", 222));
            Assert.AreEqual(2, trainerList.GetIndexOfSlot("Aspen", 0));
            Assert.AreEqual(762, trainerList.GetIndexOfSlot("Grunt", 47));
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
