using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;

public static class GameData {
    //Standard Data, doesn't change
    static Dictionary<string,string> TownNames;
    static Dictionary<string,string> CharacterNames;
    static Dictionary<string,string> ItemNames;
    private static int DecryptionKey = 129;

    //Data which depend on the gamestate. May differ over playsessions
    static Dictionary<string,string> SaveData;

    public static void Initialize(){
	TownNames = LoadDictionaryFromFile("Data/TownNames.dat");
	ItemNames = LoadDictionaryFromFile("Data/ItemNames.dat");
	CharacterNames = LoadDictionaryFromFile("Data/CharacterNames.dat");
    }


    static Dictionary<string,string> LoadDictionaryFromFile(string filename){
	Dictionary<string,string> dict = new Dictionary<string,string>();
	var lines = File.ReadAllLines(Application.dataPath + "/StreamingAssets/" +  filename);
	for (int i = 0; i < lines.Length; i+=2) {
	    var key = lines[i];
	    var value = lines[i+1];
	    dict[key] = value;
	}
	return dict;
    }

    public static string ReplaceParameters(string input,bool Debug = false){
	//Check for names
	if(Debug){
	    Initialize();
	    UnityEngine.Debug.Log(CharacterNames.Count);
	}
	Regex namesRegex = new Regex(@"{(T_|I_|C_|S_)(\w+)}");
	var matches = namesRegex.Matches(input);

	foreach (Match m in matches){
	    string prefix = m.Groups[1].Value;
	    string key = m.Groups[2].Value;
	    string s = "{" + prefix + key  + "}";
	    switch (prefix) {
		case "T_": 
		    if(!TownNames.ContainsKey(key)){
                        UnityEngine.Debug.LogError("Key <color=red>" + key + "</color> does not exist in <color=black>town names</color>");
			return "";
		    }
		    input = input.Replace( s,TownNames[key] );
		    break;
		case "I_":
		    if(!ItemNames.ContainsKey(key)){
                        UnityEngine.Debug.LogError("Key <color=red>" + key + "</color> does not exist in <color=black>character names</color>");
			return "";
		    }
		    input = input.Replace( s, ItemNames[key] );
		    break;
		case "S_":
		    if(!SaveData.ContainsKey(key)){
                        UnityEngine.Debug.LogError("Key <color=red>" + key + "</color> does not exist in <color=black>other</color>");
			return "";
		    }
		    input = input.Replace( s, SaveData[key] );
		    break;
		case "C_":
		    if(!CharacterNames.ContainsKey(key)){
                        UnityEngine.Debug.LogError("Key <color=red>" + key + "</color> does not exist in <color=black>character names</color>");
			return "";
		    }
		    input = input.Replace( s, CharacterNames[key] );
		    break;
		default:
                    UnityEngine.Debug.LogError("Key prefix " + prefix + " does not exist! ");
		    break;
	    }
	}
	return input;
    }

    public static void SaveGame(string name){
	string outfile = Application.dataPath + "/StreamingAssets/Saves/" + name + ".sav";
	if (!File.Exists(outfile))
	    File.Create(outfile).Close();
	StreamWriter sr = new StreamWriter(outfile,false);

	foreach (var key in SaveData.Keys){
	    sr.Write(XOR(key));
	    sr.Write('\n');
	    sr.Write(XOR(SaveData[key]));
	    sr.Write('\n');
	}

	sr.Close();
    }

    public static void LoadSavedGame(string filename){
	Dictionary<string,string> dict = new Dictionary<string,string>();
	var lines = File.ReadAllLines(filename);
	for (int i = 0; i < lines.Length; i+=2) {
	    string key   = XOR(lines[i]);
	    string value = XOR(lines[i+1]);

	    dict[key] = value;
	}
	SaveData = dict;
	//SaveData.Debug();

	UnityEngine.SceneManagement.SceneManager.LoadScene(SaveData["Map"]);
	var player = Resources.Load("Prefabs/player");
	Vector2 playerPos =  new Vector2(Int32.Parse(SaveData["player_x"]),Int32.Parse(SaveData["player_y"]));
	Player.Spawn(playerPos);
    }


    public static void Debug<K,V>(this Dictionary<K,V> d){
	foreach (var item in d.Keys){
            UnityEngine.Debug.Log(item + " -> " + d[item]);
	}
    }


    public static void InitializeNewGame(string name){
	//TODO: add easter eggs on names
	SaveData = new Dictionary<string, string>();
	SaveData["randomSeed"] = DateTime.Now.Ticks.GetHashCode().ToString();
	SaveData["playerName"] = System.Security.SecurityElement.Escape(name);
	SaveData["playerGold"] = "0";
	SaveData["totalPlayTime"] = "00:00";
	SaveData["Map"] = "Town1";
	SaveData["player_x"] = "0";
	SaveData["player_y"] = "0";

	//TODO: start a timer

	SaveGame(name);
    }

    static string XOR(string s){
	string ret = "";
	foreach(char c in s){
	    if (c == '\n') 
		ret+= c;
	    else
		ret+= (char)((int)c ^ DecryptionKey);
	}
	return ret;
    }
}
    
