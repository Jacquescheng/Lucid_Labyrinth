using UnityEngine;

public class ItemEntity : Entity
{
    public Item item;

    public void UpdateItemDisplay()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = item.sprite;
        gameObject.GetComponent<SpriteRenderer>().color = item.color;
    }

    public override void Action()
    {

    }
}