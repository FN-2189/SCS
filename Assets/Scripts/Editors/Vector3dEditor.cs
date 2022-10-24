using Assets.Scripts.Util.Lib;
using UnityEditor;

namespace Assets.Scripts.Editors
{
    [CustomEditor(typeof(Vector3d))]
    public class Vector3dEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.TextArea("TestTestTest");
        }
    }
}
