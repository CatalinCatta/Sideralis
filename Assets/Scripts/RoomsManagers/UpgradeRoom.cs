using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeRoom : MonoBehaviour
{
    private SpaceShipResources _spaceShipResources;
    private Controls _control;
    private Room _currentRoom;
    public bool inUse;
    [SerializeField] private GameObject upgradeTab;
    private bool _wasJustActivated;

    private void Awake()
    {
        _spaceShipResources = transform.GetComponent<SpaceShipResources>();
        _control = new Controls();
    }

    private void OnEnable() =>
        _control.Enable();

    private void OnDisable() =>
        _control.Disable();
    
    private void Update()
    {
        inUse = inUse && upgradeTab.activeSelf;

        if (!_wasJustActivated &&
            (((_control.InGame.Deselect.triggered || _control.InGame.Interact.triggered) && !inUse) ||
             !upgradeTab.activeSelf))
        {
            upgradeTab.SetActive(false);
            _currentRoom = null;
            return;
        }
        
        _wasJustActivated = false;
    }

    public void MouseInside() =>         
        inUse = true;

    public void MouseOutside() =>
        inUse = false;

    public void SetMeUpForRoom(Room room, string roomName)
    {
        _currentRoom = room;
        _wasJustActivated = true;
        upgradeTab.SetActive(true);
        upgradeTab.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = roomName;
        ChangeMyColorForRoom();
        SetUpMyNumbersForRoom();
    }
    
    public void Upgrade()
    {
        _currentRoom.lvl++;

        var shipCaryCapacityDifference = _currentRoom.shipCaryCapacity;

        _currentRoom.maxCapacity = _currentRoom.maxCrewNumber * (2 * _currentRoom.lvl + 8) - 2;
        _currentRoom.shipCaryCapacity = 25 * _currentRoom.maxCrewNumber * (_currentRoom.lvl + 1);

        shipCaryCapacityDifference -= _currentRoom.shipCaryCapacity;
        
        switch (_currentRoom.roomResourcesType)
        {
            case Resource.Energy:
                _spaceShipResources.energyCapacity -= shipCaryCapacityDifference;
                break;
            case Resource.Oxygen:
                _spaceShipResources.oxygenCapacity -= shipCaryCapacityDifference;
                break;
            case Resource.Water:
                _spaceShipResources.waterCapacity -= shipCaryCapacityDifference;
                break;
            case Resource.Food:
                _spaceShipResources.foodCapacity -= shipCaryCapacityDifference;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        SetUpMyNumbersForRoom();
    }
    
    private void ChangeMyColorForRoom()
    {
        switch (_currentRoom.roomResourcesType)
        {
            case Resource.Energy:
                Utilities.SetColor(upgradeTab.transform, new Color(1, 1, 0, 1));
                break;
            case Resource.Oxygen:
                Utilities.SetColor(upgradeTab.transform, new Color(1, 1, 1, 1));
                break;
            case Resource.Water:
                Utilities.SetColor(upgradeTab.transform, new Color(0, 0.5f, 1, 1));
                break;
            case Resource.Food:
                Utilities.SetColor(upgradeTab.transform, new Color(0.5f, 0.25f, 0.15f, 1));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Utilities.SetColor(upgradeTab.transform.GetChild(1), new Color(1, 0, 0, 1));
        upgradeTab.transform.GetChild(4).GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }
    
    private void SetUpMyNumbersForRoom()
    {
        upgradeTab.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = _currentRoom.lvl.ToString();
        upgradeTab.transform.GetChild(3).GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = _currentRoom.maxCapacity.ToString();
        upgradeTab.transform.GetChild(3).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _currentRoom.shipCaryCapacity.ToString();
        upgradeTab.transform.GetChild(3).GetChild(2).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _currentRoom.maxCrewNumber.ToString();
        upgradeTab.transform.GetChild(3).GetChild(2).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text =
            (_currentRoom.maxCrewNumber * (2 * (_currentRoom.lvl + 1) + 8) - 2).ToString();
        upgradeTab.transform.GetChild(3).GetChild(2).GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text =
            (25 * _currentRoom.maxCrewNumber * (_currentRoom.lvl + 2)).ToString();
        upgradeTab.transform.GetChild(3).GetChild(2).GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = _currentRoom.maxCrewNumber.ToString();
    }
}
