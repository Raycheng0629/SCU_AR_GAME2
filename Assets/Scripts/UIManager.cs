using System.Collections;
using TMPro;
using UnityEngine;

namespace RAY
{
    public class UIManager : MonoBehaviour
    {
        [Header("3D 顯示元件")]
        public TextMeshPro worldQuestionText;
        public TextMeshPro questionNumberText;
        public TextMeshPro timeText;
        public TextMeshPro correctCountText;

        [Header("選項文字")]
        public TextMeshPro[] optionTexts3D;

        [Header("選項方塊")]
        public Renderer[] optionCubes;

        [Header("音效管理")]
        public AudioManager audioManager;

        private QuestionManager questionManager;
        private int correctCount = 0;
        private float timeLimit = 5f;
        private float timer;
        private bool isQuizActive = false;

        private Color defaultColor;

        private void Start()
        {
            questionManager = GetComponent<QuestionManager>();
            defaultColor = optionCubes[0].material.color;
            StartQuiz();
        }

        private void Update()
        {
            if (!isQuizActive) return;

            timer -= Time.deltaTime;
            timeText.text = $"倒數：{Mathf.Ceil(timer)} 秒";

            if (timer <= 0f)
            {
                OnTimeOut();
            }
        }

        private void StartQuiz()
        {
            isQuizActive = true;
            correctCount = 0;
            questionManager.ResetQuestions();
            timer = timeLimit;
            ShowQuestion();
        }

        private void ShowQuestion()
        {
            if (questionManager.GetCurrentQuestionNumber() > questionManager.GetTotalQuestionCount())
            {
                isQuizActive = false;
                worldQuestionText.text = correctCount >= 3 ? "🎓 畢業快樂！" : "📚 學分不夠，請重補修！";
                if (audioManager != null) audioManager.PlayWin();
                return;
            }

            // 題目與資訊顯示
            worldQuestionText.text = questionManager.GetCurrentQuestionText();
            questionNumberText.text = $"第 {questionManager.GetCurrentQuestionNumber()} 題";
            correctCountText.text = $"答對題數：{correctCount}";
            timer = timeLimit;

            // 選項內容更新
            string[] options = questionManager.GetCurrentOptions();

            for (int i = 0; i < optionTexts3D.Length; i++)
            {
                if (i < options.Length)
                {
                    optionTexts3D[i].text = options[i];
                    optionCubes[i].material.color = defaultColor; // 重設顏色
                }
            }
        }

        public void OnClickAnswer(int index)
        {
            if (!isQuizActive) return;

            bool isCorrect = questionManager.CheckAnswer(index);
            if (isCorrect)
            {
                correctCount++;
                optionCubes[index].material.color = Color.green;
                if (audioManager != null) audioManager.PlayCorrect();
            }
            else
            {
                optionCubes[index].material.color = Color.red;
                if (audioManager != null) audioManager.PlayWrong();
            }

            StartCoroutine(NextQuestionDelay());
        }

        private void OnTimeOut()
        {
            StartCoroutine(NextQuestionDelay());
        }

        private IEnumerator NextQuestionDelay()
        {
            isQuizActive = false;
            yield return new WaitForSeconds(1f);

            if (questionManager.HasMoreQuestions())
            {
                questionManager.LoadNextQuestion();
                isQuizActive = true;
                ShowQuestion();
            }
            else
            {
                // 🟢 顯示結束畫面（畢業快樂 / 重補修）
                worldQuestionText.text = correctCount >= 3 ? "畢業快樂！" : "學分不夠，請重補修！";

                // 🟢 播放結尾音效（若有）
                if (audioManager != null) audioManager.PlayWin();
            }
        }


        public void ShowQuestionPanel()
        {
            StartQuiz();
        }
    }
}
