﻿using UnityEngine;

namespace Mkey
{
    public enum TrackMode
    {
        Player,
        Mouse,
        Gyroscope,
        Keyboard,
        Touch
    }

    public class CameraFollow : MonoBehaviour
    {
        public static CameraFollow Instance;

        public TrackMode track = TrackMode.Touch;
        public bool ClampPosition;
        public BoxCollider2D ClampField; // camera motion field

        [SerializeField] private GameObject player;

        private Vector3 acceleration;
        private float camHorSize;
        private float camVertSize;

        private Camera m_camera;

        //player follow options
        private Vector2 margin;
        private Vector2 smooth;

        public float ScreenRatio => (float)Screen.width / Screen.height;

        /// <summary>
        ///     Return true if Player out of X margin
        /// </summary>
        private bool OutOfXMargin => Mathf.Abs(transform.position.x - player.transform.position.x) > margin.x;

        /// <summary>
        ///     Return true if Player out of Y margin
        /// </summary>
        private bool OutOfYMargin => Mathf.Abs(transform.position.y - player.transform.position.y) > margin.y;

        /// <summary>
        ///     Camera follow mouse cursor position
        /// </summary>
        private void TrackMouseMotion()
        {
            acceleration = Vector3.Lerp(acceleration, Camera.main.ScreenToViewportPoint(Input.mousePosition),
                Time.deltaTime);
            var target = transform.position + new Vector3(acceleration.x - 0.5f, acceleration.y - 0.5f, 0);
            transform.position = Vector3.Lerp(transform.position, target, 5f * Time.deltaTime);
            ClampCameraPosInField();
        }

        /// <summary>
        ///     Camera follow Player Gameobject position
        /// </summary>
        private void TrackPlayer()
        {
            if (!player) return;
            var targetX = transform.position.x;
            var targetY = transform.position.y;

            if (OutOfXMargin)
                targetX = Mathf.Lerp(transform.position.x, player.transform.position.x, smooth.x * Time.deltaTime);

            if (OutOfYMargin)
                targetY = Mathf.Lerp(transform.position.y, player.transform.position.y, smooth.y * Time.deltaTime);
            transform.position = new Vector3(targetX, targetY, transform.position.z);
            ClampCameraPosInField();
        }

        /// <summary>
        ///     Camera rotate to mouse cursor position
        /// </summary>
        private void
            TrackMouseRotation() //http://answers.unity3d.com/questions/149022/how-to-make-camera-move-with-the-mouse-cursors.html?childToView=1097525#answer-1097525
        {
            if (!m_camera) return;
            var sensitivity = 0.00001f;
            var vp = m_camera.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                m_camera.nearClipPlane));
            vp.x -= 0.5f;
            vp.y -= 0.5f;
            vp.x *= sensitivity;
            vp.y *= sensitivity;
            vp.x += 0.5f;
            vp.y += 0.5f;
            var sp = m_camera.ViewportToScreenPoint(vp);
            var v = m_camera.ScreenToWorldPoint(sp);
            transform.LookAt(v, Vector3.up);
        }

        /// <summary>
        ///     Clamp camera position in BoxCollider2D rect. Max and Min camera position dependet from collider size, camera size
        ///     and screen ratio;
        /// </summary>
        private void ClampCameraPosInField()
        {
            if (ClampPosition)
            {
                if (!ClampField) return;

                camVertSize = m_camera.orthographicSize;
                camHorSize = camVertSize * ScreenRatio;

                Vector2 m_bsize = ClampField.bounds.size / 2.0f;
                m_bsize -= new Vector2(camHorSize, camVertSize);

                var tFieldPosition = ClampField.transform.position;

                var maxY = tFieldPosition.y + m_bsize.y;
                var minY = tFieldPosition.y - m_bsize.y;

                var maxX = tFieldPosition.x + m_bsize.x;
                var minX = tFieldPosition.x - m_bsize.x;

                var posX = Mathf.Clamp(transform.position.x, minX, maxX);
                var posY = Mathf.Clamp(transform.position.y, minY, maxY);

                transform.position = new Vector3(posX, posY, transform.position.z);
            }
        }

        /// <summary>
        ///     Camera follow gyroscope acceleration
        /// </summary>
        private void TrackGyroscope()
        {
            var dir = Vector3.zero;
            dir.x = Input.acceleration.x;
            dir.y = Input.acceleration.y;
            if (dir.sqrMagnitude > 1) dir.Normalize();
            acceleration = dir; // acceleration = Vector3.Lerp(acceleration, dir, Time.deltaTime);
            var target = transform.position + new Vector3(acceleration.x, acceleration.y, 0);
            transform.position = Vector3.Lerp(transform.position, target, 1f * Time.deltaTime);
            ClampCameraPosInField();
        }

        /// <summary>
        ///     Camera follow touch drag direction
        /// </summary>
        /// <param name="tpea"></param>
        private void TrackTouchDrag(TouchPadEventArgs tpea)
        {
            if (track == TrackMode.Touch)
            {
                Vector3 dir = tpea.DragDirection;
                // dir.x = -tpea.DragDirection.x;
                // dir.y = -tpea.DragDirection.y;

                var target = transform.position + new Vector3(dir.x, dir.y, 0);
                transform.position = Vector3.Lerp(transform.position, target, 0.02f * Time.fixedDeltaTime);
                ClampCameraPosInField();
            }
        }

        /// <summary>
        ///     Camera follow keyboard input
        /// </summary>
        /// <param name="tpea"></param>
        private void TrackKeyboard()
        {
            var dir = Vector3.zero;
            dir.y = Input.GetAxis("Vertical");
            dir.x = Input.GetAxis("Horizontal");

            var target = transform.position + new Vector3(dir.x, dir.y, 0);
            transform.position = Vector3.Lerp(transform.position, target, 1.0f * Time.deltaTime);
            ClampCameraPosInField();
        }

        #region regular

        private void Awake()
        {
            if (!player) player = GameObject.FindGameObjectWithTag("Player");
            margin = new Vector2(3, 3);
            smooth = new Vector2(1, 1);

            m_camera = GetComponent<Camera>();
            Instance = this;
        }

        private void Start()
        {
            TouchPad.Instance.ScreenDragEvent += TrackTouchDrag;
        }

        private void LateUpdate()
        {
            switch (track)
            {
                case TrackMode.Player:
                    TrackPlayer();
                    break;
                case TrackMode.Mouse:
                    TrackMouseMotion();
                    break;
                case TrackMode.Gyroscope:
                    TrackGyroscope();
                    break;
                case TrackMode.Keyboard:
                    TrackKeyboard();
                    break;
            }
        }

        private void OnDesctroy()
        {
            TouchPad.Instance.ScreenDragEvent -= TrackTouchDrag;
        }

        #endregion regular
    }
}