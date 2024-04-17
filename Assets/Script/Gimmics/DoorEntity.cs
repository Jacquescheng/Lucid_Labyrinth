using UnityEngine;
public class DoorEntity : Gimmic
{
    public bool leftside = true;
    public DoorEntity pairedDoor;
    public new void Start()
    {
        base.Start();

    }

    public override void Action()
    {
        
    }

    public void OpenDoor() {
        if (leftside) {
            GameManager.Instance.AddAction(new DisableAction(this));
            pairedDoor.OpenDoor();
        } else {
            GameManager.Instance.AddAction(new DisableAction(this));
        }
    }
}