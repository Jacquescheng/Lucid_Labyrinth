using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Vampire : Enemy
{
  

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToPlayer()
    {
        Entity player = EntityManager.Instance.entities.Find(entity => entity is PlayableChar);
        Vector2Int moveDirection = new Vector2Int(0, 0);
        Vector2Int playerDirection = player.position - position;
        bool toX;
        int xsign= playerDirection.x > 0 ? 1 : (playerDirection.x < 0 ? -1 : 0);
        int ysign = playerDirection.y > 0 ? 1 : (playerDirection.y < 0 ? -1 : 0);
        
        //change direction if player on leftside of player
        if (playerDirection.x<0)
        {
            GameManager.Instance.AddAction(new ChangeFacingAction(this, -facingDirection));
        }
        //select furthest x/y direction to move first
        if (Math.Abs(playerDirection.x) > Math.Abs(playerDirection.y))
        {

            //move x
            if(player.position.x < position.x)
            {
                moveDirection.x = -1;
            }
            else
            {
                moveDirection.x = 1;
            }
            toX = true;
        }
        else
        {
            //move y
            if (player.position.y < position.y)
            {
                moveDirection.y = -1;
            }
            else
            {
                moveDirection.y = 1;
            }
            toX = false;    
        }
        if (!EntityManager.Instance.IsPositionBlocked(position + moveDirection))
        {
            GameManager.Instance.AddAction(new MoveAction(this, moveDirection));
        }
        else
        {
            moveDirection = Vector2Int.zero;
            if(toX)
            {
                //move y
                if (player.position.y < position.y)
                {
                    moveDirection.y = -1;
                }
                else
                {
                    moveDirection.y = 1;
                }
            }
            else
            {
                if (player.position.x < position.x)
                {
                    moveDirection.x = -1;
                }
                else
                {
                    moveDirection.x = 1;
                }
            }
            if (!EntityManager.Instance.IsPositionBlocked(position + moveDirection))
            {
                GameManager.Instance.AddAction(new MoveAction(this, moveDirection));
                Debug.Log(moveDirection);
            }
        }
            
    }
    public override void Action()
    {
        MoveToPlayer();
    }
}
