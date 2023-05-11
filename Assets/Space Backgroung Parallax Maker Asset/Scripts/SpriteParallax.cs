using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
    /// <summary>
    ///     Create parallax effect along X-axe
    /// </summary>
    public class SpriteParallax : MonoBehaviour
    {
        [SerializeField] private ParallaxPlane[] planes;

        [SerializeField] private bool infiniteMap = true;

        [SerializeField] private float mapSizeX = 20.48f;

        [SerializeField] private float mapSizeY = 20.48f;

        [SerializeField] private float firstPlaneRelativeOffset;

        [SerializeField] private float lastPlaneRelativeOffset = 0.9f;

        [SerializeField] private float[] planeOfsset;

        private Vector2 camOffset;
        private Vector3 camPos; // camera position

        private List<ParallaxPlane> InfiniteGroup;
        private List<ParallaxPlane>[] InfiniteMap;
        private int length;

        private Transform m_Camera;
        private Vector3 oldCamPos; // old camera position
        private ParallaxPlane plane;
        private Vector3 planePos;

        private void Start()
        {
            m_Camera = Camera.main.transform;
            camPos = m_Camera.position;
            oldCamPos = camPos;
            length = planes.Length;

            //cache plane offsets
            firstPlaneRelativeOffset = Mathf.Clamp01(firstPlaneRelativeOffset);
            lastPlaneRelativeOffset = Mathf.Clamp01(lastPlaneRelativeOffset);
            var dKP = Mathf.Abs(lastPlaneRelativeOffset - firstPlaneRelativeOffset) / (length - 1);
            planeOfsset = new float[length];

            for (var i = 0; i < length; i++)
            {
                plane = planes[i];
                if (!plane) continue;
                planeOfsset[i] = firstPlaneRelativeOffset + i * dKP;
            }

            if (infiniteMap)
                for (var i = 0; i < length; i++)
                    if (planes[i])
                        planes[i].CreateInfinitePlane(new Vector2(mapSizeX, mapSizeY), camPos);
        }

        private void Update()
        {
            camPos = m_Camera.position;
            camOffset = camPos - oldCamPos;

            for (var i = 0; i < length; i++)
            {
                plane = planes[i];
                if (!plane) continue;
                plane.transform.Translate(new Vector3(camOffset.x * planeOfsset[i], camOffset.y * planeOfsset[i], 0),
                    Space.World);

                if (infiniteMap) plane.UpdateInfinitePlane(camPos);
            }

            oldCamPos = camPos;
        }

        private void OnValidate()
        {
            firstPlaneRelativeOffset = Mathf.Clamp01(firstPlaneRelativeOffset);
            lastPlaneRelativeOffset = Mathf.Clamp01(lastPlaneRelativeOffset);
        }
    }
}