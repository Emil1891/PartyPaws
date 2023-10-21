using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonPrompt : MonoBehaviour
{
    [Serializable]
    public struct ButtonSprites
    {
        public Sprite image;
        public char buttonName; 
    }
    
    [SerializeField] private ButtonSprites[] buttonSprites; 
    
    // How far the prompt should move 
    [SerializeField] private float moveDistance = 500f; 
    
    [SerializeField] private float moveSpeed = 3f;

    private Vector2 targetLoc; 

    private char buttonName;

    private Image image;

    private Vector2 initialSpawnLoc;

    private RectTransform rectTransform;

    private GameManager.GameState gameState;

    private bool movingUp = true; 
    
    public void SetUpPrompt(char buttonName, GameManager.GameState gameState)
    {
        this.buttonName = buttonName;
        this.gameState = gameState; 
        
        StartSetUp();
    }
    
    // Start is called before the first frame update
    private void StartSetUp()
    {
        image = GetComponent<Image>();

        // Set corresponding image 
        foreach (var btnSprite in buttonSprites)
        {
            if (btnSprite.buttonName.Equals(buttonName))
            {
                image.sprite = btnSprite.image;
                break; 
            }
        }

        rectTransform = GetComponent<RectTransform>(); 

        rectTransform.anchoredPosition = transform.position; 
        
        initialSpawnLoc = rectTransform.anchoredPosition; 
        
        switch (gameState)
        {
            case GameManager.GameState.Composing:
                targetLoc = rectTransform.anchoredPosition + Vector2.up * moveDistance;
                break;
            
            case GameManager.GameState.Transition:
            case GameManager.GameState.Reenacting:
                targetLoc = rectTransform.anchoredPosition; 
                rectTransform.anchoredPosition += Vector2.up * moveDistance;
                movingUp = false; 
                initialSpawnLoc = rectTransform.anchoredPosition; 
                break;
            
            default:
                Debug.LogWarning("Should never end up here");
                break; 
        }

        Debug.LogWarning($"Spawned new btn");
    }

    // Update is called once per frame
    private void Update()
    {
        float moveAmount = moveSpeed * Time.deltaTime;

        Vector2 MoveDir = (targetLoc - initialSpawnLoc).normalized; 
        
        rectTransform.anchoredPosition += MoveDir * moveAmount;

        float newAlpha = GetNewAlpha(); 

        // Get the alpha based on distance moved towards target location 
        Color newColor = image.color; 
        newColor.a = newAlpha;
        
        image.color = newColor; 
        
        if (movingUp && rectTransform.anchoredPosition.y - targetLoc.y > 0)
            Destroy(gameObject); 
        else if(!movingUp && rectTransform.anchoredPosition.y - targetLoc.y < 0)
            Destroy(gameObject);
    }

    private float GetNewAlpha()
    {
        // alpha broken when moving down TODO: Fix if we want the prompt to fade in when reenacting 
        if (!movingUp)
            return 1; 
        
        float distanceMovedSoFar = rectTransform.anchoredPosition.y - initialSpawnLoc.y;

        float totalDistanceToMove = Mathf.Abs(movingUp ? targetLoc.y - initialSpawnLoc.y : initialSpawnLoc.y - targetLoc.y); 

        // we want the percentage reversed since moving far equals low opacity when moving up 
        if (movingUp)
            return 1 - distanceMovedSoFar / totalDistanceToMove;
        
        return distanceMovedSoFar / totalDistanceToMove; // if moving down, we want to fade in 

    }
    
}
