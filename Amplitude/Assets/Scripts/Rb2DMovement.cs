using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rb2DMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D m_rb2D;
    [SerializeField] float m_movementSpeed;
    [SerializeField] float m_jumpForce;
    [SerializeField] string m_horizontalAxis = "Horizontal";
    [SerializeField] string m_jumpButton = "Jump";

    bool m_canJump = false;

    private void Awake()
    {
        m_rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        /*
        if (transform.position.y <= -50)
        {
            transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }*/

        if (Input.GetButtonDown(m_jumpButton) && m_canJump)
        {
            m_rb2D.velocity = new Vector2(m_rb2D.velocity.x, 0);
            m_rb2D.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetAxis(m_horizontalAxis) != 0)
        {
            //m_rb2D.AddForce(new Vector2(Input.GetAxis(m_horizontalAxis) * m_movementSpeed, 0f), ForceMode2D.Impulse);
            m_rb2D.velocity = new Vector2(Input.GetAxis(m_horizontalAxis) * m_movementSpeed, m_rb2D.velocity.y);
        }

        if (m_rb2D.velocity.y > 0)
            m_rb2D.gravityScale = 3f;
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.gameObject.CompareTag("Ground") || _collision.gameObject.CompareTag("GeneratedGround"))
        {
            m_canJump = true;
            m_rb2D.gravityScale = 0.5f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        m_canJump = false;
    }
}