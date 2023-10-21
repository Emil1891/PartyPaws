using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * 
 */

public class PlayerGameController : MonoBehaviour
{
    private GameManager gameManager; 

    private float cheerTimer;
    
    private FMOD.Studio.EventInstance AnimalSound;

    [SerializeField] private float cheerDelay = 1f;

    private void Start()
    {
        AnimalSound = FMODUnity.RuntimeManager.CreateInstance("event:/AnimalSound"); 
    }

    private void Update()
    {
        cheerTimer += Time.deltaTime; 
    }

    public void SetGameManager(GameManager newGameManager) 
    {
        gameManager = newGameManager; 
    }

    public void PlayCheerSound(InputAction.CallbackContext context)
    {
        if (!context.action.triggered)
            return; 
        
        if (cheerTimer > cheerDelay)
        {
            AnimalSound.setParameterByName("animalType", GetComponent<PlayerInfo>().playerIndex); 
            AnimalSound.start();
            cheerTimer = 0; 
        }
    }

    private void PlayNote(InputAction.CallbackContext context, char buttonName)
    {
        if (!context.action.triggered || gameManager == null) 
            return; 
        
        gameManager.ButtonPressed(buttonName); 
    }
    
    public void PlayNoteA(InputAction.CallbackContext context)
    {
        PlayNote(context, 'A'); 
    }
    
    public void PlayNoteB(InputAction.CallbackContext context)
    {
        PlayNote(context, 'B'); 
    }
    
    public void PlayNoteX(InputAction.CallbackContext context)
    {
        PlayNote(context, 'X'); 
    }
    
    public void PlayNoteY(InputAction.CallbackContext context)
    {
        PlayNote(context, 'Y'); 
    } 

}
