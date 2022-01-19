using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;
public class DirectoryBrowser : Receiver
{
  
  public List<DirectoryNode> parentNodes;

  public List<DirectoryNode> subNodes;

  public DirectoryNode startNode;


  public DirectoryNode currentDirectory;

    public Moveables moveables;

    
    
    public AudioSource audio;
    public AudioSource oneHit;

    public AudioClip moveableEnteredClip;
    public AudioClip moveableExitedClip;
    public AudioClip moveableSelectedClip;


public override void Create(){
    DestroySubNodes();
    DestroyParentNodesExcept(startNode);
    startNode.transform.position = Vector3.left * 2;
    selectedObject = null;
}

  public override void OnMoveableReceived( Moveable m ){


        if(m.GetType() == typeof(DirectoryNode)){

            DestroySubNodesExecept((DirectoryNode)m);
            


            currentDirectory = (DirectoryNode)m;


            // If its a parent node, destroy all of our sub directories
            int parentID = -1;
        
            for( int i = 0; i <parentNodes.Count; i++ ){
                if( parentNodes[i] == currentDirectory ){
                    parentID = i;
                }
            }

            if( parentID >= 0 ){
                for( int i = parentNodes.Count-1; i > parentID; i-- ){    
                    DestroyParentNode(i);
                }
            }



            List<DirectoryNode> children = GenerateChildren(m);
            subNodes = children;


            m.vel = Vector3.zero;
            m.transform.position = this.transform.position;


            
           if( parentID < 0){ parentNodes.Add(currentDirectory); }
            

            /*for( int i = 0; i < parentNodes.Count; i++ ){
                parentNodes[i].targetPosition = this.transform.position + Vector3.left * (parentNodes.Count-i);
            }*/

           // for( int i = 0; i < )

        }
    
        oneHit.clip = moveableSelectedClip;
        oneHit.Play();

  }

  public override void OnMoveableRemoved( Moveable m ){

    /*if(m == currentDirectory ){
        DestroySubNodes();
    }*/ 

  }


  public override void OnMoveableEntered(Moveable m){
    oneHit.clip = moveableEnteredClip;
      oneHit.Play();
  }


  public override void OnMoveableExited(Moveable m){

      print("exited");

    if(  selectedObject == m ){
        m.OnDeselected(this);
        m.selected = false;
        selectedObject = null;
        OnMoveableRemoved(m);
      }

      oneHit.clip = moveableExitedClip;
      oneHit.Play();
      
  }



public GameObject DirectoryNodePrefab;
public float spawnRadius;


public void DestroyParentNodes(){
    for(int i = 0; i< parentNodes.Count; i++ ){
        if( parentNodes[i]!= null ){
            moveables.JumpDeath(parentNodes[i].GetComponent<Moveable>());
            moveables.moveables.Remove(parentNodes[i].GetComponent<Moveable>());
            DestroyImmediate(parentNodes[i].gameObject);
        }
    }

    parentNodes.Clear();
}


public void DestroyParentNode(int i){
    if( parentNodes[i] != null ){
        moveables.JumpDeath(parentNodes[i].GetComponent<Moveable>());
        moveables.moveables.Remove(parentNodes[i].GetComponent<Moveable>());
        DestroyImmediate(parentNodes[i].gameObject);
        parentNodes.RemoveAt(i);
    }
}


public void DestroyParentNodesExcept(DirectoryNode n){
    for(int i = 0; i< parentNodes.Count; i++ ){
        if( parentNodes[i]!= null && parentNodes[i] != n){
            moveables.JumpDeath(parentNodes[i].GetComponent<Moveable>());
            moveables.moveables.Remove(parentNodes[i].GetComponent<Moveable>());
            DestroyImmediate(parentNodes[i].gameObject);
        }
    }

    parentNodes.Clear();
}
public void DestroySubNodes(){

    for(int i = 0; i< subNodes.Count; i++ ){
        if( subNodes[i]!= null ){
            moveables.JumpDeath(subNodes[i].GetComponent<Moveable>());
            moveables.moveables.Remove(subNodes[i].GetComponent<Moveable>());
            DestroyImmediate(subNodes[i].gameObject);
        }
    }

    subNodes.Clear();

}


public void DestroySubNodesExecept(DirectoryNode n){

    for(int i = 0; i< subNodes.Count; i++ ){
        if( subNodes[i] != null && subNodes[i] != n){
            moveables.JumpDeath(subNodes[i].GetComponent<Moveable>());
            moveables.moveables.Remove(subNodes[i].GetComponent<Moveable>());
            DestroyImmediate(subNodes[i].gameObject);
        }
    }

    subNodes.Clear();

}


public List<DirectoryNode> GenerateChildren(Moveable move){

        DirectoryNode topNode = (DirectoryNode)move;

        List<DirectoryNode> children = new List<DirectoryNode>();
        for( int i = 0; i<  topNode.folderInfo.Length; i++ ){

            float angle = (float)i / (float) topNode.folderInfo.Length;
            float nID = angle;

            angle *= Mathf.PI;

            GameObject child = GameObject.Instantiate(DirectoryNodePrefab);

            Moveable m = child.GetComponent<Moveable>();


            Vector3 outVec = (Mathf.Sin(angle)*Vector3.left -Mathf.Cos(angle) *Vector3.up);
            outVec *= -1;

            child.transform.position = transform.position + outVec ; 
            child.transform.Rotate(Vector3.forward*((-angle/Mathf.PI) * 180 + 90));
            
            DirectoryNode dn = (DirectoryNode)m;
            dn.directoryPath = topNode.folderInfo[i];
            dn.parentDirectory = topNode.directoryPath;
   
            child.name = dn.directoryPath;


            moveables.moveables.Add(m);
            moveables.JumpStart(m);

            children.Add((DirectoryNode)m);
            


        }

        return children;
    }



    public override void WhileLiving(float v)
    {


        DoReceiverLiving();

        for( int i = 0; i < subNodes.Count; i++ ){

            // pull towards center
            //subNodes[i].PullTowards( this.transform.position , .5f );


            float angle = (float)i / (float) subNodes.Count;
            float nID = angle;

            angle *= Mathf.PI;


            Vector3 outVec = (Mathf.Sin(angle)*Vector3.left -Mathf.Cos(angle) *Vector3.up);
            outVec *= -1;

            // only do forces if outside receiver
            if( subNodes[i].insideReceiver == null ){
                subNodes[i].PullTowards( this.transform.position + outVec * spawnRadius , 3);

            // subNodes[i].AddForce(Vector3.right * 2);

                // push away from others
                for( int j = i+1; j < subNodes.Count; j++  ){

                    Vector3 dif = subNodes[i].transform.position - subNodes[j].transform.position;
                
                    subNodes[i].AddForce(dif.normalized * .01f);
                    subNodes[j].AddForce(-dif.normalized  * .01f);

                }
            }

        }

        for( int i = 0; i < parentNodes.Count; i++ ){
            
            // only add forces if we aren't pulling!

            if(closestMoveable != parentNodes[i]){
                parentNodes[i].AddForce(Vector3.left);
                if( i < parentNodes.Count-1){
                    parentNodes[i].PullTowards(parentNodes[i+1].transform.position, 1);
                }else{
                    
                    // only pull in if something else isn't in there!
                    //if( parentNodes[i] == closestMoveable ){
                        parentNodes[i].PullTowards(this.transform.position + Vector3.left * pullRadius, 3);
                    ///}
                }
            }
        }
    }

}
