using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    [Header("Панели")]
    public GameObject pausePanel;
    public GameObject settingsPanel;

    [Header("Настройки")]
    public Slider sensSlider;
    public Slider distSlider;

    private ThirdPersonCamera camScript;
    private bool isPaused = false;

    void Start()
    {
        camScript = Object.FindFirstObjectByType<ThirdPersonCamera>();
        if (camScript != null)
        {
            sensSlider.value = camScript.sensitivity;
            distSlider.value = camScript.defaultDistance;
        }
        sensSlider.onValueChanged.AddListener(SetSensitivity);
        distSlider.onValueChanged.AddListener(SetDistance);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void SetSensitivity(float value)
    {
        if (camScript != null) camScript.sensitivity = value;
    }

    public void SetDistance(float value)
    {
        if (camScript != null) camScript.defaultDistance = value;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Выход из игры");
    }
}