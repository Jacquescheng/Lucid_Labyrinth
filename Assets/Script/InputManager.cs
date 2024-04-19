using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    void Awake()
    {
        Instance = this;
        this.enabled = false;
    }
    public void Update()
    {
        if (GameManager.Instance.state == GameState.PlayerTurn)
        {
            bool didPlayerAct = false;
            Vector2Int moveDirection = new Vector2Int(0, 0);
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                moveDirection.y = 1; // Up
                didPlayerAct = true;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                moveDirection.y = -1; // Down
                didPlayerAct = true;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                moveDirection.x = -1; // Left
                didPlayerAct = true;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                moveDirection.x = 1; // Right
                didPlayerAct = true;
            } 
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDirection = Vector2Int.zero; // Stay
                didPlayerAct = true;
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                GameManager.Instance.Undo();
            }


            if (didPlayerAct)
            {
                List<Entity> players = EntityManager.Instance.entities.FindAll(entity => entity is PlayableChar);
                foreach (var player_ent in players)
                {
                    PlayableChar player = (PlayableChar) player_ent;
                    if (player.Move(moveDirection) == 0) {
                        return;
                    }
                    // player.Eat();
                }

                GameManager.Instance.ChangeGameState(GameState.EnvTurn);
            }
            
        }
    }
}
