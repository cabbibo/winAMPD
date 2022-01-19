using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using System.Security.AccessControl;

using System.Security.Principal;
public class DirectoryNode : Moveable
{

    public TextMeshPro  text;
    public string directoryPath;

    public GameObject DirectoryNodePrefab;

    FileInfo[] fileInfo;
    string[] folderInfo;
    public GameObject[] subMoveables;

    public float spawnRadius;


   public override void Create(){
       
        lr = GetComponent<LineRenderer>();
        text.text = directoryPath;

         DirectoryInfo dir = new DirectoryInfo(directoryPath);
      
    fileInfo = dir.GetFiles("*.*");
    folderInfo = Directory.GetDirectories(directoryPath);

    List<string> useableFolders = new List<string>();

    
    for( int i = 0; i<  folderInfo.Length; i++ ){

        
        DirectoryInfo dir2 = new DirectoryInfo(folderInfo[i]);

        bool canAdd = true;

        try
        {
            
            FileInfo[] f = dir2.GetFiles("*.*");
        }
        catch (System.UnauthorizedAccessException ex)
        {

            canAdd = false;
        }

        if( canAdd ){ useableFolders.Add(folderInfo[i]); }
        

    }

    folderInfo = useableFolders.ToArray();


    }

    public static bool HasFolderWritePermission(string destDir)
{
   if(string.IsNullOrEmpty(destDir) || !Directory.Exists(destDir)) return false;
   try
   {
      DirectorySecurity security = Directory.GetAccessControl(destDir);
      SecurityIdentifier users = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
      foreach(AuthorizationRule rule in security.GetAccessRules(true, true, typeof(SecurityIdentifier)))
      {
          if(rule.IdentityReference == users)
          {
             FileSystemAccessRule rights = ((FileSystemAccessRule)rule);
             if(rights.AccessControlType == AccessControlType.Allow)
             {
                    if(rights.FileSystemRights == (rights.FileSystemRights | FileSystemRights.Read)) return true;
             }else{
                 print("doesnt allow");
             }
          }
       }
       return false;
    }
    catch
    {
        return false;
    }
}


    public static bool HasWritePermission(string tempfilepath)
        {
            try
            {
                System.IO.File.Create(tempfilepath + "temp.txt").Close();
                System.IO.File.Delete(tempfilepath + "temp.txt");
            }
            catch (System.UnauthorizedAccessException ex)
            {

                return false;
            }

            return true;
        }

    public override void OnSelected( Receiver r ){
        print("DIRECTORY PATH " + directoryPath);

        print( fileInfo.Length );
        for( int i = 0; i<  fileInfo.Length; i++ ){
            print( fileInfo[i] );
        }

         print( folderInfo.Length );
         subMoveables = new GameObject[ folderInfo.Length ];
        for( int i = 0; i<  folderInfo.Length; i++ ){

            float angle = (float)i / (float) folderInfo.Length;
            float nID = angle;

            angle *= Mathf.PI;

            GameObject child = GameObject.Instantiate(DirectoryNodePrefab);

            Moveable m = child.GetComponent<Moveable>();


            Vector3 outVec = (Mathf.Sin(angle)*Vector3.left -Mathf.Cos(angle) *Vector3.up);
            outVec *= -1;

            child.transform.position = transform.position + outVec  * spawnRadius  * (.5f + .5f * nID); 
            child.transform.Rotate(Vector3.forward*angle);
            m.targetPosition = transform.position + outVec * spawnRadius;
            DirectoryNode dn = (DirectoryNode)m;
            dn.directoryPath = folderInfo[i];
            m.moveables = moveables;
            moveables.moveables.Add(m);
            moveables.JumpStart(m);
            subMoveables[i] = child;
            


        }


        transform.position = r.transform.position;
        vel = new Vector3(0,0,0);

    }

    public override void OnDeselected(Receiver r ){
        for(int i = 0; i< subMoveables.Length; i++ ){
            if( subMoveables[i]!= null ){
                moveables.JumpDeath(subMoveables[i].GetComponent<Moveable>());
                moveables.moveables.Remove(subMoveables[i].GetComponent<Moveable>());
                DestroyImmediate(subMoveables[i]);
            }
        }

        subMoveables= new GameObject[0];
    }
}
