using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGameController : MonoBehaviour
{
    
    private void PlayNote(InputAction.CallbackContext context, char buttonName)
    {
        if (!context.action.triggered)
            return; 
        
        Debug.Log($"Player pressed {buttonName}"); 
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
