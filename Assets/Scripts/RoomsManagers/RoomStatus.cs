using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomStatus : MonoBehaviour
{
    [SerializeField]public GameObject roomTab;
    [SerializeField]private GameObject buildTabOpener;
    private Room _room;
    private Controls _controls;
    private DepthFirstSearch _depthFirstSearch;
    private SpaceShipManager _spaceShipManager;
    private UpgradeRoom _upgradeRoom;
    private CrewsOfRoom _crewsOfRoom;
    private bool _removable;

    private void Awake()
    {
        _depthFirstSearch = new DepthFirstSearch();
        _controls = new Controls();
        _spaceShipManager = transform.GetComponent<SpaceShipManager>();
        _upgradeRoom = transform.GetComponent<UpgradeRoom>();
        _crewsOfRoom = transform.GetComponent<CrewsOfRoom>();
    }

    private void OnEnable() =>
        _controls.Enable();

    private void OnDisable() =>
        _controls.Disable();

    private void Update()
    {
        if (_room == null)
            return;

        if (_controls.InGame.Deselect.triggered || buildTabOpener.GetComponent<Toggle>().isOn)
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

        _removable = _depthFirstSearch.IsSafeToRemove(room.gameObject, _spaceShipManager.Ship);
        
        if (_room != null)
            _room.transform.GetChild(5).gameObject.SetActive(false);
        
        roomTab.SetActive(true);
        buildTabOpener.GetComponent<Toggle>().isOn = false;
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

    public void GoToUpgrade() {
        _upgradeRoom.SetMeUpForRoom(_room, roomTab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text);
        roomTab.SetActive(false);
    }

    public void ShowCrewsList() {
        _crewsOfRoom.SetMeUpForRoom(_room, roomTab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text);
        roomTab.SetActive(false);
    }

    public void CollectNow() =>
        _room.CollectResources();
    
    public void DestroyRoom()
    {
        if (!_removable)
            return;
        
        _spaceShipManager.RemoveObjectFrom(Utilities.GetPositionInArrayOfCoordinate(_room.transform.position),
            Utilities.CheckObjectTypeIntegrity(_room switch
            {
                SmallRoom => ObjectType.SmallRoom,
                MediumRoom => ObjectType.MediumRoom,
                _ => ObjectType.BigRoom
            }, _room.transform.rotation));
        
        roomTab.SetActive(false);
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
        
        roomTab.transform.GetChild(4).GetComponent<Image>().color = _removable? new Color(1, 0, 0, 1) : new Color(0, 0, 0, 1);
        roomTab.transform.GetChild(4).GetChild(0).GetComponent<Image>().color = _removable? new Color(1, 0, 0, 1) : new Color(0.25f, 0.25f, 0.25f, 1);
        roomTab.transform.GetChild(4).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 1);
        roomTab.transform.GetChild(4).GetChild(0).GetChild(0).gameObject.SetActive(!_removable);
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
