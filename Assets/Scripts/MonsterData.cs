using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    public float maxHP = 100f;
    public float damage = 10f;
    public float speed = 2f;
}
