using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "New Torch", menuName = "Item/Torch")]
public class Torch : Item
{
    public int addRadius;

    public override void GetItem(PlayableChar player)
    {
        Light2D light2D = player.gameObject.transform.GetComponentInChildren<Light2D>();
        GameManager.Instance.AddAction(new AddLightRadiusAction(light2D, addRadius));
    }
}

class AddLightRadiusAction : IReversibleAction
{
    public Light2D light2D;
    public float radiusBefore;
    public float radiusAfter;

    public AddLightRadiusAction(Light2D light2D, float radius)
    {
        this.light2D = light2D;
        this.radiusBefore = light2D.pointLightOuterRadius;
        this.radiusAfter = radiusBefore + radius;
    }

    public void Perform()
    {
        light2D.pointLightOuterRadius = radiusAfter;
    }

    public void Undo()
    {
        light2D.pointLightOuterRadius = radiusBefore;
    }
}