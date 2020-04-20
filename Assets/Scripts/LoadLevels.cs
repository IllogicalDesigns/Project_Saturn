using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadLevels : MonoBehaviour
{
    [SerializeField] string levelToLoad;
    public void LoadLevel(string newLevel) {
        SceneManager.LoadScene(newLevel);
    }

    public void LoadLevelGenString() {
        SceneManager.LoadScene(levelToLoad);
    }

    public void RestartCurrentLevel() {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

    }
}
