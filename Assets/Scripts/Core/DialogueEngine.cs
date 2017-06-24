using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class DialogueEngine : MonoBehaviour {

    public Text DialogueBox;
    public static DialogueEngine Instance;
    static Thread t;
    static bool isReady = false;
    public static bool waitingConfirm = true;
    static string StringToShow = "";
    static string TargetString = "";
    static int Delay = 1000;

    void Start(){
	if(Instance == null)
	    Instance = this;
	ShowText("Ok please make this work", "fast");
    }

    public static void Print(string[] messages){

    }


    //This is the function which begins showing text
    public static void ShowText(string s, string speed){
	int delay = 0;
	switch (speed) {
	    case "slow":
		delay = 150;
		break;
	    case "normal":
		delay = 75;
		break;
	    case "fast":
		delay = 50;
		break;
	    default:
		break;
	}
        Instance.DialogueBox.text = "";
        TargetString = s;
        if (t != null)
        {
            t.Abort();
        }
        t = new Thread(ShowDialogue);
        Delay = delay;
        t.Start();
    }


    //Runs on a seperate thread, prints the text
    static void ShowDialogue(){
        Player.SetState(Player.ControlState.Talking);
        StringToShow = "";
        foreach (var c in TargetString)
        {
	    if (!waitingConfirm){
		StringToShow = TargetString;
		break;
	    }
	    
            isReady = false;
            StringToShow += c;
            isReady = true;
            Thread.Sleep(Delay);
        }
	waitingConfirm = true;
	while(waitingConfirm);

	StringToShow = "";
	Player.SetState(Player.ControlState.Normal);
    }


    void Update(){
        if (!isReady || !t.IsAlive) return;
        DialogueBox.text = StringToShow;
    }


}
