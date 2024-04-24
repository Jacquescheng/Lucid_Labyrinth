using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DeadScreen : MonoBehaviour
{
    public GameObject deadScreen;
    public TMP_Text cause;
    private  AudioSource[] audioSources;
    // Start is called before the first frame update
    void Start()
    {
        deadScreen.SetActive(false);
        audioSources = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Create()
    {
        cause.text = $"<color=#93278F>Beware of {GameManager.killedBy}</color>";
        GameManager.isPaused = true;
        deadScreen.SetActive(true);
    }

    public void Undo()
    {
        audioSources[0].Play();
        Invoke("DelayedUndo", 0.2f);
    }
    private void DelayedUndo()
    {
        GameManager.isDead = false;
        deadScreen.SetActive(false);
        GameManager.Instance.Undo();
        GameManager.isPaused = false;
    }

    public void Restart()
    {
        audioSources[0].Play();
        Invoke("DelayedRestart", 0.2f);
    }
    private void DelayedRestart()
    {
        SceneManager.LoadScene("GameScene");
    }

}
