using UnityEngine;

public class MediumRoom : Room
{   
    protected override void Initialize()
    {
        maxCrewNumber = 2;
        maxCapacity = maxCrewNumber * (2 * lvl + 8) - 2;
        farmingRatePerCrew = 0.05;
        shipCaryCapacity = 25 * maxCrewNumber * (lvl + 1);
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = PrefabStorage.mediumRoomSprites[(int)roomResourcesType];
    }
}