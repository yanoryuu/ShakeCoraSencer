using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private ObstaclePooler pooler;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float minX = -850f;
    [SerializeField] private float maxX = 850f;
    [SerializeField] private float spawnY = 900f;
    [SerializeField] private Transform backgroundParent;

    private float timer;
    private bool isSpawning;

    // 👇 IngameView から参照するリスト
    public readonly List<GameObject> ActiveObstacles = new();

    public void Update()
    {
        if (!isSpawning) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            Spawn();
            timer = 0f;
        }
    }

    public void BeginSpawn()
    {
        timer = 0f;
        isSpawning = true;
    }

    public void StopSpawn()
    {
        isSpawning = false;
    }

    private void Spawn()
    {
        float spawnX = Random.Range(minX, maxX);
        var obstacle = pooler.GetFromPool();
        obstacle.transform.localPosition = new Vector3(spawnX, spawnY, 0f);
        obstacle.transform.SetParent(backgroundParent);
        obstacle.SetActive(true);

        // リストに追加
        ActiveObstacles.Add(obstacle);

        var releaser = obstacle.GetComponent<ObstacleReleaser>();
        if (releaser != null)
        {
            releaser.Setup(pooler, this); // ← spawner も渡す
        }
    }

    // 👇 Releaser から呼ばれる
    public void RemoveFromActive(GameObject obstacle)
    {
        ActiveObstacles.Remove(obstacle);
    }
}