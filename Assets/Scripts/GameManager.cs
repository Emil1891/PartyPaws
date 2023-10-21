using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    public enum GameState {
        Composing = 0, Reenacting, Transition, FailWait 
    }

    private GameState currentGameState = GameState.Transition; 

    private FMOD.Studio.EventInstance NarratorSound;
    private FMOD.Studio.EventInstance NarratorCallOut;
    private FMOD.Studio.EventInstance Music;
    private FMOD.Studio.EventInstance DrumKit;

    private GameObject[] players; 

    private GameObject currentPlayer; 

    private int composerPlayerIndex = 0; 

    private int playersReenactedThisRound = 0;

    private float timer = 0; 

    private Track currentTrack = new Track();

    private ButtonPromptSpawner btnPromptSpawner; 
    
    private float songLength = 4.8f; 
    private float countdownLength = 4.8f; 

    [SerializeField] private Slider timerSlider; 
    
    [SerializeField] private Image playerImage; 

    [SerializeField] private TextMeshProUGUI[] pointsText; 
    
    [SerializeField] private TextMeshProUGUI currentPlayerText; 

    //150 BPM = 6.4
    //130 BPM = 7.385
    
    private float composeCoolDownTimer = 0; 
    
    [SerializeField] private float composeTimeDelay = 0.1f; 

    private void Start()
    {
        NarratorSound = FMODUnity.RuntimeManager.CreateInstance("event:/NarratorLines");
        NarratorCallOut = FMODUnity.RuntimeManager.CreateInstance("event:/NarratorCalloutAnimal");


        int songIndex = UnityEngine.Random.Range(1, 8);

        Music = FMODUnity.RuntimeManager.CreateInstance("event:/Music" + songIndex);

        //Ladda Drumkittet som passar ihop med l�ten
        DrumKit = FMODUnity.RuntimeManager.CreateInstance("event:/Drum Hit " + songIndex);


        if (songIndex == 1 || songIndex == 2 || songIndex == 7)
        {
            songLength = 4.8f;
            countdownLength = 4.8f;
        }
        else if(songIndex == 3 || songIndex == 4 || songIndex == 5)
        {
            songLength = 6.4f;
            countdownLength = 6.4f;
        }

        else if(songIndex == 6)
        {
            songLength = 7.385f;
            countdownLength = 7.385f;
        }

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

        // disable not used players 
        for (int i = players.Length; i < pointsText.Length; i++)
        {
            pointsText[i].gameObject.transform.parent.gameObject.SetActive(false); 
        }
        
    }

    private void Update()
    {
        if (currentGameState.Equals(GameState.FailWait))
        {
            timerSlider.value = 0; 
            return;
        }

        Music.getVolume(out var vol);

        if (vol == 0)
            Music.setVolume(1); 

        timer += Time.deltaTime;
        composeCoolDownTimer += Time.deltaTime;

        if(currentGameState.Equals(GameState.Reenacting) || currentGameState.Equals(GameState.Composing))
            timerSlider.value =  1 - ((timer - countdownLength) / songLength); 
        else if (currentGameState.Equals(GameState.Transition))
        {
            var increasePerSec = 1 / countdownLength; 
            timerSlider.value = Mathf.Clamp(timerSlider.value + increasePerSec * Time.deltaTime, 0, 1); 
        } 

        if (currentGameState.Equals(GameState.Reenacting) || currentGameState.Equals(GameState.Transition))
        {
            Note noteToSpawn = currentTrack.GetNoteToSpawn(timer); 
            
            if (noteToSpawn != null)
                btnPromptSpawner.SpawnNewPrompt(noteToSpawn.GetButtonName(), currentGameState);
            
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
            StartCoroutine(FullRoundEnd());
            yield break; 
        }

        Music.start();
        Music.setTimelinePosition(0);
        // Music.setVolume(1.0f);
        // StartCoroutine(EnableMusicVolume()); 
        Music.setVolume(1); 
        
        NarratorSound.setParameterByName("animalType", composerPlayerIndex);
        NarratorSound.start();

        currentGameState = GameState.Transition;

        timer = 0; 
        currentTrack.NewCompRoundStarted();

        playerImage.sprite = players[composerPlayerIndex].GetComponent<PlayerInfo>().sprite; 
        
        currentPlayerText.SetText("COMPOSING"); 
        
        yield return new WaitForSeconds(countdownLength); 
        
        Debug.Log("New round started");
        
        Debug.Log($"player count: {players.Length}");

        currentPlayer = players[composerPlayerIndex]; 
        
        players[composerPlayerIndex].GetComponent<PlayerInputManager>().SwitchActionMapping(PlayerInputManager.EActionMapping.CurrentPlayer); 

        currentGameState = GameState.Composing;

        composerPlayerIndex++;
        playersReenactedThisRound = 0;

        // Start coroutine that will run after song ends 
        StartCoroutine(ComposeRoundEnd());
    }

    private IEnumerator FullRoundEnd()
    {
        Debug.Log("Game over");

        currentGameState = GameState.FailWait; // not fail but serves the same purpose 
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/NarratorGameEnd");
        
        // change input to watcher 
        currentPlayer.GetComponent<PlayerInputManager>().SwitchActionMapping(PlayerInputManager.EActionMapping.Watcher);

        yield return new WaitForSeconds(12.0f); 
        
        FindObjectOfType<SceneLoader>().LoadScene("ResultsScreen"); 
    }

    private IEnumerator StartNewReenactRound()
    {
        if (playersReenactedThisRound >= players.Length)
        {
            StartCoroutine(StartNewComposeRound());
            yield break;
        }
        
        Music.start();
        Music.setTimelinePosition(0);

        // Music.setVolume(1.0f);
        // StartCoroutine(EnableMusicVolume()); 
        Music.setVolume(1); 
        
        currentGameState = GameState.Transition;
        
        timer = 0; 

        currentTrack.NewReenactStarted();

        int reenactIndex = composerPlayerIndex + playersReenactedThisRound;

        if (reenactIndex >= players.Length)
            reenactIndex -= players.Length;

        playerImage.sprite = players[reenactIndex].GetComponent<PlayerInfo>().sprite; 
        currentPlayerText.SetText("REPEATING"); 

        NarratorCallOut.setParameterByName("animalType", reenactIndex);
        NarratorCallOut.start();

        yield return new WaitForSeconds(countdownLength); 
        
        Debug.Log($"Started new reenact round");

        currentGameState = GameState.Reenacting;
        
        currentPlayer = players[reenactIndex]; 

        currentPlayer.GetComponent<PlayerInputManager>().SwitchActionMapping(PlayerInputManager.EActionMapping.CurrentPlayer);

        // Start coroutine that will run after song ends 
        StartCoroutine(ReenactRoundEnd()); 
    }

    private IEnumerator EnableMusicVolume()
    {
        yield return new WaitForSeconds(0.2f);

        Music.setVolume(1); 
    }

    private void ReenactFailed()
    {
        Music.setVolume(0.0f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/FailSound");
        
        StopAllCoroutines(); 

        StartCoroutine(PrepareForNewReenactRound(true)); 
        
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
        if (composeCoolDownTimer < composeTimeDelay)
        {
            Debug.Log($"Pressed too quickly, time since last press: {composeCoolDownTimer}");
            return;
        }

        composeCoolDownTimer = 0; 

        // composing stuff 
        // Debug.Log($"Composing {buttonName}");
        
        PlayDrumSound(buttonName); 
        
        // Create new note and add to track 
        currentTrack.PlayerComposedNewNote(new Note(timer, buttonName)); 
        
        btnPromptSpawner.SpawnNewPrompt(buttonName, currentGameState); 
    }

    private void ReenactedNewInput(char buttonName)
    {
        // reenact stuff 
        // Debug.Log($"Reenacting {buttonName}");
                
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

        StartCoroutine(StartNewReenactRound());
    }
    
    private IEnumerator ReenactRoundEnd()
    {
        // const float songLength = 16f; // TODO: GET THE LENGTH DYNAMICALLY 
        yield return new WaitForSeconds(songLength);

        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/WinSound");

        currentPlayer.GetComponent<PlayerInfo>().points++;
        pointsText[currentPlayer.GetComponent<PlayerInput>().user.id - 1].text =
            $"{currentPlayer.GetComponent<PlayerInfo>().points}"; 

        StartCoroutine(PrepareForNewReenactRound(false)); 
    }

    private IEnumerator PrepareForNewReenactRound(bool failed)
    {
        Debug.Log("Reenact round ended");

        currentGameState = GameState.Transition; 
        
        currentPlayer.GetComponent<PlayerInputManager>().SwitchActionMapping(PlayerInputManager.EActionMapping.Watcher); 
        
        playersReenactedThisRound++;

        if (failed)
        {
            Music.setVolume(0.0f);
            currentGameState = GameState.FailWait; 
            yield return new WaitForSeconds(3.5f);
        }

        StartCoroutine(StartNewReenactRound());
    }

}
