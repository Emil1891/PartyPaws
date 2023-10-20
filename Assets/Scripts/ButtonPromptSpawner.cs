using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPromptSpawner : MonoBehaviour
{
    
    [Serializable]
    public struct SpawnLocations
    {
        public char buttonName;
        public RectTransform SpawnTransform; 
    }

    [SerializeField] private SpawnLocations[] spawnLocations;

    [SerializeField] private GameObject btnPrompt;
    
    [SerializeField] private GameObject canvas;

    public void SpawnNewPrompt(char buttonName, GameManager.GameState gameState)
    {
        foreach (var btnSpawnLoc in spawnLocations)
        {
            if (btnSpawnLoc.buttonName.Equals(buttonName))
            {
                var prompt = Instantiate(btnPrompt, btnSpawnLoc.SpawnTransform.anchoredPosition, Quaternion.identity);
                prompt.transform.SetParent(canvas.transform);
                prompt.GetComponent<ButtonPrompt>().SetUpPrompt(buttonName, gameState); 
            }
        }
    }
}
