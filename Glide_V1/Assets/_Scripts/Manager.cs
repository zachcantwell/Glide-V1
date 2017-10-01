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

    private Dictionary<int, Vector2> m_activeTouches = new Dictionary<int, Vector2>();

	void Start () {
        DontDestroyOnLoad(gameObject);
        Instance = this; 	
	}

    public Vector3 GetPlayerInput()
    {
        if(SaveManager.m_instance.m_state.m_usingAccelerometor)
        {
            Vector3 a = Input.acceleration;
            a.y = a.z;
            return a; 
        }

        //if all released
        if(Input.touches.Length == 0)
        {
            m_activeTouches.Clear();
        }

        Vector3 r = Vector3.zero;
        foreach(Touch touch in Input.touches)
        {
            if(touch.phase == TouchPhase.Began)
            {
                m_activeTouches.Add(touch.fingerId, touch.position);
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                if(m_activeTouches.ContainsKey(touch.fingerId))
                {
                    m_activeTouches.Remove(touch.fingerId);
                }
            }
            else
            {
                float mag = 0;
                r = (touch.position - m_activeTouches[touch.fingerId]);
                mag = r.magnitude / 300;
                r = r.normalized * mag;

                if(r.magnitude > 1)
                {
                    r.Normalize();
                }
            }

        }
        return r;    
    }

}
