using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Abilities : NetworkBehaviour
{
    ParticleSystem fireAbility;
    // Use this for initialization
    void Start()
    {
        fireAbility = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    [Client]
    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                fireAbility.Play();
                //Process hit.
            }
        }

    }
}
