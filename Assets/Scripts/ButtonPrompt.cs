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
        
        initialSpawnLoc = GetComponent<RectTransform>().anchoredPosition; 
        
        switch (gameState)
        {
            case GameManager.GameState.Composing:
                targetLoc = rectTransform.anchoredPosition + Vector2.up * moveDistance;
                break;
            
            case GameManager.GameState.Reenacting:
                targetLoc = rectTransform.anchoredPosition; 
                rectTransform.anchoredPosition += Vector2.up * moveDistance;
                movingUp = false; 
                break;

            case GameManager.GameState.Transition:
                break;
            
            default:
                Debug.Log("Should never end up here");
                break; 
        }

    }

    // Update is called once per frame
    void Update()
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
        float distanceMovedSoFar = rectTransform.anchoredPosition.y - initialSpawnLoc.y;

        float totalDistanceToMove = Mathf.Abs(targetLoc.y - initialSpawnLoc.y) - 100; // -100 because of 

        // we want the percentage reversed since moving far equals low opacity 
        return 1 - distanceMovedSoFar / totalDistanceToMove; 

    }
    
}
