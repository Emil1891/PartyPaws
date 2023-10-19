using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerMenuManager : MonoBehaviour
{
    private static int playerCount = 0; 
    
    [SerializeField] private GameObject[] playerScreens; 
    
    [SerializeField] private string[] playerNames; 

    [SerializeField] private Sprite[] sprites; 

    // Function called when a new player is added 
    public void OnPlayerJoined()
    {
        GameObject newPlayerScreen = playerScreens[playerCount]; 
        string newPlayerText = playerNames[playerCount]; 

        newPlayerScreen.GetComponentInChildren<Image>().sprite = sprites[playerCount]; 
        newPlayerScreen.GetComponentInChildren<TextMeshProUGUI>().SetText(newPlayerText); 

        playerCount++; 
    }
}
