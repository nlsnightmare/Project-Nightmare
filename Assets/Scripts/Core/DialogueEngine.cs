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

    const int slowSpeed = 100;
    const int normalSpeed = 75;
    const int fastSpeed = 50;

    void Start(){
	if(Instance == null)
	    Instance = this;
    }

    static void SetDelay(string speed){
	int delay = 0;
	switch (speed) {
	    case "slow":
		delay = slowSpeed;
		break;
	    case "normal":
		delay = normalSpeed;
		break;
	    case "fast":
		delay = fastSpeed;
		break;
	    default:
		delay = normalSpeed;
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
	for(int i = 0; i < messages.Length; i++)
	    messages[i] = GameData.ReplaceParameters(messages[i]);
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
        Player.SetState(Player.ControlState.Talking);
	t.Start();
    }


    //Runs on a seperate thread, prints the text
    public static void ShowDialogue(string TargetString){
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
    }
//TODO: ewfwe
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
