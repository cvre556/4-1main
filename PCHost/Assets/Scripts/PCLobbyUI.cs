using FishNet;
using FishNet.Connection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PCLobbyUI : MonoBehaviour
{
    [Header("¿¬°á UI")]
    public TextMeshProUGUI connectedText;

    [Header("Å¸ÀÌ¸Ó UI")]
    public TextMeshProUGUI timerText;
    private float remainingTime = 15 * 60f; // 15ºÐ
    private bool timerRunning = false;

    [Header("¿ëÀÇÀÚ ¹öÆ° (ÁÂÃø ÇÏ´Ü)")]
    public Button suspectButton;

    [Header("¿ëÀÇÀÚÆ¯Á¤À¸·Î ¹Ù·Î°¡±â ¹öÆ°")]
    public Button goToSuspectSceneButton;

    [Header("¿ëÀÇÀÚ ÆË¾÷")]
    public GameObject suspectPopup;
    public Button closePopupButton;
    public Button prevButton;
    public Button nextButton;

    [Header("¿ëÀÇÀÚ ÇÁ·ÎÇÊ Ç¥½Ã")]
    public Image suspectImage;
    public GameObject suspectImagePlaceholder;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI ageText;
    public TextMeshProUGUI jobText;
    public TextMeshProUGUI featureText;
    public TextMeshProUGUI pageText;

    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    // [Ãß°¡] ¿ëÀÇÀÚ »çÁø ¹è¿­ - Inspector¿¡¼­ Á÷Á¢ ¿¬°á
    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    [Header("¿ëÀÇÀÚ »çÁø (¼ø¼­´ë·Î Inspector¿¡¼­ ¿¬°á)")]
    public Sprite[] suspectPhotos;
    // ¼ø¼­: 0=¼öÁý°¡, 1=°æºñ¿ø, 2=ºñ¼­, 3=Ã»¼ÒºÎ
    // Assets/Images Æú´õÀÇ ÀÌ¹ÌÁö¸¦ ÀÌ ¹è¿­¿¡ µå·¡±×ÇÏ¿© ¿¬°áÇÏ¼¼¿ä.

    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    // ¿ëÀÇÀÚ µ¥ÀÌÅÍ
    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    private SuspectData[] suspects;

    private int currentIndex = 0;

    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    // ³×Æ®¿öÅ©
    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    private int connectedCount = 0;
    private int maxPlayers = 0;

    void Start()
    {
        // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
        // [Ãß°¡] Inspector¿¡¼­ ¿¬°áÇÑ »çÁøÀ¸·Î ¿ëÀÇÀÚ µ¥ÀÌÅÍ ÃÊ±âÈ­
        // suspectPhotos ¹è¿­ÀÌ ¿¬°áµÇ¾î ÀÖÀ¸¸é °¢ ¿ëÀÇÀÚ¿¡ »çÁø Àû¿ë
        // suspectPhotos[0] = ¼öÁý°¡, [1] = °æºñ¿ø, [2] = ºñ¼­, [3] = Ã»¼ÒºÎ
        // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
        suspects = new SuspectData[]
        {
            new SuspectData(
                "¼öÁý°¡",
                38,
                "º¸¼® ¼öÁý°¡",
                "¹ÝÁö ±¸¸Å 3È¸ °ÅÀý ´çÇÔ.",
                GetPhoto(0)
            ),
            new SuspectData(
                "°æºñ¿ø ±èÁØÈ£",
                34,
                "ÀºÇà º¸¾È °æºñ",
                "´çÀÏ È­Àå½Ç 13ºÐ °ø¹é.",
                GetPhoto(1)
            ),
            new SuspectData(
                "ºñ¼­ ÀÓ¼¼¾Æ",
                31,
                "CEO Àü¼Ó ºñ¼­",
                "ÀÎ»ç¹ß·É 7³â ¹«±âÇÑ ¿¬±â.",
                GetPhoto(2)
            ),
            new SuspectData(
                "Ã»¼ÒºÎ",
                27,
                "ÀºÇà Ã»¼Ò ´ã´ç",
                "Æø¾ð ÇÇÇØ ÀÌ·Â ´Ù¼ö.",
                GetPhoto(3)
            ),
        };
        // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬

        // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
        // ³×Æ®¿öÅ© ¿¬°á Ä«¿îÆ®
        // NetworkManager null Ã¼Å© Ãß°¡ (Editor Play ¸ðµå ¿À·ù ¹æÁö)
        // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
        if (InstanceFinder.ServerManager != null)
        {
            InstanceFinder.ServerManager.OnRemoteConnectionState += OnClientConnected;
            connectedCount = Mathf.Max(0, InstanceFinder.ServerManager.Clients.Count - 1);
        }

        // NetworkManager null Ã¼Å© ÈÄ Tugboat Á¢±Ù
        if (InstanceFinder.NetworkManager != null)
        {
            var tugboat = InstanceFinder.NetworkManager
                .GetComponent<FishNet.Transporting.Tugboat.Tugboat>();
            if (tugboat != null)
                maxPlayers = tugboat.GetMaximumClients() - 1;
        }

        UpdateConnectedText();

        // ÆË¾÷ ÃÊ±â ¼û±è
        suspectPopup.SetActive(false);

        // ¹öÆ° ÀÌº¥Æ®
        suspectButton.onClick.AddListener(OpenSuspectPopup);
        closePopupButton.onClick.AddListener(CloseSuspectPopup);
        prevButton.onClick.AddListener(ShowPrev);
        nextButton.onClick.AddListener(ShowNext);
        goToSuspectSceneButton.onClick.AddListener(GoToSuspectScene);

        // Å¸ÀÌ¸Ó ½ÃÀÛ
        timerRunning = true;
    }

    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    // [Ãß°¡] »çÁø ¹è¿­¿¡¼­ ¾ÈÀüÇÏ°Ô Sprite °¡Á®¿À±â
    // index ¹üÀ§¸¦ ¹þ¾î³ª°Å³ª ¹è¿­ÀÌ ¾øÀ¸¸é null ¹ÝÈ¯
    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    private Sprite GetPhoto(int index)
    {
        if (suspectPhotos == null) return null;
        if (index < 0 || index >= suspectPhotos.Length) return null;
        return suspectPhotos[index];
    }

    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    // Å¸ÀÌ¸Ó
    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    void Update()
    {
        if (!timerRunning) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            timerRunning = false;
            UpdateTimerText();
            GoToSuspectScene();
            return;
        }

        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";

        // 3ºÐ ÀÌÇÏ¸é »¡°£»öÀ¸·Î °æ°í
        timerText.color = remainingTime <= 180f ? Color.red : Color.white;
    }

    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    // ¿ëÀÇÀÚÆ¯Á¤À¸·Î ÀÌµ¿
    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    void GoToSuspectScene()
    {
        timerRunning = false;
        SceneManager.LoadScene("SuspectScene");
    }

    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    // ÆË¾÷ ¿­±â / ´Ý±â
    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    void OpenSuspectPopup()
    {
        currentIndex = 0;
        suspectPopup.SetActive(true);
        ShowCurrentSuspect();
    }

    void CloseSuspectPopup()
    {
        suspectPopup.SetActive(false);
    }

    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    // ÆäÀÌÁö ³Ñ±â±â
    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    void ShowPrev()
    {
        currentIndex = (currentIndex - 1 + suspects.Length) % suspects.Length;
        ShowCurrentSuspect();
    }

    void ShowNext()
    {
        currentIndex = (currentIndex + 1) % suspects.Length;
        ShowCurrentSuspect();
    }

    void ShowCurrentSuspect()
    {
        SuspectData s = suspects[currentIndex];

        nameText.text = $"ÀÌ¸§: {s.Name}";
        ageText.text = $"³ªÀÌ: {s.Age}¼¼";
        jobText.text = $"Á÷¾÷: {s.Job}";
        featureText.text = $"Æ¯Â¡: {s.Feature}";
        pageText.text = $"{currentIndex + 1} / {suspects.Length}";

        if (s.Photo != null)
        {
            suspectImage.sprite = s.Photo;
            suspectImage.gameObject.SetActive(true);
            suspectImagePlaceholder.SetActive(false);
        }
        else
        {
            suspectImage.gameObject.SetActive(false);
            suspectImagePlaceholder.SetActive(true);
        }

        prevButton.gameObject.SetActive(suspects.Length > 1);
        nextButton.gameObject.SetActive(suspects.Length > 1);
    }

    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    // ³×Æ®¿öÅ© ÀÌº¥Æ®
    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    void OnClientConnected(NetworkConnection conn,
        FishNet.Transporting.RemoteConnectionStateArgs args)
    {
        if (conn.ClientId == 0) return;

        if (args.ConnectionState == FishNet.Transporting.RemoteConnectionState.Started)
        {
            connectedCount++;
            UpdateConnectedText();
            StartCoroutine(LoadJobSelectScene(conn));
        }
        else if (args.ConnectionState == FishNet.Transporting.RemoteConnectionState.Stopped)
        {
            connectedCount = Mathf.Max(0, connectedCount - 1);
            UpdateConnectedText();

            var gm = FindFirstObjectByType<GameManager>();
            gm?.OnClientDisconnected(conn.ClientId);
        }
    }

    IEnumerator LoadJobSelectScene(NetworkConnection conn)
    {
        yield return new WaitForSeconds(0.5f);
        var gm = FindFirstObjectByType<GameManager>();
        gm?.SetMaxPlayersAndLoadSceneForConnServerRpc(maxPlayers, conn.ClientId);
    }

    void UpdateConnectedText()
    {
        connectedText.text = $"¿¬°áµÈ ÀÎ¿ø: {connectedCount} / {maxPlayers}";
    }

    void OnDestroy()
    {
        // NetworkManager null Ã¼Å© ÈÄ ÀÌº¥Æ® ÇØÁ¦
        if (InstanceFinder.NetworkManager != null &&
            InstanceFinder.ServerManager != null)
            InstanceFinder.ServerManager.OnRemoteConnectionState -= OnClientConnected;
    }
}