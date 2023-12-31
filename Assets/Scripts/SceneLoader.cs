using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public Animator transition;
    public float transitionTime = 2f;

    private void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI Slide"); 
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName));
    }

    IEnumerator TransitionToScene(string sceneName)
    {
        //Start transition animation
        transition.SetTrigger("Start");
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI Slide");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }

}
