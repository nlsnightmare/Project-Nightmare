using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModManager : MonoBehaviour {
    // Use this for initialization
    public GameObject TogglePrefab;
    public GameObject Layout;
    void Start () {
	int count = 0;
	foreach (Mod m in Mod.Mods){
	    CreateToggler(m);
	    if(m.isActive) count++;
	}

	GameObject.Find("Title Text").GetComponent<Text>().text = "Active Mods: " + count;
	GameObject.Find("Installed Count").GetComponent<Text>().text = "Installed Mods: " + Mod.Mods.Count;
    }

    private void CreateToggler(Mod m){
	var newtoggle = Instantiate(TogglePrefab, Layout.transform);
	newtoggle.GetComponentInChildren<Text>().text = m.Name;
	newtoggle.GetComponent<Toggle>().onValueChanged.AddListener((state) => {
		ModManager.ToggleMod(m.Name);
	});
    }

    public static void ToggleMod(string name){
	int count = 0;
	Debug.Log(name);
	foreach (var m in Mod.Mods){
	    if(m.Name == name){
		m.isActive = !m.isActive;
		Debug.Log("found a match! Setting it to " + m.isActive);
	    }
	    if(m.isActive) count++;
	}
	GameObject.Find("Title Text").GetComponent<Text>().text = "Active Mods: " + count;
	Debug.Log("A");
    }

    public void TogglerClicked(Toggle t){
	Debug.Log(t.isOn);
    }
}
