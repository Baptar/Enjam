using DG.Tweening;
using UnityEngine;

public class RemoteInteract : ObjectInteractable
{
    public override bool GetInteractable() => bInteractable || MainManager.instance.Player.GetHasPile();
}
