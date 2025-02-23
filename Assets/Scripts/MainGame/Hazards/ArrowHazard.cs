using System;
using UnityEngine;

public class ArrowHazard : MonoBehaviour
{
    public GameObject arrowPrefab;
    [SerializeField] float shootInterval;
    private float shootIntervalLeft;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootIntervalLeft = shootInterval;
        
    }

    // Update is called once per frame
    void Update()
    {
        shootIntervalLeft -= Time.deltaTime;
        if (shootIntervalLeft <= 0)
        {
            GameObject arrow = Instantiate(arrowPrefab,transform.position,Quaternion.identity);
            arrow.transform.Rotate(0,90,0);
            arrow.transform.Rotate(0,90,0);
            shootIntervalLeft = shootInterval;
        }
    }
}
