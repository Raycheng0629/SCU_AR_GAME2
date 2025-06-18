using UnityEngine;


public class QuestionManager : MonoBehaviour
{
    [System.Serializable]
    public class QuestionData
    {
        public string question;                 // 題目文字
        public string[] options = new string[4]; // 四個選項
        public int correctIndex;               // 正確答案索引 (0 ~ 3)
    }

    [Header("題庫設定")]
    public QuestionData[] questions;

    private int currentQuestionIndex = 0;

    // ✅ 取得目前題目文字
    public string GetCurrentQuestionText()
    {
        if (currentQuestionIndex < questions.Length)
        {
            return questions[currentQuestionIndex].question;
        }
        return "題目錯誤";
    }

    // ✅ 取得目前選項內容
    public string[] GetCurrentOptions()
    {
        if (currentQuestionIndex < questions.Length)
        {
            return questions[currentQuestionIndex].options;
        }
        return new string[4];
    }

    // ✅ 判斷答案是否正確
    public bool CheckAnswer(int index)
    {
        if (currentQuestionIndex < questions.Length)
        {
            return index == questions[currentQuestionIndex].correctIndex;
        }
        return false;
    }

    // ✅ 是否還有下一題
    public bool HasMoreQuestions()
    {
        return currentQuestionIndex < questions.Length - 1;
    }

    // ✅ 載入下一題
    public void LoadNextQuestion()
    {
        if (HasMoreQuestions())
        {
            currentQuestionIndex++;
        }
    }

    // ✅ 取得當前題目編號（顯示用）
    public int GetCurrentQuestionNumber()
    {
        return currentQuestionIndex + 1;
    }

    // ✅ 總題目數
    public int GetTotalQuestionCount()
    {
        return questions.Length;
    }

    // ✅ 重置（如有重補修需求）
    public void ResetQuestions()
    {
        currentQuestionIndex = 0;
    }
}
