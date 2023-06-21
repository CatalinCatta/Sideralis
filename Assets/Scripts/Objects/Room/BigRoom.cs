using UnityEngine;

public class BigRoom : Room
{
    protected override void Initialize()
    {
        MaxCrewNumber = 4;
        maxCapacity = 7;
        farmingRatePerCrew = 7;
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = PrefabStorage.largeRoomSprites[(int)roomResourcesType];
    }
}