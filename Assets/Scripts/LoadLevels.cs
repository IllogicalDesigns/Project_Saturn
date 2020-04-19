using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadLevels : MonoBehaviour
{
    public void LoadLevel(string newLevel) {
        SceneManager.LoadScene(newLevel);
    }

    public void RestartCurrentLevel() {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

    }
}
