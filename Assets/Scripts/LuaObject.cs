using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class LuaObject {
    Dictionary<string,ScriptFunctionDelegate> events = new Dictionary<string,ScriptFunctionDelegate>();

    public LuaObject(){
    }
    public void Interact(){
	events["onInteract"]();
    }

    public void Bind(string eventName, DynValue fun){
	if (!events.ContainsKey(eventName)) {
	    ScriptFunctionDelegate del = fun.Function.GetDelegate();
	    events[eventName] = del;
	}
	else {
	    events[eventName] += fun.Function.GetDelegate();
	}
	Debug.Log("bind succeded");
    }
}
