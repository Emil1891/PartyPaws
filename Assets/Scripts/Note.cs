using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note
{

    private float time;
    private char noteValue;

    public Note(float time, char noteValue)
    {
        this.time = time;
        this.noteValue = noteValue;
    }

    public string ToString()
    {
        return "["+this.noteValue+"] ["+this.time+"]";
    }

    public float GetTime()
    {
        return time;    
    }

    public char GetNoteValue()
    { 
        return noteValue;
    }
}
