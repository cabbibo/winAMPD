using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class LineTowardsCenter : MonoBehaviour
{
    public Transform center;
    LineRenderer lr;
    // Start is called before the first frame update
    void OnEnable()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, transform.position );
        lr.SetPosition(1, Vector3.Lerp(transform.position, center.position, .4f) );
    }
}
