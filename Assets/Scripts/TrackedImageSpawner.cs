using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class TrackedImageSpawner : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public GameObject ghostPrefab;

    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // 新增
        foreach (var trackedImage in eventArgs.added)
        {
            SpawnOrUpdatePrefab(trackedImage);
        }

        // 更新
        foreach (var trackedImage in eventArgs.updated)
        {
            SpawnOrUpdatePrefab(trackedImage);
        }

        // 移除
        foreach (var trackedImage in eventArgs.removed)
        {
            if (spawnedPrefabs.TryGetValue(trackedImage.referenceImage.name, out GameObject go))
            {
                go.SetActive(false);
            }
        }
    }

    private void SpawnOrUpdatePrefab(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (!spawnedPrefabs.ContainsKey(imageName))
        {
            GameObject newPrefab = Instantiate(ghostPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
            newPrefab.name = imageName;
            spawnedPrefabs.Add(imageName, newPrefab);
        }

        GameObject obj = spawnedPrefabs[imageName];
        obj.transform.position = trackedImage.transform.position;
        obj.transform.rotation = trackedImage.transform.rotation;
        obj.SetActive(true);
    }
}
