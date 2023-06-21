using UnityEngine;

public class MediumRoom : Room
{   
    protected override void Initialize()
    {
        MaxCrewNumber = 2;
        maxCapacity = 3;
        farmingRatePerCrew = 3;
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = PrefabStorage.mediumRoomSprites[(int)roomResourcesType];
    }
}