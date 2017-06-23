using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueEngine : MonoBehaviour {

    public Text DialogueBox;
    public static DialogueEngine Instance;
    void Start(){
	if(Instance == null){
	    Instance = this;
	}
    }

    public static void Print(string message){
	Player.SetState(Player.ControlState.Talking);
	message = GameData.ReplaceParameters(message,true);
	Instance.DialogueBox.text = message;
    }
    public static void Print(string[] messages){

    }
}
