using System;
using UnityEngine;
using Combat;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class Player {
    #region Variable declerations

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

    #endregion

    #region Initialization
    public static void Initialize( ){
	PlayerPrefab = Resources.Load<GameObject>("Prefabs/player");
    }
    #endregion
    
    #region Combat Methods
    public static void TakeDamage(DamageData dd){
	if (dd.amount <= 0) return;

	PlayerStats.HP = Math.Max(PlayerStats.HP - dd.amount, dd.isLethal?0:1);
	if (PlayerStats.HP <= 0)
	{
	    Mod.Trigger("onPlayerDeath");
	    throw new NotImplementedException("The Player has died");
	}
	else
	    Mod.Trigger("onPlayerHit");
    }
    #endregion

    #region Spawn Methods
    public static void Spawn(int x, int y){
	Spawn(new Vector2(x,y));
    }
    public static void Spawn(Vector2 pos){
	playerGO = GameObject.Instantiate(PlayerPrefab,pos,Quaternion.identity);
	pController = playerGO.GetComponent<PlayerController>();
    }
    #endregion

    #region State Code
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
    #endregion
}
