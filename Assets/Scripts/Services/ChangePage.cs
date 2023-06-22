using System.Linq;
using UnityEngine;

public class ChangePage : MonoBehaviour
{
    private GameObject[] _pages;

    private void Start() =>
        _pages = transform.Cast<Transform>().Select(t=>t.gameObject).ToArray();

    public void PrevPage()
    {
        for (var i = 0; i < _pages.Length; i++)
        {
            if (!_pages[i].activeSelf) continue;
            
            _pages[i].SetActive(false);
           
            if (i > 0)
                _pages[i - 1].SetActive(true);
           
            else
                _pages[^1].SetActive(true);
            
            return;
        }
    }
    
    public void NextPage()
    {
        for (var i = 0; i < _pages.Length; i++)
        {
            if (!_pages[i].activeSelf) continue;
            
            _pages[i].SetActive(false);
          
            if (i < _pages.Length - 1)
                _pages[i + 1].SetActive(true);
          
            else
                _pages[0].SetActive(true);
       
            return;
        }
    }
}
