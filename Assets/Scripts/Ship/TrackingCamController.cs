using System.Collections;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.Ship
{
    public class TrackingCamController : MonoBehaviour
    {
        public Transform Target;

        [SerializeField]
        private float maxUp;

        [SerializeField]
        private float[] zoomValues;

        [SerializeField]
        private TMP_Text rangeDisplay;

        [SerializeField]
        private TMP_Text zoomDisplay;

        [SerializeField]
        private int maxPreciseRange;

        [SerializeField]
        private int maxRange;

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

            float xRot = transform.localRotation.eulerAngles.x;

            if(xRot > 180f)
            {
                xRot -= 360f;
            }

            // clamp x rotation
            float clampedAngle = Mathf.Clamp(xRot, -maxUp, 0f);

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

            if (InputManager.Rangefind) GetRange();
        }

        private void CycleZoom()
        {
            _currentZoomStage++;
            _currentZoomStage = _currentZoomStage >= zoomValues.Length ? 0 : _currentZoomStage;

            _cam.fieldOfView = _baseFov / zoomValues[_currentZoomStage];

            // formatting string to display
            string text = "M:";
            if (zoomValues[_currentZoomStage].ToString().Length < 2)
            {
                text += "0";
            }

            text += zoomValues[_currentZoomStage].ToString() + "x";
            zoomDisplay.text = text;
        }

        private void GetRange()
        {
            // find range
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.rotation * Vector3.forward, out hit))
            {
                // format string to display
                string text = "R:";

                // formatting is hard
                int range = (int)Mathf.Floor(hit.distance);
                if (range <= maxPreciseRange)
                {
                    for(int i = 0; i < 5 - range.ToString().Length; i++)
                    {
                        text += "0";
                    }

                    text += range;
                }
                else if(range <= maxRange)
                {
                    for (int i = 0; i < 5 - (range / 1000).ToString().Length; i++)
                    {
                        text += "0";
                    }

                    text += range / 1000 + "K";
                }
                else
                {
                    text += "00000";
                }
                rangeDisplay.text = text;
            }
        }
    }
}