using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    [SerializeField]
    string activatorTag = null;
    [SerializeField]
    bool deactivateOnExit = false;
    [SerializeField]
    GameObject[] objs = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(activatorTag))
        {
            foreach (GameObject obj in objs)
            {
                obj.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (deactivateOnExit && collision.CompareTag(activatorTag))
        {
            foreach (GameObject obj in objs)
            {
                obj.SetActive(false);
            }
        }
    }
}
