using System.IO;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class LuaCoreAPI {

    public static void Print(string s){
	if (PlayerPrefs.GetInt("debug",1) == 1) {
	    //TODO: Show Debug logs in-game if  lua debug setting is enabled
	    
	}
	Debug.Log(s);
    }

    //TODO: Implement Dialogue Engine
    public static void ShowDialogue(string[] messages, string speed){
	for (int i = 0; i < messages.Length; i++) {
	    Debug.Log("messages["+i+"] = " + messages[i]);
	}
	DialogueEngine.Print(messages,speed);
    }



    public static void LoadSprite(string path){
	// byte[] data = File.ReadAllBytes(path);
	// Texture2D texture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
	// if (texture == null)
	//     throw new System.Exception("Couldn't load texture");
	// texture.LoadImage(data);
	// texture.name = Path.GetFileNameWithoutExtension(path);
	Debug.Log(path);
	if(!File.Exists(path)){
	    Debug.Log("Path doesn't exist!");
	    return;
	}
	byte[] data = File.ReadAllBytes(path);
	Debug.Log(data);
    }

}
