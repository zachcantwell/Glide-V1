using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour {

    private Objective m_ObjectiveScript; 
    private bool m_ringActive = false;

    private void Start()
    {
        m_ObjectiveScript = FindObjectOfType<Objective>();
    }

    public void ActivateRing()
    {
        m_ringActive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(m_ringActive)
        {
            m_ObjectiveScript.NextRing();
            Destroy(gameObject, 5.0f);
        }   
    }
}
