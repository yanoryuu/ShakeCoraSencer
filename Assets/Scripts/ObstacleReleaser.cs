using UnityEngine;

public class ObstacleReleaser : MonoBehaviour
{
    private ObstaclePooler pooler;
    private Camera mainCamera;
    [SerializeField] private float hideOffset = 2f; // ちょっと下に抜けたら消す

    public void Setup(ObstaclePooler pooler, Camera cam)
    {
        this.pooler = pooler;
        this.mainCamera = cam;
    }

    public void Release()
    {
        if (mainCamera == null || pooler == null) return;

        // カメラの下端
        Vector3 bottom = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f));
        float limitY = bottom.y - hideOffset;

        if (transform.position.y < limitY)
        {
            pooler.ReturnToPool(gameObject);
        }
    }
    
}