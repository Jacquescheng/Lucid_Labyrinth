using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public GameObject endScreen;
    private  AudioSource[] audioSources;
    private  AudioSource[] allAudioSources;

    // Start is called before the first frame update
    void Start()
    {
        endScreen.SetActive(false);
        audioSources = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Create()
    {
        allAudioSources = GameObject.FindObjectsOfType<AudioSource>();
        GameManager.isPaused = true;
        endScreen.SetActive(true);
        foreach (AudioSource audio in allAudioSources){
            audio.Stop();
        }
        audioSources[1].Play();
    }

    public void Exit()
    {
        audioSources[0].Play();
        Invoke("DelayedExit", 0.2f);
    }
    private void DelayedExit()
    {
        SceneManager.LoadScene("TitleScene");
    }

}
