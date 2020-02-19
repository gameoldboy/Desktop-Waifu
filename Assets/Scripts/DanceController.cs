using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesktopMascotMaker;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

public class DanceController : MonoBehaviour
{
    struct LPRECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [DllImport("user32.dll")]
    static extern bool GetWindowRect(IntPtr hWnd, out LPRECT lpRect);

    public Transform View;
    public Transform FollowBone;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartPlay());
#if !UNITY_EDITOR
        SetCharCenter();
#endif
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
        GetWindowRect(Launcher.MainWindowHandle, out LPRECT lpRect);
        var x = lpRect.left + (lpRect.right - lpRect.left) / 2;
        var y = lpRect.top + (lpRect.bottom - lpRect.top) / 2;
        MascotMaker.Instance.Location = new Point(x - 300, y - 400);
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
