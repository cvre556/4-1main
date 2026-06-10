using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StoryStepManager : MonoBehaviour
{
    [Header("스텝별 이미지 (CanvasGroup) - 순서대로")]
    public CanvasGroup[] imageGroups;

    [Header("스텝별 텍스트 - 순서대로")]
    [TextArea(2, 5)]
    public string[] storyLines;

    [Header("표시할 TMP 텍스트 (하단 중앙 하나)")]
    public TMP_Text storyText;

    [Header("페이드인 시간 (초)")]
    public float fadeDuration = 0.5f;

    [Header("카드 슬라이드 거리 (px) - 위에서 내려오는 거리")]
    public float slideOffset = 20f;

    [Header("타이핑 속도 (초)")]
    public float charInterval = 0.03f;

    [Header("타이핑 완료 후 다음 스텝까지 대기 시간 (초)")]
    public float waitAfterTyping = 2f;

    [Header("완료 후 설정")]
    public string nextScene = "PC_LobbyScene";
    public bool unlockJobSelect = false;  // 오프닝씬에서만 true
    public bool closeServer = false;      // 성공/실패씬에서만 true

    private bool _finishedAll = false;

    // ──────────────────────────────────────
    // Start
    // ──────────────────────────────────────
    void Start()
    {
        // 모든 이미지 숨기기
        if (imageGroups != null)
        {
            foreach (var cg in imageGroups)
            {
                if (cg == null) continue;
                cg.alpha = 0f;
                cg.gameObject.SetActive(false);
            }
        }

        if (storyText != null)
            storyText.text = "";

        StartCoroutine(AutoPlay());
    }

    // ──────────────────────────────────────
    // AutoPlay — 이미지 누적 + 텍스트 순서 진행
    // ──────────────────────────────────────
    private IEnumerator AutoPlay()
    {
        int maxSteps = Mathf.Max(
            imageGroups != null ? imageGroups.Length : 0,
            storyLines != null ? storyLines.Length : 0
        );

        for (int i = 0; i < maxSteps; i++)
        {
            // 1) 이미지 페이드인 + 슬라이드 (이전 이미지 유지 → 누적)
            if (imageGroups != null &&
                i < imageGroups.Length &&
                imageGroups[i] != null)
            {
                yield return StartCoroutine(FadeInSlide(imageGroups[i]));
            }

            // 2) 하단 텍스트 타이핑
            if (storyText != null &&
                storyLines != null &&
                i < storyLines.Length)
            {
                yield return StartCoroutine(TypeLine(storyLines[i]));
            }

            // 3) 타이핑 완료 후 대기
            yield return new WaitForSeconds(waitAfterTyping);
        }

        OnAllStepsFinished();
    }

    // ──────────────────────────────────────
    // FadeInSlide — alpha 0→1 + 위에서 아래로 슬라이드
    // ──────────────────────────────────────
    private IEnumerator FadeInSlide(CanvasGroup cg)
    {
        RectTransform rt = cg.GetComponent<RectTransform>();

        // Inspector에서 설정한 목표 위치를 기준으로 시작 위치 계산
        Vector2 endPos = rt.anchoredPosition;
        Vector2 startPos = endPos + new Vector2(0f, slideOffset); // 위에서 내려옴

        cg.gameObject.SetActive(true);
        cg.alpha = 0f;
        rt.anchoredPosition = startPos;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);

            // EaseOut 커브: 처음 빠르고 끝에서 부드럽게 정착
            float ease = 1f - (1f - t) * (1f - t);

            cg.alpha = Mathf.Lerp(0f, 1f, ease);
            rt.anchoredPosition = Vector2.Lerp(startPos, endPos, ease);

            yield return null;
        }

        cg.alpha = 1f;
        rt.anchoredPosition = endPos;
    }

    // ──────────────────────────────────────
    // TypeLine — 한 글자씩 타이핑
    // ──────────────────────────────────────
    private IEnumerator TypeLine(string line)
    {
        storyText.text = "";

        for (int i = 0; i < line.Length; i++)
        {
            storyText.text += line[i];
            yield return new WaitForSeconds(charInterval);
        }
    }

    // ──────────────────────────────────────
    // OnAllStepsFinished — 씬 전환
    // ──────────────────────────────────────
    private void OnAllStepsFinished()
    {
        if (_finishedAll) return;
        _finishedAll = true;

        var gm = FindFirstObjectByType<GameManager>();

        // 직업 선택 잠금 해제 (오프닝 완료 시)
        if (unlockJobSelect)
            gm?.UnlockJobSelectServerRpc();

        // 서버 종료 (성공/실패씬 완료 시)
        if (closeServer)
        {
            var nm = FishNet.InstanceFinder.NetworkManager;
            if (nm != null && nm.IsServerStarted)
                nm.ServerManager.StopConnection(true);
        }

        SceneManager.LoadScene(nextScene);
    }
}