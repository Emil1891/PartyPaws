using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    
    public enum GameState {
        Composing = 0, Reenacting, Transition 
    }

    public GameState currentGameState = GameState.Transition; 
    
    private GameObject[] players; 

    private GameObject currentPlayer; 

    private int composerPlayerIndex = 0;

    private GameObject[] playersPlayedThisRound = new GameObject[4];

    [SerializeField] private TextMeshProUGUI timerText;

    private float timer = 0; 

    private Track currentTrack = new Track();

    private ButtonPromptSpawner btnPromptSpawner; 
    
    private FMOD.Studio.EventInstance DrumKit;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        players = GameObject.FindGameObjectsWithTag("Player");

        // Sort players in the joined order 
        Array.Sort(players, (p1, p2) => p1.GetComponent<PlayerInput>().user.index - p2.GetComponent<PlayerInput>().user.index);

        SetStartingPlayer(); 

        btnPromptSpawner = FindObjectOfType<ButtonPromptSpawner>(); 
        
        DrumKit = FMODUnity.RuntimeManager.CreateInstance("event:/Drum Hit 1");

        StartNewComposeRound(); 
    }

    private void Update()
    {
        if(currentGameState.Equals(GameState.Transition))
        {
            timerText.SetText("");
            return; 
        }
        
        timer += Time.deltaTime; 
        timerText.SetText($"Time elapsed: {timer}");

        if (currentGameState.Equals(GameState.Reenacting)) 
        {
            if (currentTrack.PlayerMissedNote(timer))
            {
                ReenactFailed(); 
            }
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
        
        currentTrack.listOfComposerNotes.Clear(); 
        timer = 0; 

        // Start coroutine that will run after song ends 
        StartCoroutine(ComposeRoundEnd()); 
    }

    private void StartNewReenactRound()
    {
        Debug.Log($"Started new reenact round");

        currentGameState = GameState.Reenacting;
        timer = 0; 
        currentTrack.NewReenactStarted();
        
        // Start coroutine that will run after song ends 
        StartCoroutine(ReenactRoundEnd()); 
    }

    private void ReenactFailed()
    {
        StartNewReenactRound();

        Debug.Log("Failed");
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
                ComposedNewInput(buttonName);
                break; 
            
            case GameState.Reenacting:
                ReenactedNewInput(buttonName);
                break; 
            
            default: 
                Debug.LogWarning("Non existing game state, something is prob wrong!");
                break;
        }
    }

    private void ComposedNewInput(char buttonName)
    {
        // composing stuff 
        Debug.Log($"Composing {buttonName}");
        
        PlayDrumSound(buttonName); 
        // Create new note and add to track 
        currentTrack.listOfComposerNotes.Add(new Note(timer, buttonName)); 
        
        btnPromptSpawner.SpawnNewPrompt(buttonName, currentGameState); 
    }

    private void ReenactedNewInput(char buttonName)
    {
        // reenact stuff 
        Debug.Log($"Reenacting {buttonName}");
                
        PlayDrumSound(buttonName);

        // get next button in the track 
        // var nextNote = currentTrack.GetNextNote(); 

        if (currentTrack.PlayedCorrectNote(buttonName, timer))
        {
            // success 
        }
        else
        {
            // failed 
            ReenactFailed(); 
        }
    }

    private void PlayDrumSound(char buttonName)
    {
        switch(buttonName){
            case 'A':
                DrumKit.setParameterByName("hitType", 0);
                DrumKit.start();
            break;

            case 'X':
                DrumKit.setParameterByName("hitType", 1);
                DrumKit.start();
            break;

            case 'Y':
                DrumKit.setParameterByName("hitType", 2);
                DrumKit.start();
            break;

            case 'B':
                DrumKit.setParameterByName("hitType", 3);
                DrumKit.start();
            break;

        }
    }

    // Called when a new compose round starts and will wait until the song ends before doing its functionality  
    private IEnumerator ComposeRoundEnd()
    {
        const float songLength = 16f;
        yield return new WaitForSeconds(songLength); 
        
        Debug.Log("Compose round ended"); 
        
        StartNewReenactRound();
        
        // more awesome stuff 
    }
    
    // Called when a new compose round starts 
    private IEnumerator ReenactRoundEnd()
    {
        const float songLength = 16f;
        yield return new WaitForSeconds(songLength); 
        
        Debug.Log("Reenact round ended"); 
        // more awesome stuff 
        
        StartNewReenactRound();
    }
    
}
