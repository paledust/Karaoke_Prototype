using UnityEngine;
using UnityEngine.Audio;

public class ZoneSilence : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    private const string AMB_Volume = "amb_volume";
    private const string AMB_Cutoff = "amb_cutoff";

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            mixer.SetFloat(AMB_Volume, -25);
            mixer.SetFloat(AMB_Cutoff, 5000);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            mixer.SetFloat(AMB_Volume, 0);
            mixer.SetFloat(AMB_Cutoff, 22000);
        }
    }
}
