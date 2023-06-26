using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomStatus : MonoBehaviour
{
    [SerializeField]private GameObject roomTab;
    [SerializeField]private GameObject buildTabOppener;
    private Room _room;
    private Controls _controls;
    
    
    private void Awake() =>
        _controls = new Controls();

    private void OnEnable() =>
        _controls.Enable();

    private void OnDisable() =>
        _controls.Disable();

    private void Update()
    {
        if (_room == null)
            return;

        if (_controls.InGame.Deselect.triggered || buildTabOppener.GetComponent<Toggle>().isOn)
            roomTab.SetActive(false);
            
        if (!roomTab.activeSelf)
        {
            _room.transform.GetChild(5).gameObject.SetActive(false);
            _room = null;
            return;
        }
        
        SetUpMyNumbersForRoom();
    }

    public void SetMeUpForRoom(Room room)
    {
        if (_room == room)
            return;
        
        if (_room != null)
            _room.transform.GetChild(5).gameObject.SetActive(false);
        
        roomTab.SetActive(true);
        buildTabOppener.GetComponent<Toggle>().isOn = false;
        _room = room;
        switch (room)
        {
            case SmallRoom:
                roomTab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Small ";
                break;
            case MediumRoom:
                roomTab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Medium ";
                break;
            case BigRoom:
                roomTab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Large ";
                break;                
        }
        
        ChangeMyColorForRoom();
    }

    private void ChangeMyColorForRoom()
    {
        switch (_room.roomResourcesType)
        {
            case Resource.Energy:
                Utilities.SetColor(roomTab.transform, new Color(1, 1, 0, 1));
                roomTab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text += "Energy Room";
                break;
            case Resource.Oxygen:
                Utilities.SetColor(roomTab.transform, new Color(1, 1, 1, 1));
                roomTab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text += "Oxygen Room";
                break;
            case Resource.Water:
                Utilities.SetColor(roomTab.transform, new Color(0, 0.5f, 1, 1));
                roomTab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text += "Water Room";
                break;
            case Resource.Food:
                Utilities.SetColor(roomTab.transform, new Color(0.5f, 0.25f, 0.15f, 1));
                roomTab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text += "Food Room";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Utilities.SetColor(roomTab.transform.GetChild(0), new Color(1, 0, 0, 1));
        Utilities.SetColor(roomTab.transform.GetChild(4), new Color(1, 0, 0, 1));
    }

    private void SetUpMyNumbersForRoom()
    {
        roomTab.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            Utilities.DoubleToString(_room.actualCapacity);
        roomTab.transform.GetChild(2).transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
            Utilities.DoubleToString(_room.maxCapacity);
        
        roomTab.transform.GetChild(5).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =_room.CrewsNumber().ToString();
        roomTab.transform.GetChild(5).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = _room.maxCrewNumber.ToString();

        roomTab.transform.GetChild(3).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _room.CrewsNumber() > 0? Utilities.DoubleToTime(
            (_room.maxCapacity - _room.actualCapacity) / (_room.farmingRatePerCrew * _room.CrewsNumber()) * 2) : "";
    }
    
}
