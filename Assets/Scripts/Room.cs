using UnityEngine;

public class Room : MonoBehaviour
{
    public Crew[] crews = new Crew[5];
    private ActorManager _actorManager;
    public const int MaxCrewNumber = 5;
    public int CrewSpaceLeft => MaxCrewNumber - crews.Length;
    
    void Start()
    {
        _actorManager = FindObjectOfType<ActorManager>();
    }
    
    public void AddMeToActorManager()
    {
        _actorManager.curentRoom = this;
    }

    public void RemoveMeFromActorManager()
    {
        _actorManager.curentRoom = null;
    }
}