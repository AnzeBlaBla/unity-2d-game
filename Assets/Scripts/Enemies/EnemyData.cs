using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "NimamoŠe/Enemy Data", order = 1)]
public class EnemyData : ScriptableObject
{
    public GameObject prefab;
    public Sound spawnSound;
}
