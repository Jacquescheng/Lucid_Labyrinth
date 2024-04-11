using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayableChar : Entity
{
    // Start is called before the first frame update
    void Start()
    {

    }


    public int Move(Vector2Int moveDirection)
    {
        // Move the character
        Tilemap levelTilemap = GameManager.Instance.LevelTilemap;
        Vector2Int targetCell = position + moveDirection;


        if (!EntityManager.Instance.IsPositionBlocked(targetCell))
        {
            GameManager.Instance.AddAction(new MoveAction(this, moveDirection));
            return 1;
        }

        return 0;

    }

    // public void Eat() { // For testing only
    //     foreach (var entity in EntityManager.Instance.entities)
    //     {
    //         if (entity != this && entity.position == position && entity.isActive)
    //         {
    //             Debug.Log("Eating " + entity.name);
    //             GameManager.Instance.AddAction(new DisableAction(entity));
    //         }
    //     }
    // }

    public override void Action()
    {

    }
}
