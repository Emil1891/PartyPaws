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

    private FMOD.Studio.EventInstance NarratorSound;
    private FMOD.Studio.EventInstance Music;

    private GameObject[] players; 

    private GameObject currentPlayer; 

    private int composerPlayerIndex = 0; 

    private int playersReenactedThisRound = 0;

    [SerializeField] private TextMeshProUGUI timerText;

    private float timer = 0; 

    private Track currentTrack = new Track();

    private ButtonPromptSpawner btnPromptSpawner; 
    
    private FMOD.Studio.EventInstance DrumKit;

    private float songLength = 4.8f; 
    
    private const float countdownLength = 4.8f; 

    private void Start()
    {
        NarratorSound = FMODUnity.RuntimeManager.CreateInstance("event:/NarratorLines");

        //TODO: Kalla på random val av låt
        Music = FMODUnity.RuntimeManager.CreateInstance("event:/Music2");
        //Ladda Drumkittet som passar ihop med låten
        DrumKit = FMODUnity.RuntimeManager.CreateInstance("event:/Drum Hit 2");

        // DontDestroyOnLoad(gameObject);
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in players)
            player.GetComponent<PlayerGameController>().SetGameManager(this);

        // Sort players in the joined order / or random 
        // Array.Sort(players, (p1, p2) => p1.GetComponent<PlayerInput>().user.index - p2.GetComponent<PlayerInput>().user.index);
        // Array.Sort(players, (p1, p2) => Random.Range(-10, 10)); 

        btnPromptSpawner = FindObjectOfType<ButtonPromptSpawner>();


        currentPlayer = players[0]; 

        StartCoroutine(StartNewComposeRound());
        Music.start();
    }

    private void Update()
    {
        timer += Time.deltaTime; 
        
        if(currentGameState.Equals(GameState.Transition))
            timerText.SetText("Transitioning");
        else
            timerText.SetText($"Time elapsed: {(timer - countdownLength):0.00}"); 

        if (currentGameState.Equals(GameState.Reenacting) || currentGameState.Equals(GameState.Transition))
        {
            Note noteToSpawn = currentTrack.GetNoteToSpawn(timer); 
            if (noteToSpawn != null)
            {
                btnPromptSpawner.SpawnNewPrompt(noteToSpawn.GetButtonName(), GameState.Reenacting); 
            }
            
            if (currentGameState.Equals(GameState.Reenacting) && currentTrack.PlayerMissedNote(timer))
            {
                Debug.Log("failed from update");
                ReenactFailed(); 
            }
        } 

    }

    private IEnumerator StartNewComposeRound()
    {
        Debug.Log($"Composer index: {composerPlayerIndex}, players length: {players.Length}");
        
        if (composerPlayerIndex >= players.Length)
        {
            Music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            FullRoundEnd();
            yield break; 
        }
        
        Music.setVolume(1.0f);
        Music.setTimelinePosition(0);
        NarratorSound.setParameterByName("animalType", composerPlayerIndex);
        NarratorSound.start();

        currentGameState = GameState.Transition; 

        yield return new WaitForSeconds(countdownLength); 
        
        Debug.Log("New round started");
        
        Debug.Log($"player count: {players.Length}");

        currentPlayer = players[composerPlayerIndex]; 

        players[composerPlayerIndex].GetComponent<PlayerInputManager>().SwitchActionMapping(PlayerInputManager.EActionMapping.CurrentPlayer); 

        currentGameState = GameState.Composing; 
        
        currentTrack.NewCompRoundStarted(); 
        
        composerPlayerIndex++;
        playersReenactedThisRound = 0; 

        // Start coroutine that will run after song ends 
        StartCoroutine(ComposeRoundEnd());
    }

    private void FullRoundEnd()
    {
        Debug.Log("Game over"); 

        // change input to watcher 
        currentPlayer.GetComponent<PlayerInputManager>().SwitchActionMapping(PlayerInputManager.EActionMapping.Watcher); 
        
        // TODO: Load end scene that shows points/podium 
        FindObjectOfType<SceneLoader>().LoadScene("ResultsScreen");
        
    }

    private IEnumerator StartNewReenactRound()
    {
        if (playersReenactedThisRound >= players.Length)
        {
            StartCoroutine(StartNewComposeRound());
            yield break;
        }

        Music.setVolume(1.0f);
        Music.setTimelinePosition(0);

        currentGameState = GameState.Transition;

        yield return new WaitForSeconds(countdownLength); 
        
        Debug.Log($"Started new reenact round");

        currentGameState = GameState.Reenacting;
        
        currentTrack.NewReenactStarted(); 

        int reenactIndex = composerPlayerIndex + playersReenactedThisRound;

        if (reenactIndex >= players.Length)
            reenactIndex -= players.Length; 
        
        currentPlayer = players[reenactIndex]; 
        
        currentPlayer.GetComponent<PlayerInputManager>().SwitchActionMapping(PlayerInputManager.EActionMapping.CurrentPlayer);

        // Start coroutine that will run after song ends 
        StartCoroutine(ReenactRoundEnd()); 
    }

    private void ReenactFailed()
    {
        Music.setVolume(0.0f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/FailSound");
        
        StopAllCoroutines(); 

        PrepareForNewReenactRound(); 
        
        Debug.Log("Failed"); 
    }
    
    public void ButtonPressed(char buttonName)
    {
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
        currentTrack.composerNotes.Add(new Note(timer, buttonName)); 
        
        btnPromptSpawner.SpawnNewPrompt(buttonName, currentGameState); 
    }

    private void ReenactedNewInput(char buttonName)
    {
        // reenact stuff 
        Debug.Log($"Reenacting {buttonName}");
                
        PlayDrumSound(buttonName); 

        // this returns true if there are no more notes as well right now 
        if (currentTrack.PlayedCorrectNote(buttonName, timer))
        {
            // success 
            Debug.Log($"success hit");
        }
        else
        {
            // failed 
            Debug.Log($"Failed. new input");
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
        // const float songLength = 16f; // TODO: GET THE LENGTH DYNAMICALLY 
        yield return new WaitForSeconds(songLength); 
        
        Debug.Log("Compose round ended");

        currentPlayer.GetComponent<PlayerInputManager>().SwitchActionMapping(PlayerInputManager.EActionMapping.Watcher);

        timer = 0; 

        StartCoroutine(StartNewReenactRound());
    }
    
    // Called when a new compose round starts 
    private IEnumerator ReenactRoundEnd()
    {
        // const float songLength = 16f; // TODO: GET THE LENGTH DYNAMICALLY 
        yield return new WaitForSeconds(songLength);

        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/WinSound");

        PrepareForNewReenactRound(); 
    }

    private void PrepareForNewReenactRound()
    {
        Debug.Log("Reenact round ended");

        currentGameState = GameState.Transition; 
        
        currentPlayer.GetComponent<PlayerInputManager>().SwitchActionMapping(PlayerInputManager.EActionMapping.Watcher); 
        
        playersReenactedThisRound++;

        timer = 0; 

        StartCoroutine(StartNewReenactRound());
    }

}
