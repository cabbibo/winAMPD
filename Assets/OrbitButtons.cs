using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;
public class OrbitButtons : Cycle
{

public float speed;
public float radius;

public TransformBuffer tb;


public Vector3[] vel;

public override void OnLive(){

vel = new Vector3[tb.transforms.Length];


}
public static void SphericalToCartesian(float radius, float polar, float elevation, out Vector3 outCart){
        float a = radius * Mathf.Cos(elevation);
        outCart.x = a * Mathf.Cos(polar);
        outCart.y = radius * Mathf.Sin(elevation);
        outCart.z = a * Mathf.Sin(polar);
    }


    public override void WhileLiving(float v)
    {

        Random.seed = 0;
        for( int i = 0; i < tb.transforms.Length; i++ ){

            Transform t = tb.transforms[i];

            Vector3 upVec = new Vector3( Mathf.Sin( i * 10212) ,  -Mathf.Sin( i * 3131) , Mathf.Cos( i * 12144) );

            Vector3 x = Vector3.Cross( upVec , Vector3.up ).normalized;
            Vector3 y = Vector3.Cross( x , upVec ).normalized;


            float fTime = speed*Time.time  * (1 + .3f * Mathf.Sin(i)) + i * 313;
            //t.position =transform.position  + (Mathf.Sin(fTime) * x - Mathf.Cos(fTime) * y) *radius;


            y = Vector3.up;
            x = Vector3.right;

            float angle = (float)i / (float)tb.transforms.Length;

            angle *= Mathf.PI * .7f;
            angle += Mathf.PI /4;
            
           Vector3 outVec;
            //print("hmm)");
           SphericalToCartesian( radius , Random.Range(0,2 * Mathf.PI), Random.Range( 0, Mathf.PI / 2) , out outVec );


           Vector3 targetPosition =  transform.position + (Mathf.Sin(angle) * x - Mathf.Cos(angle) * y) *radius;


            Vector3 force = Vector3.zero;

            force -= (t.position - targetPosition) * 100 * (1 + Mathf.Sin(i * 121213 + Time.time * .1f) * .1f);
           vel[i] += force * Time.deltaTime;
            t.position += vel[i] * Time.deltaTime;

            vel[i] *= .96f;
           // t.position = transform.position + outVec;//(Mathf.Sin(angle) * x - Mathf.Cos(angle) * y) *radius;

        }
    }

}
