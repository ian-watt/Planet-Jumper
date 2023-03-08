using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public static Radar Instance;

    public List<GameObject> radarObjects;
    public GameObject radarPrefab;
    public List<GameObject> borderObjects;
    public float switchDistance =10;
    public Transform helpTransform;
    public float maxRenderDistance;

    private void Awake()
    {
        Instance = this;
        borderObjects= new List<GameObject>();
        radarObjects = new List<GameObject>();

    }

    void Start()
    {
    }

     void Update()
    {
       for(int i = 0; i < radarObjects.Count; i++)
        {
            if(radarObjects[i] != null)
            {
                if (Vector3.Distance(radarObjects[i].transform.position, transform.position) < maxRenderDistance && radarObjects[i] != null)
                {
                    if (Vector3.Distance(radarObjects[i].transform.position, transform.position) > switchDistance)
                    {
                        //switch to borderObjects
                        helpTransform.LookAt(radarObjects[i].transform);
                        borderObjects[i].transform.position = transform.position + switchDistance * helpTransform.forward;
                        borderObjects[i].layer = LayerMask.NameToLayer("Radar");
                        radarObjects[i].layer = LayerMask.NameToLayer("Invisible");


                    }
                    else
                    {
                        //switch back to radarObjects
                        borderObjects[i].layer = LayerMask.NameToLayer("Invisible");
                        radarObjects[i].layer = LayerMask.NameToLayer("Radar");

                    }
                }
                else
                {
                    borderObjects[i].layer = LayerMask.NameToLayer("Invisible");
                    radarObjects[i].layer = LayerMask.NameToLayer("Invisible");

                }
            }
            else
            {
                radarObjects.RemoveAt(i);
                borderObjects.RemoveAt(i);
            }
        }


    }

    public void CreateRadarObject(Planet obj)
    {
            GameObject go = Instantiate(radarPrefab, obj.transform.position, Quaternion.identity, obj.transform);
            radarObjects.Add(go);
            GameObject goBorder = Instantiate(radarPrefab, obj.transform.position, Quaternion.identity, obj.transform);
            borderObjects.Add(goBorder);
            obj.borderObject = go;
    }

    public void RemoveRadarObject(GameObject obj)
    {
        radarObjects.Remove(obj);
        borderObjects.Remove(obj);
    }
}
