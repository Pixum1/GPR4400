using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float m_TurnSpeed;
    [SerializeField, Range(1, 100)]
    private float m_TurnSpeedPercentIncrease = 10;
    [SerializeField, Min(2)]
    private float m_NormalSpeed = 10;
    [SerializeField, Min(3)]
    private float m_MaxMoveSpeed = 20;
    [SerializeField, Min(1)]
    private float m_MinMoveSpeed = 5;
    [SerializeField, Min(1)]
    private float m_Acceleration = 1;
    [SerializeField, Min(1)]
    private float m_Decceleration = 1;
    [SerializeField, Range(1, 100)]
    private float m_AnchorPercentMultiplier = 30;

    private CinemachineVirtualCamera m_Camera;

    private float moveSpeed;
    private bool anchored = true;
    private Rigidbody rb;

    private void Awake()
    {
        m_Camera = FindObjectOfType<CinemachineVirtualCamera>();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_Camera.Follow = this.transform;
        m_Camera.LookAt = this.transform;
        anchored = true;
    }

    private void Update()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        float moveSpeedPercent = moveSpeed / 100 * m_TurnSpeedPercentIncrease;

        if (!anchored)
        {

            if (verticalMovement > 0)
            {
                if (moveSpeed < m_MaxMoveSpeed)
                    moveSpeed += m_Acceleration * Time.deltaTime;
                else
                    moveSpeed = m_MaxMoveSpeed;
            }
            else if (verticalMovement < 0)
            {
                if (moveSpeed > m_MinMoveSpeed)
                    moveSpeed -= m_Acceleration * Time.deltaTime;
                else
                    moveSpeed = m_MinMoveSpeed;
            }
            else if (verticalMovement == 0)
            {
                if (moveSpeed > m_NormalSpeed)
                    moveSpeed -= m_Decceleration * Time.deltaTime;
                else if (moveSpeed < m_NormalSpeed)
                    moveSpeed += m_Decceleration * Time.deltaTime;
            }

            transform.Rotate(Vector3.up, horizontalMovement * (m_TurnSpeed * moveSpeedPercent) * Time.deltaTime);
        }
        else if (anchored)
        {
            if (moveSpeed > 0)
                moveSpeed -= (m_MaxMoveSpeed / 100 * m_AnchorPercentMultiplier) * Time.deltaTime;
            else if (moveSpeed <= 0)
                moveSpeed = 0;

            transform.Rotate(Vector3.up, m_TurnSpeed* horizontalMovement * Time.deltaTime);
        }


        transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);



        if (Input.GetKeyDown(KeyCode.Space))
            anchored = !anchored;
    }
}
