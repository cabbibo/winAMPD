using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class Moveable: Cycle
{

    public Moveables moveables;
    public float forceTowardsLocation;
    public Collider collider;

    public float distanceFromCamera;
    public bool held;

    public Vector3 targetPosition;
    public Vector3 pullPosition;

    public float currTargetForce;
    public float heldTargetForce;

    public float releasedTargetForce;

    public float forceMultiplier;


    public List<Receiver> possibleReceivers;
    public Receiver closestReceiver;
    public float closestReceiverDistance;



public float dampening;

    public Vector3 vel;
    public Vector3 force;

    public AudioSource audio;
    public AudioSource oneHit;

    public float totalForce;


    protected LineRenderer lr;

    public override void Create()
    {
        lr = GetComponent<LineRenderer>();


    }
    public virtual void OnPickup(){
        held = true;
        distanceFromCamera = (transform.position - data.camera.position).magnitude;
    }

    public virtual void OnRelease(){
        held = false;
        currTargetForce = releasedTargetForce;
    }


    public virtual void WhileHeld(){

        targetPosition =  data.camera.position  + data.events.RD * distanceFromCamera;
        currTargetForce = heldTargetForce;

    } 


    public override void Activate(){
        vel = Vector3.zero;
        force = Vector3.zero;
        currTargetForce = releasedTargetForce;
    }
    
    Vector3 newForce;

    Receiver insideReceiver;
    Receiver oInsideReceiver;

    public AudioClip enterReceiverSound;
    public AudioClip exitReceiverSound;
    Vector3 closestDelta;

    public float oldSpeed;
    public bool selected;
    public override void WhileLiving(float v){

        

        force = Vector3.zero;
        totalForce = 0;


        closestReceiverDistance = 1000;

        for( int i  = 0; i < possibleReceivers.Count; i++ ){

            Receiver r  =possibleReceivers[i];

            Vector3 delta = (r.transform.position - transform.position );

            float dist = delta.magnitude;

            if( dist < closestReceiverDistance){
                closestReceiver = r;
                closestReceiverDistance =dist;
                closestDelta = delta;
            }

        

        }



        oInsideReceiver = insideReceiver;       

        if( closestReceiverDistance < closestReceiver.pullRadius ){
            newForce =  closestDelta * closestReceiver.pullValue * Time.deltaTime;
            force += newForce;
            totalForce += newForce.magnitude;
            closestReceiver.moveableInside = this;
            insideReceiver = closestReceiver;
        }else{
            closestReceiver.moveableInside = null;
            insideReceiver = null;
        }

 

        if( oInsideReceiver == null && insideReceiver != null ){

            print("enterIn");
            EnterReceiver(insideReceiver);
        }

        if( oInsideReceiver != null && insideReceiver == null ){
            
            print("enterPit");
            ExitReceiver(oInsideReceiver);
        }


        if( selected == true  && held == false ){

        }else{
            oldSpeed=  vel.magnitude;
            newForce =   -(transform.position - targetPosition) * currTargetForce * Time.deltaTime;
            force += newForce;
            totalForce += newForce.magnitude;
            vel += force * Time.deltaTime * forceMultiplier;
            transform.position += vel;
            vel *= dampening;
        }

        if( held ){
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, targetPosition);
        }else{
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1,  transform.position);
        }


        if( insideReceiver != null ){

            audio.pitch = totalForce  * 10;
            audio.volume = totalForce  * 10;
        }else{

            audio.pitch = audio.pitch  * .8f;
            audio.volume = audio.volume  * .8f;
        }


        if( vel.magnitude < .001f  && insideReceiver != null && held == false ){
            if( insideReceiver.selectedObject != this ){
                insideReceiver.selectedObject = this;
                selected = true;
                OnSelected( insideReceiver);
            }
        }




    }


    public void EnterReceiver(Receiver r){
        oneHit.clip = enterReceiverSound;
        oneHit.Play();
    }

    
    public void ExitReceiver(Receiver r){
        
        print(r.selectedObject );

        print("hjmmmm");

        oneHit.clip = exitReceiverSound;
        oneHit.Play();

    
        if( r.selectedObject == this ){

            print("deslecting");
            OnDeselected(r);
            selected = false;
            r.selectedObject = null;
        }
    }



    public virtual void OnSelected(Receiver r){
        print("selected");
    }

        public virtual void OnDeselected(Receiver r){
        print("deselected");
    }


}
