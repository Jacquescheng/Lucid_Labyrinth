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
        foreach (var entity in entities)
        {
            if (entity.isActive)
                entity.Action();
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
        foreach (var entity in entities)
        {
            if ((entity.position == position) && entity.isBlocking && entity.isActive)
            {
                return true;
            }
        }
        return false;
    }

    
}

