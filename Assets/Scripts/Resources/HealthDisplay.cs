using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class HealthDisplay : MonoBehaviour
    {
        private Health healthToDisplay;
        private Text healthShowing;

        private void Awake() 
        {
            healthToDisplay = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            healthShowing = GetComponent<Text>();
        }

        private void Update() 
        {
            healthShowing.text = String.Format("{0:0}%", healthToDisplay.GetPercentage());
        }
    }
}