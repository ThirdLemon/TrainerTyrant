# TrainerTyrant
This is a project aimed at high-accessibility manipulation of trainer data in Generation 5 Pokemon Games(B/W/B2/W2).

TrainerTyrant aims to transform the high-density, computer readable information of game code and extracted NARC files and make them editable through more human-readable interface. Primarily JSON, however support for spreadsheets will come in the future. A benefit of being able to manipulate JSON to write trainer data is the ability for users to write their own code to trivially edit trainer data through JSON. The hope is to continue improving on TrainerTyrant until most knowledge and interface barriers are removed, to reduce the risk of human error while editting trainer data and increasing the accessibility of romhacking for new users. 

# Building TrainerTyrant
Clone [NARCLord](https://github.com/ThirdLemon/NARCLord) and this repository into separate folders in the same root. Build NARCLord separately first, then build this project.

# Using TrainerTyrant
TrainerTyrant works by loading and building .narc files, which must be first manually extracted from the ROM with another tool. The tool the developer uses for this is [NitroExplorer2B](https://projectpokemon.org/home/files/file/2070-nitro-explorer). 

## Preparing Data for use in Trainer Tyrant
TrainerTyrant works by modifying the so called 'TRData' and 'TRPoke' Narc files from within the games files. In order to manipulate these, you must first extract them from the games files using a DS ROM browser like NitroExplorer2B. 

Load your ROM, and browse to the folder a/0/9, which contains TRData and TRPoke. In Pokemon Black and White, TRData and TRPoke are files a/0/9/2 and a/0/9/3 respectively, and in Pokemon Black and White 2, TRData and TRPoke are files a/0/9/1 and a/0/9/2 respectively. Extract TRData and TRPoke to somewhere you can easily find again, and make sure to name the extract Narcs something that communicates which file they are, such as "BW2_a091_TRData.narc". Naming them with the .narc extension is not necessary, but is recommended.

## Converting to JSON
Open Trainer Tyrant. The interface is simple. On the left, there are four buttons for changing the 'Definition' files. Definition files are used for mapping the byte based data within the narcs to human readable elements. For example, the Item JSON file contains a list of every item in black and white 2, each mapped correctly to their index. When Trainer Tyrant is started up these are automatically set to the default files contained within the subfolder DefaultJSON. Messing with the definition files is generally left for more advanced users, who want TrainerTyrant to know about other changes they've made within the ROM. If you are hacking Black and White and not Black and White 2, however, you will need to change the Trainer Definition file. Click the 'Open Trainer JSON' button, and open the file DefaultJSON/BlackWhiteSlots.

Besides the Definition files, in the center there are the buttons for converting the narc folders to JSON. Using the Load buttons, select the narc files you extracted from your rom. Once you have selected both your TRData and TRPoke narcs, the Convert Narcs button will become enabled. If everything you've done to this point has went smoothly, you'll be able to press the Convert NARCs button, and save the JSON file to your computer.

## Editting the JSON
The JSON file produced by TrainerTyrant is likely to be quite large for a text file. It is recommended to navigate it by using Ctrl+F to search for the name of the trainer you're looking for. Trainers are all identified by a Name ID property, which is a combination of their Name and their Variation, which uniquely identifies them from other trainers with their same name. If multiple trainers have the same name and variation, the JSON will not be permitted to convert back to a narc folder. Each Trainer is separated into two properties: Their Trainer Data and their Pokemon Data, which is roughly equivalent to the data stored in the TRData narc and the TRPoke narc respectively. 

TrainerData must always contain the properties Identification, Trainer Class, Battle Type, Format, Base Money, and AI Flags. However, they may also contain the properties Items, Healer, or Unknown. Items represents the battle items a trainer has, such as full restores. Healer represents whether the trainer heals you after a battle, like a nurse or doctor trainer. The Unknown property represents the unknown data stored in TRData. It's not recommended to change the Unknown Property, at risk of unintended side-effects, as what this property controls is unknown. In order for a trainer to have specific moves or specific held items on their pokemon, the Format property under Trainer Data must indicate this. 

Pokemon Data contains from 1 to 6 items representing Pokemon. You can easily add or remove pokemon from the trainer as simply as adding or removing the object from the Pokemon Data list. A Pokemon must contain the properties Pokemon, Form, Level, Difficulty, and Miscellaneous. Difficulty is the encoding of how many IVs a pokemon has, and what nature it has. It's not know the exact method this translates. Difficulty maxes out at 255, which will give the pokemon IVs of 31 in every stat. Pokemon may also contain an Item and Moves field, but they must contain these properties if the Trainer Data's Format property indicates that the trainer has custom moves or custom items. If a pokemon contains these fields without the Trainer Data's Format property indicating it, the held item and move will be ignored when converting back to the narc.

Generally, if you're unsure how to customize the trainer to match what you want from it, try looking for examples of what you want elsewhere in the JSON file. 

It is important that the JSON you write match the JSON schema used to validate the JSON before conversion. If you are having trouble with matching the JSON schema, or you want a more indepth look at every aspect of the JSON format used here, you can look at the JSON schema [here](https://pastebin.com/YFtpr6nh).

## Converting from JSON
Once you have editted your JSON file to your liking, return to the TrainerTyrant application. Select the Convert JSON to NARCs button, and select your JSON file. After a couple seconds, TrainerTyrant will have created to Narc files just like you extracted from the ROM in the same folder as your JSON file. If you are having trouble with JSON validation, make sure to check the Log.txt file created in the same folder as TrainerTyrant, and consult the JSON schema to see where you might have errors. Since the error messages merely tell you the index of the offending object, it can be useful to check the trainer definitions folder to see which trainer is at a given index. Simply multiply the given index by 5, then scroll through the definition folder until you find the line at that index. The trainer data given there should match the index of the offending JSON object, so you can then use the name and variation given by the definition file to find the invalid trainer in the JSON file.

## Reinserting data to your ROM
Use your DS ROM browser to navigate back to the folder a/0/9 in your ROM, then select the corresponding file to your TRData and TRPoke narcs and then re-insert the narcs.

# Advanced Tricks
It's possible to use a single JSON file to hold multiple 'games' of information. By inserting extra trainers into your converted JSON file, but with Name ID's that aren't in the vanilla game, the extra data won't be converted into the narc when you're finished editting. But by creating a duplicate Trainer Definition file, and changing the Name IDs of select trainers to match the extra trainer's IDs, the extra trainers will be converted in place of the original trainers. This can be used to store 'difficulty levels' within the same file. 

For example, in vanilla white 2, there is only one ghetsis fight. You can then place another ghetsis fight in the JSON file, with the same name but a variation of 1, and stronger pokemon without changing the fight normally. But if you load a trainer definition file that says that Ghetsis's slot is looking for a name ID of Ghetsis with a varation of 1, the buffed ghetsis fight will then take the place of ghetsis instead.

There is an 'Alter JSON' option provided in TrainerTyrant, which lets you merge a smaller JSON file into a source JSON file. The fight(s) in the smaller JSON file will be added to the source file, if the source file doesn't contain them, or overwrite them otherwise. This can be useful for keeping things organize with a file system.
