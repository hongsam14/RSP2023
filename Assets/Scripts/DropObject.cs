using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DropObject : MonoBehaviour
{
    public Action<DropObject> killAction { get; set; } = null;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        killAction(this);
    }
}
