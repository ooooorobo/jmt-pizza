using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeKeyManager : MonoBehaviour
{
    public GameObject escapePanelPrefab;
    public GameObject escapePanel;

    void Start()
    {
        InitializePanel();
            
        DontDestroyOnLoad(escapePanel.gameObject);
        DontDestroyOnLoad(gameObject);
        
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Loading") return;
        InitializePanel();
    }

    public void InitializePanel()
    {
        escapePanel = Instantiate(escapePanelPrefab);

        escapePanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(Application.Quit);
        escapePanel.transform.SetParent(GameObject.FindWithTag("Canvas").transform);
        RectTransform rt = escapePanel.GetComponent<RectTransform>();
        rt.offsetMin = new Vector2(0, 0);
        rt.offsetMax = new Vector2(0, 0);
        escapePanel.transform.localScale = new Vector3(1,1,1);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IGameManager gameManager = IGameManager.Instance();

            if (gameManager)
            {
                gameManager.PauseGame();
            }
            else
            {
                escapePanel.SetActive(true);
            }
        }
    }
}
