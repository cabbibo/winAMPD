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


        public AudioClip hoverOverSound;
    public AudioClip hoverOutSound;
    public AudioClip selectedClip;

    public Color hoverColor;
    public Color heldColor;
    public Color selectedColor;
    public Color baseColor;
    

public float heldScale;
public float selectedScale;
public float baseScale;
public float hoveredScale;


MaterialPropertyBlock mpb;

public Renderer renderer;

    
    public bool selfStart;
    public void OnEnable(){
        if( selfStart ){
            data.moveables.JumpStart(this);
        }
    }


    public override void Create()
    {

        MoveableCreate();


    }

    public void MoveableCreate(){   
        
        timeSettled = 0;
        lr = GetComponent<LineRenderer>();

               
        lr = GetComponent<LineRenderer>();

        selected = false;
        hovered = false;
        held = false;
        insideReceiver = null;
        closestReceiver = null;

        SetColor(baseColor);
        SetScale(baseScale);

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




    


public bool hovered;
public void SetColor(Color c){
    if( mpb == null ){ mpb = new MaterialPropertyBlock(); }
    renderer.GetPropertyBlock(mpb);
    mpb.SetColor("_Color", c);
    renderer.SetPropertyBlock(mpb);
}

public void SetScale( float s ){
    transform.localScale = Vector3.one * s;

}
public virtual void OnHoverOver(){

    hovered = true;
    
    if( held == false ){
        SetColor(hoverColor);
        SetScale(hoveredScale);
        data.audio.Play(hoverOverSound);
    }
}

public virtual void OnHoverOut(){
    hovered = false;
    if( held == false ){
        if( selected ){
            SetColor(selectedColor);
            SetScale(selectedScale);
        }else{
            SetColor(baseColor);
            SetScale(baseScale);
        }
        data.audio.Play(hoverOutSound);
    }
} 

public virtual void WhileHovered(){

}

    public virtual void OnPickup(){
        held = true;
        distanceFromCamera = (transform.position - data.camera.position).magnitude;
        SetColor(heldColor);
        SetScale(heldScale);
    }

    public virtual void OnRelease(){
        held = false;

        if( hovered == false ){
            if( selected ){
                SetColor(selectedColor);
                SetScale(selectedScale);
            }else{
                SetColor(baseColor);
                SetScale(baseScale);
            }
        }else{
            SetColor(hoverColor);
            SetScale(hoveredScale);
        }
    }



    public virtual void OnSelected( Receiver r ){

        print("selected : " + this.gameObject.name );
        selected = true;
        SetColor(selectedColor);
        SetScale(selectedScale);
        data.audio.Play(selectedClip);


    }

    
    public virtual void OnDeselected( Receiver r ){

        print("deselected : " + this.gameObject.name );
        selected = false;
        SetColor(baseColor);
        SetScale(baseScale);


    }


    public virtual void WhileHeld(){


        Vector3 targetPosition =  data.camera.position  + data.events.RD * distanceFromCamera;
        if(lockZ ){ targetPosition.z = 0; }

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1,  targetPosition);

        PullTowards( targetPosition , mousePullForce);

    } 



}
