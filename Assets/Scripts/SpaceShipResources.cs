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
    
    
    private IEnumerator UpdateResources()
    {
        while (true)
        {
            energy = Math.Max(energy - 1, 0);
            oxygen = Math.Max(oxygen - 1, 0);
            water = Math.Max(water - 1, 0);
            food = Math.Max(food - 1, 0);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Update()
    {
        statusBar.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = energy.ToString();
        statusBar.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = oxygen.ToString();
        statusBar.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = water.ToString();
        statusBar.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = food.ToString();
    }
}
