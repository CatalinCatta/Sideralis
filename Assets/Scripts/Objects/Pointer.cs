using UnityEngine;

public class Pointer : MonoBehaviour
{
    private Transform arrowTransform;
    private float counter = 1f;
    private float speed = -0.025f;
    private Vector3 _position;
    
    private void Start()
    {
        var pointerTransform = transform;
        arrowTransform = pointerTransform.GetChild(0).transform;
        _position = pointerTransform.position;
    }

    private void Update()
    {
        arrowTransform.position = new Vector3(_position.x, _position.y + counter, _position.z);

        if (counter > 2f)
            speed = -0.025f;
            
        if (counter < 1f)
            speed = 0.025f;
        
        counter += speed;
    }
}
