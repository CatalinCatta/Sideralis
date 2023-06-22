using UnityEngine;
using UnityEngine.UI;

public class ConstructSelector : MonoBehaviour
{
    [SerializeField] private Sprite deselectedSprite; 
    [SerializeField] private Sprite selectedSprite; 
    private Controls _controls;
    public Image currentSelectedImage;
    private SpaceShipManager _spaceShipManager;
    private ActorManager _actorManager;
    private PrefabStorage _prefabStorage;
    
    private void Awake() =>
        _controls = new Controls();

    private void Start()
    {
        _spaceShipManager = FindObjectOfType<SpaceShipManager>();
        _actorManager = FindObjectOfType<ActorManager>();
        _prefabStorage = FindObjectOfType<PrefabStorage>();
    }

    private void OnEnable() =>
        _controls.Enable();

    private void OnDisable() =>
        _controls.Disable();

    
    public void SelectMe(Image image)
    {
        DeselectCurrentImage();
        
        image.sprite = selectedSprite;
        image.color = new Color(0, 1, 0, 1);
        
        currentSelectedImage = image;
        
        _spaceShipManager.CreateConstructPlacesFor(ObjectSize.Small, image.gameObject.GetComponentInChildren<ConstructMaterial>().objectType);
    }

    private void Update()
    {
        if (_controls.InGame.Move.triggered)
            DeselectCurrentImage();
    }

    public void DeselectCurrentImage()
    {
        if (currentSelectedImage == null) return;
        
        currentSelectedImage.sprite = deselectedSprite;
        currentSelectedImage.color = new Color(1, 1, 1, 0.5f);
        currentSelectedImage = null;
        
        _actorManager.DestroyAllChildrenOf(_prefabStorage.constructPlacesParent.gameObject);
    }
}
