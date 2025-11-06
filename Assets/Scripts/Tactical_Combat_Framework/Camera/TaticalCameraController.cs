using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tactics2D
{
    [RequireComponent(typeof(Camera))]
    public class TacticalCameraController : MonoBehaviour
    {
        [Header("Auto Focus Settings")]
        [SerializeField] private float smoothTime = 0.5f;
        [SerializeField] private float padding = 2f;
        [SerializeField] private float minZoom = 5f;
        [SerializeField] private float maxZoom = 25f;

        [Header("Manual Zoom")]
        [SerializeField] private bool allowManualZoom = true;
        [SerializeField] private float scrollSensitivity = 5f;

        private Camera cam;
        private Vector3 velocity;
        private float targetZoom;
        private bool autoFocus = true;

        private void Awake()
        {
            cam = GetComponent<Camera>();
            if (!cam.orthographic)
                cam.orthographic = true;
        }

        private void Start()
        {
            targetZoom = cam.orthographicSize;
        }

        private void LateUpdate()
        {
            if (autoFocus)
            {
                FocusOnAllUnits();
            }

            if (allowManualZoom)
            {
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (Mathf.Abs(scroll) > 0.01f)
                {
                    autoFocus = false; // player takes control
                    targetZoom -= scroll * scrollSensitivity;
                    targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
                }

                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * 4f);
            }
        }

        private void FocusOnAllUnits()
        {
            var units = FindObjectsOfType<Unit>().Where(u => u.IsAlive).ToList();
            if (units.Count == 0) return;

            // Compute bounding box of all units
            var minX = units.Min(u => u.transform.position.x);
            var maxX = units.Max(u => u.transform.position.x);
            var minY = units.Min(u => u.transform.position.y);
            var maxY = units.Max(u => u.transform.position.y);

            var center = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, transform.position.z);
            var sizeX = (maxX - minX) / 2f + padding;
            var sizeY = (maxY - minY) / 2f + padding;
            float desiredZoom = Mathf.Max(sizeX / cam.aspect, sizeY);

            desiredZoom = Mathf.Clamp(desiredZoom, minZoom, maxZoom);
            targetZoom = Mathf.Lerp(targetZoom, desiredZoom, Time.deltaTime * 2f);

            // Smoothly move and zoom
            transform.position = Vector3.SmoothDamp(transform.position, center, ref velocity, smoothTime);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * 3f);
        }

        /// <summary>
        /// Re-enables automatic focus (e.g., after teleport or big camera move).
        /// </summary>
        public void EnableAutoFocus(bool enable)
        {
            autoFocus = enable;
        }

        /// <summary>
        /// Call this to refocus immediately on all units.
        /// </summary>
        public void ForceRecenter()
        {
            autoFocus = true;
            FocusOnAllUnits();
        }
    }
}
