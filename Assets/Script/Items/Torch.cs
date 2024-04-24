using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "New Torch", menuName = "Item/Torch")]
public class Torch : Item
{
    public int addRadius;

    public override void GetItem(PlayableChar player)
    {
        GameManager.Instance.AddAction(new AddLightRadiusAction(player, addRadius));
    }
}

class AddLightRadiusAction : IReversibleAction
{
    public Light2D light2D;
    public CinemachineVirtualCamera virtualCamera;
    public float outerRadiusBefore;
    public float outerRadiusAfter;
    public float innerRadiusBefore;
    public float innerRadiusAfter;
    public float orthoBefore;
    public float orthoAfter;

    public AddLightRadiusAction(PlayableChar player, float radius)
    {
        this.light2D = player.gameObject.transform.GetComponentInChildren<Light2D>();
        this.virtualCamera = player.gameObject.transform.GetComponentInChildren<CinemachineVirtualCamera>();
        this.outerRadiusBefore = light2D.pointLightOuterRadius;
        this.outerRadiusAfter = light2D.pointLightOuterRadius + radius;
        this.innerRadiusBefore = light2D.pointLightInnerRadius;
        this.innerRadiusAfter = light2D.pointLightInnerRadius + radius/2; 
        this.orthoBefore = virtualCamera.m_Lens.OrthographicSize;
        this.orthoAfter = virtualCamera.m_Lens.OrthographicSize + radius/2;
    }

    public void Perform()
    {
        light2D.pointLightOuterRadius = outerRadiusAfter;
        light2D.pointLightInnerRadius = innerRadiusAfter;
        virtualCamera.m_Lens.OrthographicSize = orthoAfter;
    }

    public void Undo()
    {
        light2D.pointLightOuterRadius = outerRadiusBefore;
        light2D.pointLightInnerRadius = innerRadiusBefore;
        virtualCamera.m_Lens.OrthographicSize = orthoBefore;
    }
}