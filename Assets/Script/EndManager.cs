using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    //[SerializeField] private Player player;

    //public float cherry;
    public GameObject finishUI;
    public GameObject FadeUI;

    public float restartTimer;
    private CanvasGroup fadeCanvasGroup;
    private AudioSource audioSource;

    public GameObject[] fruit;

    [SerializeField]
    private int fruitNum;

    private void Awake()
    {
        fadeCanvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();

    }

    void Start()
    {

        fruit = GameObject.FindGameObjectsWithTag("Cherry");
        fruitNum = fruit.Length;

    }

    public void CherryEnding()
    {
        //cherry--;
        //for迴圈 寫偵測arrey是不是空的 arrey 長度
        /*
        for (int i = 0; i < fruit.Length; i++)
        {
            if (fruit[i] == null)
            {
                fruitNum--;
            }
        }*/
        /*
        fruitNum =0;
        for (int i = 0; i < fruit.Length; i++)
        {
            if (fruit[i] != null)
            {
                fruitNum++;
            }
        }*/
        fruitNum--;
        if (fruitNum == 0)
        {
            Player.instance.playEnd = true;
            finishUI.SetActive(true);
            FadeUI.SetActive(true);
            StartCoroutine(FadeOut());
            StartCoroutine(DoAudioSourceVolumeOut());
            Invoke("Restart", restartTimer);
        }
    }

    IEnumerator FadeOut()
    {
        float currentTime = 0f;
        while (currentTime < restartTimer)
        {
            currentTime += Time.deltaTime;
            fadeCanvasGroup.alpha = 0 + (currentTime / restartTimer);
            yield return null;
        }
    }

    private IEnumerator DoAudioSourceVolumeOut()
    {
        float startVolume = audioSource.volume;
        float time = 0f;

        while (time < restartTimer)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, time / restartTimer);
            yield return null;
        }

    }
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
        

}
