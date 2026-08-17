
using Godot;
using Godot.Collections;

namespace DragonXVI.Translation
{
    /// <summary>
    /// This class manages importing translations from JSON files.
    /// </summary>
    [GlobalClass, Tool]
    public abstract partial class TranslationImporter : GodotObject
    {
        /// <summary>
        /// The dictionary key checked in loaded files to determine what language this file
        /// belongs too.
        /// See the Godot language codes in the docs to know what ones to use.
        /// </summary>
        public const string LocaleCheckKey = "locale";

        /// <summary>
        /// This is where extra translation data is stored.
        /// E.g. this can store a godot array filled with random strings to pick from.
        /// </summary>
        // long.,.,.,.,.,.,.,.,,
        private static readonly Dictionary<string, Dictionary<string, Variant>> ExtraData = [];

        /// <summary>
        /// Retrive data from the loaded extra data.
        /// Returns an empty string if that data does not exist.
        /// </summary>
        /// <param name="key">The key for the data you want.</param>
        /// <param name="locale">The locale who's data you want. This will usually be TranslationServer.GetLocale.</param>
        public static Variant GetExtraData(string key, string locale)
        {
            if (!ExtraData.TryGetValue( locale, out Dictionary< string, Variant > data ))
            {
                GD.PushError("Getting extra data for a locale we dont have! ", locale);
                return "";
            }

            if (!data.TryGetValue( key, out Variant returnData ))
            {
                GD.PushError("Getting extra data that we dont have! ", locale, " ", key);
                return "";
            }

            return returnData;
        }
        /// <summary>
        /// Retrive data from the loaded extra data.
        /// Returns an empty string if that data does not exist.
        /// </summary>
        /// <param name="translationDict">Godot dict full of the translations you want.</param>
        public static Godot.Translation ParseDictToTranslation(Dictionary translationDict)
        {
            if (!translationDict.ContainsKey(LocaleCheckKey))
            {
                GD.PushError("Translation dict doesn't contain a locale key! Translations must have a \"", LocaleCheckKey, "\" key.");
                return null;
            }

            Godot.Translation translation = new();
            Dictionary<string, Variant> extra = [];
            foreach (Variant key in translationDict.Keys)
            {
                // only add strings as keys
                if (key.VariantType != Variant.Type.String)
                {
                    continue;
                }

                Variant value = translationDict[key];
                if (value.VariantType == Variant.Type.String)
                {
                    translation.AddMessage(key.ToString(), value.ToString());
                }
                else
                {
                    extra[key.ToString()] = value;
                }
            }


            string locale = translationDict[LocaleCheckKey].ToString();
            translation.Locale = locale;
            TranslationServer.AddTranslation(translation);
            ExtraData[locale] = extra;
            return translation;
        }
        /// <summary>
        /// Loads a file as text and parses it for translations. Can be inside the "res" or "user" spaces.
        /// If the file contains a valid dict, it gets parsed. Otherwise an error is printed
        /// and nothing happens.
        /// Dictionary is parsed via "ParseDictToTranslation"
        /// </summary>
        /// <param name="path">Path to the file you want to parse.</param>
        public static void ParseFileForDict(string path)
        {
            if (!FileAccess.FileExists(path))
            {
                GD.PushError("Trying to get a file that doesnt exist! ", path);
                return;
            }

            FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
            if (file == null)
            {
                GD.PushError($"File open error! Path: {path}, Error: {FileAccess.GetOpenError()}");
                return;
            }

            string fileString = file.GetAsText();
            Variant jsonParsed = Json.ParseString(fileString);
            if (jsonParsed.VariantType == Variant.Type.Nil)
            {
                file.Close();
                file.Dispose();
                GD.PushError("Translation parsed failed! ", path);
                return;
            }

            if (jsonParsed.VariantType == Variant.Type.Dictionary)
            {
                _ = ParseDictToTranslation(jsonParsed.AsGodotDictionary());
            }
            else
            {
                GD.PushError($"Translation parsed, but the parsed type wasnt a dictionary! Path: {path}, Type: {jsonParsed.VariantType}");
            }

            file.Close();
            file.Dispose();
        }
        /// <summary>
        /// Searches the directory for translation jsons, and sends any that it finds to
        /// ParseFileForDict.
        /// By default, this searches all sub directories too.
        /// </summary>
        /// <param name="dirPath">Path to the file you want to parse.</param>
        public static void ParseDirForFiles(string dirPath, bool searchSubdirs = true)
        {
            if (dirPath[^1] == '/')
            {
                dirPath = dirPath[..^1];
            }

            if (!DirAccess.DirExistsAbsolute(dirPath))
            {
                GD.PushError("Trying to parse a folder that doesnt exist! ", dirPath);
                return;
            }

            DirAccess dir = DirAccess.Open(dirPath);
            if (dir == null)
            {
                GD.PushError($"Failded to open folder: {dirPath}, Error: {DirAccess.GetOpenError()}");
                return;
            }

            _ = dir.ListDirBegin();
            string fileName = dir.GetNext();
            while (fileName != "")
            {
                if (dir.CurrentIsDir())
                {
                    if (searchSubdirs)
                    {
                        ParseDirForFiles(dir.GetCurrentDir() + "/" + fileName);
                    }
                }
                else
                {
                    ParseFileForDict(dir.GetCurrentDir() + "/" + fileName);
                }

                fileName = dir.GetNext();
            }
        }
    }
}