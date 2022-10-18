using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Ship
{
    public class TrackingCamController : MonoBehaviour
    {
        public Transform Target;

        [SerializeField]
        private float maxUp;

        // Use this for initialization
        void Start()
        {

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

            Debug.Log(transform.localRotation.eulerAngles);
        }
    }
}