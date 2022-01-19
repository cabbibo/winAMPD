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
            moveables[i].moveables = this;
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

    public override void WhileLiving(float v ){
        if(heldMoveable!= null ){
            heldMoveable.WhileHeld();
        }
    }
    
   
}
