public class BigRoom : Room
{
    protected override void Initialize()
    {
        MaxCrewNumber = 4;
        MaxCapacity = 7;
        FarmingRatePerCrew = 7;
    }
}