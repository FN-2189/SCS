using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Ship
{
    public class TrackingCamController : MonoBehaviour
    {
        public Transform Target;

        [SerializeField]
        private float maxUp;

        [SerializeField]
        float[] zoomValues;

        private Camera _cam;

        private float _baseFov;
        private int _currentZoomStage;

        private bool _zoomReleased;

        // Use this for initialization
        void Start()
        {
            _cam = GetComponent<Camera>();
            _baseFov = _cam.fieldOfView;
        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(Target);
            // set z rotation to 0
            transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, 0f));

            // clamp x rotation
            float clampedAngle = Mathf.Clamp(transform.localRotation.eulerAngles.x - 360f, -maxUp, 0f);

            transform.localRotation = Quaternion.Euler(new Vector3(clampedAngle, transform.localEulerAngles.y, 0f));

            if (!_zoomReleased)
            {
                if (InputManager.CycleZoom == 0) _zoomReleased = true;
            }
            else if(InputManager.CycleZoom > 0)
            {
                CycleZoom();
                _zoomReleased = false;
            }
        }

        public void CycleZoom()
        {
            _currentZoomStage++;
            _currentZoomStage = _currentZoomStage >= zoomValues.Length ? 0 : _currentZoomStage;

            _cam.fieldOfView = _baseFov / zoomValues[_currentZoomStage];
        }
    }
}