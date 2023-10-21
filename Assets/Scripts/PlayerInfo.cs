using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInfo : MonoBehaviour
{
    
    [SerializeField] private Sprite[] sprites; 
    [SerializeField] private string[] names;

    public int playerIndex = 0; 
    
    public static int playerCounter = 0; 
    
    [HideInInspector] public string playerName;

    [HideInInspector] public Sprite sprite;

    public int points;

    private void Start()
    {
        playerIndex = playerCounter++;
        
        sprite = sprites[playerIndex]; 
        playerName = names[playerIndex];
    }
    
}
