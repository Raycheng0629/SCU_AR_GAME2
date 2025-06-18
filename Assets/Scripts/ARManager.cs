using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;
using System.Collections.Generic;

namespace RAY
{
    public class ARManager : MonoBehaviour
    {
        [Header("生成的物件（小精靈 Prefab）")]
        public GameObject ghostPrefab;

        private ARRaycastManager arRay;
        private bool isPlaced;

        private GameObject dialoguePanel;
        private GameObject questionPanel;

        private void Awake()
        {
            arRay = GetComponent<ARRaycastManager>();

            dialoguePanel = GameObject.Find("DialoguePanel");
            questionPanel = GameObject.Find("QuestionPanel");

            if (dialoguePanel != null) dialoguePanel.SetActive(false);
            if (questionPanel != null) questionPanel.SetActive(false);
        }

        private void Update()
        {
            if (isPlaced) return;

#if UNITY_EDITOR
            // ✅ 電腦滑鼠點擊（模擬用）
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Input.mousePosition;
                Debug.Log($"🖱️ Editor 滑鼠點擊座標：{mousePos}");

                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.Log($"✅ 偵測到模擬平面，位置：{hit.point}");
                    GameObject ghost = Instantiate(ghostPrefab, hit.point, Quaternion.identity);
                    ghost.SetActive(true);
                    isPlaced = true;
                    StartCoroutine(SwitchToQuestion());
                }
                else
                {
                    Debug.LogWarning("❌ Editor 模式未擊中任何物件");
                }
            }
#else
            // ✅ 手機觸控
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Vector2 touchPos = Input.GetTouch(0).position;
                TryPlaceObject(touchPos);
            }
#endif
        }

        private void TryPlaceObject(Vector2 screenPos)
        {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (arRay.Raycast(screenPos, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                Debug.Log($"✅ 手機偵測平面，生成於：{hitPose.position}");
                GameObject ghost = Instantiate(ghostPrefab, hitPose.position, hitPose.rotation);
                ghost.SetActive(true);
                isPlaced = true;
                StartCoroutine(SwitchToQuestion());
            }
            else
            {
                Debug.LogWarning("❌ 未偵測到 AR 平面");
            }
        }

        private IEnumerator SwitchToQuestion()
        {
            if (dialoguePanel != null) dialoguePanel.SetActive(true);
            if (questionPanel != null) questionPanel.SetActive(false);

            yield return new WaitForSeconds(5f);

            if (dialoguePanel != null) dialoguePanel.SetActive(false);
            if (questionPanel != null) questionPanel.SetActive(true);

            UIManager ui = FindObjectOfType<UIManager>();
            if (ui != null) ui.ShowQuestionPanel();
        }
    }
}
