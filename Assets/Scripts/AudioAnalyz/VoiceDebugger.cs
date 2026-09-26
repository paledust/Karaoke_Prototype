using System.Runtime.InteropServices;
using AudioAnalysis;
using UnityEngine;

public class VoiceDebugger : MonoBehaviour
{
    [SerializeField] private AudioAnalyzer audioAnalyzer;
    [SerializeField, ShowOnly] private string frequency = "detected frequency";
    [SerializeField, ShowOnly] private string note = "detected note";
    [SerializeField, ShowOnly] private float volume;

    [Header("Draw Line")]
    [SerializeField] private Material mat;
    
    [DllImport("AudioPluginDemo")]
    private static extern float PitchDetectorGetFreq(int index);
    [DllImport("AudioPluginDemo")]
    private static extern int PitchDetectorDebug(float[] data);
    
    float[] history = new float[1000];
    float[] debug = new float[65536];

    void Update()
    {
        frequency = audioAnalyzer.m_freq.ToString() + "Hz";
        note = audioAnalyzer.m_note;
        volume = audioAnalyzer.m_volumeLevel;
    }
    Vector3 Plot(float[] data, int num, float x0, float y0, float w, float h, Color col, float thr)
    {
        GL.Begin(GL.LINES);
        GL.Color(col);
        float xs = w / num, ys = h;
        float px = 0, py = 0;
        for (int n = 1; n < num; n++)
        {
            float nx = x0 + n * xs, ny = y0 + data[n] * ys;
            if (n > 1 && data[n] > thr && data[n - 1] > thr)
            {
                GL.Vertex3(px, py, 0);
                GL.Vertex3(nx, ny, 0);
            }
            px = nx;
            py = ny;
        }
        GL.End();
        return new Vector3(x0 + w, py, 0);
    }
    void OnRenderObject()
    {
        mat.SetPass(0);

        GL.Begin(GL.LINES);
        GL.Color(Color.green);
        GL.Vertex3(-5, 0, 0);
        GL.Vertex3(5, 0, 0);
        GL.End();

        for (int n = 1; n < history.Length; n++)
            history[n - 1] = history[n];
        history[history.Length - 1] = PitchDetectorGetFreq(0);
        transform.position = Plot(history, history.Length, -45.0f, 0.0f, 50.0f, 0.01f, Color.white, 0.1f);
        int num = PitchDetectorDebug(debug);
        Plot(debug, num, -5.0f, 1.0f, 10.0f, 0.0002f, Color.red, 0.1f);
    }
}
