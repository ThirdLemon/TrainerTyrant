using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace TrainerTyrant
{
    public class TrainerJSONValidator
    {
        private static readonly string PlainTextSchema = @"{
                'allOf': [
                  {
                    'type': 'object',
                    'description': 'A complete represenation of a trainer.',
                    'properties': {
                      'Trainer Data': {
                        'type': 'object',
                        'description': 'All data about that trainer from the TRData NARC, excepting pokemon count.',
                        'properties': {
                          'Identification': {
                            'type': 'object',
                            'description': 'Information about where in the narcs this trainer is supposed to be.',
                            'properties': {
                              'Number ID': {
                                'type': 'integer',
                                'description': 'The offset within the narc.',
                                'minimum': 0
                              },
                              'Name ID': {
                                'type': 'object',
                                'description': 'Identify the location within the narc via a combination of name and unique id(Variation). Compared to a lookup table.',
                                'properties': {
                                  'Name': {
                                    'type': 'string',
                                    'description': 'The name of the trainer.'
                                  },
                                  'Variation': {
                                    'type': 'integer',
                                    'description': 'A unique ID for each trainer name, counting up from zero.',
                                    'minimum': 0
                                  }
                                },
                                'required': [
                                  'Name',
                                  'Variation'
                                ]
                              }
                            },
                            'anyOf': 
                            [
                              { 'required': ['Number ID'] },
                              { 'required': ['Name ID'] }
                            ]
                          },
                          'Trainer Class': {
                            'type': 'object',
                            'description': 'Represents the Trainer Class the trainers falls into.',
                            'properties': {
                    'Number ID': {
                        'type': 'integer',
                                'description': 'The trainer class offset of the given class.'
                              }
                },
                            'required': [
                              'Number ID'
                            ]
                          },
                          'Battle Type': {
                'enum': ['Single', 'Double', 'Triple', 'Rotation'],
                            'description': 'The type of battle they fight you in.'
                          },
                          'Format': {
                'type': 'object',
                            'description': 'The format of the Pokemon Data.',
                            'properties': {
                    'Moves': {
                        'type': 'boolean',
                                'description': 'Whether the Pokemon Data contains custom moves.'
                              },
                              'Items': {
                        'type': 'boolean',
                                'description': 'Whether the Pokemon Data contains custom hold items.'
                              }
                },
                            'required': [
                              'Moves',
                              'Items'
                            ]
                          },
                          'Base Money': {
                'type': 'integer',
                            'description': 'The base amount of money this trainer gives.',
                            'minimum': 0,
                            'maximum': 255
                          },
                          'Items': {
                'type': 'array',
                            'description': 'A list of four item slots that this trainer may use, e.g, Full Restores.',
                            'items': 
                            {
                    'type': ['string', 'null' ]
                            },
                            'minItems': 4,
                            'maxItems': 4
                          },
                          'AI Flags': {
                'type': 'object',
                            'description': 'A list of AI properties the trainer can have.',
                            'properties': {
                    'Check Bad Moves': {
                        'type': 'boolean',
                                'description': 'Wont use move that have no effect.'
                              },
                              'Evaluate Attacks': {
                        'type': 'boolean',
                                'description': 'Will consider some moves to be better than others.'
                              },
                              'Expert': {
                        'type': 'boolean',
                                'description': 'Is smarter.'
                              },
                              'Setup First Turn': {
                        'type': 'boolean',
                                'description': 'Prioritizes setup sometimes.'
                              },
                              'Risky': {
                        'type': 'boolean',
                                'description': 'Prioritizes risky moves.'
                              },
                              'Prefer Strong Moves': {
                        'type': 'boolean',
                                'description': 'Exact effects unknown.'
                              },
                              'Prefer Baton Pass': {
                        'type': 'boolean',
                                'description': 'Exact effects unknown.'
                              },
                              'Double Battle': {
                        'type': 'boolean',
                                'description': 'Runs proper logic for double battles.'
                              }
                },
                            'required': [
                              'Check Bad Moves',
                              'Evaluate Attacks',
                              'Expert',
                              'Setup First Turn',
                              'Risky',
                              'Prefer Strong Moves',
                              'Prefer Baton Pass',
                              'Double Battle'
                            ]
                          },
                          'Healer': {
                'type': 'boolean',
                            'description': 'Whether the trainer is a healer, like a doctor or a nurse trainer. Implicitly assumed to be false if not set.'
                          },
                          'Reward': {
                'type': ['string', 'null'],
                            'description': 'Item given as a reward for beating the trainer, such as by small court trainers or rangers.'
                          }
                        },
                        'required': [
                          'Identification',
                          'Trainer Class',
                          'Battle Type',
                          'Format',
                          'Base Money',
                          'AI Flags'
                        ]
                      },
                      'Pokemon Data': {
                'type': 'array',
                        'description': 'All information about the trainers pokemon from the TRPoke narc.',
                        'items':
                        {
                    'type': 'object',
                          'description': 'A comprehensive description of the pokemon.',
                          'properties': {
                        'Pokemon': {
                            'type': 'string',
                              'description': 'The pokemons species.'
                            },
                            'Form': {
                            'type': 'integer',
                              'description': 'The form number of the pokemon, for things like rotom forms.',
                              'minimum': 0,
                              'maximum': 255
                            },
                            'Level': {
                            'type': 'integer',
                              'description': 'The level of the pokemon.',
                              'minimum': 0,
                              'maximum': 100
                            },
                            'Difficulty': {
                            'type': 'integer',
                              'description': 'Defines the nature and iv total. Higher is stronger.',
                              'minimum': 0,
                              'maximum': 255
                            },
                            'Miscellaneous': {
                            'type': 'object',
                              'description': 'The values represented by the \'unknown\' byte.',
                              'properties': {
                                'Gender': {
                                    'enum': ['Random', 'Female', 'Male'],
                                  'description': 'Gender of the pokemon.'
                                },
                                'Ability': {
                                    'type': 'integer',
                                  'description': 'The ability slot of the pokemon. 0 and 1 are abilities 1 and 2, and 2 is the hidden ability.',
                                  'minimum': 0,
                                  'maximum': 3
                                }
                            },
                              'required': [
                                'Gender',
                                'Ability'
                              ]
                            },
                            'Moves': {
                            'type': 'array',
                              'description': 'The four moveslots. If the moves property under format is false, this doesn\'t need to be filled in.',
                              'items': 
                              {
                                'type': ['string', 'null']
                              },
                              'minItems': 4,
                              'maxItems': 4
                            },
                            'Item': {
                            'type': ['null', 'string'],
                              'description': 'The hold item. If the items property under format is false, this doesn\'t need to be filled in.'
                            }
                    },
                          'required': [
                            'Pokemon',
                            'Form',
                            'Level',
                            'Difficulty',
                            'Miscellaneous'
                          ]
                        },
                        'minItems': 1,
                        'maxItems': 6
                      }
                    },
                    'required': [
                      'Trainer Data',
                      'Pokemon Data'
                    ]
                  },
                  {
                'anyOf': [
                      {
                    'properties': {
                        'Trainer Data': {
                            'properties': {
                                'Format': {
                                    'properties': {
                                        'Moves': {
                                            'const': false
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                      {
                    'properties': {
                        'Pokemon Data': {
                            'items': {
                                'required': ['Moves']
                            }
                        }
                    }
                }
                    ]
                  },
                  {
                'anyOf': [
                      {
                    'properties': {
                        'Trainer Data': {
                            'properties': {
                                'Format': {
                                    'properties': {
                                        'Items': {
                                            'const': false
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                      {
                    'properties': {
                        'Pokemon Data': {
                            'items': {
                                'required': ['Item']
                            }
                        }
                    }
                }
                    ]
                  }
                ]
              }";

        private static readonly string PlainTextMultiSchema = @"{
        'items':  {
                'allOf': [
                  {
                    'type': 'object',
                    'description': 'A complete represenation of a trainer.',
                    'properties': {
                      'Trainer Data': {
                        'type': 'object',
                        'description': 'All data about that trainer from the TRData NARC, excepting pokemon count.',
                        'properties': {
                          'Identification': {
                            'type': 'object',
                            'description': 'Information about where in the narcs this trainer is supposed to be.',
                            'properties': {
                              'Number ID': {
                                'type': 'integer',
                                'description': 'The offset within the narc.',
                                'minimum': 0
                              },
                              'Name ID': {
                                'type': 'object',
                                'description': 'Identify the location within the narc via a combination of name and unique id(Variation). Compared to a lookup table.',
                                'properties': {
                                  'Name': {
                                    'type': 'string',
                                    'description': 'The name of the trainer.'
                                  },
                                  'Variation': {
                                    'type': 'integer',
                                    'description': 'A unique ID for each trainer name, counting up from zero.',
                                    'minimum': 0
                                  }
                                },
                                'required': [
                                  'Name',
                                  'Variation'
                                ]
                              }
                            },
                            'anyOf': 
                            [
                              { 'required': ['Number ID'] },
                              { 'required': ['Name ID'] }
                            ]
                          },
                          'Trainer Class': {
                            'type': 'object',
                            'description': 'Represents the Trainer Class the trainers falls into.',
                            'properties': {
                    'Number ID': {
                        'type': 'integer',
                                'description': 'The trainer class offset of the given class.'
                              }
                },
                            'required': [
                              'Number ID'
                            ]
                          },
                          'Battle Type': {
                'enum': ['Single', 'Double', 'Triple', 'Rotation'],
                            'description': 'The type of battle they fight you in.'
                          },
                          'Format': {
                'type': 'object',
                            'description': 'The format of the Pokemon Data.',
                            'properties': {
                    'Moves': {
                        'type': 'boolean',
                                'description': 'Whether the Pokemon Data contains custom moves.'
                              },
                              'Items': {
                        'type': 'boolean',
                                'description': 'Whether the Pokemon Data contains custom hold items.'
                              }
                },
                            'required': [
                              'Moves',
                              'Items'
                            ]
                          },
                          'Base Money': {
                'type': 'integer',
                            'description': 'The base amount of money this trainer gives.',
                            'minimum': 0,
                            'maximum': 255
                          },
                          'Items': {
                'type': 'array',
                            'description': 'A list of four item slots that this trainer may use, e.g, Full Restores.',
                            'items': 
                            {
                    'type': ['string', 'null' ]
                            },
                            'minItems': 4,
                            'maxItems': 4
                          },
                          'AI Flags': {
                'type': 'object',
                            'description': 'A list of AI properties the trainer can have.',
                            'properties': {
                    'Check Bad Moves': {
                        'type': 'boolean',
                                'description': 'Wont use move that have no effect.'
                              },
                              'Evaluate Attacks': {
                        'type': 'boolean',
                                'description': 'Will consider some moves to be better than others.'
                              },
                              'Expert': {
                        'type': 'boolean',
                                'description': 'Is smarter.'
                              },
                              'Setup First Turn': {
                        'type': 'boolean',
                                'description': 'Prioritizes setup sometimes.'
                              },
                              'Risky': {
                        'type': 'boolean',
                                'description': 'Prioritizes risky moves.'
                              },
                              'Prefer Strong Moves': {
                        'type': 'boolean',
                                'description': 'Exact effects unknown.'
                              },
                              'Prefer Baton Pass': {
                        'type': 'boolean',
                                'description': 'Exact effects unknown.'
                              },
                              'Double Battle': {
                        'type': 'boolean',
                                'description': 'Runs proper logic for double battles.'
                              }
                },
                            'required': [
                              'Check Bad Moves',
                              'Evaluate Attacks',
                              'Expert',
                              'Setup First Turn',
                              'Risky',
                              'Prefer Strong Moves',
                              'Prefer Baton Pass',
                              'Double Battle'
                            ]
                          },
                          'Healer': {
                'type': 'boolean',
                            'description': 'Whether the trainer is a healer, like a doctor or a nurse trainer. Implicitly assumed to be false if not set.'
                          },
                          'Reward': {
                'type': ['string', 'null'],
                            'description': 'Item given as a reward for beating the trainer, such as by small court trainers or rangers.'
                          }
                        },
                        'required': [
                          'Identification',
                          'Trainer Class',
                          'Battle Type',
                          'Format',
                          'Base Money',
                          'AI Flags'
                        ]
                      },
                      'Pokemon Data': {
                'type': 'array',
                        'description': 'All information about the trainers pokemon from the TRPoke narc.',
                        'items':
                        {
                    'type': 'object',
                          'description': 'A comprehensive description of the pokemon.',
                          'properties': {
                        'Pokemon': {
                            'type': 'string',
                              'description': 'The pokemons species.'
                            },
                            'Form': {
                            'type': 'integer',
                              'description': 'The form number of the pokemon, for things like rotom forms.',
                              'minimum': 0,
                              'maximum': 255
                            },
                            'Level': {
                            'type': 'integer',
                              'description': 'The level of the pokemon.',
                              'minimum': 0,
                              'maximum': 100
                            },
                            'Difficulty': {
                            'type': 'integer',
                              'description': 'Defines the nature and iv total. Higher is stronger.',
                              'minimum': 0,
                              'maximum': 255
                            },
                            'Miscellaneous': {
                            'type': 'object',
                              'description': 'The values represented by the \'unknown\' byte.',
                              'properties': {
                                'Gender': {
                                    'enum': ['Random', 'Female', 'Male'],
                                  'description': 'Gender of the pokemon.'
                                },
                                'Ability': {
                                    'type': 'integer',
                                  'description': 'The ability slot of the pokemon. 0 and 1 are abilities 1 and 2, and 2 is the hidden ability.',
                                  'minimum': 0,
                                  'maximum': 3
                                }
                            },
                              'required': [
                                'Gender',
                                'Ability'
                              ]
                            },
                            'Moves': {
                            'type': 'array',
                              'description': 'The four moveslots. If the moves property under format is false, this doesn\'t need to be filled in.',
                              'items': 
                              {
                                'type': ['string', 'null']
                              },
                              'minItems': 4,
                              'maxItems': 4
                            },
                            'Item': {
                            'type': ['null', 'string'],
                              'description': 'The hold item. If the items property under format is false, this doesn\'t need to be filled in.'
                            }
                    },
                          'required': [
                            'Pokemon',
                            'Form',
                            'Level',
                            'Difficulty',
                            'Miscellaneous'
                          ]
                        },
                        'minItems': 1,
                        'maxItems': 6
                      }
                    },
                    'required': [
                      'Trainer Data',
                      'Pokemon Data'
                    ]
                  },
                  {
                'anyOf': [
                      {
                    'properties': {
                        'Trainer Data': {
                            'properties': {
                                'Format': {
                                    'properties': {
                                        'Moves': {
                                            'const': false
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                      {
                    'properties': {
                        'Pokemon Data': {
                            'items': {
                                'required': ['Moves']
                            }
                        }
                    }
                }
                    ]
                  },
                  {
                'anyOf': [
                      {
                    'properties': {
                        'Trainer Data': {
                            'properties': {
                                'Format': {
                                    'properties': {
                                        'Items': {
                                            'const': false
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                      {
                    'properties': {
                        'Pokemon Data': {
                            'items': {
                                'required': ['Item']
                            }
                        }
                    }
                }
                    ]
                  }
                ]
              }
           }";

        private readonly static JSchema TrainerRepresentationValidator = JSchema.Parse(PlainTextSchema);

        private readonly static JSchema MultiTrainerRepresentationValidator = JSchema.Parse(PlainTextMultiSchema);

        /**
         * 
         */
        public static bool ValidateTrainerJSON(string JSON)
        {
            try
            {
                Newtonsoft.Json.Linq.JObject parsedJSON = Newtonsoft.Json.Linq.JObject.Parse(JSON);

                return parsedJSON.IsValid(TrainerRepresentationValidator);
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidateTrainerJSON(string JSON, out IList<string> errors)
        {
            try
            {
                Newtonsoft.Json.Linq.JObject parsedJSON = Newtonsoft.Json.Linq.JObject.Parse(JSON);

                bool result = parsedJSON.IsValid(TrainerRepresentationValidator, out errors);

                return result;
            }
            catch
            {
                errors = new List<string>() { "JSON provided was not an object or some other error occured." };

                return false;
            }
        }

        public static bool ValidateTrainerListJSON(string JSON)
        {
            try
            {
                Newtonsoft.Json.Linq.JArray parsedJSON = Newtonsoft.Json.Linq.JArray.Parse(JSON);
                return parsedJSON.IsValid(MultiTrainerRepresentationValidator);
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidateTrainerListJSON(string JSON, out IList<string> errors)
        {
            try
            {
                Newtonsoft.Json.Linq.JArray parsedJSON = Newtonsoft.Json.Linq.JArray.Parse(JSON);

                bool result = parsedJSON.IsValid(MultiTrainerRepresentationValidator, out errors);

                return result;
            }
            catch
            {
                errors = new List<string>() { "JSON provided was not an array or some other error occured." };

                return false;
            }
        }
    }
}
