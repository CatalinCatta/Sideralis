using UnityEngine;

public class Crew : MonoBehaviour
{
    private SpaceShipResources _shipResources;
    public double maxHp;
    public double currentHp;
    public double dmg;
    public double lvl;
    
    private void Awake()
    {
        _shipResources = FindObjectOfType<SpaceShipResources>();
    }

    private void Start()
    {
        _shipResources.foodConsumption += 0.02;
        _shipResources.oxygenConsumption += 0.02;
        _shipResources.waterConsumption += 0.02;

        maxHp = 100;
        currentHp = maxHp;
        dmg = 10;
        lvl = 1;
    }
}
