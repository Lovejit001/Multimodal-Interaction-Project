using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Camera Images")]
    public Image[] cameraImages;

    [Header("Sprites")]
    public Sprite[] normalSprites;
    public Sprite staticSprite;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI statusText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip staticSound;

    private int selectedRoom = -1;

    private int intruderRoom = -1;

    private float surviveTime = 60f;

    private bool gameEnded = false;

    private Coroutine intruderCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(RandomIntruders());
    }

    public float lockShuffleInterval = 2f;
    private float lockShuffleTimer;
    private void Update()
    {
        if (gameEnded)
            return;

        surviveTime -= Time.deltaTime;

        timerText.text =
            "Time: " + Mathf.CeilToInt(surviveTime);

        lockShuffleTimer += Time.deltaTime;
        if (lockShuffleTimer >= lockShuffleInterval)
        {
            ShuffleLocks();
            lockShuffleTimer = 0f;
        }

        if (surviveTime <= 0)
        {
            WinGame();
        }
    }

    public RoomButton[] roomButtons;
    public void SelectRoom(int roomID)
    {
        selectedRoom = roomID;

        for (int i = 0; i < roomButtons.Length; i++)
        {
            roomButtons[i].SetSelected(roomButtons[i].roomID == roomID);
        }

        statusText.text = "Selected Room " + (roomID + 1);
    }

    IEnumerator RandomIntruders()
    {
        while (!gameEnded)
        {
            yield return new WaitForSeconds(
                Random.Range(4f, 10f));

            SpawnIntruder();
        }
    }

    void SpawnIntruder()
    {
        if (intruderRoom != -1)
            return;

        intruderRoom = Random.Range(0, 4);

        cameraImages[intruderRoom].sprite =
            staticSprite;

        audioSource.PlayOneShot(staticSound);

        intruderCoroutine =
            StartCoroutine(IntruderTimer());
    }

    IEnumerator IntruderTimer()
    {
        yield return new WaitForSeconds(5f);

        LoseGame();
    }

    public void TryLockRoom(int roomID)
    {
        if (intruderRoom == -1)
            return;

        if (roomID == intruderRoom)
        {
            StopCoroutine(intruderCoroutine);

            cameraImages[intruderRoom].sprite =
                normalSprites[intruderRoom];

            intruderRoom = -1;

            ShuffleCameras();

            statusText.text =
                "Door locked successfully!";
        }
        else
        {
            LoseGame();
        }
    }

    public int GetSelectedRoom()
    {
        return selectedRoom;
    }

    public GameObject WinPanel;
    void WinGame()
    {
        gameEnded = true;
        WinPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public GameObject LosePanel;
    public VoiceInput voiceInput;
    public void LoseGame()
    {
        gameEnded = true;
        LosePanel.SetActive(true);
        voiceInput.ForceStopListening();
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public RectTransform[] Locks;

    public void ShuffleLocks()
    {
        for (int i = 0; i < Locks.Length; i++)
        {
            int randomIndex = Random.Range(i, Locks.Length);

            RectTransform temp = Locks[i];
            Locks[i] = Locks[randomIndex];
            Locks[randomIndex] = temp;
        }

        for (int i = 0; i < Locks.Length; i++)
        {
            Locks[i].SetSiblingIndex(i);
        }
    }

    public RectTransform[] cameraScreens;

    public void ShuffleCameras()
    {
        for (int i = 0; i < cameraScreens.Length; i++)
        {
            int randomIndex =
                Random.Range(i, cameraScreens.Length);

            RectTransform temp =
                cameraScreens[i];

            cameraScreens[i] =
                cameraScreens[randomIndex];

            cameraScreens[randomIndex] =
                temp;
        }

        for (int i = 0; i < cameraScreens.Length; i++)
        {
            cameraScreens[i].SetSiblingIndex(i);
        }
    }
}