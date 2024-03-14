using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/GenericItem")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public Color color = Color.white;

    
}