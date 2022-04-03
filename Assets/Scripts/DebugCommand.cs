using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class DebugCommand<T> : DebugCommandBase
{
    private Action<T> command;
    
    // constructor 
    public DebugCommand(string id, Action<T> command) : base(id)
    {

        this.command = command;
    }

    // it will invoke the command
    public void Invoke(T value)
    {
        command.Invoke(value);
    }


}

