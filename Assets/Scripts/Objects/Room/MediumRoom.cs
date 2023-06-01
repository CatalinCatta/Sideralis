public class MediumRoom : Room
{   
    protected override void Initialize()
    {
        MaxCrewNumber = 2;
        MaxCapacity = 3;
        FarmingRatePerCrew = 3;
    }
}