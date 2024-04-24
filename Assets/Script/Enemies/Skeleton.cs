using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Skeleton : Enemy
{
    public override string Label => "Skeleton";

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveStraight()
    {
        Tilemap levelTilemap = GameManager.Instance.LevelTilemap;
        if (EntityManager.Instance.IsPositionBlocked(position + facingDirection))
        {

            GameManager.Instance.AddAction(new ChangeFacingAction(this, -facingDirection));

        }

        if (!EntityManager.Instance.IsPositionBlocked(position +facingDirection))
            GameManager.Instance.AddAction(new MoveAction(this, facingDirection));
    }

    public override void Action()
    {
        MoveStraight();
    }

}
