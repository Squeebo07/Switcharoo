using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public Text stateText;
    public string stateWords1;
    public string stateWords2;
    bool fail = false;
    bool fall = false;
    bool succeed = false;

    public GameObject canvas;
    public GameObject warning;
    public GameObject fallOff;

    public void Start()
    {
        canvas.gameObject.SetActive(false);
    }

    public void UpdateHUD()
    {
        // Tell Game which state the game is in
        if (fail == true)
        {
            // flash exclamation points at player then have go to game over scene
            canvas.gameObject.SetActive(true);
            stateText.text = stateWords1.ToString();
        }
        if (succeed == true)
        {
            // Show player the win screen
            canvas.gameObject.SetActive(true);
            stateText.text = stateWords2.ToString();
        }
        if (fall == true)
        {
            canvas.gameObject.SetActive(true);
            stateText.text = stateWords1.ToString();

        }
    }

    public void WinGame()
    {
        succeed = true;
        UpdateHUD();
    }

    public void EnemyPlayerDetected()
    {
        fail = true;
        StartFlashing();
    }

    public void StartFlashing()
    {
        StopAllCoroutines();
        StartCoroutine("GameOver");
    }

    void StopBlinking()
    {
        StopAllCoroutines();
    }

    public IEnumerator GameOver()
    {
        while (true)
        {
            GameObject detected = Instantiate(warning, new Vector3(0, 0, 0), Quaternion.identity);
            Destroy(detected, 0.5f);
            yield return new WaitForSeconds(1f);
            UpdateHUD();
            break;
        }


    }

    public void FallOff()
    {
        fall = true;
        UpdateHUD();
    }


    public void GameLevel ()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
