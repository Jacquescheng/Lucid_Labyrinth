using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionPage : MonoBehaviour
{
    public GameObject instructionPage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (instructionPage.activeSelf) {
                ResumeGame();
            }
        }
    }

    public void Create() {
        instructionPage.SetActive(true);
        GameManager.isPaused = true;
    }

    public void ResumeGame()
    {
        instructionPage.SetActive(false);
        GameManager.isPaused = false;
        gameObject.GetComponent<OverlayUI>().Create();
    }
}
