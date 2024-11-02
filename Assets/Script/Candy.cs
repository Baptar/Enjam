public class Candy : ObjectGrabbable
{
    // Start is called before the first frame update
    void Start()
    {
        canTake = false;
    }

    public void OnCandyGive()
    {
        playerPickUp.bHasGrabbleObject = false;
        Destroy(gameObject);
    }
}
