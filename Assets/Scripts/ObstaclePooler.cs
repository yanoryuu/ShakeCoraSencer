using System.Collections.Generic;
using UnityEngine;

public class ObstaclePooler : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private Transform backgroundParent; // Background をここに入れる

    private readonly Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        // プールを作成
        for (int i = 0; i < poolSize; i++)
        {
            var obj = Instantiate(obstaclePrefab, backgroundParent);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetFromPool()
    {
        GameObject obj = null;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            // 足りなくなったら増やしてもいい
            obj = Instantiate(obstaclePrefab, backgroundParent);
        }

        obj.SetActive(true);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}