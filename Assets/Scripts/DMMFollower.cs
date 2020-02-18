using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Windows.Forms;
using System.Drawing;
using DesktopMascotMaker;

public class DMMFollower : MonoBehaviour
{
    public MascotMakerMulti mascotMakerMulti;

    public Vector2 Offset = new Vector2(100, 100);

    void Start()
    {
        // mascotMakerMulti must not be null
        Debug.Assert(mascotMakerMulti != null, "mascotMakerMulti != null", transform);
    }

    void Update()
    {
        mascotMakerMulti.Location = Point.Add(MascotMaker.Instance.Location, new Size((int)Offset.x, (int)Offset.y));
    }
}
