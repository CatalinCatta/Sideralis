using System.Linq;
using UnityEngine;

public class Crew : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private bool _crewSelected;
    private bool _selectingCrew;
    private ActorManager _actorManager;
    public Room room;
    
    void Start()
    {
        _actorManager = FindObjectOfType<ActorManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<BoxSelection>() || _crewSelected) return;
        _spriteRenderer.color = new Color(0.3f, 1f, 0.28f);
        _actorManager.selectedCrewNumber += 1;
        _crewSelected = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<BoxSelection>() || !Input.GetMouseButton(0)) return;
        _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        _crewSelected = false;
        _actorManager.selectedCrewNumber = 0;
    }

    private void Update()
    {
        if (_crewSelected && Input.GetMouseButtonDown(0) && !_selectingCrew && _actorManager.curentRoom != null && _actorManager.curentRoom.CrewSpaceLeft>= _actorManager.selectedCrewNumber)
        {
            room.crews = room.crews.Where(crew => crew != this).ToList();
            _actorManager.curentRoom.crews.Add(this);
            room = _actorManager.curentRoom;
            var screenToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(screenToWorldPoint.x, screenToWorldPoint.y, -5f);
            _crewSelected = false;
            _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            _actorManager.selectedCrewNumber = 0;
        }

        if (Input.GetMouseButtonDown(1))
        {
            _crewSelected = false;
            _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            _actorManager.selectedCrewNumber = 0;
        }
        _selectingCrew = false;
    }

    private void OnMouseDown()
    {
        _selectingCrew = true;
        _crewSelected = true;
        _spriteRenderer.color = new Color(0.3f, 1f, 0.28f);
        _actorManager.selectedCrewNumber = 1;
    }
}