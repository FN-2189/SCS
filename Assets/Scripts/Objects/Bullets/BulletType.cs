using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet", menuName = "Bullet")]
public class BulletType : ScriptableObject
{
    public string Name;
    public Material Material;
    public float DamageMultiplier;
    public float Caliber;
}
