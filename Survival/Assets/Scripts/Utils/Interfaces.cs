using UnityEngine;
using System.Collections.Generic;

public  interface IDamageable
{
    void DealDamage(int amount,bool isPlayer);
}

public interface IFarmable
{

    int GiveExp();
    List<ResourceOwned> GiveResources();
}


