using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public struct ScriptTest
{
    public string script;
}

public class Mic : MonoBehaviour
{
    private AudioClip _record;
    private AudioSource _aud;

    private float startTime;

    private void Start()
    {
        _aud = GetComponent<AudioSource>();

    }

    public void SavSnd()
    {
        SavWav.Save("C:/Users/Admin/Music/Record", _aud.clip);
    }

    public void Send()
    {
        SendText();
        SendAudio();
    }

    public void SendText()
    {
        ScriptTest scripttest = new ScriptTest();
        scripttest.script = "배열이 들어감";
        HttpInfo info = new HttpInfo();
        info.url = "http://172.20.10.7:8080/ppt/edit_script"; //어디다 보내는지 알아야함
        info.body = JsonUtility.ToJson(scripttest);
        info.contentType = "application/json";
        info.onComplete = (DownloadHandler downloadHandler) =>
        {
            print(downloadHandler.text); //확인용
        };
    }

    public void SendAudio()
    {
        HttpInfo info = new HttpInfo();
        info.url = "http://172.20.10.7:8080/ppt/compare_similarity"; //url 확인필요
        info.contentType = "multipart/form-data";
        info.body = "C:\\Users\\Admin\\Music\\Record.WAV";

        info.onComplete = (DownloadHandler downloadHandler) =>
        {
            print("결과 크기 : " + downloadHandler.data.Length);
        };

        StartCoroutine(HttpMgr.GetInstance().UploadFileByFormData(info));
    }

    public void EndRcd()
    {
        int lastTime = Microphone.GetPosition(null);

        if (lastTime == 0)
            return;
        else
        {
            Microphone.End(Microphone.devices[0]);

            float[] samples = new float[_record.samples];

            _record.GetData(samples, 0);

            float[] cutSamples = new float[lastTime];

            Array.Copy(samples, cutSamples, cutSamples.Length - 1);

            _record = AudioClip.Create("Notice", cutSamples.Length, 1, 44100, false);

            _record.SetData(cutSamples, 0);
        }
        _aud.clip = _record;
    }


    public void RecSnd()
    {
        //_record = Microphone.Start(Microphone.devices[0].ToString(), false, 50, 44100);
        _record = Microphone.Start(Microphone.devices[0].ToString(), true, 100, 44100);

    }
}