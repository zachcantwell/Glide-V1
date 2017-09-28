using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public static Manager Instance
    {
        get;
        set;
    }

    public Material m_playerMaterial;
    public Color[] m_playerColors = new Color[10];
    public GameObject[] m_playerTrails = new GameObject[10];

    public int m_currentLevel = 0;
    public int m_menuFocus = 0;

	void Start () {
        DontDestroyOnLoad(gameObject);
        Instance = this; 	
	}
	

}
