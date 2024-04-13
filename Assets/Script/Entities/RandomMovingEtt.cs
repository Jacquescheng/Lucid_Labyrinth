using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomMovingEtt : Entity
{


    public int MoveRandom()
    {
        // Move the character
        Tilemap levelTilemap = GameManager.Instance.LevelTilemap;

        List<Vector2Int> four_dir = new List<Vector2Int>
        {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0)
        };
        List<Vector2Int> possibleMoves = new List<Vector2Int>();

        foreach (var dir in four_dir)
        {
            if (!EntityManager.Instance.IsPositionBlocked(position + dir))
            {
                possibleMoves.Add(dir);
            }
        }

        if (possibleMoves.Count == 0)
        {
            return 0;
        }

        Vector2Int moveDirection = possibleMoves[Random.Range(0, possibleMoves.Count)];
        
        GameManager.Instance.AddAction(new MoveAction(this, moveDirection));


        return 1;

    }

    public override void Action()
    {
        MoveRandom();
    }
}