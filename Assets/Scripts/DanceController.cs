using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesktopMascotMaker;
using System.Drawing;

public class DanceController : MonoBehaviour
{

    public Transform View;
    public Transform FollowBone;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartPlay());
        // StartCoroutine(SetCharCenter());
        SetCharCenter();
        View.position = new Vector3(FollowBone.position.x, 0.9f, FollowBone.position.z);
    }

    IEnumerator StartPlay()
    {
        yield return new WaitForSeconds(1);
        var animator = GetComponent<Animator>();
        animator.enabled = true;
    }

    void SetCharCenter()
    {
        var width = Screen.resolutions[Screen.resolutions.Length - 1].width;
        var height = Screen.resolutions[Screen.resolutions.Length - 1].height;
        MascotMaker.Instance.Location = new Point(width / 2 - 300, height / 2 - 400);
    }

    // Update is called once per frame
    void Update()
    {
        View.position = Vector3.Lerp(View.position, new Vector3(FollowBone.position.x, View.position.y, FollowBone.position.z), Time.deltaTime * 4);
    }

    public void Reset()
    {
        var animator = GetComponent<Animator>();
        animator.Play("miku_Face_vmd", 0, 0);
        animator.Play("miku_Body(Center)_vmd", 1, 0);
        var mmd4MecanimModel = GetComponent<MMD4MecanimModel>();
        mmd4MecanimModel.audioSource.time = 0;
        mmd4MecanimModel.audioSource.Play();
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
