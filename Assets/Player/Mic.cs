using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mic : MonoBehaviour
{
    private AudioClip _record;
    private AudioSource _aud;

    private float startTime;

    private void Start()
    {
        _aud = GetComponent<AudioSource>();

    }

    public void PlaySnd()
    {
        _aud.Play();
    }

    public void SavSnd()
    {
        SavWav.Save("C:/Users/Admin/Music/Record", _aud.clip);
    }

    public void EndRcd1()
    {
        Microphone.End(Microphone.devices[0].ToString());
        _aud.clip = _record;
    }

    public void EndRcd2()
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



    
    
    