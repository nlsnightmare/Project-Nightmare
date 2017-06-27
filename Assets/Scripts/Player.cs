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
	Confirm,
	Paused
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

    public static ControlState GetState(){
	return pController.PlayerState;
    }
    public static void SetState(ControlState state){
	pController.PlayerState = state;
	if (pController.PlayerState != ControlState.Normal){
	    pController.StopMoving();
	    Debug.Log("wat");
	}
    }
    public static void SetState(string state){
	switch (state) {
	    case "Stunned":
		SetState( ControlState.Stunned );
		break;
	    case "Normal":
		SetState( ControlState.Normal );
		break;
	    case "Talking":
		SetState( ControlState.Talking );
		break;
	    case "Paused":
		SetState( ControlState.Paused );
		break;
	    default:
		throw new Exception("ControlState '" + state + "' doesn't exist!");
	}
    }
}
