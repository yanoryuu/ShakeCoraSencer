using UnityEngine;

public class ObstacleReleaser : MonoBehaviour
{
    private ObstaclePooler pooler;
    private ObstacleSpawner spawner;
    [SerializeField] private float hideOffset = 2f;

    public void Setup(ObstaclePooler pooler, ObstacleSpawner spawner)
    {
        this.pooler = pooler;
        this.spawner = spawner;
    }

    public void Update()
    {
        if (pooler == null) return;

        if (transform.position.y < -126f)
        {
            pooler.ReturnToPool(gameObject);
            spawner?.RemoveFromActive(gameObject);
        }
    }
}