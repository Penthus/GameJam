using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
    }
}
