public class SmallRoom : Room
{
    protected override void Initialize()
    {
        MaxCrewNumber = 1;
        MaxCapacity = 1;
        FarmingRatePerCrew = 1;
    }
}