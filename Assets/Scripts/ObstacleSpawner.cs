using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private ObstaclePooler pooler;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float spawnInterval = 1.5f; // 何秒ごとに出すか
    [SerializeField] private float preSpawnOffset = 2f;  // 画面に映る少し前（上）で出す
    [SerializeField] private float minX = -850f;
    [SerializeField] private float maxX = 850f;

    private float timer;

    private void Reset()
    {
        mainCamera = Camera.main;
    }

    public void StartSpawning()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            Spawn();
            timer = 0f;
        }
    }

    private void Spawn()
    {
        // カメラの上端よりちょい上のY座標を調べる
        Vector3 top = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f));
        float spawnY = top.y + preSpawnOffset;

        // Xは指定範囲でランダム
        float spawnX = Random.Range(minX, maxX);

        var obstacle = pooler.GetFromPool();
        obstacle.transform.position = new Vector3(spawnX, spawnY, 0f);

        // 画面外に出たら戻すスクリプトにプールを渡す
        var releaser = obstacle.GetComponent<ObstacleReleaser>();
        if (releaser != null)
        {
            releaser.Setup(pooler, mainCamera);
        }
    }
}