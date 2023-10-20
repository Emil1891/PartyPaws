using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{

    private List<Note> listOfNotes;
    private float currentTime = 0f;
    public float targetTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        listOfNotes = new();
    }

    // Update is called once per frame
    void Update()
    {

        currentTime += Time.deltaTime;

        

        if (Input.GetKeyDown("k"))
        {
            Debug.Log("k was pressed at" + currentTime);
            Note note = new Note(currentTime, 'k');
            listOfNotes.Add(note);
        }

        if (Input.GetKeyDown("j"))
        {
            Debug.Log("j was pressed at" + currentTime);
            Note note = new Note(currentTime, 'j');
            listOfNotes.Add(note);
        }

        if (Input.GetKeyDown("l"))
        {
            Debug.Log("l was pressed at" + currentTime);
            Note note = new Note(currentTime, 'l');
            listOfNotes.Add(note);
        }
        if (Input.GetKeyDown("b"))
        {
            foreach (Note note in listOfNotes)
            {
                string str = note.GetTime() + " - " + note.GetNoteValue();
                Debug.Log(str);
            }

        }

    }
}
