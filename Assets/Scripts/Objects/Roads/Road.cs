using System;
using UnityEngine;

public abstract class Road : MonoBehaviour
{
    private ActorManager _actorManager;
    private SpaceShipResources _shipResources;

    private void Awake()
    {
        _shipResources = FindObjectOfType<SpaceShipResources>();
        _actorManager = FindObjectOfType<ActorManager>();
    }

    private void Start() =>
        _shipResources.energyConsumption += 0.01;

    public void AddMeToActorManager()
    {
        if (_actorManager != null)
            _actorManager.currentObject = transform.gameObject;
    }

    public void RemoveMeFromActorManager()
    {
        if (_actorManager != null)
            _actorManager.currentObject = null;
    }

    private void OnDestroy() =>
        _shipResources.energyConsumption -= 0.01;
}