using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;

public class DoInterface : Cycle
{


    private bool clickThrough = true;
    private bool prevClickThrough = true;
 
    public Camera mainCamera;

    public bool onInterface;
    public float distanceOnPickUp;

    public GameObject fullInterface;

    public bool holding = false;
    
    RaycastHit hit = new RaycastHit();

    public Collider myCollider;


    public float distanceFromCamera;
    public override void WhileLiving( float v ){
    

        Vector3 ro = mainCamera.ScreenPointToRay(Input.mousePosition).origin;
        Vector3 rd =  mainCamera.ScreenPointToRay(Input.mousePosition).direction;
        // If our mouse is overlapping an object
        clickThrough = Physics.Raycast (ro,rd, out hit, 100,
                Physics.DefaultRaycastLayers);
    


        if (clickThrough) {

            if( hit.collider.gameObject == fullInterface ){
                onInterface = true;
            }else{
                onInterface = false;
            }
        }else{
            onInterface = false;
        }


        if( Input.GetMouseButtonDown(0)){
            DoClick();
        }

        if( Input.GetMouseButtonUp(0)){
            DoLetGo();
        }


        if( holding == true ){

            fullInterface.transform.position = ro + rd * distanceOnPickUp;
        }

 
    }


    void DoClick(){

         if( hit.collider == myCollider ){
            print(":sdasda");
            distanceOnPickUp =  (hit.collider.gameObject.transform.position - mainCamera.transform.position).magnitude;;
            holding = true;
        }
    }

    void DoLetGo(){
        holding = false;
    }

}
