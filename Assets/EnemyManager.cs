using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{

    public Transform[] m_SpawnPoints;
    public Transform[] m_PatrolPoints;

    public GameObject[] m_EnemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        SpawnNewEnemy();
    }

    private void OnEnable()
    {
        Enemy.OnEnemyKilled += SpawnNewEnemy;
    }

    private void SpawnNewEnemy()
    {
        Instantiate(m_EnemyPrefab[Random.Range(0, m_SpawnPoints.Length)],
            m_SpawnPoints[Random.Range(0, m_SpawnPoints.Length)].transform.position,
            Quaternion.identity);
    }
}

