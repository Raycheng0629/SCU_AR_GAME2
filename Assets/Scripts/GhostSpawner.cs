using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab;

    public void SpawnGhost()
    {
        // 產生在攝影機前方 2 公尺處
        Vector3 spawnPos = Camera.main.transform.position + Camera.main.transform.forward * 2f;
        Instantiate(ghostPrefab, spawnPos, Quaternion.identity);
    }
}
