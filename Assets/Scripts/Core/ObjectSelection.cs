using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelection : MonoBehaviour
{
    [SerializeField] GameObject objectToHighlight;

    private void OnMouseEnter() {
        objectToHighlight.GetComponent<Renderer>().material.color = Color.red;
    }

    private void OnMouseExit() {
        objectToHighlight.GetComponent<Renderer>().material.color = Color.white;
    }
}
