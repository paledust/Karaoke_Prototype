using SimpleAudioSystem;
using UnityEngine;

public class AnimationEventHelper : MonoBehaviour
{
    [SerializeField] private float volumeScale = 1;
    public void PlayAudioData(Object data)
    {
        if(data is AudioData_SO audioData)
            AudioManager.Instance.PlaySFX(audioData.AudioKey, volumeScale);            
        else
            Debug.LogWarning("AnimationEventHelper: Data didn't match!!!");
    }
}
