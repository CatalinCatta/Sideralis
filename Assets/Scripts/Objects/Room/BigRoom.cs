public class BigRoom : Room
{
    protected override void Initialize()
    {
        MaxCrewNumber = 4;
        maxCapacity = 7;
        farmingRatePerCrew = 7;
    }
}