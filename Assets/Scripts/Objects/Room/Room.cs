using System.Linq;
using UnityEngine;
using System.Collections;

public abstract class Room : MonoBehaviour
{
    protected static int MaxCrewNumber;
    public Crew[] crews;
    private ActorManager _actorManager;
    public int lvl;
    public int maxCapacity;
    public int farmingRatePerCrew;
    public double actualCapacity;
    public Resource roomResourcesType;
    protected PrefabStorage PrefabStorage;

    public int CrewSpaceLeft => 
        crews.Count(crew => crew == null);
    
    public int CrewsNumber() =>
        crews.Count(crew => crew != null);
    
    protected abstract void Initialize();
    
    private void Start()
    {
        PrefabStorage = FindObjectOfType<PrefabStorage>(); 
        _actorManager = FindObjectOfType<ActorManager>();
        lvl = 0;
        actualCapacity = 0;
        Initialize();
        crews = new Crew[MaxCrewNumber];
        StartCoroutine(Farm());
    }

    public void AddMeToActorManager()
    {
        if (_actorManager == null)
            return;
        
        _actorManager.currentRoom = this;
        _actorManager.currentObject = transform.gameObject;
    }

    public void RemoveMeFromActorManager()
    {
        if (_actorManager == null)
            return;

        _actorManager.currentRoom = null;
        _actorManager.currentObject = null;
    }

    private IEnumerator Farm()
    {
        while (actualCapacity < maxCapacity * lvl)
        {
            actualCapacity += farmingRatePerCrew * lvl * CrewsNumber() * 0.01;
            yield return new WaitForSeconds(0.5f);
        }
    } 
}