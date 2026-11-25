using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform camera;

    void Start()
    {
        camera = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(camera);
        transform.rotation = Quaternion.Euler(90, transform.eulerAngles.y, 0);
    }
}
