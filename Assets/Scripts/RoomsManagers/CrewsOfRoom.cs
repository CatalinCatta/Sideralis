using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class CrewsOfRoom : MonoBehaviour
{
    private SpaceShipResources _spaceShipResources;
    private Controls _control;
    private Room _currentRoom;
    public bool inUse;
    [SerializeField] private GameObject listTab;
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
        inUse = inUse && listTab.activeSelf;

        if (!_wasJustActivated &&
            (((_control.InGame.Deselect.triggered || _control.InGame.Interact.triggered) && !inUse) ||
             !listTab.activeSelf))
        {
            listTab.SetActive(false);
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
        listTab.gameObject.SetActive(true);
        listTab.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = roomName;
        ChangeMyColorForRoom();
        SetUpMyNumbersForRoom();
    }
    
     private void ChangeMyColorForRoom()
    {
        switch (_currentRoom.roomResourcesType)
        {
            case Resource.Energy:
                Utilities.SetColor(listTab.transform, new Color(1, 1, 0, 1));
                break;
            case Resource.Oxygen:
                Utilities.SetColor(listTab.transform, new Color(1, 1, 1, 1));
                break;
            case Resource.Water:
                Utilities.SetColor(listTab.transform, new Color(0, 0.5f, 1, 1));
                break;
            case Resource.Food:
                Utilities.SetColor(listTab.transform, new Color(0.5f, 0.25f, 0.15f, 1));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Utilities.SetColor(listTab.transform.GetChild(0), new Color(1, 0, 0, 1));
        listTab.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        listTab.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        listTab.transform.GetChild(2).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        listTab.transform.GetChild(2).GetChild(1).GetChild(3).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

     public void SelectCrew(int crewNr)
     {
         _currentRoom.crews[crewNr].transform.GetComponent<CrewMovement>().SelectCrew();
         listTab.gameObject.SetActive(false);
     }
     
     public void SelectAllCrews()
     {
         foreach (var crew in _currentRoom.crews)
             crew.transform.GetComponent<CrewMovement>().SelectMultipleCrew();
         listTab.gameObject.SetActive(false);
         
     }
    
    private void SetUpMyNumbersForRoom()
    {
        for (var i = 0; i < _currentRoom.maxCrewNumber; i++)
        {
            listTab.transform.GetChild(2).GetChild(1).GetChild(i).GetChild(1).GetChild(0)
                .GetComponent<TextMeshProUGUI>().text = _currentRoom.crews[i].lvl.ToString();
            listTab.transform.GetChild(2).GetChild(1).GetChild(i).GetChild(2).GetChild(0)
                .GetComponent<TextMeshProUGUI>().text = Utilities.DoubleToString(_currentRoom.crews[i].currentHp) + " / " + Utilities.DoubleToString(_currentRoom.crews[i].maxHp);
            listTab.transform.GetChild(2).GetChild(1).GetChild(i).GetChild(3).GetChild(0)
                .GetComponent<TextMeshProUGUI>().text = Utilities.DoubleToString(_currentRoom.crews[i].dmg);
        }

        for (var i = 0; i < 4; i++)
            listTab.transform.GetChild(2).GetChild(1).GetChild(i).gameObject.SetActive(i < _currentRoom.maxCrewNumber);
    }
    
}
