using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
    
public class CrewStatus : MonoBehaviour
{
    [SerializeField]public GameObject crewTab;
    [SerializeField]private GameObject buildTabOpener;
    private Crew _crew;
    private Controls _controls;
    private SpaceShipManager _spaceShipManager;
    public bool inUse;
    private bool _wasJustActivated;

    private void Awake()
    {
        _controls = new Controls();
        _spaceShipManager = transform.GetComponent<SpaceShipManager>();
    }

    private void OnEnable() =>
        _controls.Enable();

    private void OnDisable() =>
        _controls.Disable();

    private void Update()
    {
        if (_crew == null)
            return;
        
        inUse = inUse && crewTab.activeSelf;

        if (!_wasJustActivated &&
            (((_controls.InGame.Deselect.triggered || _controls.InGame.Interact.triggered) && !inUse) ||
             !crewTab.activeSelf))
        {
            crewTab.SetActive(false);
            _crew = null;
            return;
        }   
        
        _wasJustActivated = false;
        SetUpMyNumbersForCrew();
    }

    public void MouseInside() =>         
        inUse = true;
    
    public void MouseOutside() =>
        inUse = false;
    
    public void SetMeUpForCrew(Crew crew)
    {
        _wasJustActivated = true;
        crewTab.SetActive(true);
        buildTabOpener.GetComponent<Toggle>().isOn = false;
        _crew = crew;
    }
    
    private void SetUpMyNumbersForCrew()
    {
        crewTab.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text =
            Utilities.DoubleToString(_crew.currentHp);
        crewTab.transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text =
            Utilities.DoubleToString(_crew.maxHp);
        
        crewTab.transform.GetChild(4).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Lvl: " + _crew.lvl;
        
        crewTab.transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text =
            Utilities.DoubleToString(_crew.currentXp);
        crewTab.transform.GetChild(4).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text =
            Utilities.DoubleToString(_crew.xpForNextLvl);
        
        crewTab.transform.GetChild(7).GetChild(1).GetComponent<TextMeshProUGUI>().text =
            Utilities.DoubleToString(_crew.dmg);
    }
}