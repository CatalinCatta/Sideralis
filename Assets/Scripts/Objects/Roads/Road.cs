using UnityEngine;

public abstract class Road : MonoBehaviour
{
    private ActorManager _actorManager;

    private void Awake() =>
        _actorManager = FindObjectOfType<ActorManager>();

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
    
}