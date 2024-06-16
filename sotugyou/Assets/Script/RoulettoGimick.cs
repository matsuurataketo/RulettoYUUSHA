using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoulettoGimick : MonoBehaviour
{
    public float rotationSpeed = 100f; // ��]���x
    private bool hasRotatedRight = false;
    private bool hasRotatedLeft = false;
    private float totalRotationRight = 0f;
    private float totalRotationLeft = 0f;

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            float step = rotationSpeed * Time.deltaTime * Mathf.Abs(scrollInput);

            if (scrollInput > 0)
            {
                // �E��]
                transform.Rotate(0, 0, -step);
                totalRotationRight += step;
                if (totalRotationRight >= 360f)
                {
                    hasRotatedRight = true;
                    totalRotationRight = 0f;
                }
            }
            else if (scrollInput < 0)
            {
                // ����]
                transform.Rotate(0, 0, step);
                totalRotationLeft += step;
                if (totalRotationLeft >= 360f)
                {
                    hasRotatedLeft = true;
                    totalRotationLeft = 0f;
                }
            }

            // �����̉�]������������֐����Ăяo��
            if (hasRotatedRight && hasRotatedLeft)
            {
                BothRotationsCompleted();
            }
        }
    }

    private void BothRotationsCompleted()
    {
        Debug.Log("Both rotations are completed!");
        // �����ɌĂяo���֐�����������
        // ��F SomeFunction();
    }
}
