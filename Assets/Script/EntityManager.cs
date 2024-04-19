using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance;
    public List<Entity> entities = new List<Entity>();


    private void Awake()
    {
        Instance = this;

    }

    public void CreateEntity(GameObject prefab, Vector2Int position, Vector2Int faceDirection = default)
    {
        Entity entity = Instantiate(prefab, Vector3Int.zero, Quaternion.identity).GetComponent<Entity>();
        entity.Init(position, faceDirection);
        if (entity is ItemEntity) {
            ItemEntity item = (ItemEntity) entity;
            item.UpdateItemDisplay();
        }
        // entities.Add(entity);
    }

    public void CreateItemEntity(Item item, Vector2Int position)
    {
        GameObject item_entity = Resources.Load<GameObject>("Prefabs/ItemEntity");
        ItemEntity itemEntity = Instantiate(item_entity, Vector3Int.zero, Quaternion.identity).GetComponent<ItemEntity>();
        itemEntity.Init(position);
        itemEntity.item = item;
        itemEntity.UpdateItemDisplay();
        // entities.Add(itemEntity);
    }

    public void EnvTurn()
    {
        List<PlayableChar> players = new List<PlayableChar>();
        foreach (var entity in entities)
        {
            if (entity.isActive)
                if (entity is PlayableChar @char)
                {
                    players.Add(@char);
                }
                else
                {
                    entity.Action();
                }
        }
        foreach (var player in players)
        {
            player.Action();
        }
        if (InputManager.Instance.bufferedMoves.Count > 0)
        {
            foreach (var move in InputManager.Instance.bufferedMoves)
            {
                PlayableChar player = move.Item1;
                Vector2Int moveDirection = move.Item2;
                if (player.isActive) {
                    player.Move(moveDirection);
                    player.Action();
                }
            }
            InputManager.Instance.bufferedMoves.Clear();
        }
        foreach (var entity in entities)
        {
            entity.UpdateObject();
        }


        GameManager.Instance.ChangeGameState(GameState.EndTurn);
    }

    public void UpdateEntites()
    {
        foreach (var entity in entities)
        {
            entity.UpdateObject();
        }
    }

    public bool IsPositionBlocked(Vector2Int position)
    {
        TileBase targetTile = GameManager.Instance.LevelTilemap.GetTile((Vector3Int)position);
        if (targetTile == null)
        {
            return true;
        }
        if (GameManager.Instance.BlockingTiles.ContainsKey(targetTile.name))
        {
            if (GameManager.Instance.BlockingTiles[targetTile.name])
            {
                return true;
            }
        }
        Entity entity = CheckEntityinPos(position);
        if (entity != null)
        {
            if (entity.isActive && entity.isBlocking)
            {
                return true;
            }
        }
        return false;
    }

    public Entity CheckEntityinPos(Vector2Int position)
    {
        foreach (var entity in entities)
        {
            if (entity.position == position && entity.isActive)
            {
                return entity;
            }
        }
        return null;
    }

    
}

