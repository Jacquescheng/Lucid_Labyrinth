using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Vampire : Enemy
{
    public override string Label => "Vampire";
    public Vector2Int petrolPoint;
    public bool SpawnIsPetrolPoint = true;
    public int chaseRange = 5;
    public int retreatRange = 10;

    // [NonSerialized]
    public int actionMode = 0;

    new void Start()
    {
        base.Start();
        if (SpawnIsPetrolPoint)
        {
            petrolPoint = position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToPosition(Vector2Int targetPosition)
    {
        Vector2Int moveDirection = new Vector2Int(0, 0);
        Vector2Int targetDirection = targetPosition - position;
        bool toX;
        int xsign= targetDirection.x > 0 ? 1 : (targetDirection.x < 0 ? -1 : 0);
        int ysign = targetDirection.y > 0 ? 1 : (targetDirection.y < 0 ? -1 : 0);
        
        //change direction if player on leftside of player
        if (targetDirection == Vector2Int.zero)
        {
            return;
        }

        Vector2Int tempFacingDirection = facingDirection;
        tempFacingDirection.x = xsign;
        GameManager.Instance.AddAction(new ChangeFacingAction(this, tempFacingDirection));
        
        //select furthest x/y direction to move first
        if (Math.Abs(targetDirection.x) > Math.Abs(targetDirection.y))
        {

            //move x
            moveDirection.x = xsign;
            toX = true;
        }
        else if (Math.Abs(targetDirection.x) < Math.Abs(targetDirection.y))
        {
            //move y
            moveDirection.y = ysign;
            toX = false;    
        } else {
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                //move x
                moveDirection.x = xsign;
                toX = true;
            }
            else
            {
                //move y
                moveDirection.y = ysign;
                toX = false;
            }
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
                moveDirection.y = ysign;
            }
            else
            {
                //move x
                moveDirection.x = xsign;
            }
            if (!EntityManager.Instance.IsPositionBlocked(position + moveDirection))
            {
                GameManager.Instance.AddAction(new MoveAction(this, moveDirection));
                // Debug.Log(moveDirection);
            }
        }
            
    }
    public override void Action()
    {
        Entity player = EntityManager.Instance.entities.Find(entity => entity is PlayableChar);
        Vector2Int playerPosition = player.position;
        if (Vector2Int.Distance(playerPosition, petrolPoint) <= chaseRange)
        {
            GameManager.Instance.AddAction(new ChangeChasingAction(this, 1));
        }
        else if (Vector2Int.Distance(playerPosition, petrolPoint) >= retreatRange)
        {
            GameManager.Instance.AddAction(new ChangeChasingAction(this, 0));
        }

        if (actionMode == 0)
        {
            MoveToPosition(petrolPoint);
        }
        else if (actionMode == 1)
        {
            MoveToPosition(playerPosition);
        }
    }
}

public class ChangeChasingAction : IReversibleAction
{
    public Vampire vampire;
    public int actionModeBefore;
    public int actionModeAfter;
    public ChangeChasingAction(Vampire vampire, int actionMode)
    {
        this.vampire = vampire;
        this.actionModeBefore = vampire.actionMode;
        this.actionModeAfter = actionMode;
    }

    public void Perform()
    {
        vampire.actionMode = actionModeAfter;
    }

    public void Undo()
    {
        vampire.actionMode = actionModeBefore;
    }
}
