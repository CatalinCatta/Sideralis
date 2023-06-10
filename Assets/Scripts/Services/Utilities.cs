using UnityEngine;

public static class Utilities
{
    public static readonly int[][] Directions =
    {
        new[] { -1, 0 },         // Up
        new[] { 1, 0 },          // Down
        new[] { 0, -1 },         // Left
        new[] { 0, 1 }           // Right
    };
    
    public static Vector3 GetInGameCoordinateForPosition(double x, double y, float z) => 
        new ((float)((y - 24.5) * 10), (float)((24.5 - x) * 10), z);

    public static (double, double) GetPositionInArrayOfCoordinate(Vector2 position) => 
        ((double)-Mathf.RoundToInt(position.y) / 10 + 24.5, (double)Mathf.RoundToInt(position.x) / 10 + 24.5);
    
    public static void SetTransparencyRecursive(Transform currentTransform, float transparency)
    {
        
        Renderer renderer = currentTransform.GetComponent<Renderer>();

        if (renderer != null)
        {
            Material[] materials = renderer.materials;
            foreach (Material material in materials)
            {
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                Color color = material.color;
                color.a = transparency;
                material.color = color;
            }
        }

        for (int i = 0; i < currentTransform.childCount; i++)
        {
            Transform child = currentTransform.GetChild(i);
            SetTransparencyRecursive(child, transparency);
        }
    }
}
