using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public GameObject endScreen;
    // Start is called before the first frame update
    void Start()
    {
        endScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Create()
    {
        GameManager.isPaused = true;
        endScreen.SetActive(true);
    }

    public void Exit()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
