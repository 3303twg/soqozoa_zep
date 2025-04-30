using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Flappy_PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public float flapPower = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // �����̽� �Է� �� ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * flapPower, ForceMode2D.Impulse);
        }

        // ���� �ӵ� �������� ȸ��
        if (rb.velocity.y != 0)
        {
            float angle = Mathf.Atan2(rb.velocity.y, 1f) * Mathf.Rad2Deg; // x=1 �������� ���� ���
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}