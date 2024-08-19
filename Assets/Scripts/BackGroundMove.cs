using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    [SerializeField]
    private Transform UpRightTransform;
    [SerializeField]
    private Transform DownLeftTransform;
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private float MoveRatio;
    Vector3 lastPosition;
    void Start()
    {
        lastPosition = Player.transform.position;
    }

    void Update()
    {
        Vector3 currentPosition = Player.transform.position;
        Vector3 vector3 = currentPosition - lastPosition;
        if ( UpRightTransform.position.x >= transform.position.x && UpRightTransform.position.y >= transform.position.y
            && transform.position.x >= DownLeftTransform.position.x && transform.position.y >= DownLeftTransform.position.y)
        {
            transform.Translate( vector3.normalized * MoveRatio * Time.deltaTime);
        }
        if(UpRightTransform.position.x < transform.position.x)
        {
            transform.position = new Vector3(UpRightTransform.position.x,transform.position.y, transform.position.z);
        }
        if (UpRightTransform.position.x < transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, UpRightTransform.position.y, transform.position.z);
        }
        if (transform.position.x < DownLeftTransform.position.x)
        {
            transform.position = new Vector3(DownLeftTransform.position.x, transform.position.y, transform.position.z);
        }
        if (transform.position.y < DownLeftTransform.position.y)
        {
            transform.position = new Vector3(transform.position.x, DownLeftTransform.position.y, transform.position.z);
        }
        lastPosition = currentPosition;
    }
}
