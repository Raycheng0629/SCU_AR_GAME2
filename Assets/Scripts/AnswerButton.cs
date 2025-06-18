using UnityEngine;
using RAY; // ✅ UIManager 在 RAY 命名空間中

public class AnswerButton : MonoBehaviour
{
    public int answerIndex; // 此按鈕代表哪個選項 (0 ~ 3)

    private UIManager uiManager;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    private void OnMouseDown()
    {
        if (uiManager != null)
        {
            uiManager.OnClickAnswer(answerIndex);
        }
        else
        {
            Debug.LogWarning("❗ UIManager 未找到，請確認該物件有掛 UIManager.cs 且在場景中啟用");
        }
    }
}
