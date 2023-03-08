using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{

    [SerializeField]
    private float scrollSpeed;


    private void Start()
    {

    }

    void Update()
    {


        MeshRenderer mr = GetComponent<MeshRenderer>();

        Material mat = mr.material;

        Vector2 offset = mat.mainTextureOffset;

        offset.x = (transform.position.x / transform.localScale.x) / scrollSpeed;
        offset.y = (transform.position.y / transform.localScale.y) / scrollSpeed;


        mat.mainTextureOffset = offset;



    }
}
