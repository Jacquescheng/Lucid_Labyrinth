using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class PlayableChar : Entity
{
    // Start is called before the first frame update
    public int keys = 0;
    public int invincibleCounter = 0;
    private AudioSource[] audioSources;

    private AudioSource[] allAudioSources;
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
    
    private void ItemIteraction(ItemEntity itemEntity) {
        GameManager.Instance.AddAction(new DisableAction(itemEntity));
        itemEntity.item.GetItem(this);
    }

    private void EnemyIteraction(Enemy enemy) {
        if (invincibleCounter > 0)
        {
            return;
        }

        GameManager.Instance.AddAction(new DisableAction(this));
        GameManager.isDead = true;
        GameManager.killedBy = enemy.Label;
        Debug.Log($"You are killed by {enemy.Label}!");
    }

    public void GimmicIteraction(Gimmic gimmic, AudioSource death) {
        if (gimmic is SpikeEntity spikeEntity)
        {
            if (spikeEntity.open && invincibleCounter == 0)
            {
                GameManager.Instance.AddAction(new DisableAction(this));
                GameManager.isDead = true;
                GameManager.killedBy = spikeEntity.Label;
                Debug.Log($"You are killed by {spikeEntity.Label}!");
                allAudioSources = GameObject.FindObjectsOfType<AudioSource>();
                foreach (AudioSource audio in allAudioSources){
                    audio.Stop();
                }
                death.Play();
            }
        }
    }

    public void DoorInteraction(DoorEntity doorEntity, AudioSource doorOpened) {
        if (keys > 0) {
            GameManager.Instance.AddAction(new UpdateKeyCountAction(this, keys - 1));
            if (doorEntity.leftside)
            {
                doorEntity.OpenDoor();
                doorOpened.Play();
            }
            else
            {
                doorEntity.pairedDoor.OpenDoor();
                doorOpened.Play();
            }
        }
    }

    private void BedInteraction() {
        GameManager.Instance.AddAction(new DisableAction(this));
        GameManager.won = true;
        Debug.Log($"Congrat! You won!");
    }

    public override void Action()
    {
        audioSources = GetComponents<AudioSource>();
        audioSources[0].Play();
        foreach (var entity in EntityManager.Instance.entities)
        {
            if (entity != this && entity.position == position && entity.isActive)
            {
                if (entity is ItemEntity entity1)
                {
                    ItemIteraction(entity1);
                    audioSources[3].Play();
                }
                else if (entity is Enemy enemy)
                {
                    EnemyIteraction(enemy);
                    allAudioSources = GameObject.FindObjectsOfType<AudioSource>();
                    foreach (AudioSource audio in allAudioSources){
                        audio.Stop();
                    }
                    audioSources[1].Play();
                } 
                else if (entity is Gimmic gimmic) 
                {
                    GimmicIteraction(gimmic, audioSources[1]);
                }
            }
            if (entity != this && Vector2.Distance(entity.position, position) < 2 && entity.isActive)
            {
                if (entity is DoorEntity doorEntity)
                {
                    if(keys <= 0){
                        audioSources[4].Play();
                    }
                    DoorInteraction(doorEntity, audioSources[5]);
                }
                if (entity is BedEntity bedEntity)
                {
                    BedInteraction();
                }
            }
        }  

    }

    public void EndTurn()
    {
        if (invincibleCounter > 0)
        {
            GameManager.Instance.AddAction(new InvincibleAction(this, invincibleCounter - 1));
        }
    }
}

public class InvincibleAction : IReversibleAction
{
    public PlayableChar player;
    public int invincibleCounterBefore;
    public int invincibleCounterAfter;
    public InvincibleAction(PlayableChar player, int invincibleCounter)
    {
        this.player = player;
        this.invincibleCounterBefore = player.invincibleCounter;
        this.invincibleCounterAfter = invincibleCounter;

    }

    public void Perform()
    {
        player.invincibleCounter = invincibleCounterAfter;
        player.isBlocking = player.invincibleCounter > 0;
        player.gameObject.GetComponent<SpriteRenderer>().color = player.invincibleCounter > 0 ? Color.red : Color.white;
    }

    public void Undo()
    {
        player.invincibleCounter = invincibleCounterBefore;
        player.isBlocking = player.invincibleCounter > 0;
        player.gameObject.GetComponent<SpriteRenderer>().color = player.invincibleCounter > 0 ? Color.red : Color.white;
    }
}
