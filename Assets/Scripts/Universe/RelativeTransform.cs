using Assets.Scripts.Util.Lib;
using UnityEngine;


// Big WIP
namespace Assets.Scripts.Universe
{
    public class RelativeTransform : MonoBehaviour
    {
        public Vector3d AbsPosition;

        private void Awake()
        {
            AbsPosition = new Vector3d(transform.position);
        }
    }
}
