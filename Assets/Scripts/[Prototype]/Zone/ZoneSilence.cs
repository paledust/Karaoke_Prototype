using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using WhisperPrototype;

public class ZoneSilence : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    private const string AMB_Volume = "amb_volume";
    private const string AMB_Cutoff = "amb_cutoff";
    private const string SFX_Volume = "whisper_volume";
    private static int layer = 0;
    private bool isEffecting;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(layer == 0)
            {
                mixer.DOKill();
                mixer.DOSetFloat(AMB_Volume, -40, 1);
                mixer.DOSetFloat(AMB_Cutoff, 5000, 1);
                mixer.DOSetFloat(SFX_Volume, 0, 1f);
            }
            isEffecting = true;
            layer ++;
        }
    }
    void OnDestroy()
    {
        if(isEffecting)
        {
            layer --;
            isEffecting = false;
            layer = Mathf.Max(0, layer);
            if(layer == 0)
            {
                mixer.DOKill();
                mixer.DOSetFloat(AMB_Volume, 0, 1);
                mixer.DOSetFloat(AMB_Cutoff, 22000, 1);
                mixer.DOSetFloat(SFX_Volume, -40, 1f);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(isEffecting)
            {
                layer --;
                isEffecting = false;
                layer = Mathf.Max(0, layer);
                if(layer == 0)
                {
                    mixer.DOKill();
                    mixer.DOSetFloat(AMB_Volume, 0, 1);
                    mixer.DOSetFloat(AMB_Cutoff, 22000, 1);
                    mixer.DOSetFloat(SFX_Volume, -40, 1f);
                }
            }
        }
    }
}
