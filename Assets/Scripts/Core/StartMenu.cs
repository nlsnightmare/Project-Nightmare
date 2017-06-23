using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {

    public Text DebugText;
    public static StartMenu Instance;
    // Use this for initialization
    void Start () {
	Instance = this;
	GameData.Initialize();
	Player.Initialize();
	LoadSavedGames();
	Mod.LoadAllMods();
    }


    public static void Print(string text,string color = "white"){
	Instance.DebugText.text = "<color=" + color + ">" + text + "</color>";
    }

    public GameObject MainMenu_GO;
    public GameObject ModManager_GO;
    public void ToggleModManager(){
	ModManager_GO.SetActive(!ModManager_GO.activeSelf);
	MainMenu_GO.SetActive(!MainMenu_GO.activeSelf);
    }

    public GameObject LoadGame_GO;
    public void ToggleLoadGame(){
	LoadGame_GO.SetActive(!LoadGame_GO.activeSelf);
	MainMenu_GO.SetActive(!MainMenu_GO.activeSelf);
    }

    public GameObject NewGame_GO;
    public void ToggleNewGame(){
	NewGame_GO.SetActive(!NewGame_GO.activeSelf);
	MainMenu_GO.SetActive(!MainMenu_GO.activeSelf);
    }

    public InputField NewGameText;
    public void NewGame(){
	string name = NewGameText.text;
	if(name == "" )
	{
	    Print( "Please Enter a name" );
	    return;
	}
	string fileName = NewGameText.text + ".sav";
	string[] names = Directory.GetFiles(Application.dataPath + "/StreamingAssets/Saves", "*.sav");
	foreach(var n in names)
	    if( Path.GetFileName(n) == fileName )
	    {
		Print("A save file with the name '" + name + "' already exists!");
		return;
	    }
	File.Create(Application.dataPath + "/StreamingAssets/Saves/" + fileName ).Close();
	GameData.InitializeNewGame(name);
    }

    public GameObject SaveGamePrefab;
    public GameObject SavedGamesLayout;
    void LoadSavedGames(){
	foreach (var file in Directory.GetFileSystemEntries(Application.dataPath + "/StreamingAssets/Saves/","*.sav")){
	    var go = Instantiate(SaveGamePrefab, SavedGamesLayout.transform);
	    //TODO: add more information, loading 
	    go.GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(file);
	    go.GetComponent<Button>().onClick.AddListener(() => {
		    GameData.LoadSavedGame(file);
		});
	}
    }
}
