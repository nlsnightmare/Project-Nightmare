using UnityEngine;
using MoonSharp.Interpreter;
//hello
[MoonSharpUserData]
public class Player {

    public enum ControlState {
	Normal,
	Stunned,
	Talking
    }

    private static PlayerController pController;
    static GameObject PlayerPrefab;
    static GameObject playerGO;
    public static void Initialize( ){
	PlayerPrefab = Resources.Load<GameObject>("Prefabs/player");
    }
    
    public static void Spawn(Vector2 pos){
	playerGO = GameObject.Instantiate(PlayerPrefab,pos,Quaternion.identity);
	pController = playerGO.GetComponent<PlayerController>();

	Mod.Trigger("onLoad");
    }

    public static void SetState(ControlState state){
	pController.PlayerState = state;
    }
    public static void SetState(string state){
	Debug.Log("A");
	switch (state) {
	    case "Stunned":
		pController.PlayerState = ControlState.Stunned;
		break;
	    case "Normal":
		pController.PlayerState = ControlState.Normal;
		break;
	    case "Talking":
		pController.PlayerState = ControlState.Talking;
		break;
	}
    }

    public static void DebugLua(string s){
	Debug.Log(s);
    }
}
