using System.Linq;
using UnityEngine;
using System.Collections;

public abstract class Room : MonoBehaviour
{
    protected static int MaxCrewNumber;
    public Crew[] crews;
    private ActorManager _actorManager;
    public int Lvl;
    public int MaxCapacity;
    public int FarmingRatePerCrew;
    public double ActualCapacity;
    
    public int CrewSpaceLeft => 
        crews.Count(crew => crew == null);
    
    public int CrewsNumber() =>
        crews.Count(crew => crew != null);
    
    protected abstract void Initialize();
    
    private void Start()
    {
        _actorManager = FindObjectOfType<ActorManager>();
        Lvl = 0;
        ActualCapacity = 0;
        Initialize();
        crews = new Crew[MaxCrewNumber];
        StartCoroutine(Farm());
    }

    public void AddMeToActorManager() =>
        _actorManager.currentRoom = this;

    public void RemoveMeFromActorManager() =>
        _actorManager.currentRoom = null;

    private IEnumerator Farm()
    {
        while (ActualCapacity < MaxCapacity * Lvl)
        {
            ActualCapacity += FarmingRatePerCrew * Lvl * CrewsNumber() * 0.01;
            yield return new WaitForSeconds(0.5f);
        }
    } 
}