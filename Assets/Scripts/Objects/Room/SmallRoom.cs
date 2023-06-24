using UnityEngine;

public class SmallRoom : Room
{
    protected override void Initialize()
    {
        maxCrewNumber = 1;
        maxCapacity = 2*lvl + 6;
        farmingRatePerCrew = 0.05;
        shipCaryCapacity = 25*(lvl+1);
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = PrefabStorage.smallRoomSprites[(int)roomResourcesType];
    }
}