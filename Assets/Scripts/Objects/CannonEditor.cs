using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    [CustomEditor(typeof(Cannon))]
    public class CannonEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            var cannon = target as Cannon;

            cannon.traverseSpeed = EditorGUILayout.Vector2Field("Traverse Speed", new Vector2(Mathf.Max(0f, cannon.traverseSpeed.x), Mathf.Max(0f, cannon.traverseSpeed.y)));

            cannon.CanRotateAround = EditorGUILayout.Toggle("Can Rotate 360°", cannon.CanRotateAround);

            EditorGUI.indentLevel++;
            using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(!cannon.CanRotateAround)))
            {
                if (group.visible)
                {
                    cannon.MaxTraverseLeft = EditorGUILayout.FloatField("Max Traverse Left", Mathf.Max(0f, cannon.MaxTraverseLeft));
                    cannon.MaxTraverseRight = EditorGUILayout.FloatField("Max Traverse Right", Mathf.Max(0f, cannon.MaxTraverseRight));
                }
            }

            cannon.MaxTraverseUp = EditorGUILayout.FloatField("Max Traverse Up", Mathf.Max(0f, cannon.MaxTraverseUp));
            cannon.MaxTraverseDown = EditorGUILayout.FloatField("Max Traverse Down", Mathf.Max(0f, cannon.MaxTraverseDown));

            EditorGUI.indentLevel--;
            EditorGUILayout.Space(20f);

            cannon.Turret = (GameObject)EditorGUILayout.ObjectField(label: "Turret", obj: cannon.Turret, objType:typeof(GameObject), allowSceneObjects: false);
            cannon.BarrelMount = (GameObject)EditorGUILayout.ObjectField(label: "Barrle Mount", obj: cannon.BarrelMount, objType: typeof(GameObject), allowSceneObjects: false);
            cannon.Barrel = (GameObject)EditorGUILayout.ObjectField(label: "Barrel", obj: cannon.Barrel, objType: typeof(GameObject), allowSceneObjects: false);

            EditorGUILayout.Space(10f);
            cannon.BarrelMountOffset = EditorGUILayout.Vector3Field("Barrel Mount Offset", cannon.BarrelMountOffset);
            cannon.BarrelOffset = EditorGUILayout.Vector3Field("Barrel Offset", cannon.BarrelOffset);
            cannon.BulletSpawnOffset = EditorGUILayout.FloatField("Bullet Spawn Offset", cannon.BulletSpawnOffset);

            EditorGUILayout.Space(10f);
            cannon.BulletType = (GameObject)EditorGUILayout.ObjectField(label: "Bullet Type", obj: cannon.BulletType, objType: typeof(GameObject), allowSceneObjects: false);
            cannon.FireRate = EditorGUILayout.FloatField("Fire Rate", Mathf.Max(0f, cannon.FireRate));
            cannon.MuzzleVelocity = EditorGUILayout.FloatField("Muzzle Velocity", Mathf.Max(0f, cannon.MuzzleVelocity));

        }
    }
}