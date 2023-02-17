using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherCherry : MonoBehaviour
{
    public  void Death()
    {
        FindObjectOfType<Player>().CherryCount();
        FindObjectOfType<EndManager>().CherryEnding();
        Destroy(gameObject);
    }
}
