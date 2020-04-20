using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadLevels : MonoBehaviour
{
    [SerializeField] string levelToLoad;
    public void LoadLevel(string newLevel) {
        Time.timeScale = 1f;
        SceneManager.LoadScene(newLevel);
    }

    public void LoadLevelGenString() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelToLoad);
    }

    public void RestartCurrentLevel() {
        Time.timeScale = 1f;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

    }
}
