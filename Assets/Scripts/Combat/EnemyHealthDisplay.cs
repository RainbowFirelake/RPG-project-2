using System;
using RPG.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Text text;
        private Health enemyHealth;
        
        private void Start() 
        {
            text = GetComponent<Text>();
        }
        
        private void Update() 
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().GetTarget() != null)
            {
                enemyHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().GetTarget();
                text.text = String.Format("{0:0}%", enemyHealth.GetPercentage());
            }
            else 
            {
                text.text = "Нету";
                return;
            }
        }
    }
}