using System;
using UnityEngine;

public class VoiceMarker : MonoBehaviour
{
    [SerializeField] private string detectTag;
    [SerializeField] private SpriteRenderer volumeMarker;
    [SerializeField] private SpriteRenderer pitchMarker;
    [SerializeField] private Color falseColor;
    [SerializeField] private Color trueColor;
    private Transform voiceBallTrans;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == detectTag)
        {
            pitchMarker.color = trueColor;
            voiceBallTrans = other.transform;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == detectTag)
        {
            pitchMarker.color = falseColor;
            voiceBallTrans = null;
        }
    }

    void Update()
    {
        if (voiceBallTrans != null)
        {
            if (voiceBallTrans.localScale.x > 1.5f)
                volumeMarker.color = trueColor;
            else
                volumeMarker.color = falseColor;
        }
        else
        {
            if(volumeMarker.color != falseColor)
                volumeMarker.color = falseColor;
        }
    }
}
