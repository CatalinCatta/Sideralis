using UnityEngine;

public class UiAwake : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;

    private void OnDisable() =>
        cameraController.anyOtherUiOnUse = false;
}
