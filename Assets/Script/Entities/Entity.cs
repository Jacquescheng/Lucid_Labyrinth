using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Vector2Int position;
    public Vector2Int facingDirection;
    public bool isBlocking;
    public bool isActive;


    public virtual void Init(Vector2Int position, Vector2Int facingDirection = default)
    {
        this.position = position;
        this.facingDirection = facingDirection;
        transform.position = GameManager.Instance.LevelTilemap.GetCellCenterWorld((Vector3Int)position);
    }

    public virtual void UpdateObject() {
        transform.position = GameManager.Instance.LevelTilemap.GetCellCenterWorld((Vector3Int)position);
        gameObject.GetComponent<SpriteRenderer>().enabled = isActive;
        gameObject.GetComponent<SpriteRenderer>().flipX = facingDirection.x == -1;

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

public class ChangeFacingAction : IReversibleAction
{
    public Entity entity;
    public Vector2Int oldFacingDirection;
    public Vector2Int facingDirection;
    
    public ChangeFacingAction(Entity entity, Vector2Int facingDirection)
    {
        this.entity = entity;
        this.oldFacingDirection = entity.facingDirection;
        this.facingDirection = facingDirection;
    }

    public void Perform()
    {
        entity.facingDirection = facingDirection;
    }

    public void Undo()
    {
        entity.facingDirection = oldFacingDirection;
    }
}