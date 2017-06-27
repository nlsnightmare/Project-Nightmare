using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class LuaObject {
    ScriptFunctionDelegate interact;
    public void Interact(){
	interact();
    }

    public void Bind(string function, DynValue fun){
	if (function == "onInteract")
	    interact += fun.Function.GetDelegate();
    }
}
