using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private const int MaxCrewNumber = 5;
    public List<Crew> crews = new();
    private ActorManager _actorManager;

    public int CrewSpaceLeft => 
        MaxCrewNumber - crews.Count;

    private void Start() => 
        _actorManager = FindObjectOfType<ActorManager>();

    public void AddMeToActorManager() =>
        _actorManager.currentRoom = this;

    public void RemoveMeFromActorManager() =>
        _actorManager.currentRoom = null;
}