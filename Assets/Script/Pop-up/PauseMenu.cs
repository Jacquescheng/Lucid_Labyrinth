using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameManager.isPaused && pauseMenu.activeSelf) {
                ResumeGame();
            } else if (!GameManager.isPaused){
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
        audioSource.Play();
        pauseMenu.SetActive(false);
        GameManager.isPaused = false;
    }

    public void Exit()
    {
        audioSource.Play();
        Invoke("DelayedExit", 0.2f);
    }
    private void DelayedExit() 
    {
        SceneManager.LoadScene("TitleScene");
    }
}
