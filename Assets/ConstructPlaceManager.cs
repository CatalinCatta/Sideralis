// using UnityEngine;
// using UnityEngine.EventSystems;
//
// public class ConstructPlaceManager : MonoBehaviour, IDropHandler
// {
//     
//     public GameObject[] dropZoneGameObjects; // Array of game objects that represent drop zones
//
//     private List<DropZone> dropZones = new List<DropZone>(); // List of active drop zones
//
//     private void Start()
//     {
//         // Add DropZone components to all drop zone game objects
//         foreach (GameObject dropZoneGameObject in dropZoneGameObjects)
//         {
//             DropZone dropZone = dropZoneGameObject.AddComponent<DropZone>();
//             dropZones.Add(dropZone);
//             dropZoneGameObject.SetActive(false); // Start with all drop zones inactive
//         }
//     }
//
//     public void ShowDropZone(int index)
//     {
//         // Show a drop zone at the specified index
//         if (index >= 0 && index < dropZoneGameObjects.Length)
//         {
//             dropZoneGameObjects[index].SetActive(true);
//         }
//     }
//
//     public void HideDropZone(int index)
//     {
//         // Hide a drop zone at the specified index
//         if (index >= 0 && index < dropZoneGameObjects.Length)
//         {
//             dropZoneGameObjects[index].SetActive(false);
//         }
//     }
// }
// }
