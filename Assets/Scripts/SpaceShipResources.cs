using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class SpaceShipResources : MonoBehaviour
{
    [SerializeField] private GameObject statusBar;
    public double energy;
    public double oxygen;
    public double water;
    public double food;
    
    public double energyCapacity;
    public double oxygenCapacity;
    public double waterCapacity;
    public double foodCapacity;
    
    public double energyConsumption;
    public double oxygenConsumption;
    public double waterConsumption;
    public double foodConsumption;

    private void Start() =>
        StartCoroutine(UpdateResources());

    private IEnumerator UpdateResources()
    {
        while (true)
        {
            energy = Math.Max(energy - energyConsumption, 0);
            oxygen = Math.Max(oxygen - oxygenConsumption, 0);
            water = Math.Max(water - waterConsumption, 0);
            food = Math.Max(food - foodConsumption, 0);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Update()
    {
        statusBar.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Utilities.DoubleToString(energy);
        statusBar.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Utilities.DoubleToString(oxygen);
        statusBar.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Utilities.DoubleToString(water);
        statusBar.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Utilities.DoubleToString(food);
        statusBar.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = Utilities.DoubleToString(energyCapacity);
        statusBar.transform.GetChild(1).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = Utilities.DoubleToString(oxygenCapacity);
        statusBar.transform.GetChild(2).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = Utilities.DoubleToString(waterCapacity);
        statusBar.transform.GetChild(3).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = Utilities.DoubleToString(foodCapacity);
    }
    
}
