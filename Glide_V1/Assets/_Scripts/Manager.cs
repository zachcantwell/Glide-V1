using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public static Manager Instance
    {
        get;
        set;
    }

    public int m_currentLevel = 0;
    public int m_menuFocus = 0;

	void Start () {
        DontDestroyOnLoad(gameObject);
        Instance = this; 	
	}
	

}
