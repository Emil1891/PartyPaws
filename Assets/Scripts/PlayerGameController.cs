using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGameController : MonoBehaviour
{

    public void PlayNote(InputAction.CallbackContext context)
    {
        // Only do things when it is pressed/triggered 
        if (!context.action.triggered)
            return; 
        
        Debug.Log($"I played tone {context.action.id}"); 
    }

}
