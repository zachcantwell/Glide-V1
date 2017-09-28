using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreLoader : MonoBehaviour {

    private CanvasGroup m_fadeGroup;
    private float m_loadTime;
    private float m_minimumLogoTime = 3.0f; // Min time of scene

    void Start () {
        //grab only canvas grp in scene
        m_fadeGroup = FindObjectOfType<CanvasGroup>();

        m_fadeGroup.alpha = 1f;

        if(Time.time < m_minimumLogoTime)
        {
            m_loadTime = m_minimumLogoTime;
        }
        else
        {
            m_loadTime = Time.time; 
        }
	}
	
	// Update is called once per frame
	void Update () {
        //Fade in
        if(Time.time < m_minimumLogoTime)
        {
            m_fadeGroup.alpha = 1 - Time.time; 
        }

        //fade out
        if(Time.time > m_minimumLogoTime && m_loadTime != 0)
        {
            m_fadeGroup.alpha = Time.time - m_minimumLogoTime;
            if(m_fadeGroup.alpha >= 1)
            {
                SceneManager.LoadScene("MainMenu"); 
            }   
        }
	}
}
