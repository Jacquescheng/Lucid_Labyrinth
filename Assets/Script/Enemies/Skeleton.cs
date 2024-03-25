using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Skeleton : Entity
{
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        facingDirection = new Vector2Int(1, 0);
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

            GameManager.Instance.AddAction(new ChangeFacingAction(this, -direction));
        }
       
        GameManager.Instance.AddAction(new MoveAction(this, facingDirection));
    }

    public override void Action()
    {
        MoveStraight(facingDirection);
    }

    public override void UpdateObject()
    {
        base.UpdateObject();
        spriteRenderer.flipX = facingDirection.x == -1;
    }

}
