using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour {

    public Transform m_shopWayPoint;
    public Transform m_levelWayPoint;

    public float m_rotationSpeed;
    public float m_positionSpeed; 

    private Vector3 m_startPosition;
    private Quaternion m_startRotation;

    private Vector3 m_desiredPosition;
    private Quaternion m_desiredRotation;

	// Use this for initialization
	void Start () {
        m_startPosition = m_desiredPosition = transform.localPosition;
        m_startRotation = m_desiredRotation = transform.localRotation; 	
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = Vector3.Lerp(transform.localPosition, m_desiredPosition, m_positionSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, m_desiredRotation, m_rotationSpeed * Time.deltaTime);	
	}

    public void BackToMainMenu()
    {
        m_desiredPosition = m_startPosition;
        m_desiredRotation = m_startRotation;
    }

    public void MoveToShop()
    {
        m_desiredPosition = m_shopWayPoint.localPosition;
        m_desiredRotation = m_shopWayPoint.localRotation;
    }

    public void MoveToLevel()
    {
        m_desiredPosition = m_levelWayPoint.localPosition;
        m_desiredRotation = m_levelWayPoint.localRotation;
    }
}
