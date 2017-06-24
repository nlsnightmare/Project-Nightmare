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
    }

    static void SetDelay(string speed){
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
        Delay = delay;
    }

    public static void Print(string message, string speed){
	SetDelay(speed);
	StartDialogueThread(message,speed);
    }

    public static void Print(string[] messages, string speed){
	SetDelay(speed);
	StartDialogueThread(messages,speed);
    }

    //This is the function which begins showing text
    public static void StartDialogueThread(string s, string speed){
        if (t != null)
        {
            t.Abort();
        }
        t = new Thread( () => {
		DialogueEngine.ShowDialogue(s);
	    });
	t.Start();
    }

    public static void StartDialogueThread(string[] s, string speed){
        if (t != null)
        {
            t.Abort();
        }
        t = new Thread( () => {
		DialogueEngine.ShowDialogue(s);
	    });
	t.Start();
    }


    //Runs on a seperate thread, prints the text
    public static void ShowDialogue(string TargetString){
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

    public static void ShowDialogue(string[] messages){
	foreach (var msg in messages){
			ShowDialogue(msg);
			waitingConfirm = true;
			Thread.Sleep(10);
		}
		StringToShow = "";
	}


    void Update(){
        if (!isReady || !t.IsAlive) return;
        DialogueBox.text = StringToShow;
    }


}
