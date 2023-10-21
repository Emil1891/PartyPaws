using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenScript : MonoBehaviour
{

    public Animator d;
    public Animator g;
    public Animator p;
    public Animator w;
    public SceneLoader sl;
    private FMOD.Studio.EventInstance Screech;
    private FMOD.Studio.EventInstance CuteNote;



    // Start is called before the first frame update
    void Start()
    {
        Screech = FMODUnity.RuntimeManager.CreateInstance("event:/AnimalSound");
        CuteNote = FMODUnity.RuntimeManager.CreateInstance("event:/UI/IntroNotes");

        StartCoroutine(PlaySplash());
    }

    IEnumerator PlaySplash()
    {
        yield return new WaitForSeconds(1.7f);

        

        p.SetTrigger("PolarBear");
        Screech.setParameterByName("animalType", 1);
        Screech.start();
        CuteNote.setParameterByName("notesInSuccession", 0);
        CuteNote.start();

        yield return new WaitForSeconds(0.4f);
        Screech.setParameterByName("animalType", 0);
        Screech.start();
        CuteNote.setParameterByName("notesInSuccession", 1);
        CuteNote.start();

        g.SetTrigger("Goose");

        yield return new WaitForSeconds(0.4f);

        Screech.setParameterByName("animalType", 3);
        Screech.start();
        CuteNote.setParameterByName("notesInSuccession", 2);
        CuteNote.start();

        w.SetTrigger("Worm");

        yield return new WaitForSeconds(0.4f);

        Screech.setParameterByName("animalType", 2);
        Screech.start();
        CuteNote.setParameterByName("notesInSuccession", 3);
        CuteNote.start();

        d.SetTrigger("Dolphin");

        yield return new WaitForSeconds(2f);

        sl.LoadScene("PlayerJoinScene");

        yield return null;

    }
}
