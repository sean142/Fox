using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDirlog : MonoBehaviour
{
    public GameObject enterDirlog;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            enterDirlog.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enterDirlog.SetActive(false);
        }
    }
}
