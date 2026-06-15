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

    private void Update()
    {
        if (gameEnded)
            return;

        surviveTime -= Time.deltaTime;

        timerText.text =
            "Time: " + Mathf.CeilToInt(surviveTime);

        if (surviveTime <= 0)
        {
            WinGame();
        }
    }

    public void SelectRoom(int roomID)
    {
        selectedRoom = roomID;

        statusText.text =
            "Selected Room " + (roomID + 1);
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

        LoseGame("Intruder entered!");
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

            statusText.text =
                "Door locked successfully!";
        }
        else
        {
            LoseGame("Wrong room locked!");
        }
    }

    public int GetSelectedRoom()
    {
        return selectedRoom;
    }

    void WinGame()
    {
        gameEnded = true;

        SceneManager.LoadScene("WinScene");
    }

    void LoseGame(string reason)
    {
        gameEnded = true;

        Debug.Log(reason);

        SceneManager.LoadScene("LoseScene");
    }
}