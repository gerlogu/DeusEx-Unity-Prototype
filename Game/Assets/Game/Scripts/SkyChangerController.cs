using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
public class SkyChangerController : MonoBehaviour
{

    public Volume m_Volume;
    public Gradient topGradient;
    public Gradient middleGradient;
    public Gradient botGradient;
    public float maxTime = 30;
    public float time = 0;
    public Color top;
    public Color middle;
    public Color bot;
    public bool front = true;
    GradientSky gr;

    // Start is called before the first frame update
    void Start()
    {
        VolumeProfile profile = m_Volume.sharedProfile;
        if (!profile.TryGet<GradientSky>(out gr))
        {
            gr = profile.Add<GradientSky>(true);
        }
        else
        {
            profile.TryGet<GradientSky>(out gr);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        float eval = time / maxTime;
        top = topGradient.Evaluate(eval);
        middle = middleGradient.Evaluate(eval);
        bot = botGradient.Evaluate(eval);
        gr.top.value = top;
        gr.middle.value = middle;
        gr.bottom.value = bot;

        if (front)
        {
            time += Time.deltaTime;
            if(time >= maxTime)
            {
                front = false;
            }
        }
        else
        {
            time -= Time.deltaTime;
            if(time <= 0)
            {
                front = true;
            }
        }
            
    }
}
