using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Crew> crews = new();
    private ActorManager _actorManager;
    public const int MaxCrewNumber = 5;
    public int CrewSpaceLeft => MaxCrewNumber - crews.Count;
    
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