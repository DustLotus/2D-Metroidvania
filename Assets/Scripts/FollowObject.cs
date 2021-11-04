using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector3 offSet;

    private void Start()
    {
        transform.position = objectToFollow.position + offSet;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = objectToFollow.position + offSet;
    }
}
