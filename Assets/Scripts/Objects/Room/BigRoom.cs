using UnityEngine;

public class BigRoom : Room
{
    protected override void Initialize()
    {
        maxCrewNumber = 4;
        maxCapacity = 4*(2 * lvl + 8) - 2;
        farmingRatePerCrew = 0.05;
        shipCaryCapacity = 100 * (lvl + 1);
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = PrefabStorage.largeRoomSprites[(int)roomResourcesType];
    }
}