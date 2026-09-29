using UnityEngine;

public class ShovelGrabbable : ObjectGrabbable
{
    public int numberdig = 0;

    public void Dig()
    {
        Debug.Log("Dig");
        numberdig++;
        if (numberdig >= 3)
        {
            MainManager.instance.Player.Drop();
            Destroy(this);
        }
    }

    public override void Drop()
    {
        SetLayer(LayerMask.NameToLayer("Default"));
        OnDropEvent?.Invoke();
        
        objectGrabPointTransform = null;
        gameObject.SetActive(false);
    }
}
