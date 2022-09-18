using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;

namespace TrainerTyrant
{
    public class ExternalDataJSONValidator
    {
        private static readonly string MoveListSchema = @"{
                                                            'properties': {
                                                            'Move Data': {
                                                                'items': {
                                                                'type': 'string',
                                                                'description': 'Move name.'
                                                                }
                                                            }
                                                            },
                                                            'required': ['Move Data']
                                                        }";
        private static readonly string PokemonListSchema = @"{
                                                            'properties': {
                                                            'Pokemon Data': {
                                                                'items': {
                                                                'type': 'string',
                                                                'description': 'Pokemon Name.'
                                                                }
                                                            }
                                                            },
                                                            'required': ['Pokemon Data']
                                                        }";
        private static readonly string ItemListSchema = @"{
                                                          'properties': {
                                                            'Item Data': {
                                                              'items': {
                                                                'type': 'string',
                                                                'description': 'Item Name.'
                                                              }
                                                            }
                                                          },
                                                          'required': ['Item Data']
                                                        }";


        private static readonly JSchema MoveListValidator = JSchema.Parse(MoveListSchema);
        private static readonly JSchema PokemonListValidator = JSchema.Parse(PokemonListSchema);
        private static readonly JSchema ItemListValidator = JSchema.Parse(ItemListSchema);

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
    }
}
