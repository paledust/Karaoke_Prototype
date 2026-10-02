using UnityEngine;

public class PerRenderWhisper : PerRendererBehavior
{
    public float dissolveStart = 1;
    private const string DISSOLVE_START = "_DissolveStart";
    protected override void UpdateProperties()
    {
        base.UpdateProperties();
        mpb.SetFloat(DISSOLVE_START, dissolveStart);
    }
}
