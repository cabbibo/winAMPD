using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class nAudioForm : Form
{

    public LoopbackAudio loopback;

    public Texture t;

    public override void SetStructSize()
    {
        structSize = 1;
    }
    public override void SetCount(){
        count = loopback.SpectrumSize;
        print( count );
    }


    public override void WhileLiving(float v)
    {

        if( loopback.PostScaledSpectrumData != null ){
            SetData(loopback.PostScaledSpectrumData);
        }
    }
}
