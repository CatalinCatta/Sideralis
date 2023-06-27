using System.Collections;
using UnityEngine;

public class Crew : MonoBehaviour
{
    private SpaceShipResources _shipResources;
    public double maxHp;
    public double currentHp;
    public double dmg;
    public double lvl;
    public double currentXp;
    public double xpForNextLvl;
    
    private void Awake()
    {
        _shipResources = FindObjectOfType<SpaceShipResources>();
    }
    
    private void Start()
    {
        _shipResources.foodConsumption += 0.02;
        _shipResources.oxygenConsumption += 0.02;
        _shipResources.waterConsumption += 0.02;

        maxHp = 75;
        currentHp = maxHp;
        dmg = 7.5;
        lvl = 0;
        currentXp = 0;
        xpForNextLvl = 0;
        
        StartCoroutine(GetXp());
    }

    private void Update()
    {
        if (currentXp < xpForNextLvl)
            return;

        lvl++;
        xpForNextLvl = 25 * (3 * (lvl + 1) + 2) * lvl;
        dmg += 2.5;
        maxHp += 25;
        currentHp = maxHp;
    }   

    private IEnumerator GetXp()
    {
        while (true)
        {
            currentXp += 2;
            yield return new WaitForSeconds(1);
        }
    }
}
