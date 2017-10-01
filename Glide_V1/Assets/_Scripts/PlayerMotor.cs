using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMotor : MonoBehaviour {

    private float m_deathTime;
    private float m_deathDuration;
    public GameObject m_deathExplosion;

    private CharacterController m_controller;
    private float m_baseSpeed = 10f;
    private float m_rotSpeedX = 9f;
    private float m_rotSpeedY = 3f;

    private void Start()
    {
        m_controller = GetComponent<CharacterController>();

        GameObject trail = Instantiate(Manager.Instance.m_playerTrails[SaveManager.m_instance.m_state.m_activeTrail]);
        trail.transform.SetParent(transform.GetChild(0));
        trail.transform.localEulerAngles = Vector3.forward * -90f; 
    }

    private void Update()
    {
        if(m_deathTime != 0)
        {
            if(Time.time - m_deathTime > m_deathDuration)
            {
                SceneManager.LoadScene("Game");    
            }

            return;
        }

        Vector3 moveVector = transform.forward * m_baseSpeed;

        Vector3 inputs = Manager.Instance.GetPlayerInput();

        Vector3 yaw = inputs.x * transform.right * m_rotSpeedX * Time.deltaTime;
        Vector3 pitch = inputs.y * transform.up * m_rotSpeedY * Time.deltaTime;
        Vector3 dir = yaw + pitch;

        float maxX = Quaternion.LookRotation(moveVector + dir).eulerAngles.x;

        if(maxX < 90f && maxX > 70 || maxX > 270 && maxX < 290)
        {
            // dont do anything here
        }
        else
        {
            moveVector += dir;
            transform.rotation = Quaternion.LookRotation(moveVector);
        }

        m_controller.Move(moveVector * Time.deltaTime);

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        m_deathTime = Time.time;

        GameObject go = Instantiate(m_deathExplosion) as GameObject;
        go.transform.position = transform.position;

        transform.GetChild(0).gameObject.SetActive(false);
    }

}
