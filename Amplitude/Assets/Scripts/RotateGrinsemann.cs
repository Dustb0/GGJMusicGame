using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGrinsemann : MonoBehaviour
{
    [SerializeField] Transform m_gameObject;

    [Range(-30, 30)]
    [SerializeField] float m_degreesPerSec;
    [SerializeField] float m_randomDegrees;
    [SerializeField] float m_rotationSpeed;

    float m_changeDirectionTime, m_timer;

    float m_currentRotation, m_randomRotation;

    private void Awake()
    {
        m_changeDirectionTime = 0;
        m_timer = m_changeDirectionTime;
        m_rotationSpeed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
        m_timer -= Time.deltaTime;

        if (m_timer > 0)
            return;
        else
        {
            m_changeDirectionTime = Random.Range(5f, 10f);
            m_timer = m_changeDirectionTime;
            m_randomDegrees = Random.Range(2f, 5f) * m_degreesPerSec;
            m_degreesPerSec = -m_degreesPerSec;
            m_rotationSpeed = Random.Range(1f, 1.5f);
        }
    }

    private void Rotation()
    {        
        m_randomRotation = m_rotationSpeed * m_randomDegrees * Time.deltaTime;
        m_currentRotation = transform.localRotation.eulerAngles.z;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, m_currentRotation + m_randomRotation));
    }
}
