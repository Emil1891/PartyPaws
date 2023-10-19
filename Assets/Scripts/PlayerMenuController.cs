using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMenuController : MonoBehaviour
{
    [SerializeField] private int sceneToLoad = 1; 

    private void OnEnable()
    {   
        Debug.Log("Enabled");
    }

}
