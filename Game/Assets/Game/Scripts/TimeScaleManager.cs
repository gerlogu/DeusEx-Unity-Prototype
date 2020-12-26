using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    public bool slowMotion = false;
    public float smooth = 8f;
    public float timeScale = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float timeScaleFactor = 0.01f;
        //fireRate -= Time.deltaTime;
        if (Time.timeScale > .96f && Time.timeScale != 1 && !slowMotion)
        {
            Time.timeScale = 1;
            //Time.fixedDeltaTime = originalTimeFixedDeltaTime;
            Time.fixedDeltaTime = Time.timeScale * timeScaleFactor;
        }
        else if (Time.timeScale <= .96f && Time.timeScale != 0 && !slowMotion)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, timeScale, smooth * Time.deltaTime);
            Time.fixedDeltaTime = Time.timeScale * timeScaleFactor;
        }

        //if (Time.timeScale > .96f && Time.timeScale != 1 && !slowMotion)
        //{
        //    Time.timeScale = 1;
        //    //Time.fixedDeltaTime = originalTimeFixedDeltaTime;
        //    Time.fixedDeltaTime = Time.timeScale * timeScaleFactor;
        //}
        //else if (Time.timeScale <= .96f && Time.timeScale != 0)
        //{
            // Time.timeScale = Mathf.Lerp(Time.timeScale, timeScale, smooth * Time.deltaTime);
            // Time.fixedDeltaTime = Time.timeScale * timeScaleFactor;
        //}

        if (slowMotion)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, timeScale, 12 * Time.deltaTime);
            Time.fixedDeltaTime = Time.timeScale * timeScaleFactor;
        }
    }

    public void RestoreTimeDilation()
    {
        smooth = 8f;
        slowMotion = false;
        timeScale = 1;
    }

    public void SlowMotionKill()
    {
        smooth = 8f;
        timeScale = 0.15f;
        slowMotion = true;
        Invoke("RestoreTimeDilation", 0.2f);
    }
}
