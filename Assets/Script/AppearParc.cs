using System.Collections;
using UnityEngine;

public class AppearParc : MonoBehaviour
{
    [SerializeField] private GameObject parc;
    [SerializeField] private float timeBetweenParc = 2f;
    [SerializeField] private NeighboorDoor1 door1;
    [SerializeField] private CameraShake cameraShake;
    
    [SerializeField] private GameObject invisibleWall;
    [SerializeField] private Vector3 targetPosition;
    
    public void ParcAppear()
    {
        StopAllCoroutines();
        cameraShake.shakeDuration = timeBetweenParc;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Park/ParkAppear", transform.position);
        StartCoroutine(MoveParc());
    }

    IEnumerator MoveParc()
    {
        float timeElapsed = 0;
        Vector3 startPosition = transform.position;

        while (timeElapsed < timeBetweenParc)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / timeBetweenParc);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        door1.TocHard();
        door1.canTake = true;
        invisibleWall.SetActive(false);
    }
}
