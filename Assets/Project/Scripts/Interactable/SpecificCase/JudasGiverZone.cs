using UnityEngine;
using UnityEngine.Localization;

public class JudasGiverZone : ZoneInteractable
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        
        if (GetInteractable())
        {
            GiveJudas();
        }
    }
    
    public override bool GetInteractable()
    {
        return bInteractable && bInInteractionZone;
    }

    private void GiveJudas()
    {
        bInteractable = false;
        MainManager.instance.JudasesManager.GenerateJudas();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        MainManager.instance.Player.bInInteractionZone = false;
    }
}
