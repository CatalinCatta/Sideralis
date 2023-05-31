using System.Linq;
using UnityEngine;

public abstract class Room : MonoBehaviour
{
    protected static int MaxCrewNumber;
    public Crew[] crews;
    private ActorManager _actorManager;

    public int CrewSpaceLeft => 
        crews.Count(crew => crew == null);
    
    public int CrewsNumber() =>
        crews.Count(crew => crew != null);
    
    protected abstract void Initialize();
    
    private void Start()
    {
        _actorManager = FindObjectOfType<ActorManager>();
        Initialize();
        crews = new Crew[MaxCrewNumber];
    }

    public void AddMeToActorManager() =>
        _actorManager.currentRoom = this;

    public void RemoveMeFromActorManager() =>
        _actorManager.currentRoom = null;
}