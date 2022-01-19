using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
    }

    public void OnDown(){
        GetComponent<LineRenderer>().SetPosition(0,events.RO);
        GetComponent<LineRenderer>().SetPosition(1,events.RD * 100);
        transform.position = events.RO + events.RD * 20;
    }
}
