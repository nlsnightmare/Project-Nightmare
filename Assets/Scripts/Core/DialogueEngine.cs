using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class DialogueEngine : MonoBehaviour {

    public Text DialogueBox;
    public static DialogueEngine Instance;
    void Start(){
	if(Instance == null)
	    Instance = this;
	DialogueBox = GameObject.Find("DialogueBox").GetComponent<Text>();
    }

    public static void Print(string message){
	Player.SetState(Player.ControlState.Talking);
	message = GameData.ReplaceParameters(message,true);
	Instance.DialogueBox.text = message;
    }
    public static void Print(string[] messages){

    }



    static Thread t;
    static bool isReady = false;
    static string StringToShow = "";
    static string TargetString = "";
    static int Delay;

    static void ShowDialogue()
    {
        Player.SetState(Player.ControlState.Talking);
        StringToShow = "";
        foreach (var c in TargetString)
        {
            isReady = false;
            StringToShow += c;
            isReady = true;
            Thread.Sleep(Delay);
        }
    }


    void Update()
    {
        if (!isReady || !t.IsAlive) return;
        DialogueBox.text = StringToShow;
    }

    public static void ShowText(string s, int delay)
    {
        Instance.DialogueBox.text = "";
        TargetString = s;
        Debug.Log(t);
        if (t != null)
        {
            t.Abort();
        }
        t = new Thread(ShowDialogue);
        Delay = delay;
        t.Start();
    }

}
