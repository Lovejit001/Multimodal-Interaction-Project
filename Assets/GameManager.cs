using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject[] rooms;
    public GameObject intruder;
    public Button lockButton;

    [Header("Mini-Game UI")]
    public GameObject miniGamePanel;
    public Slider[] sliders;
    public RectTransform[] targetVisuals;

    [Header("Gameplay Settings")]
    public float intruderSpawnInterval = 10f;
    public float timeToLock = 8f;

    private int currentRoom = 0;
    private int intruderRoom = -1;
    private Coroutine intruderTimerCoroutine;
    private GameObject activeIntruder;

    private bool isMiniGameActive = false;
    private float[] targetValues = new float[3];
    private float targetTolerance = 4f;

    void Start()
    {
        if (miniGamePanel != null) miniGamePanel.SetActive(false);
        SwitchToRoom(0);
        StartCoroutine(SpawnIntruderRoutine());
    }

    void Update()
    {
        if (isMiniGameActive)
        {
            CheckMiniGameSolution();
        }
    }

    public void SwitchToRoom(int roomIdx)
    {
        currentRoom = roomIdx;

        for (int i = 0; i < rooms.Length; i++) rooms[i].SetActive(i == currentRoom);

        UpdateIntruderVisibility();
    }

    public void PressLock()
    {
        if (intruderRoom == currentRoom && intruderRoom != -1)
        {
            Debug.Log($"Intruder detected in Room {currentRoom}! Initiating locking procedure...");
            StartMiniGame();
        }
        else
        {
            Debug.Log("No intruder in this room. Game Over!");
            EndGame();
        }
    }

    void StartMiniGame()
    {
        isMiniGameActive = true;
        miniGamePanel.SetActive(true);

        for (int i = 0; i < sliders.Length; i++)
        {
            targetValues[i] = Random.Range(0f, 100f);
            sliders[i].value = Random.value > 0.5f ? 0f : 100f;
            PositionTargetVisual(i);
        }
    }

    void PositionTargetVisual(int idx)
    {
        if (targetVisuals == null || idx >= targetVisuals.Length || targetVisuals[idx] == null) return;

        RectTransform visual = targetVisuals[idx];

        visual.anchorMin = new Vector2(0, 0);
        visual.anchorMax = new Vector2(0, 1);
        visual.pivot = new Vector2(0.5f, 0.5f);

        RectTransform sliderRect = sliders[idx].GetComponent<RectTransform>();
        float trackWidth = sliderRect.rect.width;

        float targetWidthPercent = (targetTolerance * 2f) / 100f;
        float actualPixelWidth = trackWidth * targetWidthPercent;

        float targetCenterPercent = targetValues[idx] / 100f;
        float actualPixelPosition = trackWidth * targetCenterPercent;

        visual.sizeDelta = new Vector2(actualPixelWidth, 0);
        visual.anchoredPosition = new Vector2(actualPixelPosition, 0);
    }

    void CheckMiniGameSolution()
    {
        bool allAligned = true;

        for (int i = 0; i < sliders.Length; i++)
        {
            float difference = Mathf.Abs(sliders[i].value - targetValues[i]);
            if (difference > targetTolerance)
            {
                allAligned = false;
                break;
            }
        }

        if (allAligned)
        {
            isMiniGameActive = false;
            miniGamePanel.SetActive(false);
            Debug.Log("Mini-game solved!");
            CatchIntruder();
        }
    }

    IEnumerator SpawnIntruderRoutine()
    {
        while (true)
        {
            while (intruderRoom != -1) yield return null;

            float randomCooldown = Random.Range(3f, intruderSpawnInterval);
            Debug.Log($"Facility secure. Next intruder spawning in {randomCooldown:F1} seconds...");
            yield return new WaitForSeconds(randomCooldown);

            if (lockButton.interactable == false) yield break; 

            intruderRoom = Random.Range(0, rooms.Length);
            Debug.Log($"Intruder spawned at Room {intruderRoom}!");
            SpawnIntruder();
            intruderTimerCoroutine = StartCoroutine(IntruderCountdown());
        }
    }

    IEnumerator IntruderCountdown()
    {
        yield return new WaitForSeconds(timeToLock);
        Debug.Log("Intruder escaped! Game Over!");
        EndGame();
    }

    void SpawnIntruder()
    {
        activeIntruder = Instantiate(intruder, rooms[intruderRoom].transform, false);
        activeIntruder.transform.SetAsLastSibling();

        RectTransform roomRect = rooms[intruderRoom].GetComponent<RectTransform>();
        float maxW = roomRect.rect.width / 2 - 50f;
        float maxH = roomRect.rect.height / 2 - 50f;

        Vector3 randomPos = new Vector3(Random.Range(-maxW, maxW), Random.Range(-maxH, maxH), 0);
        activeIntruder.transform.localPosition = randomPos;
        UpdateIntruderVisibility();
    }

    void UpdateIntruderVisibility()
    {
        if (activeIntruder != null) activeIntruder.SetActive(intruderRoom == currentRoom);
    }

    void CatchIntruder()
    {
        if (intruderTimerCoroutine != null) StopCoroutine(intruderTimerCoroutine);

        Destroy(activeIntruder);
        intruderRoom = -1;
        Debug.Log($"Successfully locked door in Room {currentRoom}");
    }

    void EndGame()
    {
        StopAllCoroutines();
        if (activeIntruder != null) Destroy(activeIntruder);
        if (lockButton != null) lockButton.interactable = false;
        isMiniGameActive = false;
        if (miniGamePanel != null) miniGamePanel.SetActive(false);
    }
}
