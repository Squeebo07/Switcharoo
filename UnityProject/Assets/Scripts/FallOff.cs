using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOff : MonoBehaviour {
    public GameManager getScript;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "NoGround")
            getScript.FallOff();
    }
}
