using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema;

namespace TrainerTyrant
{
    public class LearnsetSetJSONValidator
    {
        private static readonly string PlainTextSchema = @"{
              'patternProperties': {
                '': {
                  'type': 'array',
                  'description': 'All pokemon listed fall under this.',
                  'items': {
                    'description': 'Representation of moves learned by the pokemon.',
                    'properties': {
                      'Level': {
                        'type': 'integer',
                        'description': 'The level at which the move is learned.'
                      },
                      'Move': {
                        'type': 'string',
                        'description': 'The move learned.'
                      }
                    },
                    'required': ['Level', 'Move']
                  }
                }
              }
            }";

        private static readonly JSchema LearnsetSetValidator = JSchema.Parse(PlainTextSchema);

        public static bool ValidateLearnsetSetJSON(string JSON)
        {
            try
            {
                Newtonsoft.Json.Linq.JObject parsedJSON = Newtonsoft.Json.Linq.JObject.Parse(JSON);

                return parsedJSON.IsValid(LearnsetSetValidator);
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidateLearnsetSetJSON(string JSON, out IList<string> errors)
        {
            try
            {
                Newtonsoft.Json.Linq.JObject parsedJSON = Newtonsoft.Json.Linq.JObject.Parse(JSON);

                bool result = parsedJSON.IsValid(LearnsetSetValidator, out errors);

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
