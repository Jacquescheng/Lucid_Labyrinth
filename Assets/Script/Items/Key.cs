using UnityEngine;

[CreateAssetMenu(fileName = "New Key", menuName = "Item/Key")]
public class Key : Item
{
    public int keyId;

    public override void GetItem(PlayableChar player)
    {
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