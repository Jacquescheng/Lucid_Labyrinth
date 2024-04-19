using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class DeadScreen : MonoBehaviour
{
    public GameObject deadScreen;
    public TMP_Text cause;
    // Start is called before the first frame update
    void Start()
    {
        deadScreen.SetActive(false);
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
        GameManager.isDead = false;
        deadScreen.SetActive(false);
        GameManager.Instance.Undo();
        GameManager.isPaused = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }
}
