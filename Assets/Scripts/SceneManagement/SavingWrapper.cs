using System.Collections;
using UnityEngine;
using RPG.Saving;
using RPG.SceneManagement;

public class SavingWrapper : MonoBehaviour
{
    const string defaultSaveFile = "save";
    [SerializeField] float fadeInTime = 0.2f;

    IEnumerator Start() 
    {
        Fader fader = FindObjectOfType<Fader>();
        fader.FadeOutImmediately();
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
        yield return fader.FadeIn(fadeInTime);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            Load();
        }    
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Save();
        }
    }

    public void Load()
    {
        GetComponent<SavingSystem>().Load(defaultSaveFile);
    }

    public void Save()
    {
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }
}
