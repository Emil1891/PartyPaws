using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{

    // The array index is the scene index and the string is the action mapping to use in that scene 
    [SerializeField] private string[] actionMappingsNameOnScene; 

    // The player input component 
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private GameObject[] players; 
    
    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);

        players = GameObject.FindGameObjectsWithTag("Player"); 
    }

    public void StartGame(InputAction.CallbackContext context)
    {
        int sceneToLoad = 1; // Should prob be handled in a better way 
        
        if (!context.action.triggered || SceneManager.GetActiveScene().buildIndex == sceneToLoad)
            return;

        SceneManager.LoadScene(sceneToLoad); 

        SwitchActionMapping(sceneToLoad); 
        
        Debug.Log($"Player {gameObject.name} requested start game");
        
    }

    private void SwitchActionMapping(int sceneIndex)
    {
        playerInput.SwitchCurrentActionMap(actionMappingsNameOnScene[sceneIndex]);

    }
}
