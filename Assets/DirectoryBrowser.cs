using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMMATERIA;
using TMPro;
using MagicCurve;
public class DirectoryBrowser : Receiver
{
  
  public List<DirectoryNode> parentNodes;

  public List<DirectoryNode> subNodes;
  public List<FileNode> fileNodes;

  public DirectoryNode startNode;


    public Moveables moveables;

    
    
    public AudioSource audio;
    public AudioSource oneHit;

    public AudioClip moveableEnteredClip;
    public AudioClip moveableExitedClip;
    public AudioClip moveableSelectedClip;


    public Moveable hoveredNode;

    public TextMeshPro selectedInfo;
    public TextMeshPro hoveredInfo;

    public Curve directorCurve;


public GameObject DirectoryNodePrefab;
public GameObject FileNodePrefab;
public float spawnRadius;
public float fileSpawnRadius;


public LineRenderer hoveredLR;
public LineRenderer toFirstParentLR;
public LineRenderer folderConnectionLR;


public override void Create(){
    DestroySubNodes();
    DestroyFileNodes();
    
    DestroyParentNodesExcept(startNode);
     startNode.transform.position = transform.position + Vector3.left * 2;
    selectedObject = null;
    firstFrame = 0;
    
}

public override void OnLived(){
    print("dsds");
    OnMoveableEntered(startNode);
    OnMoveableReceived(startNode);
}

  public override void OnMoveableReceived( Moveable m ){


        if(m.GetType() == typeof(DirectoryNode)){

                DirectoryNode fNode = (DirectoryNode)m;

                DestroySubNodesExecept(fNode);
                DestroyFileNodes();
            
        
                if( selectedObject != null ){
                if( selectedObject != m ){

                    OnMoveableRemoved(m);

                }
                }



                // Check to see if its a parent node
                int parentID = -1;
                for( int i = 0; i <parentNodes.Count; i++ ){
                    if( parentNodes[i] == fNode ){
                        parentID = i;
                    }
                }

                // if it is, then go an destroy all the nodes beneath it
                if( parentID >= 0 ){
                    for( int i = parentNodes.Count-1; i > parentID; i-- ){    
                        DestroyParentNode(i);
                    }
                }

                List<DirectoryNode> children = GenerateChildren(m);
                subNodes = children;

                List<FileNode> c2 = GenerateFileChildren(m);
                fileNodes = c2;
                
                // Add it in as a parent node if we haven't yet!
                if( parentID < 0){ parentNodes.Add(fNode); }


    

        }


         if(m.GetType() == typeof(FileNode)){

                FileNode fNode = (FileNode)m;
                //DestroySubNodes();
            
                fNode.PlayAudio();

           
   

        }

        oneHit.clip = moveableSelectedClip;
        oneHit.Play();


        m.vel = Vector3.zero;
        m.transform.position = this.transform.position;

        print(m.name);
        print(this.transform.position);

        m.OnSelected(this);


  }

  public override void OnMoveableRemoved( Moveable m ){

       if(m.GetType() == typeof(FileNode)){
            FileNode fNode = (FileNode)m;
            fNode.StopAudio();
       }

           
      m.OnDeselected(this);
  }


  public override void OnMoveableEntered(Moveable m){
    oneHit.clip = moveableEnteredClip;
      oneHit.Play();
  }


  public override void OnMoveableExited(Moveable m){


     if(  selectedObject == m ){
        m.OnDeselected(this);
        m.selected = false;
        selectedObject = null;
        OnMoveableRemoved(m);
      }

      oneHit.clip = moveableExitedClip;
      oneHit.Play();
      
  }



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
public void DestroyFileNodes(){

    for(int i = 0; i< fileNodes.Count; i++ ){
        if( fileNodes[i]!= null ){
            moveables.JumpDeath(fileNodes[i].GetComponent<Moveable>());
            moveables.moveables.Remove(fileNodes[i].GetComponent<Moveable>());
            DestroyImmediate(fileNodes[i].gameObject);
        }
    }

    fileNodes.Clear();

}
public void DestroyFileNodesExcept(FileNode f){

    for(int i = 0; i< fileNodes.Count; i++ ){
        if( fileNodes[i]!= null && fileNodes[i]!= f){
            moveables.JumpDeath(fileNodes[i].GetComponent<Moveable>());
            moveables.moveables.Remove(fileNodes[i].GetComponent<Moveable>());
            DestroyImmediate(fileNodes[i].gameObject);
        }
    }

    fileNodes.Clear();

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


public List<FileNode> GenerateFileChildren(Moveable move){

        DirectoryNode topNode = (DirectoryNode)move;

        List<FileNode> children = new List<FileNode>();
        for( int i = 0; i<  topNode.audioFiles.Length; i++ ){

            float angle = (float)i / (float) topNode.audioFiles.Length;
            float nID = angle;

            angle *= Mathf.PI;

            GameObject child = GameObject.Instantiate(FileNodePrefab);

            Moveable m = child.GetComponent<Moveable>();


            Vector3 outVec = (Mathf.Sin(angle)*Vector3.left -Mathf.Cos(angle) *Vector3.up);
            outVec *= -1;

            child.transform.position = transform.position + outVec * fileSpawnRadius; 
            child.transform.Rotate(Vector3.forward*((-angle/Mathf.PI) * 180 + 90));
            
            FileNode fn = (FileNode)m;
            fn.path = topNode.audioFiles[i];
   
            child.name = fn.path;

            moveables.moveables.Add(m);
            moveables.JumpStart(m);

            children.Add((FileNode)m);

        }

        return children;
    }




    public int firstFrame = 0;
    public override void WhileLiving(float v)
    {
        if( firstFrame == 1 ){
            
        OnMoveableReceived(startNode);
    
        } firstFrame ++;


        DoReceiverLiving();




        hoveredNode = null;

        for( int i = 0; i < subNodes.Count; i++ ){

            // pull towards center
            //subNodes[i].PullTowards( this.transform.position , .5f );


            float angle = (float)i / (float) subNodes.Count;
            float nID = angle;

            angle *= Mathf.PI;

            if(subNodes[i].hovered|| subNodes[i].held ){
                hoveredNode = subNodes[i];
            }


            Vector3 outVec = (Mathf.Sin(angle)*Vector3.left -Mathf.Cos(angle) *Vector3.up);
            outVec *= -1;


            Vector3 p = directorCurve.GetPositionFromValueAlongCurve(nID);

            Quaternion r = directorCurve.GetRotationFromValueAlongCurve(nID);
            r.eulerAngles += new Vector3(0,0,90);


            // only do forces if outside receiver
            if( subNodes[i].insideReceiver == null ){
                subNodes[i].PullTowards( p , 3);

                subNodes[i].transform.rotation = Quaternion.Slerp( subNodes[i].transform.rotation , r ,.1f );
                subNodes[i].transform.LookAt( Camera.main.transform);
                subNodes[i].transform.Rotate( Vector3.up * 180);
            // subNodes[i].AddForce(Vector3.right * 2);

                // push away from others
                for( int j = 0; j < subNodes.Count; j++  ){

                    if( i != j ){

                        Vector3 dif = subNodes[i].transform.position - subNodes[j].transform.position;
                    
                        subNodes[i].AddForce(dif.normalized * .02f);

                        if( subNodes[i].hovered || subNodes[i].held ){
                            subNodes[j].AddForce(-dif.normalized * .5f);
                        }

                        

                    }

                }
            }

        }



        for( int i = 0; i < fileNodes.Count; i++ ){


            // pull towards center
            //fileNodes[i].PullTowards( this.transform.position , .5f );


            float angle = (float)i / (float) fileNodes.Count;
            float nID = angle;

            angle *= Mathf.PI;

            if(fileNodes[i].hovered|| fileNodes[i].held ){
                hoveredNode = fileNodes[i];
            }


            Vector3 outVec = (Mathf.Sin(angle)*Vector3.left -Mathf.Cos(angle) *Vector3.up);
            outVec *= -1;

            // only do forces if outside receiver
            if( fileNodes[i].insideReceiver == null ){
                fileNodes[i].PullTowards( this.transform.position + outVec * fileSpawnRadius , 3);

            // fileNodes[i].AddForce(Vector3.right * 2);

                // push away from others
                for( int j = 0; j < fileNodes.Count; j++  ){

                    if( i != j ){

                        Vector3 dif = fileNodes[i].transform.position - fileNodes[j].transform.position;
                    
                        fileNodes[i].AddForce(dif.normalized * .02f);

                        if( fileNodes[i].hovered || fileNodes[i].held ){
                            fileNodes[j].AddForce(-dif.normalized * .5f);
                        }

                        

                    }

                }
            }

        }






        for( int i = 0; i < parentNodes.Count; i++ ){
            

              if(parentNodes[i].hovered|| parentNodes[i].held ){
                hoveredNode = parentNodes[i];
            }
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
        


        if( hoveredNode != null ){
            hoveredInfo.text = hoveredNode.name;
            hoveredInfo.transform.position = transform.position - transform.forward * .5f;
           // hoveredInfo.transform.position = hoveredNode.transform.position - transform.forward * .5f;
            //hoveredInfo.transform.rotation = Quaternion.LookRotation(Vector3.forward);
            hoveredLR.SetPosition(0,hoveredNode.transform.position );
            hoveredLR.SetPosition(1,transform.position );
        }else{
            hoveredInfo.text = "nothignHovered";
            hoveredLR.SetPosition(0,transform.position );
            hoveredLR.SetPosition(1,transform.position );
            
            hoveredInfo.transform.position =Vector3.one * 1000;
        }

        if( selectedObject != null ){
            if(selectedObject.GetType() == typeof(DirectoryNode)){
                selectedInfo.text = ((DirectoryNode)selectedObject).text.text;
            }else if( selectedObject.GetType() == typeof(FileNode)){
                selectedInfo.text = ((FileNode)selectedObject).path;
            }
        }else{
            selectedInfo.text = "nothing selected";
        }




    }

}
