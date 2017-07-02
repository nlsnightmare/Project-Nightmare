using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{

	// Use this for initialization
	public string DoorId;
	public Vector2 Position;
	public string levelName;
	public bool Locked = false;

	public void InteractWithPlayer()
	{
		if (!Locked)
		{
			SceneManager.LoadScene(levelName);
			PlayerController.SetPlayerPosition(Position);
		}
		else if (false)
		{
			//TODO: add player inventory
		}
		else
		{
			//display a message saying the door is locked
			string[] msg = {
"You try to open the door",
"It was locked"
		};
			// DialogueEngine.Print(msg, "normal");
			throw new System.NotImplementedException("dialogue engine is missing!");
		}
	}
}
