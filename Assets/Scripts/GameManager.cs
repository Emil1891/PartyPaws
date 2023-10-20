using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))] 
public class GameManager : MonoBehaviour
{
    
    private GameObject[] players;

    private GameObject currentPlayer;

    private AudioSource audioPlayer; 
    
    // private Track currentTrack; // something like this 

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        players = GameObject.FindGameObjectsWithTag("Player");

        // Get the starting player 
        foreach (var player in players)
        {
            if (player.GetComponent<PlayerInputManager>().GetCurrentActionMapping()
                .Equals(PlayerInputManager.EActionMapping.CurrentPlayer))
            {
                currentPlayer = player;
                break; 
            }
        }

        audioPlayer = GetComponent<AudioSource>(); 
    }

    public void ButtonPressed(char buttonName)
    {
        Debug.Log($"Pressed {buttonName}"); 
        
        // audioPlayer.PlayOneShot(); 
        
        // if "recording/composing" has started 
        // Create new note 
        // add to track 
    }
    
}
