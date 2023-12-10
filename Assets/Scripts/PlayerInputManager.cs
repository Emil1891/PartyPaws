using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    private static List<GameObject> players = new();

    private static bool hasRequestedJoin = false; 

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);

        players.Add(gameObject);

        hasRequestedJoin = false;
    }

    public void StartGame(InputAction.CallbackContext context)
    {
        const string sceneToLoad = "GameScene";
        
        // Do nothing if the action is other than pressed or trying to load the current scene 
        if (hasRequestedJoin || !context.action.triggered || SceneManager.GetActiveScene().name.Equals(sceneToLoad))
            return;
        
        if (players.Count < 2)
        {
            Debug.Log("Min 2 players required");
            return; 
        }

        hasRequestedJoin = true; 

        FindObjectOfType<SceneLoader>().LoadScene(sceneToLoad); 
        
        // Set all player action mappings to watchers 
        foreach (var player in players)
        {
            player.GetComponent<PlayerInputManager>().SwitchActionMapping(EActionMapping.Watcher);
            player.GetComponent<PlayerGameController>().enabled = true;
        }

        players = new();
        PlayerMenuManager.playerCount = 0; 
    }

    public void SwitchActionMapping(EActionMapping actionMap) 
    { 
        playerInput.SwitchCurrentActionMap(actionMappingsName[(int)actionMap]);
        currentActionMap = actionMap; 
    }

    public EActionMapping GetCurrentActionMapping()
    {
        return currentActionMap; 
    }

}
