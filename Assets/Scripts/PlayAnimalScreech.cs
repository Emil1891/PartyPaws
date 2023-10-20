using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimalScreech : MonoBehaviour
{
    private FMOD.Studio.EventInstance AnimalSound;

    [SerializeField] private int animalID;
   
    private void Start(){
            AnimalSound = FMODUnity.RuntimeManager.CreateInstance("event:/AnimalSound");
    }

    public void Screech()
    {
         //   AnimalSound = FMODUnity.RuntimeManager.CreateInstance("event:/AnimalSound");
        AnimalSound.setParameterByName("animalType", animalID);
        AnimalSound.start();
    }
    
    // Update is called once per frame
    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Screech();
        }
    }

}
