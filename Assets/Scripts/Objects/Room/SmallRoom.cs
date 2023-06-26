using UnityEngine;

public class SmallRoom : Room
{
    protected override void Initialize()
    {
        maxCrewNumber = 1;
        maxCapacity = maxCrewNumber * (2 * lvl + 8) - 2;
        farmingRatePerCrew = 0.05;
        shipCaryCapacity = 25 * maxCrewNumber * (lvl + 1);
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = PrefabStorage.smallRoomSprites[(int)roomResourcesType];
    }
}