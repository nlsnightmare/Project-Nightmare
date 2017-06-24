using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MoonSharp.Interpreter;

public class Mod {
    string sourceCode;
    Script script;

    public enum ModuleType {
	NPC,
	ITEM,
	SPELL,
	NULL
    }
    public static List<Mod> Mods    = new List<Mod>();
    public static List<Mod> LuaCore = new List<Mod>();

    public ModuleType type;
    public string Name;
    public bool isActive = true;

    //TODO: add exeptions or smth for erros in code
    public void LoadScript(string code){
	sourceCode = code;
	script = new Script();
	AddGlobalsToScript(ref script);
	DynValue ret = script.DoString(sourceCode);

	switch (script.Globals.Get("Module").String) {
	    case "NPC": {
		type = ModuleType.NPC;
		break;
	    }
	    case "ITEM": {
		type = ModuleType.ITEM;
		break;
	    }
	    case "SPELL": {
		type = ModuleType.SPELL;
		break;
	    }
	    default:
		type = ModuleType.NULL;
		break;
	}
    }


    //TODO: Add params to Lua call function
    public void Call(string fun){
	var f = script.Globals[fun];
	if(f != null){
	    script.Call(f);
	}
    }

    public static void AddGlobalsToScript(ref Script s){
	s.Globals[ "player" ] = typeof( Player );
	s.Globals[  "core"  ] = typeof( LuaCoreAPI );
    }

    //TODO: Refactor Lua Mod loading
    public static bool LoadLua( string mainFile ,bool isCore = false){
	var mainFileCode = File.ReadAllText(mainFile);
	Mod m = new Mod();
	Script s = new Script();

	string name = Path.GetFileName(Path.GetDirectoryName(mainFile));

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
	AddGlobalsToScript(ref s);
	m.script = s;
	m.sourceCode = mainFileCode;
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
	if (isCore)
	    LuaCore.Add(m);
	else
	    Mods.Add(m);
	m.Name = name;

	return true;
    }

    public static void LoadAllMods(){
	UserData.RegisterAssembly();
	string builtinPath = Application.dataPath + "/StreamingAssets/Core/";
	string[] coreLuaScripts = Directory.GetFiles(builtinPath,"*.lua");
	foreach (var luaScript in coreLuaScripts){
	    if(!LoadLua(luaScript))
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
    }
}
