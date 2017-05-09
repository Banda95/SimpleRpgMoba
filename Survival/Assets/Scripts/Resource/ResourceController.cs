using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ResourceType
{
    None,
    Crystal,
    Wood,
    LifeEssence
}
public enum ResourceQuality
{
    None,
    Raw,
    Good,
    Perfect,
    ShittyAwsome
}

public struct ResourceOwned
{
    public ResourceType type;
    public ResourceQuality quality;
    public int amount;
    public void SetUp(ResourceType t, ResourceQuality q, int a)
    {
        type = t; quality = q; amount = a;
    }
}
[RequireComponent (typeof(Rigidbody))]
public class ResourceController : MonoBehaviour,IDamageable,IFarmable {

    public int resourceHealth = 1;
    public int fallSpeed = 80;
    public ResourceType resourceType = ResourceType.None;
    public ResourceQuality resourceQuality = ResourceQuality.None;


    private Rigidbody rb;
    private float deathTime = 5f;
    private List<ResourceType> availableResources;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

        InitializeAvaiableResources();
    }
	

    IEnumerator DestroyResource()
    {
        resourceHealth--;
        yield return new WaitForSeconds(deathTime);
        Destroy(this.gameObject);
    }

    public void DealDamage(int amount,bool isPlayer)
    {
        resourceHealth -= amount;

        if (resourceHealth == 0)
        {
            switch (resourceType)
            {
                case ResourceType.Wood:
                    rb.isKinematic = false;
                    rb.AddForce(transform.forward * fallSpeed);
                    break;
                case ResourceType.Crystal:
                    rb.isKinematic = false; //TODO al mesh collider ciò non piace.
                    rb.AddForce(Vector3.down * fallSpeed);
                    break;
                default: break;
            }
            StartCoroutine(DestroyResource());
        }
    }

    public int GiveExp()
    {
        return 10;
    }

    public List<ResourceOwned> GiveResources()
    {
        List<ResourceOwned> result = new List<ResourceOwned>();
        for(int i = 0; i<availableResources.Count; i++)
        {
            ResourceOwned tmp = new ResourceOwned();
            tmp.SetUp(availableResources[i], resourceQuality, Random.Range(1, 5));
            result.Add(tmp);
        }        

        return result;
    }

    private void InitializeAvaiableResources()
    {
        availableResources = new List<ResourceType>();
        switch (resourceType)
        {
            case ResourceType.Wood:
                availableResources.Add(ResourceType.Wood);             
                break;
            case ResourceType.Crystal:
                availableResources.Add(ResourceType.Crystal);
                break;
            case ResourceType.LifeEssence:
                availableResources.Add(ResourceType.LifeEssence);
                break;
            default: break;
        }
    }  
}

