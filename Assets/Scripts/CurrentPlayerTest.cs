using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPlayerTest : MonoBehaviour
{

    public Animator currentPlayerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            currentPlayerAnimator.SetTrigger("PolarBear");
        }
        if (Input.GetKeyDown("d"))
        {
            currentPlayerAnimator.SetTrigger("Dolphin");
        }
        if (Input.GetKeyDown("w"))
        {
            currentPlayerAnimator.SetTrigger("Worm");
        }
        if (Input.GetKeyDown("g"))
        {
            currentPlayerAnimator.SetTrigger("Goose");
        }
    }
}
