using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))] 
public class GameManager : MonoBehaviour
{
    
    private enum GameState {
        Composing = 0, Reenacting, Transition 
    }

    private GameState currentGameState = GameState.Transition; 
    
    private GameObject[] players; 

    private GameObject currentPlayer; 

    private int composerPlayerIndex = 0; 

    private AudioSource audioPlayer;
    
    private GameObject[] playersPlayedThisRound = new GameObject[4];

    [SerializeField] private TextMeshProUGUI timerText;

    private float timer = 0; 

    // private Track currentTrack; // something like this? 

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        players = GameObject.FindGameObjectsWithTag("Player");

        // Sort players in the joined order 
        Array.Sort(players, (p1, p2) => p1.GetComponent<PlayerInput>().user.index - p2.GetComponent<PlayerInput>().user.index);

        SetStartingPlayer(); 

        audioPlayer = GetComponent<AudioSource>(); 
        
        StartNewComposeRound(); 
    }

    private void Update()
    {
        if (currentGameState.Equals(GameState.Composing) || currentGameState.Equals(GameState.Reenacting))
        {
            timer -= Time.deltaTime; 
            timerText.SetText($"Time left: {timer}"); 
        }
        else
        {
            timerText.SetText(""); 
        }
    }

    private void SetStartingPlayer()
    {
        // Get the starting player 
        foreach (var player in players)
        {
            player.GetComponent<PlayerGameController>().SetGameManager(this); 
            
            if (player.GetComponent<PlayerInputManager>().GetCurrentActionMapping()
                .Equals(PlayerInputManager.EActionMapping.CurrentPlayer))
            {
                currentPlayer = player;
            }
        }
    }

    private void StartNewComposeRound()
    {
        Debug.Log("New round started");

        currentGameState = GameState.Composing; 

        // Start coroutine that will run after song ends 
        StartCoroutine(ComposeRoundEnd()); 
    }

    private void StartNewReenactRound()
    {
        Debug.Log($"Started new reenact round");

        currentGameState = GameState.Reenacting; 
        
        // Start coroutine that will run after song ends 
        StartCoroutine(ReenactRoundEnd()); 
    }
    
    public void ButtonPressed(char buttonName)
    {
        // Debug.Log($"Pressed {buttonName}");

        switch (currentGameState)
        {
            case GameState.Transition: 
                Debug.Log("Transitioning between states, no input"); 
                return; 
            
            case GameState.Composing:
                // composing stuff 
                Debug.Log($"Composing {buttonName}");
                PlayDrumSound(buttonName); 
                // Create new note 
                // add to track 
                break; 
            
            case GameState.Reenacting:
                // reenact stuff 
                Debug.Log($"Reenacting {buttonName}");
                
                PlayDrumSound(buttonName); 
                
                // get next button in the track 
                
                // if no more buttons 
                    // return 
                
                // if track finished 
                    // success 
                
                // if player timed their press and pressed correct button 
                    // cancel reenacting 
                    // 
                break; 
            
            default: 
                Debug.LogWarning("Non existing game state, something is prob wrong!");
                break;
        }

        // audioPlayer.PlayOneShot(); // Play the associated sound via Fmod 
    }

    private void PlayDrumSound(char buttonName)
    {
        if (buttonName == 'A')
        {
            //Change parameter of FMOD Instance Drum Sample Type
            //Start instance
        }
        
        
    }

    // Called when a new compose round starts and will wait until the song ends before doing its functionality  
    private IEnumerator ComposeRoundEnd()
    {
        const float songLength = 16f;
        timer = songLength; 
        yield return new WaitForSeconds(songLength); 
        
        Debug.Log("Compose round ended"); 
        
        StartNewReenactRound();
        
        // more awesome stuff 
    }
    
    // Called when a new compose round starts 
    private IEnumerator ReenactRoundEnd()
    {
        const float songLength = 16f;
        timer = songLength; 
        yield return new WaitForSeconds(songLength); 
        
        Debug.Log("Reenact round ended"); 
        // more awesome stuff 
        
        StartNewReenactRound();
    }
    
}
