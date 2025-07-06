using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsSpawner : MonoBehaviour
{
    public Asteroids asteriodsPrefab;
    [SerializeField] private float spawnRate = 2.0f;
    [SerializeField] private int spawnAmount = 1;
    [SerializeField] private float spawnDistance = 15.0f;
    [SerializeField] private float trajectoryVariance = 15.0f;
    private void Start()
    {
        InvokeRepeating(nameof(Spawn),spawnRate,spawnRate);
    }
    private void Spawn()
    {
        for(int i = 0; i < spawnAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * this.spawnDistance;
            Vector3 spawnPoint = this.transform.position + spawnDirection;

            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            Quaternion rotation =  Quaternion.AngleAxis(variance,Vector3.forward);

            Asteroids asteriod = Instantiate(asteriodsPrefab, spawnPoint, rotation);
            asteriod.size = Random.Range(asteriod.minSize,asteriod.maxSize);
            asteriod.SetTrajectory(rotation * -spawnDirection);
        }
    }
}
