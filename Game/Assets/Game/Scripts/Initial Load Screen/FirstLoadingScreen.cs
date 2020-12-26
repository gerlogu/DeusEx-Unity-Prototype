using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstLoadingScreen : MonoBehaviour
{
    [SerializeField] Image fade;
    [SerializeField] GameObject background;
    [SerializeField] GameObject loader;
    [SerializeField] TextMeshProUGUI loadPercentageText;
    [HideInInspector] int sceneIndex = 1;

    float backgroundNewPosition;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        fade.gameObject.SetActive(true);
        if(background)
            backgroundNewPosition = background.GetComponent<RectTransform>().position.x - 200;
        Invoke("LoadScene", 0.2f);
    }

    private void Update()
    {
        if (background)
            background.GetComponent<RectTransform>().position = Vector3.Lerp(background.GetComponent<RectTransform>().position, new Vector3(backgroundNewPosition, background.GetComponent<RectTransform>().position.y, background.GetComponent<RectTransform>().position.z), Time.unscaledDeltaTime * 0.1f);
    }

    public void HideLoader()
    {
        loader.SetActive(false);
        Time.timeScale = 1;
    }


    /// <summary>
    /// Loads the Scene.
    /// </summary>
    void LoadScene()
    {
        Time.timeScale = 0;
        GetComponent<Animator>().SetTrigger("FadeIn");
        StartCoroutine(LoadASynchrously(sceneIndex));
    }

    IEnumerator LoadASynchrously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        //slider.enabled = true;

        while (!operation.isDone)
        {
            //float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if(loadPercentageText)
                loadPercentageText.text = (int)(operation.progress * 100) + "%";
            yield return null;

        }

        GetComponent<Animator>().SetTrigger("FadeOutIn");
    }
}
