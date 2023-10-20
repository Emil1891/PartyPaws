using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/**
 * 
 */

public class PlayerInputManager : MonoBehaviour
{
    public enum EActionMapping  
    {
        MenuMapping = 0, CurrentPlayer = 1, Watcher = 2 
    }

    private EActionMapping currentActionMap = EActionMapping.MenuMapping; 

    // The array index is from the enum above and the string is the action mapping to use in that state  
    [SerializeField] private string[] actionMappingsName; 

    // The player input component 
    [SerializeField] private PlayerInput playerInput; 

    private static GameObject[] players; 

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);

        players = GameObject.FindGameObjectsWithTag("Player"); 
    }

    public void StartGame(InputAction.CallbackContext context)
    {
        const int sceneToLoad = 1; // Should prob be handled in a better way 
        
        // Do nothing if the action is other than pressed or trying to load the current scene 
        if (!context.action.triggered || SceneManager.GetActiveScene().buildIndex == sceneToLoad)
            return;

        SceneManager.LoadScene(sceneToLoad); 
        
        Debug.Log($"Player {gameObject.name} requested start game");

        // Get a random starting index to randomize who plays first 
        int startingPlayerIndex = Random.Range(0, players.Length);

        // Set the starting player's action mapping to current player and the rest to watchers 
        for (int i = 0; i < players.Length; i++)
        {
            if(i == startingPlayerIndex)
                players[i].GetComponent<PlayerInputManager>().SwitchActionMapping(EActionMapping.CurrentPlayer); 
            else
                players[i].GetComponent<PlayerInputManager>().SwitchActionMapping(EActionMapping.Watcher);
        }
    }

    private void SwitchActionMapping(EActionMapping actionMap) 
    { 
        playerInput.SwitchCurrentActionMap(actionMappingsName[(int)actionMap]);
        currentActionMap = actionMap; 
    }

    public EActionMapping GetCurrentActionMapping()
    {
        return currentActionMap; 
    }

}
