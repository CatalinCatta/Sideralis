using UnityEngine;

public class SmallRoom : Room
{
    protected override void Initialize()
    {
        MaxCrewNumber = 1;
        maxCapacity = 1;
        farmingRatePerCrew = 1;
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = PrefabStorage.smallRoomSprites[(int)roomResourcesType];
    }
}