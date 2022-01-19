using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;


[ExecuteAlways]
public class Moveable: Cycle
{


    public bool lockZ;
    public float forceTowardsLocation;
    public Collider collider;

    public float mousePullForce;

    public float distanceFromCamera;
    public bool held;
    public float forceMultiplier;

    public Receiver insideReceiver;


    public List<Receiver> possibleReceivers;
    public Receiver closestReceiver;
    public float closestReceiverDistance;



public float dampening;

    public Vector3 vel;
    public Vector3 force;
    public Vector3 oPos;


    public float totalForce;


    protected LineRenderer lr;


    public float settleTime;

    
    public bool selfStart;
    public void OnEnable(){
        if( selfStart ){
            data.moveables.JumpStart(this);
        }
    }


    public override void Create()
    {

        timeSettled = 0;
        lr = GetComponent<LineRenderer>();


    }
    public virtual void OnPickup(){
        print("pick up");
        held = true;
        distanceFromCamera = (transform.position - data.camera.position).magnitude;
    }

    public virtual void OnRelease(){
        print("drop");
        held = false;
    }


    public virtual void WhileHeld(){


        Vector3 targetPosition =  data.camera.position  + data.events.RD * distanceFromCamera;
        if(lockZ ){ targetPosition.z = 0; }

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1,  targetPosition);

        PullTowards( targetPosition , mousePullForce);

    } 


    public override void Activate(){
        vel = Vector3.zero;
        force = Vector3.zero;
    }
    
    Vector3 newForce;
    Vector3 closestDelta;

    public float oldSpeed;
    public bool selected;

    public float timeSettled;
    public override void WhileLiving(float v){

       // print(force);

        oPos.x = transform.position.x;
        oPos.y = transform.position.y;
        oPos.z = transform.position.z;
      
        float deltaTime = 1f/60f;
        oldSpeed =  vel.magnitude;

        vel += force * deltaTime * forceMultiplier;
        transform.position += vel;
    
        vel *= dampening;


        if( !held ){
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1,  transform.position);
        }

        
        force = Vector3.zero;
        totalForce = 0;






    }


    public void AddForce( Vector3 newForce ){
        totalForce += newForce.magnitude;
        force += newForce;
    }

    public void PullTowards( Vector3 pos , float pow ){
        AddForce( (this.transform.position-pos) * -pow);
    }

    public virtual void EnterReceiver(Receiver r){}
    public virtual void ExitReceiver(Receiver r){}
    public virtual void OnSelected(Receiver r){
        print("selected");
    }

    public virtual void OnDeselected(Receiver r){
        print("deselected");
    }


}
