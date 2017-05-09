using UnityEngine;
using UnityEngine.Networking;

public class NetworkTransform : NetworkBehaviour
{
    public bool WasMoving { get { return position != transform.position; } }

    [SyncVar]
    private Vector3 position;
    [SyncVar]
    private Quaternion rotation;

    void Awake()
    {
        position = transform.position;
        rotation = transform.rotation;
    }


    void Update()
    {
        if (isServer)
        {
            position = transform.position;
            rotation = transform.rotation;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 15);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 15);
        }
    }
}
