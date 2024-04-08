using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public float horizontalSpeed = 250f;
    public float verticalSpeed = 250f;
    public float canvasWidth;
    public float canvasHeight;

    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasWidth = Screen.width;
        canvasHeight = Screen.height;
        MoveText();
    }

    void Update()
    {
        MoveText();
    }

    void MoveText()
    {
        rectTransform.anchoredPosition += new Vector2(horizontalSpeed * Time.deltaTime, verticalSpeed * Time.deltaTime);
        CheckBoundaries();
    }

    void CheckBoundaries()
    {
        float halfWidth = rectTransform.sizeDelta.x / 2;
        float halfHeight = rectTransform.sizeDelta.y / 2;

        float screenLeft = -canvasWidth / 2 + halfWidth;
        float screenRight = canvasWidth / 2 - halfWidth;
        float screenTop = canvasHeight / 2 - halfHeight;
        float screenBottom = -canvasHeight / 2 + halfHeight;

        if (rectTransform.anchoredPosition.x < screenLeft)
        {
            horizontalSpeed = Mathf.Abs(horizontalSpeed);
        }
        else if (rectTransform.anchoredPosition.x > screenRight)
        {
            horizontalSpeed = -Mathf.Abs(horizontalSpeed);
        }

        if (rectTransform.anchoredPosition.y < screenBottom)
        {
            verticalSpeed = Mathf.Abs(verticalSpeed);
        }
        else if (rectTransform.anchoredPosition.y > screenTop)
        {
            verticalSpeed = -Mathf.Abs(verticalSpeed);
        }
    }

}
