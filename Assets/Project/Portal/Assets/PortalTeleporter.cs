using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform player;
    public Transform receiver;

    private CharacterController controller;
    private bool playerIsOverlapping = false;
    private float dotProductOnEnter;

    void Start()
    {
        controller = player.GetComponent<CharacterController>();    
    }

    void Update()
    {
        if (!playerIsOverlapping) return;

        Vector3 portalToPlayer = player.position - transform.position;
        float dotProduct = Vector3.Dot(transform.forward, portalToPlayer);

        if (dotProductOnEnter > 0f && dotProduct < 0f)
        {
            float rotationDiff = -Quaternion.Angle(transform.rotation, receiver.rotation);
            rotationDiff += 180f;

            Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;

            player.position = receiver.position + positionOffset;
            player.Rotate(Vector3.up, rotationDiff);
            Physics.SyncTransforms();

            playerIsOverlapping = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            playerIsOverlapping = true;
            // ✅ On mémorise de quel côté le joueur est entré
            dotProductOnEnter = Vector3.Dot(transform.forward, player.position - transform.position);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
            playerIsOverlapping = false;
    }
}