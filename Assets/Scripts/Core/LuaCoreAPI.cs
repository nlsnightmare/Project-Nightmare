using System.IO;
using UnityEngine;
using UnityEngine.UI;
using MoonSharp.Interpreter;

namespace LuaAPI {
    
    [MoonSharpUserData]
    public class LuaCoreAPI
    {

	public static void TestData(DynValue e){
	    Combat.DamageData dd = new Combat.DamageData(e);
	}

	public static void Print(string s){
	    if (PlayerPrefs.GetInt("debug", 1) == 1)
	    {
		//TODO: Show Debug logs in-game if  lua debug setting is enabled
	    }
	    Debug.Log(s);
	}

	public static void ShowDialogue(string message, string speed){
	    string[] m = new string[1];
	    m[0] = message;
	    ShowDialogue(m, speed);
	}
	public static void ShowDialogue(string[] messages, string speed){
	    throw new System.NotImplementedException("Dialogue engine is not ready");
	    //DialogueEngine.Print(messages, speed);
	}


	public static LuaObject Create(DynValue args){
	    LuaObject lObj = new LuaObject();
	
	    //Input defaults here
	    string name;
	    bool hasCollision;
	    string imagePath; 
	    int pixelsPerUnit; 
	    int x;
	    int y;
	    {
		if (args.Table.Get("name") == DynValue.Nil)
		    name = "new lua object";
		else
		    name = args.Table.Get("name").String;

		if (args.Table.Get("image") == DynValue.Nil)
		    imagePath = "null";
		else
		    //TODO: Add placeholder image for custom objects
		    imagePath = args.Table.Get("image").String; 
		if ( args.Table.Get("pixelsPerUnit") == DynValue.Nil ) 
		    pixelsPerUnit = 64;
		else
		    pixelsPerUnit = (int)args.Table.Get("pixelsPerUnit").Number; 

		if (args.Table.Get("x") == DynValue.Nil)
		    x = 0;
		else
		    x = (int)args.Table.Get("x").Number;
		if (args.Table.Get("y") == DynValue.Nil)
		    y = 0;
		else
		    y = (int)args.Table.Get("y").Number;

		if (args.Table.Get("collision") == DynValue.Nil)
		    hasCollision = true;
		else
		    hasCollision = args.Table.Get("collision").Boolean;
	    }

	    //Create the gameobject
	    var go = new GameObject();
	    go.name = name;
	    go.layer = LayerMask.NameToLayer("Interactable");
	    var driver = go.AddComponent<LuaObjectDriver>();
	    driver.obj = lObj;
	    go.transform.position = new Vector2(x, y);
	    lObj.pos = go.transform;
	
	    //Load and add the sprite
	    if (!File.Exists(imagePath)){
		Debug.Log("Path '" +imagePath + "' doesn't exist!");
	    }

	    var sr = go.AddComponent<SpriteRenderer>();
	    byte[] data = File.ReadAllBytes(imagePath);
	    Texture2D texture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
	    if (texture == null)
		throw new System.Exception("Couldn't load texture");
	    texture.LoadImage(data);
	    var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
	    sr.sprite = sprite;


	    //Add collision
	    //TODO: Add support for complex colliders
	    if (hasCollision)
		go.AddComponent<BoxCollider2D>();

	    return lObj;
	}

    }

    [MoonSharpUserData]
    public class LuaGUIAPI {
	//TODO: Add font and position to the created text gameobject
	public static void CreateText(DynValue args){
	    var go = new GameObject();
	    var t = go.AddComponent<Text>();
	    var pos = go.AddComponent<RectTransform>();
	    t.text = args.Table.Get("text").String;
	}
    }

}
