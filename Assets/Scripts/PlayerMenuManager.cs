using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

/**
 * Handles join screen behaviour 
 */

public class PlayerMenuManager : MonoBehaviour
{    
    
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
        
        // TODO: Play animal related sound here? 
    }
    
}
