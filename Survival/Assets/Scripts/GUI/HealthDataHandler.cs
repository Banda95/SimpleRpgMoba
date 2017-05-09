using UnityEngine;
using UnityEngine.UI;


class HealthDataHandler : MonoBehaviour
{
    private Text health;
    private Text baseHealth;

    void Awake()
    {
        health = GetComponentsInChildren<Text>()[0];
        baseHealth = GetComponentsInChildren<Text>()[1];
    }


    public void SetHealth(int newHealth)
    {
        health.text = newHealth.ToString();
    }
    public void SetBaseHealth(int newHealth)
    {
        baseHealth.text = newHealth.ToString();
    }

}

