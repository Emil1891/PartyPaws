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

    private float cheerTimer;

    [SerializeField] private float cheerDelay = 1f; 

    private void Awake()
    {
        enabled = false; 
    }

    private void Update()
    {
        cheerTimer += Time.deltaTime; 
    }

    public void SetGameManager(GameManager gameManager) 
    {
        this.gameManager = gameManager; 
    }

    public void PlayCheerSound(InputAction.CallbackContext context)
    {
        if (!context.action.triggered)
            return; 
        
        if (cheerTimer > cheerDelay)
        {
            // play cheer sound 
        }
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
