using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Vector2Int position;
    public bool isBlocking;
    public bool isActive;


    public virtual void Init(Vector2Int position)
    {
        this.position = position;
        transform.position = GameManager.Instance.LevelTilemap.GetCellCenterWorld((Vector3Int)position);
    }

    public virtual void UpdateObject() {
        transform.position = GameManager.Instance.LevelTilemap.GetCellCenterWorld((Vector3Int)position);
        gameObject.GetComponent<SpriteRenderer>().enabled = isActive;

    }
    public abstract void Action();
    
}

public class MoveAction : IReversibleAction
{
    public Entity entity;
    public Vector2Int moveDirection;
    
    public MoveAction(Entity entity, Vector2Int moveDirection)
    {
        this.entity = entity;
        this.moveDirection = moveDirection;
    }

    public void Perform()
    {
        entity.position += moveDirection;
    }

    public void Undo()
    {
        entity.position -= moveDirection;
    }
}

public class DisableAction : IReversibleAction
{
    public Entity entity;
    public bool isActive;
    
    public DisableAction(Entity entity)
    {
        this.entity = entity;
        this.isActive = entity.isActive;
    }

    public void Perform()
    {
        entity.isActive = false;
    }

    public void Undo()
    {
        entity.isActive = isActive;
    }
}