using UnityEngine;

public class MediumRoom : Room
{   
    protected override void Initialize()
    {
        maxCrewNumber = 2;
        maxCapacity = 2*(2 * lvl + 8) - 2;
        farmingRatePerCrew = 0.05;
        shipCaryCapacity = 50 * (lvl + 1);
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = PrefabStorage.mediumRoomSprites[(int)roomResourcesType];
    }
}