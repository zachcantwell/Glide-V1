using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public Transform m_lookAt;
    private Vector3 m_desiredPosition;
    private float m_offset = 1.5f;
    private float m_distance = 3.5f; 

	void Update () {
        m_desiredPosition = m_lookAt.position + (-transform.forward * m_distance) + (transform.up * m_offset);
        transform.position = Vector3.Lerp(transform.position, m_desiredPosition, 0.05f);

        transform.LookAt(m_lookAt.position + Vector3.up * m_offset);
    }
}
