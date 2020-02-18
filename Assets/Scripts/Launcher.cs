using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Launch());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Launch()
    {
        yield return null;
        Screen.SetResolution(600, 600, FullScreenMode.Windowed);
        yield return null;
        SceneManager.LoadScene("Everyday");
    }
}
