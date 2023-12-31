using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WinScreenManager : MonoBehaviour
{

    [SerializeField] private Animator[] animators; 
    private FMOD.Studio.EventInstance RoundOver;

    [SerializeField] private TextMeshProUGUI[] pointsText;

    [SerializeField] private TextMeshProUGUI winText; 
    
    [SerializeField] private TextMeshProUGUI restartText; 

    private GameObject[] players; 

    // Start is called before the first frame update
    void Start()
    {

        RoundOver = FMODUnity.RuntimeManager.CreateInstance("event:/UI/RoundOver");
        players = GameObject.FindGameObjectsWithTag("Player");

        var unsortedPlayers = GameObject.FindGameObjectsWithTag("Player"); 

        Array.Sort(players, (p1, p2) => p1.GetComponent<PlayerInfo>().points - p2.GetComponent<PlayerInfo>().points);
        Array.Reverse(players); // amazing sort 

        for(int i = 0; i < players.Length; i++)
        {
            animators[i].SetTrigger(players[i].GetComponent<PlayerInfo>().playerName); 
            
            players[i].GetComponent<PlayerInputManager>().SwitchActionMapping(PlayerInputManager.EActionMapping.Watcher); 
            
            pointsText[i].SetText($"{unsortedPlayers[i].GetComponent<PlayerInfo>().points}"); 
        }

        for(int i = players.Length; i < animators.Length; i++) 
        {
            animators[i].gameObject.SetActive(false); 
            pointsText[i].gameObject.transform.parent.gameObject.SetActive(false); 
        }

        if (players[0].GetComponent<PlayerInfo>().points == players[1].GetComponent<PlayerInfo>().points)
        {
            RoundOver.setParameterByName("animalType", 4);
            RoundOver.start();
            winText.SetText("MULTIPLE WINNERS!"); 
        }
        else
        {
            RoundOver.setParameterByName("animalType", players[0].GetComponent<PlayerInfo>().playerIndex); 
            RoundOver.start();
            winText.SetText($"{players[0].GetComponent<PlayerInfo>().playerName.ToUpper()} WINS!"); 
        }

    }

    private bool destroyedPlayers = false; 
    private void Update()
    {

        if (Time.timeSinceLevelLoad > 5.0f)
            restartText.gameObject.SetActive(true); 

        if (Time.timeSinceLevelLoad > 5.0f && (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.JoystickButton2)) && !destroyedPlayers)
        {
            RoundOver.stop(STOP_MODE.ALLOWFADEOUT);

            foreach (var player in players)
            {
                Destroy(player); 
            }

            destroyedPlayers = true;

            PlayerInfo.playerCounter = 0; 
            
            FindObjectOfType<SceneLoader>().LoadScene("PlayerJoinScene");
        }
        
    }
}
