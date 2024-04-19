using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameManager.isPaused && pauseMenu.activeSelf) {
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        GameManager.isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        GameManager.isPaused = false;
    }

    public void Exit()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
