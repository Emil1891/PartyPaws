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


    // Start is called before the first frame update
    void Start()
    {
        Screech = FMODUnity.RuntimeManager.CreateInstance("event:/AnimalSound");
        StartCoroutine(PlaySplash());
    }

    IEnumerator PlaySplash()
    {
        yield return new WaitForSeconds(1.7f);

        

        p.SetTrigger("PolarBear");
        Screech.setParameterByName("animalType", 1);
        Screech.start();

        yield return new WaitForSeconds(0.4f);
        Screech.setParameterByName("animalType", 0);
        Screech.start();
        g.SetTrigger("Goose");

        yield return new WaitForSeconds(0.4f);

        Screech.setParameterByName("animalType", 3);
        Screech.start();
        w.SetTrigger("Worm");

        yield return new WaitForSeconds(0.4f);

        Screech.setParameterByName("animalType", 2);
        Screech.start();
        d.SetTrigger("Dolphin");

        yield return new WaitForSeconds(2f);

        sl.LoadScene("PlayerJoinScene");

        yield return null;

    }
}
