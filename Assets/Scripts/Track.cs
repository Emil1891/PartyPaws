using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track 
{

    // keeps track of the note that the spawner composed 
    public List<Note> composerNotes = new();
    
    // keeps track of the notes that the reenactor has correctly inputted 
    public List<Note> reenactorNotes = new();
    
    // keeps track of the spawned notes in the reenact phase 
    public List<Note> spawnedNotes = new();

    // private float currentTime = 0f;
    public float targetTime = 10f;
    
    // How close the reenactors need to be to consider it a success 
    private float graceRange = 0.5f;

    private int reenactCounter = 0;

    private float spawnTime = 2f; 

    public bool PlayerMissedNote(float time)
    {
        if (reenactorNotes.Count == composerNotes.Count)
            return false; 
        
        // check all composed notes 
        foreach (var note in composerNotes)
        {
            // if player has reenacted it correctly, check next note 
            if (reenactorNotes.Contains(note))
                continue;

            // more time has passed than what is required to consider it a success, then player has failed 
            if (time > note.GetTime() + graceRange)
            {
                Debug.Log($"1out of time range, diff: {time - note.GetTime() + graceRange}, time: {time}, reenactor size: {reenactorNotes.Count}");
                return true;
            }

        }

        return false; 
    }

    public Note GetNoteToSpawn(float time)
    {
        foreach (var note in composerNotes)
        {
            // already spawned note 
            if (spawnedNotes.Contains(note))
                continue;

            // if it's time to spawn the note 
            if (Mathf.Abs(note.GetTime() - time) <= spawnTime)
            {
                spawnedNotes.Add(note);
                return note;
            }
        }

        // no note to spawn 
        return null; 
    }

    public void NewReenactStarted()
    {
        reenactCounter = 0; 
        reenactorNotes.Clear(); 
        spawnedNotes.Clear(); 
    }

    public void NewCompRoundStarted()
    {
        reenactCounter = 0; 
        reenactorNotes.Clear(); 
        spawnedNotes.Clear(); 
        composerNotes.Clear(); 
    }
    
    public bool PlayedCorrectNote(char buttonName, float time)
    {
        // has reenacted all notes 
        if (reenactCounter >= composerNotes.Count)
        {
            Debug.Log($"has acted out all notes. reenact counter: {reenactCounter}");
            return true;
        }

        var nextNote = composerNotes[reenactCounter++];

        // pressed wrong button 
        if (!nextNote.GetButtonName().Equals(buttonName))
        {
            Debug.Log($"Pressed wrong button");
            return false; 
        }

        // if not within time range 
        if (Mathf.Abs(time - nextNote.GetTime()) > graceRange)
        {
            Debug.Log($"2Out of time range, time diff: {Mathf.Abs(time - nextNote.GetTime()) > graceRange}");
            return false; 
        }

        // success 
        reenactorNotes.Add(nextNote); 
        Debug.Log($"size: {reenactorNotes.Count}");
        return true; 
    }
    
}
