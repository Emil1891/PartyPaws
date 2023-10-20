using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Should prob be renamed to PlayerController, since it now handles all player behaviour 
 */

public class PlayerGameController : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); 
    }

    private void PlayNote(InputAction.CallbackContext context, char buttonName)
    {
        // context.action.actionMap
        if (!context.action.triggered) 
            return; 
        
        gameManager.ButtonPressed(buttonName);
        
        // Debug.Log($"Player pressed {buttonName}"); 
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
