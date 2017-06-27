using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class LuaObject {
    Dictionary<string,ScriptFunctionDelegate> events = new Dictionary<string,ScriptFunctionDelegate>();

    public void Trigger(string eventName){
	Debug.Log("yayyyyyyyy");
	if (events.ContainsKey(eventName)){
	    events[eventName]();
	    Debug.Log("im called");
	}
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
