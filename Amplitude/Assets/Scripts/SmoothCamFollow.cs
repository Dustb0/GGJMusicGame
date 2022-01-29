using UnityEngine;

public class SmoothCamFollow : MonoBehaviour
{
    [SerializeField] Transform m_playerPivot;
    [SerializeField] Vector3 m_offset;

    [Range (1, 10)]
    [SerializeField] float m_smoothfactor;

    void FixedUpdate ()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 desiredPosition = m_playerPivot.position + m_offset;
        Vector3 smoothedFollowing = Vector3.Lerp (transform.position, desiredPosition, m_smoothfactor * Time.fixedDeltaTime);
        transform.position = smoothedFollowing;
    }
}