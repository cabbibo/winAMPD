using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class rayDebug : MonoBehaviour
{

    public InputEvents events;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        GetComponent<LineRenderer>().SetPosition(0,events.RO);
        GetComponent<LineRenderer>().SetPosition(1,events.RD * 100);
        transform.position = events.RO + events.RD * 20;
    }

    public void OnDown(){
        GetComponent<LineRenderer>().SetPosition(0,events.RO);
        GetComponent<LineRenderer>().SetPosition(1,events.RD * 100);
        transform.position = events.RO + events.RD * 20;
    }
}
