using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class LuaCoreAPI {

    public static void Print(string s){
	//TODO: Show Debug logs in-game if  lua debug setting is enabled
	Debug.Log(s);
    }

    //TODO: Implement Dialogue Engine
    public static void ShowDialogue(string[] messages, string speed){
	foreach (var m in messages){
	    Debug.Log(m);
	}
	DialogueEngine.ShowText(messages[0],speed);
    }

}
