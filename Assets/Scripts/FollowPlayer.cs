using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = GameManager.Instance.player.transform.position - offset;
    }
}
