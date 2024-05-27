using UnityEngine;

public class Player : MonoBehaviour
{
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 postion = transform.position;
            postion.x = mousPos.x;
            transform.position = postion;
        }

        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                Vector3 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 postion = transform.position;
                postion.x = mousPos.x;
                transform.position = postion;
            }
        }
    }
}
