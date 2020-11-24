using UnityEngine;

using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoint;

        private bool isDead = false;

        private void Start() 
        {
            healthPoint = GetComponent<BaseStats>().GetStat(Stat.Health);
            StartDie();
        }

        private void StartDie()
        {
            if (healthPoint < 1)
            Die();
        }

        public bool IsDead()
        {   
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoint = Mathf.Max(healthPoint - damage, 0);
            print(damage);
            if(healthPoint <= 0) 
            {
                Die();
                ExperienceAward(instigator);
            }
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public float GetPercentage()
        {
            return (healthPoint / GetComponent<BaseStats>().GetStat(Stat.Health)) * 100;
        }

        private void ExperienceAward(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.Experience));
        }

        public object CaptureState()
        {
            return healthPoint;
        }

        public void RestoreState(object state)
        {
            healthPoint = (float)state;
            if (healthPoint == 1)
            {
                Die();
            }
        }
    }
}