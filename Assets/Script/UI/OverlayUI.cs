using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OverlayUI : MonoBehaviour
{
    public GameObject overlayUI;
    private PlayableChar player;
    public TMP_Text keys;
    public TMP_Text status;
    public GameObject spacebar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) {
            keys.text = $"<color=#93278F>Keys found: {player.keys}/3</color>";
            if (player.invincibleCounter > 0) {
                spacebar.SetActive(false);
                status.text = $"<color=#93278F>Invincible steps: {player.invincibleCounter}</color>";
            } else {
                spacebar.SetActive(true);
                status.text = "";
            }
        }
    }

    public void Create() {
        overlayUI.SetActive(true);
        GameObject playerObj = GameObject.FindWithTag("Player");
        player = playerObj.GetComponent<PlayableChar>();
    }
}
