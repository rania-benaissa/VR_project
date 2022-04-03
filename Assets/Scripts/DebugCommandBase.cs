using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DebugCommandBase
{
    private string id;

    public string ID
    {
        get
        {
            return id;
        }
    }


    // constructor 

    public DebugCommandBase(string id)
    {

        this.id = id;

    }


}
