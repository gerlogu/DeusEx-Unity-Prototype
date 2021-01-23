using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PostProcessManagerHDRP : MonoBehaviour
{
    [SerializeField] private Volume m_Volume;
    [SerializeField] private PlayerMovement m_Player;
    [SerializeField] private float originalChromaticAberration = 0.1f;

    ChromaticAberration chromaticAberration;
    ColorAdjustments colorAdjustments;
    Vignette vignette;
    LiftGammaGain liftGammaGain;

    bool slowMotion;

    // Start is called before the first frame update
    void Start()
    {
        if (!m_Player)
        {
            m_Player = FindObjectOfType<PlayerMovement>();
        }

        if (!m_Volume)
        {
            m_Volume = GetComponent<Volume>();
        }

        m_Volume.sharedProfile.TryGet<ChromaticAberration>(out chromaticAberration);
        m_Volume.sharedProfile.TryGet<ColorAdjustments>(out colorAdjustments);
        m_Volume.sharedProfile.TryGet<Vignette>(out vignette);
        m_Volume.sharedProfile.TryGet<LiftGammaGain>(out liftGammaGain);

        m_Player.OnDash += InitDashEffect;
    }

    void InitDashEffect()
    {
        chromaticAberration.intensity.value = 0.3f;
        
        // FindObjectOfType<Camera>().fieldOfView = FindObjectOfType<Camera>().fieldOfView + 3;
        // Debug.Log("Dash Occlusion");
    }

    // Update is called once per frame
    void Update()
    {
        chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, originalChromaticAberration, 5* Time.deltaTime);

        if(FindObjectOfType<TimeScaleManager>().timeScale < 0.5f)
        {
            colorAdjustments.saturation.value = Mathf.Lerp(colorAdjustments.saturation.value, 12.5f, 20 * Time.unscaledDeltaTime);
            vignette.smoothness.value = Mathf.Lerp(vignette.smoothness.value, 0.225f, 20 * Time.unscaledDeltaTime);
            liftGammaGain.gain.value = Vector4.Lerp(liftGammaGain.gain.value, new Vector4(liftGammaGain.gain.value.x, liftGammaGain.gain.value.y, liftGammaGain.gain.value.z, 0.05f), 20 * Time.unscaledDeltaTime);
        }
        else
        {
            colorAdjustments.saturation.value = Mathf.Lerp(colorAdjustments.saturation.value, 5, 5 * Time.unscaledDeltaTime);
            vignette.smoothness.value = Mathf.Lerp(vignette.smoothness.value, 0.3f, 5 * Time.unscaledDeltaTime);
            liftGammaGain.gain.value = Vector4.Lerp(liftGammaGain.gain.value, new Vector4(liftGammaGain.gain.value.x, liftGammaGain.gain.value.y, liftGammaGain.gain.value.z, 0), 5 * Time.unscaledDeltaTime);
        }
        
    }
}
