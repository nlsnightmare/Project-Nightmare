using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class LuaObject {
    Dictionary<string,ScriptFunctionDelegate> events = new Dictionary<string,ScriptFunctionDelegate>();
    public Transform pos;

    public void Trigger(string eventName,params DynValue[] args ){
	if (events.ContainsKey(eventName))
	    events[eventName](args);
    }

    public void Bind(string eventName, DynValue fun){
	if (!events.ContainsKey(eventName)) {
	    ScriptFunctionDelegate del = fun.Function.GetDelegate();
	    events[eventName] = del;
	}
	else {
	    events[eventName] += fun.Function.GetDelegate();
	}
    }

    public DynValue GetPosition(){
	DynValue ret = new DynValue();
	ret = DynValue.NewTuple(DynValue.NewNumber(pos.position.x),DynValue.NewNumber(pos.position.y));
	return ret;
    }
}
