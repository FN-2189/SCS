using Assets.Scripts.Util.Lib;
using UnityEditor;

#if UNITY_EDITOR
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
#endif
