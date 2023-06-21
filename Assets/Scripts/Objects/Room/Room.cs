using System;
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
    public int shipCaryCapacity;
    public double farmingRatePerCrew;
    public double actualCapacity;
    public Resource roomResourcesType;
    protected PrefabStorage PrefabStorage;
    private SpaceShipResources _shipResources;

    public int CrewSpaceLeft => 
        crews.Count(crew => crew == null);
    
    public int CrewsNumber() =>
        crews.Count(crew => crew != null);
    
    protected abstract void Initialize();
    
    private void Start()
    {
        PrefabStorage = FindObjectOfType<PrefabStorage>(); 
        _actorManager = FindObjectOfType<ActorManager>();
        _shipResources = FindObjectOfType<SpaceShipResources>();
        lvl = 1;
        actualCapacity = 0;
        Initialize();
        crews = new Crew[MaxCrewNumber];
        StartCoroutine(Farm());
        transform.GetChild(4).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = PrefabStorage.collectIconsSprites[(int)roomResourcesType];
        switch (roomResourcesType)
        {
            case Resource.Energy:
                _shipResources.energyCapacity += shipCaryCapacity;
                _shipResources.energy += shipCaryCapacity / 2;
                break;
            
            case Resource.Oxygen:
                _shipResources.oxygenCapacity += shipCaryCapacity;
                _shipResources.oxygen += shipCaryCapacity / 2;
                _shipResources.energyConsumption += MaxCrewNumber * 0.02;
                break;
            
            case Resource.Water:
                _shipResources.waterCapacity += shipCaryCapacity;
                _shipResources.water += shipCaryCapacity / 2;
                _shipResources.energyConsumption += MaxCrewNumber * 0.02;
                break;
            
            case Resource.Food:
                _shipResources.foodCapacity += shipCaryCapacity;
                _shipResources.food += shipCaryCapacity / 2;
                _shipResources.energyConsumption += MaxCrewNumber * 0.02;
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
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
            actualCapacity = Math.Min(actualCapacity + farmingRatePerCrew * CrewsNumber(), maxCapacity);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Update()
    {
        if (actualCapacity > maxCapacity * 0.75)
            transform.GetChild(4).gameObject.SetActive(true);
    }

    public void CollectResources()
    {
        transform.GetChild(4).gameObject.SetActive(false);
        var actualCapacityBucket = actualCapacity;
        
        switch (roomResourcesType)
        {
            case Resource.Energy:
                actualCapacity = Math.Max(0, actualCapacity - (_shipResources.energyCapacity - _shipResources.energy));
                _shipResources.energy = Math.Min(_shipResources.energy + actualCapacityBucket, _shipResources.energyCapacity);
                break;
            
            case Resource.Oxygen:
                actualCapacity = Math.Max(0, actualCapacity - (_shipResources.oxygenCapacity - _shipResources.oxygen));
                _shipResources.oxygen = Math.Min(_shipResources.oxygen + actualCapacityBucket, _shipResources.oxygenCapacity);
                break;
            
            case Resource.Water:
                actualCapacity = Math.Max(0, actualCapacity - (_shipResources.waterCapacity - _shipResources.water));
                _shipResources.water = Math.Min(_shipResources.water + actualCapacityBucket, _shipResources.waterCapacity);
                break;
            
            case Resource.Food:
                actualCapacity = Math.Max(0, actualCapacity - (_shipResources.foodCapacity - _shipResources.food));
                _shipResources.food = Math.Min(_shipResources.food + actualCapacityBucket, _shipResources.foodCapacity);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }

        StartCoroutine(Farm());
    }

    private void OnDestroy()
    {
        switch (roomResourcesType)
        {
            case Resource.Energy:
                _shipResources.energyCapacity -= shipCaryCapacity;
                _shipResources.energy = Math.Min(_shipResources.energy, _shipResources.energyCapacity);
                break;
            
            case Resource.Oxygen:
                _shipResources.oxygenCapacity -= shipCaryCapacity;
                _shipResources.energyConsumption -= MaxCrewNumber * 0.02;
                _shipResources.energy = Math.Min(_shipResources.oxygen, _shipResources.oxygenCapacity);
                break;
            
            case Resource.Water:
                _shipResources.waterCapacity -= shipCaryCapacity;
                _shipResources.energyConsumption -= MaxCrewNumber * 0.02;
                _shipResources.energy = Math.Min(_shipResources.water, _shipResources.waterCapacity);
                break;
            
            case Resource.Food:
                _shipResources.foodCapacity -= shipCaryCapacity;
                _shipResources.energyConsumption -= MaxCrewNumber * 0.02;
                _shipResources.energy = Math.Min(_shipResources.food, _shipResources.foodCapacity);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}