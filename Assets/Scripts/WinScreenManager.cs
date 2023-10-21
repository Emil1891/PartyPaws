using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WinScreenManager : MonoBehaviour
{

    [SerializeField] private Animator[] animators; 
    private FMOD.Studio.EventInstance RoundOver;


    // Start is called before the first frame update
    void Start()
    {

        RoundOver = FMODUnity.RuntimeManager.CreateInstance("event:/UI/RoundOver");
        var players = GameObject.FindGameObjectsWithTag("Player");

        Array.Sort(players, (p1, p2) => p1.GetComponent<PlayerInfo>().points - p2.GetComponent<PlayerInfo>().points);
        Array.Reverse(players); // amazing sort 

        RoundOver.setParameterByName("animalType", players[0].GetComponent<PlayerInput>().user.id - 1);
        RoundOver.start();

    for(int i = 0; i < players.Length; i++)
        {
            animators[i].SetTrigger(players[i].GetComponent<PlayerInfo>().playerName); 
            
            players[i].GetComponent<PlayerInputManager>().SwitchActionMapping(PlayerInputManager.EActionMapping.MenuMapping); 
        }

        for(int i = players.Length; i < animators.Length; i++) 
        {
            animators[i].gameObject.SetActive(false); 
        }

    }

}
