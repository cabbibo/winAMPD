using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;
public class Receiver : Cycle
{
  

  public float pullRadius;
  public float pullValue;


  public Moveable moveableInside;

  public Moveable selectedObject;

  public Moveable closestMoveable;

  public Moveable secondClosest;


  public virtual void OnMoveableReceived( Moveable m ){

  }

  public virtual void OnMoveableRemoved( Moveable m ){
  
  }



  public virtual void OnMoveableEntered( Moveable m ){
  
  }

   public virtual void OnMoveableExited( Moveable m ){

   }

public float selectTime;
public float selectionSpeed;
public float selectionDistance;

public float settledTime;

  public override void WhileLiving(float v){

    DoReceiverLiving();
  

  }

  public void DoReceiverLiving(){

      List<Moveable> moveables = data.moveables.moveables;
      List<Moveable> moveablesInside = new List<Moveable>();

      float closestLength = 1000;
      Moveable oldClosest = closestMoveable;
      for( int i = 0; i< moveables.Count; i++ ){

        Vector3 dif = transform.position-moveables[i].transform.position;
        float l = dif.magnitude;

        Vector3 oDif = transform.position-moveables[i].oPos;
        float oL = oDif.magnitude;

        if( l < closestLength ){
          closestLength = l;
          closestMoveable = moveables[i];
        }

        
        // pull in any moveables that are inside this radius
        if( l < pullRadius ){
          moveables[i].AddForce(dif * pullValue);


          // if any are held, push away whatever all others! helps us only have 1 item
          // in the zone at a time
          if( moveables[i].held == true){
             for( int j = 0; j< moveables.Count; j++ ){
              if( i != j ){

                Vector3 d = (moveables[i].transform.position - moveables[j].transform.position);
                moveables[j].AddForce(-.8f*d.normalized / (.1f + d.magnitude *  d.magnitude));
              }

             }
          }
          
          if( oL >= pullRadius ){
            moveables[i].insideReceiver = this;
            OnMoveableEntered( moveables[i] );              
          }
        }

        if( l >= pullRadius && oL < pullRadius ){
            moveables[i].insideReceiver = null;
          OnMoveableExited(moveables[i]);
          if(  selectedObject == moveables[i]  ){
              moveables[i] .OnDeselected(this);
              moveables[i] .selected = false;
              selectedObject = null;
              OnMoveableRemoved(moveables[i] );
            }

        }


        
      }

      if( closestMoveable.vel.magnitude < selectionSpeed && closestLength < selectionDistance &&  closestMoveable == oldClosest && closestMoveable.held == false){
      
        settledTime += 1;

        if( settledTime > selectTime){
          if( selectedObject != closestMoveable ){
            selectedObject = closestMoveable;
            OnMoveableReceived(closestMoveable);
          }
        }

      }else{
        settledTime = 0;
      }





  }
  
  



}
