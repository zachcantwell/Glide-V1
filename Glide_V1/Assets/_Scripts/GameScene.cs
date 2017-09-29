using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour {

    private CanvasGroup m_fadeGroup;
    private float m_fadeDuration = 2f;
    private bool m_gameStarted = false;

    private void Start()
    {
        SceneManager.LoadScene(Manager.Instance.m_currentLevel.ToString(), LoadSceneMode.Additive);
        m_fadeGroup = FindObjectOfType<CanvasGroup>();
        m_fadeGroup.alpha = 1; 

    }

    private void Update()
    {
        if(Time.timeSinceLevelLoad <= m_fadeDuration)
        {
            m_fadeGroup.alpha = 1 - (Time.timeSinceLevelLoad / m_fadeDuration);
        }
        else if(!m_gameStarted)
        {
            m_fadeGroup.alpha = 0;
            m_gameStarted = true;
        }
    }
    public void CompleteLevel()
    {
        //complete level and save progress
        SaveManager.m_instance.CompleteLevel(Manager.Instance.m_currentLevel);

        // focus lvl selection
        Manager.Instance.m_menuFocus = 1;

        ExitScene();
    }

    public void ExitScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
