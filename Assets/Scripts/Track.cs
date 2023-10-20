using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track 
{

    public List<Note> listOfComposerNotes = new();
    
    public List<Note> listOfReenactorNotes = new();

    // private float currentTime = 0f;
    public float targetTime = 10f;
    
    // How close the reenactors need to be to consider it a success 
    private float graceRange = 0.1f;

    private int reenactCounter = 0; 

    public bool PlayerMissedNote(float time)
    {
        // check all composed notes 
        foreach (var note in listOfComposerNotes)
        {
            // if player has reenacted it correctly, check next note 
            if (listOfReenactorNotes.Contains(note))
                continue;

            // more time has passed than what is required to consider it a success, then player has failed 
            if (time > note.GetTime() + graceRange)
                return true; 
        }

        return false; 
    }

    public void NewReenactStarted()
    {
        reenactCounter = 0; 
    }
    
    public bool PlayedCorrectNote(char buttonName, float time)
    {
        // has reenacted all notes 
        if (reenactCounter >= listOfReenactorNotes.Count)
            return true; 
        
        var nextNote = listOfComposerNotes[reenactCounter++];

        // pressed wrong button 
        if (!nextNote.GetButtonName().Equals(buttonName))
        {
            return false; 
        }

        // if not within time range 
        if (Mathf.Abs(time - nextNote.GetTime()) > graceRange)
        {
            return false; 
        }

        // success 
        return true; 
    }

    // public void AddNote(char buttonName)
    // {
    //     listOfNotes.Add(new Note(currentTime, buttonName)); 
    // }

    // Start is called before the first frame update
    // void Start()
    // {
    //     listOfNotes = new();
    // }

    // Update is called once per frame
    // void Update()
    // {
    //
    //     currentTime += Time.deltaTime;
    //
    //     
    //
    //     if (Input.GetKeyDown("k"))
    //     {
    //         Debug.Log("k was pressed at" + currentTime);
    //         Note note = new Note(currentTime, 'k');
    //         listOfNotes.Add(note);
    //     }
    //
    //     if (Input.GetKeyDown("j"))
    //     {
    //         Debug.Log("j was pressed at" + currentTime);
    //         Note note = new Note(currentTime, 'j');
    //         listOfNotes.Add(note);
    //     }
    //
    //     if (Input.GetKeyDown("l"))
    //     {
    //         Debug.Log("l was pressed at" + currentTime);
    //         Note note = new Note(currentTime, 'l');
    //         listOfNotes.Add(note);
    //     }
    //     if (Input.GetKeyDown("b"))
    //     {
    //         foreach (Note note in listOfNotes)
    //         {
    //             string str = note.GetTime() + " - " + note.GetNoteValue();
    //             Debug.Log(str);
    //         }
    //
    //     }

    // }
}
