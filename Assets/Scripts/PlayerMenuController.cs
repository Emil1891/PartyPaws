using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMenuController : MonoBehaviour
{

    private void OnEnable()
    {
        Debug.Log("Enabled"); 
    }

    public void StartGame(InputAction.CallbackContext context)
    {
        if (!context.action.triggered)
            return;

        SceneManager.LoadScene(0);

        Debug.Log($"Player {gameObject.name} requested start game");
        
    }
    
}
