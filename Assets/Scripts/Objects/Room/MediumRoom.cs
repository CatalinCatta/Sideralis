public class MediumRoom : Room
{   
    protected override void Initialize()
    {
        MaxCrewNumber = 2;
        maxCapacity = 3;
        farmingRatePerCrew = 3;
    }
}