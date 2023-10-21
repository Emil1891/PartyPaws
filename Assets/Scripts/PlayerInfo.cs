using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInfo : MonoBehaviour
{
    
    [SerializeField] private Sprite[] sprites; 
    [SerializeField] private string[] names;
    
    [HideInInspector] public string playerName;

    [HideInInspector] public Sprite sprite;

    public int points;

    private void Start()
    {
        uint playerIndex = GetComponent<PlayerInput>().user.id - 1; 
        sprite = sprites[playerIndex]; 
        playerName = names[playerIndex]; 
    }
    
}
