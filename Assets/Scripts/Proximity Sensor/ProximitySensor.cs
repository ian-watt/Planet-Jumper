using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProximitySensor : MonoBehaviour
{
    public Planet currentlyTargeting;
    public TextMeshProUGUI proximityText;
    float max;


    private void Update()
    {
        
        GetClosestPlanet();
        DisplayProximity();
    }

    void GetClosestPlanet()
    {
        max = float.MaxValue;
        for (int i = 0; i < GameManager.Instance.currentPlanets.Count; i++)
        {
            if (Vector3.Distance(transform.position, GameManager.Instance.currentPlanets[i].transform.position) < max)
            {
                max = Vector3.Distance(transform.position, GameManager.Instance.currentPlanets[i].transform.position);
                currentlyTargeting = GameManager.Instance.currentPlanets[i];
            }
        }
    }

    void DisplayProximity()
    {
        proximityText.text = "" + Vector3.Distance(GameManager.Instance.player.transform.position, currentlyTargeting.transform.position);
    }

}
