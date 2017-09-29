using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour {

    private List<Transform> m_rings = new List<Transform>();

    private int m_ringPassed = 0;

    public Material m_activeRing;
    public Material m_inactiveRing;
    public Material m_finalRing;

    private void Start()
    {
        foreach(Transform t in transform)
        {
            m_rings.Add(t);
            t.GetComponent<MeshRenderer>().material = m_inactiveRing;
        }

        if(m_rings.Count == 0)
        {
            Debug.Log("add some rings");
            return;
        }

        //actvate first ring
        m_rings[m_ringPassed].GetComponent<MeshRenderer>().material = m_activeRing;
        m_rings[m_ringPassed].GetComponent<Ring>().ActivateRing();
    }

    public void NextRing()
    {
        m_rings[m_ringPassed].GetComponent<Animator>().SetTrigger("CollectionTrigger");
        m_ringPassed++;

        if(m_ringPassed == m_rings.Count)
        {
            Victory();
            return;
        }

        if(m_ringPassed == m_rings.Count - 1)
        {
            m_rings[m_ringPassed].GetComponent<MeshRenderer>().material = m_finalRing;
        }
        else
        {
            m_rings[m_ringPassed].GetComponent<MeshRenderer>().material = m_activeRing;
        }

        m_rings[m_ringPassed].GetComponent<Ring>().ActivateRing();
    }

    private void Victory()
    {
        FindObjectOfType<GameScene>().CompleteLevel();
    }
}
