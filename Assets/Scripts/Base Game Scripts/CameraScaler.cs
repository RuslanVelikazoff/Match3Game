using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    private Board board;
    public float cameraOffset;
    public float aspectRatio = 0.625f;
    public float padding = 2;
    public float yOffset = 1;

    // Use this for initialization
    void Start()
    {
        board = FindObjectOfType<Board>();
        if (board != null)
        {
            RepositionCamera(board.Width - 1, board.Height - 1);
        }
    }

    void RepositionCamera(float x, float y)
    {
        Vector3 tempPosition = new Vector3(x / 2, y / 2 + yOffset, cameraOffset);
        transform.position = tempPosition;
        if (board.Width >= board.Height)
        {
            Camera.main.orthographicSize = (board.Width / 2 + padding) / aspectRatio;
        }
        else
        {
            Camera.main.orthographicSize = board.Height / 2 + padding;
        }
    }
}
