using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Item/InvinciPotion")]
public class InvinciPotion : Item
{
    public int duration;

    public override void GetItem(PlayableChar player)
    {
        GameManager.Instance.AddAction(new InvincibleAction(player, player.invincibleCounter + duration+1));
    }
}
