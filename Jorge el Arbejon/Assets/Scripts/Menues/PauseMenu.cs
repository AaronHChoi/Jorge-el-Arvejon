using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;

    private static PauseMenu instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (pauseMenu == null)
        {
            FindPauseMenu(); // Buscar el PauseMenu dentro de HUD
        }

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (pauseMenu == null)
        {
            FindPauseMenu(); // Buscar nuevamente si la referencia se perdió
        }

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
        else
        {
            Debug.LogError("PauseMenu no encontrado.");
        }
    }

    public void ResumeGame()
    {
        if (pauseMenu == null)
        {
            FindPauseMenu();
        }

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }

    void FindPauseMenu()
    {
        GameObject hud = GameObject.Find("HUD"); // Buscar el HUD en la escena

        if (hud != null)
        {
            pauseMenu = hud.transform.Find("PauseMenu")?.gameObject; // Buscar PauseMenu dentro de HUD
        }

        if (pauseMenu == null)
        {
            Debug.LogError("PauseMenu no encontrado dentro de HUD. Asegúrate de que el nombre es correcto y que está activo en la jerarquía.");
        }
    }
}
