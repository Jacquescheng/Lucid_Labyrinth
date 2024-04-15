using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Skeleton : Entity
{
    private SpriteRenderer spriteRenderer;
    private Vector2Int currentDirection= new Vector2Int(1, 0);

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void MoveStraight(Vector2Int direction)
    {
        Tilemap levelTilemap = GameManager.Instance.LevelTilemap;
       
        if (EntityManager.Instance.IsPositionBlocked(position + direction))
        {
<<<<<<< Updated upstream
            //horizontal
            if (direction.y == 0)
            {
                //facing right
                if (direction.x > 0)
                {
                    spriteRenderer.flipX = true;
                }
                //facing left
                else
                {
                    spriteRenderer.flipX = false;
                }

            }
            currentDirection = new Vector2Int(-direction.x, -direction.y);
=======
            GameManager.Instance.AddAction(new ChangeFacingAction(this, -facingDirection));
>>>>>>> Stashed changes
        }
       
        GameManager.Instance.AddAction(new MoveAction(this, currentDirection));
    }

    public override void Action()
    {
        MoveStraight(currentDirection);
    }
}