using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 startPos;
    private Transform curPos;

    private bool isRight;
    private bool isMoving;

    public float maxMovingDistance;
    public float movingSpeed;

    private void Start()
    {
        startPos = transform.position;
        curPos = transform;
        isRight = true;
    }
    private void Update()
    {
        if(isMoving)
            Moving();
    }

    void Moving()
    {
        if (isRight)
        {
            curPos.position = new Vector3(curPos.position.x - movingSpeed, transform.position.y,transform.position.z);
            if (startPos.x - curPos.position.x  >= maxMovingDistance) isRight = false;
        }
        else
        {
            curPos.position = new Vector3(curPos.position.x + movingSpeed, transform.position.y, transform.position.z);
            if (startPos.x <= curPos.position.x) isRight = true;
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isMoving = true;
            collision.transform.parent = curPos;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isMoving = false;
            collision.transform.parent = null;
        }
    }
}
