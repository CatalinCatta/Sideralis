using UnityEngine;

public class Pointer : MonoBehaviour
{
    private Transform _arrowTransform;
    private float _counter = 1f;
    private float _speed = -0.025f;
    private Vector3 _position;
    
    private void Awake()
    {
        var pointerTransform = transform;
        _arrowTransform = pointerTransform.GetChild(0).transform;
        _position = pointerTransform.position;
    }

    private void Update()
    {
        _arrowTransform.position = new Vector3(_position.x, _position.y + _counter, _position.z);

        _speed = _counter switch
        {
            > 2f => -0.025f,
            < 1f => 0.025f,
            _ => _speed
        };

        _counter += _speed;
    }
}
