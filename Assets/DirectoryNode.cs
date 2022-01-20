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

    public FileInfo[] fileInfo;
    public string[] folderInfo;
    public GameObject[] subMoveables;

    public string parentDirectory;

   public override void Create(){

       
        MoveableCreate();

        if( parentDirectory == null || parentDirectory == "" ){
            text.text = directoryPath;
        }else{
            text.text =directoryPath.Replace(parentDirectory,"");
        }

         DirectoryInfo dir = new DirectoryInfo(directoryPath);
        
        fileInfo = dir.GetFiles("*.*");
        folderInfo = Directory.GetDirectories(directoryPath);

        List<string> useableFolders = new List<string>();

        // getting a list of all folders we can go into!
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






}
