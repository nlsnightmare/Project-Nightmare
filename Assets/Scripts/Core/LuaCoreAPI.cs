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
	for (int i = 0; i < messages.Length; i++) {
	    Debug.Log("messages["+i+"] = " + messages[i]);
	}
	DialogueEngine.Print(messages,speed);
    }

}
