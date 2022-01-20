using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class Moveables: Cycle
{



    public List<Moveable> moveables;


    public Moveable heldMoveable;



    public override void Create(){

        for( int i = 0; i < moveables.Count; i++ ){
            SafeInsert(moveables[i]);
        }

    }

    public void OnDown(){       

        if( data.events.hitTag == "moveable"){

            for( int i = 0; i < moveables.Count; i++ ){

                if( moveables[i].collider == data.events.hit.collider ){
                    moveables[i].OnPickup();
                    heldMoveable = moveables[i];
                }
            }
        }

    }



    public void OnUp(){
        if( heldMoveable != null){
            heldMoveable.OnRelease();
            heldMoveable = null;
        }
    }


public string oHitTag;
public string hitTag;

public Collider hitCollider;
public Collider oHitCollider;
public Moveable hoveredMoveable;
    public override void WhileLiving(float v ){

        hitTag = data.events.hitTag;
        hitCollider = data.events.hit.collider;
        if( hitCollider != oHitCollider){


            if( hitTag == "moveable"){


                for( int i = 0; i < moveables.Count; i++ ){

                    if( moveables[i].collider == data.events.hit.collider ){
                        
                        if( hoveredMoveable == null ){
                            HoverOver( moveables[i] );
                        }else{
                            if( moveables[i] != hoveredMoveable ){
                                HoverOut( hoveredMoveable);
                                HoverOver(moveables[i]);
                            }
                        }
                    }
                }


            }

            if( hitTag != "moveable"){
                if( hoveredMoveable != null ){
                    HoverOut(hoveredMoveable);
                }
            }

            
        }
        oHitTag = hitTag;
        oHitCollider = hitCollider;

        if(heldMoveable!= null ){
            heldMoveable.WhileHeld();
        }

        if( hoveredMoveable != null ){
            hoveredMoveable.WhileHovered();
        }


    }

    public void HoverOver(Moveable m){
        m.OnHoverOver();
        hoveredMoveable = m;
    }

    public void HoverOut(Moveable m){
        m.OnHoverOut();
        hoveredMoveable = null;
    }


    public override void Destroy(){
        for( int i = 0; i < moveables.Count; i++){

            if( moveables[i] == null ){
                moveables.RemoveAt(i);
            }
        }
    }
    
   
}
