using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour {

    private CanvasGroup m_fadeGroup;
    private float m_fadeDuration = 2f;
    private bool m_gameStarted = false;

    public Transform m_arrow;
    private Transform m_playerTransform;
    public Objective m_Objective;
    private Vector3 m_ringPos;

    private void Start()
    {
        m_playerTransform = FindObjectOfType<PlayerMotor>().transform;
        SceneManager.LoadScene(Manager.Instance.m_currentLevel.ToString(), LoadSceneMode.Additive);
        m_fadeGroup = FindObjectOfType<CanvasGroup>();
        m_fadeGroup.alpha = 1; 
    }

    private void Update()
    {
        if(m_Objective != null)
        {
            //rotate arrow
            Debug.Log(m_Objective + " is set");

            if(m_Objective.GetCurrentRing().position != null)
            {
                 m_ringPos = m_Objective.GetCurrentRing().position;
            }
            Vector3 dir = m_playerTransform.InverseTransformPoint(m_ringPos);
            float a = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            a += 180;
            m_arrow.transform.localEulerAngles = new Vector3(0f, 180f, a);
        }
        else
        {
            Debug.Log("m_Objective = null");
        }

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
