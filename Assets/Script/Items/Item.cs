using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/GenericItem")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public Color color = Color.white;

    public virtual void GetItem(PlayableChar player) {
            GameManager.Instance.AddAction(new UpdateKeyCountAction(player, player.keys + 1));
            Debug.Log("Picked up a key! Total keys: " + player.keys);
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