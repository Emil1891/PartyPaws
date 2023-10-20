using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; 

public class PlayerMenuManager : MonoBehaviour
{
    [SerializeField] private InputAction startGameInputAction; 
    
    [Serializable]
    public struct PlayerInfo 
    {
        public GameObject screen;
        public string name;
        public Sprite sprite; 
    }

    [SerializeField] private PlayerInfo[] playerInfo; 
    
    private static int playerCount = 0; 

    // Function called when a new player is added 
    public void OnPlayerJoined()
    {
        PlayerInfo newInfo = playerInfo[playerCount]; 

        newInfo.screen.GetComponentInChildren<Image>().sprite = newInfo.sprite; 
        newInfo.screen.GetComponentInChildren<TextMeshProUGUI>().SetText(newInfo.name); 

        playerCount++; 
    }
}
