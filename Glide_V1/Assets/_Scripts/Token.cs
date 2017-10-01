using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour {

    private void Update()
    {
        transform.Rotate(Vector3.up * 90f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        SaveManager.m_instance.m_state.m_gold++;
        SaveManager.m_instance.Save();
        Destroy(gameObject);
    }
}
