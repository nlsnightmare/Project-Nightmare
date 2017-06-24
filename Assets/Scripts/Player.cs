using UnityEngine;
using System;
using System.Threading;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class Player {

    public enum ControlState {
	Normal,
	Stunned,
	Talking,
	Confirm
    }

    private static PlayerController pController;
    static GameObject PlayerPrefab;
    static GameObject playerGO;
    public static void Initialize( ){
	PlayerPrefab = Resources.Load<GameObject>("Prefabs/player");
    }
    
    public static void TakeDamage(int amount,bool isLethal){
	if (amount <= 0) return;

	PlayerStats.Hp = Math.Min(PlayerStats.Hp - amount, isLethal?0:1);
	if (PlayerStats.Hp == 0)
	    Mod.Trigger("onPlayerDeath");
	else
	    Mod.Trigger("onPlayerHit");

    }

    public static void Spawn(int x, int y){
	Spawn(new Vector2(x,y));
    }
    public static void Spawn(Vector2 pos){
	playerGO = GameObject.Instantiate(PlayerPrefab,pos,Quaternion.identity);
	pController = playerGO.GetComponent<PlayerController>();
    }

    public static void SetState(ControlState state){
	pController.PlayerState = state;
    }
    public static void SetState(string state){
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
}
