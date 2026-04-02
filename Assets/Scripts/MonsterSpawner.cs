using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawner : MonoBehaviour
{
    public Monster zombunnyPrefab;
    public Monster zombearPrefab;
    public Monster hellephantPrefab;

    public MonsterData[] monsterData;
    public Transform[] spawnPoints;
    public float maxDistance = 15f;

    private List<Monster> monsters = new List<Monster>();

    private int wave;
    
    private void Update()
    {
        if (monsters.Count == 0 && monsters.Count < 30)
        {
            SpawnWave();
        }
    }
    private void SpawnWave()
    {
        wave++;
        int count = Mathf.RoundToInt(wave * 5.5f);

        for (int i = 0; i < count; i++)
        {
            SpawnMonster();
        }

    }

    private void SpawnMonster()
    {
        var randomPos = Random.insideUnitSphere * maxDistance;

        // 해당 위치 근처에 유효한 NavMesh가 없으면 생성 취소
        if (!NavMesh.SamplePosition(randomPos, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
        {
            return;
        }

        randomPos = hit.position;
        var prefabIndex = GetWeightedRandomIndex();
        var prefab = prefabIndex switch
        {
            0 => zombunnyPrefab,
            1 => zombearPrefab,
            2 => hellephantPrefab,
            _ => null
        };  
        var monster = Instantiate(prefab, randomPos, Quaternion.identity);  
        monster.Setup(monsterData[prefabIndex]);
        monsters.Add(monster);

        monster.onDead.AddListener(() => monsters.Remove(monster));
        //monster.onDead.AddListener(() => gameManager.AddScore(100));
        
    }

    private int GetWeightedRandomIndex()
    {
        float[] weights = 
        {
            0.6f, // zombunny
            0.3f, // zombear
            0.1f  // hellephant
        };

        float totalWeight = 0f; 
        foreach (var weight in weights)
        {
            totalWeight += weight;
        }

        float randomValue = Random.value * totalWeight;

        float cumulativeWeight = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue < cumulativeWeight)
            { 
                return i;
            }
        }
        return weights.Length - 1;  
    }
}
