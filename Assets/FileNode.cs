using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileNode : Moveable
{
  
  public string path;
     public override void Create(){

       
        MoveableCreate();
        print("fileNodeCreated");
        print(path);
    }


    public void PlayAudio(){
        StartCoroutine(LoadAudio());
    }

    AudioClip clip;
    private IEnumerator LoadAudio()
    {

        print("asdada");
        WWW request = GetAudioFromFile(path);
        yield return request;

        clip = request.GetAudioClip();
        clip.name = path;

        PlayAudioFile();
    }

    private void PlayAudioFile()
    {

        data.audio.mainAudio.clip = clip;
        data.audio.mainAudio.Play();

    }

    private WWW GetAudioFromFile(string path)
    {
        WWW request = new WWW(path);
        return request;
    }

    public void StopAudio(){
        data.audio.mainAudio.Stop();
    }
}
