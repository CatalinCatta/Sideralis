public class SmallRoom : Room
{
    protected override void Initialize()
    {
        MaxCrewNumber = 1;
        maxCapacity = 1;
        farmingRatePerCrew = 1;
    }
}