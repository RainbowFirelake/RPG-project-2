using UnityEngine;
using UnityEngine.UI;
using RPG.Resources;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats stats;

        private void Awake()
        {
            stats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            GetComponent<Text>().text = string.Format("{0:0}", stats.GetLevel());
        }
    }
}