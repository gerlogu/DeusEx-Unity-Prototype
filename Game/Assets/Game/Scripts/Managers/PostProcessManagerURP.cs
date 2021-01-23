using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
//using UnityEngine.Rendering.Universal;

public class PostProcessManagerURP : MonoBehaviour
{
    public float aberrationIntensity = 0f;

    [Header("Lens Distortion")]
    public float maxLensDistortionIntensity = 10;
    public float minLensDistortionIntensity = 0;
    public float lensDistortionIntensity = 0f;

    [Header("Chromatic Aberration")]
    public float chromaticAberration = 0.125f;
    public float currentChromaticAberration = 0.125f;

    private float originalLensDistortionIntensity;
    private float newLensDistortionIntensity;
    //ChromaticAberration chromaticAberrationLayer = null;
    //LensDistortion lensDistortionLayer = null;
    //ChromaticAberration chromaticAberrationLayer = null;

    private PlayerMovement player;

    Volume volume;
    // Start is called before the first frame update
    void Start()
    {
        // somewhere during initializing
        volume = gameObject.GetComponent<Volume>();
        //volume.profile.TryGet<LensDistortion>(out lensDistortionLayer);
        //volume.profile.TryGet<ChromaticAberration>(out chromaticAberrationLayer);
        //volume.profile.TryGetSettings(out lensDistortionLayer);
        //originalLensDistortionIntensity = lensDistortionLayer.intensity.value;

        if (FindObjectOfType<PlayerMovement>())
        {
            player = FindObjectOfType<PlayerMovement>();
            player.OnInitSprint += () => {
                newLensDistortionIntensity = -maxLensDistortionIntensity;
            };

            player.OnEndSprint += () => {
                newLensDistortionIntensity = minLensDistortionIntensity;
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
        //aberrationIntensity = Mathf.Lerp(aberrationIntensity, 0f, 5f * Time.deltaTime);

        //chromaticAberrationLayer.intensity.value = aberrationIntensity;

        if (FindObjectOfType<PlayerMovement>() && !player)
        {
            player = FindObjectOfType<PlayerMovement>();
            player.OnInitSprint += () => {
                newLensDistortionIntensity = -maxLensDistortionIntensity;
            };

            player.OnEndSprint += () => {
                newLensDistortionIntensity = minLensDistortionIntensity;
            };
        }

        if (player)
        {
            lensDistortionIntensity = Mathf.Lerp(lensDistortionIntensity, newLensDistortionIntensity, 2f * Time.deltaTime);
            // lensDistortionLayer.intensity.value = lensDistortionIntensity;
        }

        currentChromaticAberration = Mathf.Lerp(currentChromaticAberration, chromaticAberration, 2f * Time.deltaTime);

        //chromaticAberrationLayer.intensity.value = currentChromaticAberration;
    }
}
