using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinState : MonoBehaviour {

    public GameManager getScript;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "WinState")
            getScript.WinGame();
    }
}
