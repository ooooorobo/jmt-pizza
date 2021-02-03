using UnityEngine;
using UnityEngine.SceneManagement;

public class MyStageManager: MonoBehaviour
{
    public void Restart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}