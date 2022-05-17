using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private float moveSpeed;
    private bool anchored = true;

    private void Start()
    {
        anchored = true;
    }

    private void Update()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        float moveSpeedPercent = moveSpeed / 100 * m_TurnSpeedPercentIncrease;

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
        else if (verticalMovement == 0 && !anchored)
        {
            if (moveSpeed > m_NormalSpeed)
                moveSpeed -= m_Decceleration * Time.deltaTime;
            else if (moveSpeed < m_NormalSpeed)
                moveSpeed += m_Decceleration * Time.deltaTime;
        }


        transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);

        transform.Rotate(Vector3.up, horizontalMovement * (m_TurnSpeed * moveSpeedPercent) * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.Space))
            anchored = !anchored;

        if (anchored)
            if (moveSpeed > 0)
                moveSpeed -= (m_MaxMoveSpeed / 100 * m_AnchorPercentMultiplier) * Time.deltaTime;
            else
                moveSpeed = 0;
    }
}
