using UnityEngine;
using System.Collections.Generic;

public class PrefabStorage : MonoBehaviour
{
    [SerializeField] public GameObject bigRoom;
    [SerializeField] public GameObject mediumRoom;
    [SerializeField] public GameObject smallRoom;
    [SerializeField] public GameObject smallConstructPlace;
    [SerializeField] public GameObject mediumConstructPlace;
    [SerializeField] public GameObject largeConstructPlace;
    [SerializeField] public GameObject road;
    [SerializeField] public GameObject crossRoad;
    [SerializeField] public GameObject lRoad;
    [SerializeField] public GameObject tRoad;
    [SerializeField] public GameObject crew;
    [SerializeField] public GameObject pointer;
    [SerializeField] public GameObject mergeButton;
    
    [SerializeField] public Sprite smallSnappingPoint;
    [SerializeField] public Sprite mediumSnappingPoint;
    [SerializeField] public Sprite largeSnappingPoint;
    
    [SerializeField] public Sprite roadSprite;
    [SerializeField] public Sprite crossRoadSprite;
    [SerializeField] public Sprite lRoadSprite;
    [SerializeField] public Sprite tRoadSprite;

    [SerializeField] public List<Sprite> crewSprites;
    [SerializeField] public List<RuntimeAnimatorController> crewAnimations;
    [SerializeField] public List<RuntimeAnimatorController> crewWorkAnimations;
    
    [SerializeField] public Transform constructMaterialCloneParent;
    [SerializeField] public Transform constructPlacesParent;
    
    [SerializeField] public List<Sprite> largeRoomSprites;
    [SerializeField] public List<Sprite> mediumRoomSprites;
    [SerializeField] public List<Sprite> smallRoomSprites;
    
    [SerializeField] public List<Sprite> collectIconsSprites;
}
