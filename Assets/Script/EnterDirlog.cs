using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDirlog : MonoBehaviour
{
    public GameObject enterDirlog;
    public GameObject[] fruit;

    [SerializeField]
    private int fruitNum;
    [SerializeField]
    private bool canTrans;
    [SerializeField]
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canTrans)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        fruit = GameObject.FindGameObjectsWithTag("Cherry");

        fruitNum = fruit.Length;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" &&fruitNum ==0)
        {
            canTrans = true;

            enterDirlog.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canTrans = false;
            enterDirlog.SetActive(false);
        }
    }

    public void CherryEnding()
    {
        fruitNum--;
    }
}
