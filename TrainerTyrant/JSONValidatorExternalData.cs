using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;

namespace TrainerTyrant
{
    public class JSONValidatorExternalData
    {
        private static readonly string MoveListSchema = @"{
                                                            'properties': {
                                                            'Moves': {
                                                                'items': {
                                                                'type': 'string',
                                                                'description': 'Move name.'
                                                                }
                                                            }
                                                            },
                                                            'required': ['Moves']
                                                        }";
        private static readonly string PokemonListSchema = @"{
                                                            'properties': {
                                                            'Pokemon': {
                                                                'items': {
                                                                'type': 'string',
                                                                'description': 'Pokemon Name.'
                                                                }
                                                            }
                                                            },
                                                            'required': ['Pokemon']
                                                        }";
        private static readonly string ItemListSchema = @"{
                                                          'properties': {
                                                            'Items': {
                                                              'items': {
                                                                'type': 'string',
                                                                'description': 'Item Name.'
                                                              }
                                                            }
                                                          },
                                                          'required': ['Items']
                                                        }";
        private static readonly string SlotListSchema = @"{
                                                          'properties': {
                                                            'Slots': {
                                                              'items': {
                                                                'type': 'object',
                                                                'description': 'Item representing the trainer who should be here. The combination of Name and Variation is a unique ID to each slot.',
                                                                'properties': {
                                                                  'Name': {
                                                                    'type': 'string',
                                                                    'description': 'The name of the trainer who should be here.'
                                                                  },
                                                                  'Variation': {
                                                                    'type': 'integer',
                                                                    'description': 'The variation number of the trainer. Counts up from the beginning of the array per each trainer who has the same name.'
                                                                  },
                                                                  'Export Name': {
                                                                    'type': 'string',
                                                                    'description': 'The display name of the trainer when exported to something like smogon.'
                                                                  }
                                                                },
                                                                'required': ['Name', 'Variation']
                                                              }
                                                            }
                                                          },
                                                          'required': ['Slots']
                                                        }";

        private static readonly JSchema MoveListValidator = JSchema.Parse(MoveListSchema);
        private static readonly JSchema PokemonListValidator = JSchema.Parse(PokemonListSchema);
        private static readonly JSchema ItemListValidator = JSchema.Parse(ItemListSchema);
        private static readonly JSchema SlotListValidator = JSchema.Parse(SlotListSchema);

        public static bool ValidateMoveListJSON(string JSON)
        {
            try
            {
                JObject parsedJSON = JObject.Parse(JSON);

                return parsedJSON.IsValid(MoveListValidator);
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidateMoveListJSON(string JSON, out IList<string> errors)
        {
            try
            {
                JObject parsedJSON = JObject.Parse(JSON);

                bool result = parsedJSON.IsValid(MoveListValidator, out errors);

                return result;
            }
            catch
            {
                errors = new List<string>() { "JSON provided was not an object or some other error occured." };

                return false;
            }
        }

        public static bool ValidatePokemonListJSON(string JSON)
        {
            try
            {
                JObject parsedJSON = JObject.Parse(JSON);

                return parsedJSON.IsValid(PokemonListValidator);
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidatePokemonListJSON(string JSON, out IList<string> errors)
        {
            try
            {
                JObject parsedJSON = JObject.Parse(JSON);

                bool result = parsedJSON.IsValid(PokemonListValidator, out errors);

                return result;
            }
            catch
            {
                errors = new List<string>() { "JSON provided was not an object or some other error occured." };

                return false;
            }
        }

        public static bool ValidateItemListJSON(string JSON)
        {
            try
            {
                JObject parsedJSON = JObject.Parse(JSON);

                return parsedJSON.IsValid(ItemListValidator);
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidateItemListJSON(string JSON, out IList<string> errors)
        {
            try
            {
                JObject parsedJSON = JObject.Parse(JSON);

                bool result = parsedJSON.IsValid(ItemListValidator, out errors);

                return result;
            }
            catch
            {
                errors = new List<string>() { "JSON provided was not an object or some other error occured." };

                return false;
            }
        }

        public static bool ValidateSlotListJSON(string JSON)
        {
            try
            {
                JObject parsedJSON = JObject.Parse(JSON);

                return parsedJSON.IsValid(SlotListValidator);
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidateSlotListJSON(string JSON, out IList<string> errors)
        {
            try
            {
                JObject parsedJSON = JObject.Parse(JSON);

                bool result = parsedJSON.IsValid(SlotListValidator, out errors);

                return result;
            }
            catch
            {
                errors = new List<string>() { "JSON provided was not an object or some other error occured." };

                return false;
            }
        }
    }
}
