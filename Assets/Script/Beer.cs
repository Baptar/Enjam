using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beer : ObjectGrabbable
{
    [SerializeField] private Animator beerAnimation;
    // Start is called before the first frame update
    void Start()
    {
        canTake = true;
    }
    
    public void BeerAnimation()
    {
        beerAnimation.Play("glouglouMieux", 0, 0.0f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Park/BeerDrunk");
    }
    
    public void OnBeerDrunken()
    {
        Drop();
        canTake = false;
        playerPickUp.OnBeerDrunken();
    }
}
