using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;


public class Mod {
    string sourceCode;
    Script script;
    string path;

    public static string CorePath = Application.dataPath +  "/StreamingAssets/Core/";

    public enum ModuleType {
	NPC,
	ITEM,
	SPELL,
	NULL
    }
    public static List<Mod> Mods    = new List<Mod>();
    public static Dictionary<string, Mod> LuaCore = new Dictionary<string, Mod>();

    public ModuleType type;
    public string Name;
    public bool isActive = true;

    //TODO: Add params to Lua call function
    public void Call(string fun){
	var f = script.Globals[fun];
	if(f != null){
	    script.Call(f);
	}
    }

    public static void AddGlobalsToMod(ref Mod m){
	var s = m.script;
	s.Globals[ "player" ] = typeof( Player );
	s.Globals[  "core"  ] = typeof( LuaCoreAPI );

	s.Globals[ "__dir"  ] = m.path;
    }

    //TODO: Refactor Lua Mod loading
    public static bool LoadLua( string mainFile ,bool isCore = false){
	var mainFileCode = File.ReadAllText(mainFile);
	Mod m = new Mod();
	Script s = new Script();

	m.script = s;
	m.path = Path.GetDirectoryName(mainFile);
	m.sourceCode = mainFileCode;
	AddGlobalsToMod(ref m);
	string name = Path.GetFileName(mainFile).Replace(".lua", "");
	try{
	    DynValue ret = s.DoString(mainFileCode);
	}
	catch(SyntaxErrorException e){
	    //TODO: add a setting for hiding debug messages
	    if (false) {
		StartMenu.Print("Error while reading mod: '" + name + "' " + e.DecoratedMessage,"red");
	    }
	    else{
		StartMenu.Print("Error while reading mod: '" + name + "'. No mods will be loaded","yellow");
	    }
	    return false;
	}
	catch(ScriptRuntimeException e){
	    Debug.Log("Error While parsing script");
	}
	switch (s.Globals.Get("Module").String) {
	    case "NPC": {
		m.type = ModuleType.NPC;
		break;
	    }
	    case "ITEM": {
		m.type = ModuleType.ITEM;
		break;
	    }
	    case "SPELL": {
		m.type = ModuleType.SPELL;
		break;
	    }
	    default:
		StartMenu.Print("File '" + Path.GetDirectoryName(mainFile) + "' doesn't have a Module Type!!!");
		break;
	}
	m.Name = name;
	if (isCore)
	{
	    LuaCore[name] = m;
	}
	else
	    Mods.Add(m);

	return true;
    }

    public static void LoadAllMods(){
	UserData.RegisterAssembly();
	string[] coreLuaScripts = Directory.GetFiles(CorePath,"*.lua");
	foreach (var luaScript in coreLuaScripts){
	    if(!LoadLua(luaScript,true))
		Debug.LogError("Core Lua Script '" + luaScript + "' contains an error!!!");
	}

	string modPath = Application.dataPath + "/StreamingAssets/Mods/";
	string[] folders = Directory.GetDirectories(modPath);
	foreach (var folder in folders){
	    string mainFile = folder + "/main.lua";
	    if (!File.Exists(mainFile)) 
		continue;

	    //If LoadLua return false, it means there was an error while loading
	    if( !LoadLua(mainFile) )
		return;
	}

	if(Mods.Count == 0)
	    StartMenu.Print("No mods installed");
	else{
	    int npcs = 0, items = 0, spells = 0;
	    foreach (var mod in Mods){
		switch (mod.type) {
		    case ModuleType.NPC: {
			npcs++;
			break;
		    }
		    case ModuleType.SPELL: {
			spells++;
			break;
		    }
		    case ModuleType.ITEM: {
			items++;
			break;
		    }
		    default:
			break;
		}
	    }
	    string message = string.Format("Successfully loaded {0} npcs, {1} items and {2} spells!!!",npcs,items,spells);
	    StartMenu.Print(message,"green");
	}
    }

    public static void Trigger(string eventName)
    {
	//TODO: Optimize Mod.Trigger so that it doesn't have to loop through each mod every time
	foreach(Mod m in Mods)
	    m.Call(eventName);
	foreach(Mod m in LuaCore.Values)
	    m.Call(eventName);
    }
}
