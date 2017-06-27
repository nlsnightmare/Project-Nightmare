using System.IO;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class LuaCoreAPI
{

    public static void Print(string s)
    {
	if (PlayerPrefs.GetInt("debug", 1) == 1)
	{
	    //TODO: Show Debug logs in-game if  lua debug setting is enabled
	}
	Debug.Log(s);
    }

    //TODO: Implement Dialogue Engine
    public static void ShowDialogue(string[] messages, string speed)
    {
	for (int i = 0; i < messages.Length; i++)
	{
	    Debug.Log("messages[" + i + "] = " + messages[i]);
	}
	DialogueEngine.Print(messages, speed);
    }



    public static LuaObject Create(DynValue args){
	LuaObject lObj = new LuaObject();
	
	var imagePath = args.Table.Get("image").String;
	int pixelsPerUnit = (int)args.Table.Get("pixelsPerUnit").Number;
	int x = (int)args.Table.Get("x").Number;
	int y = (int)args.Table.Get("y").Number;

	if (!File.Exists(args.Table.Get("image").String)){
	    Debug.Log("Path doesn't exist!");
	}
	var go = new GameObject();
	go.transform.position = new Vector2(x, y);
	var sr = go.AddComponent<SpriteRenderer>();
	byte[] data = File.ReadAllBytes(imagePath);
	Texture2D texture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
	if (texture == null)
	    throw new System.Exception("Couldn't load texture");
	texture.LoadImage(data);
	var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
	sr.sprite = sprite;

	return lObj;
    }

}
