using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

public class Launcher : MonoBehaviour
{
    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
    static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    static extern int GetWindowLong(IntPtr hwnd, int nIndex);
    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);
    delegate bool EnumWindowsCallBack(IntPtr hwnd, IntPtr lParam);
    [DllImport("user32")]
    static extern int EnumWindows(EnumWindowsCallBack lpEnumFunc, IntPtr lParam);
    [DllImport("user32")]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref IntPtr lpdwProcessId);

    const int LWA_ALPHA = 0x2;
    const int GWL_EXSTYLE = -20;
    const int WS_EX_LAYERED = 0x80000;

    public static IntPtr MainWindowHandle { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Screen.SetResolution(600, 600, FullScreenMode.Windowed);
        StartCoroutine(HideWindow());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator HideWindow()
    {
        yield return null;
#if !UNITY_EDITOR
        IntPtr pid = (IntPtr)Process.GetCurrentProcess().Id;
        EnumWindows(new EnumWindowsCallBack(EnumWindCallback), pid);
        int extStyle = GetWindowLong(MainWindowHandle, GWL_EXSTYLE);
        SetWindowLong(MainWindowHandle, GWL_EXSTYLE, extStyle | WS_EX_LAYERED);
        SetLayeredWindowAttributes(MainWindowHandle, 0, 0, LWA_ALPHA);
#endif

        SceneManager.LoadScene("Everyday");
    }

    static bool EnumWindCallback(IntPtr hwnd, IntPtr lParam)
    {
        IntPtr pid = IntPtr.Zero;
        GetWindowThreadProcessId(hwnd, ref pid);
        if (pid == lParam)  //判断当前窗口是否属于本进程
        {
            MainWindowHandle = hwnd;
            return false;
        }
        return true;
    }
}
