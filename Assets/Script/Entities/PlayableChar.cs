using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayableChar : Entity
{
    // Start is called before the first frame update
    public int keys = 0;


    public int Move(Vector2Int moveDirection)
    {
        // Move the character
        Tilemap levelTilemap = GameManager.Instance.LevelTilemap;
        Vector2Int targetCell = position + moveDirection;


        if (!EntityManager.Instance.IsPositionBlocked(targetCell))
        {
            this.facingDirection = moveDirection;
            GameManager.Instance.AddAction(new MoveAction(this, moveDirection));
            return 0;
        }

        var entity = EntityManager.Instance.CheckEntityinPos(targetCell);
        if (entity is Enemy)
            return 2;

        return 1;

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
    public void ItemIteraction(ItemEntity itemEntity) {
        if (itemEntity.item is Key)
        {
            GameManager.Instance.AddAction(new UpdateKeyCountAction(this, keys + 1));
            Debug.Log("Picked up a key! Total keys: " + keys);
            GameManager.Instance.AddAction(new DisableAction(itemEntity));
        }

    }

    public void EnemyIteraction(Enemy enemy) {
        GameManager.Instance.AddAction(new DisableAction(this));
        GameManager.isDead = true;
        GameManager.killedBy = enemy.Label;
        Debug.Log($"You are killed by {enemy.Label}!");
    }

    public void GimmicIteraction(Gimmic gimmic) {
        if (gimmic is SpikeEntity spikeEntity)
        {
            if (spikeEntity.open)
            {
                GameManager.Instance.AddAction(new DisableAction(this));
                GameManager.isDead = true;
                GameManager.killedBy = spikeEntity.Label;
                Debug.Log($"You are killed by {spikeEntity.Label}!");
            }
        }
    }

    public void DoorInteraction(DoorEntity doorEntity) {
        if (keys > 0) {
            GameManager.Instance.AddAction(new UpdateKeyCountAction(this, keys - 1));
            if (doorEntity.leftside)
            {
                doorEntity.OpenDoor();
            }
            else
            {
                doorEntity.pairedDoor.OpenDoor();
            }
        }
    }

    public override void Action()
    {
        foreach (var entity in EntityManager.Instance.entities)
        {
            if (entity != this && entity.position == position && entity.isActive)
            {
                if (entity is ItemEntity entity1)
                {
                    ItemIteraction(entity1);
                }
                else if (entity is Enemy enemy)
                {
                    EnemyIteraction(enemy);
                } else if (entity is Gimmic gimmic) 
                {
                    GimmicIteraction(gimmic);
                }
            }
            if (entity != this && Vector2.Distance(entity.position, position) < 2 && entity.isActive)
            {
                if (entity is DoorEntity doorEntity)
                {
                    DoorInteraction(doorEntity);
                }
            }
        }  

    }
}

public class UpdateKeyCountAction : IReversibleAction
{
    public PlayableChar player;
    public int keysBefore;
    public int keysAfter;
    public UpdateKeyCountAction(PlayableChar player, int keys)
    {
        this.player = player;
        this.keysBefore = player.keys;
        this.keysAfter = keys;
    }

    public void Perform()
    {
        player.keys = keysAfter;
    }

    public void Undo()
    {
        player.keys = keysBefore;
    }
}
