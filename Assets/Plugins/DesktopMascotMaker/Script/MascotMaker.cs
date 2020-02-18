using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenCvSharp;
using UnityEngine;

namespace DesktopMascotMaker
{
    // Token: 0x02000002 RID: 2
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("DesktopMascotMaker/MascotMaker")]
    public class MascotMaker : MonoBehaviour
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        private MascotMaker()
        {
        }

        // Token: 0x06000002 RID: 2
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern short UnregisterClass(string lpClassName, IntPtr hInstance);

        // Token: 0x06000003 RID: 3
        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        // Token: 0x06000004 RID: 4
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000005 RID: 5 RVA: 0x00002120 File Offset: 0x00000320
        public static MascotMaker Instance
        {
            get
            {
                if (MascotMaker.instance == null)
                {
                    MascotMaker.instance = (UnityEngine.Object.FindObjectOfType(typeof(MascotMaker)) as MascotMaker);
                    if (MascotMaker.instance == null)
                    {
                        UnityEngine.Debug.LogWarning("There is no DesktopMaker in your scene!");
                    }
                }
                return MascotMaker.instance;
            }
        }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000006 RID: 6 RVA: 0x00002175 File Offset: 0x00000375
        // (set) Token: 0x06000007 RID: 7 RVA: 0x0000217D File Offset: 0x0000037D
        public int Opacity
        {
            get
            {
                return (int)this.opacity;
            }
            set
            {
                this.opacity = (byte)Mathf.Clamp(value, 0, 255);
            }
        }

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000008 RID: 8 RVA: 0x00002192 File Offset: 0x00000392
        public static IntPtr MainWindowHandle
        {
            get
            {
                return MascotMaker.mainWindowHandle;
            }
        }

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000009 RID: 9 RVA: 0x00002199 File Offset: 0x00000399
        public bool IsMouseHover
        {
            get
            {
                return this.isMouseHover;
            }
        }

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x0600000A RID: 10 RVA: 0x000021A1 File Offset: 0x000003A1
        // (set) Token: 0x0600000B RID: 11 RVA: 0x000021BB File Offset: 0x000003BB
        public int Left
        {
            get
            {
                if (this.Form != null)
                {
                    return this.Form.Left;
                }
                return -1;
            }
            set
            {
                if (this.Form != null)
                {
                    this.Form.Left = value;
                }
            }
        }

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x0600000C RID: 12 RVA: 0x000021D4 File Offset: 0x000003D4
        // (set) Token: 0x0600000D RID: 13 RVA: 0x000021EE File Offset: 0x000003EE
        public int Top
        {
            get
            {
                if (this.Form != null)
                {
                    return this.Form.Top;
                }
                return -1;
            }
            set
            {
                if (this.Form != null)
                {
                    this.Form.Top = value;
                }
            }
        }

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x0600000E RID: 14 RVA: 0x00002207 File Offset: 0x00000407
        // (set) Token: 0x0600000F RID: 15 RVA: 0x00002227 File Offset: 0x00000427
        public Point Location
        {
            get
            {
                if (this.Form != null)
                {
                    return this.Form.Location;
                }
                return new Point(0, 0);
            }
            set
            {
                if (this.Form != null)
                {
                    this.Form.Location = value;
                }
            }
        }

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x06000010 RID: 16 RVA: 0x00002240 File Offset: 0x00000440
        // (set) Token: 0x06000011 RID: 17 RVA: 0x0000225A File Offset: 0x0000045A
        public int Width
        {
            get
            {
                if (this.Form != null)
                {
                    return this.Form.Width;
                }
                return -1;
            }
            set
            {
                if (this.Form != null)
                {
                    this.MascotFormSize.x = (float)value;
                }
            }
        }

        // Token: 0x17000009 RID: 9
        // (get) Token: 0x06000012 RID: 18 RVA: 0x00002274 File Offset: 0x00000474
        // (set) Token: 0x06000013 RID: 19 RVA: 0x0000228E File Offset: 0x0000048E
        public int Height
        {
            get
            {
                if (this.Form != null)
                {
                    return this.Form.Height;
                }
                return -1;
            }
            set
            {
                if (this.Form != null)
                {
                    this.MascotFormSize.y = (float)value;
                }
            }
        }

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x06000014 RID: 20 RVA: 0x000022A8 File Offset: 0x000004A8
        public int ScreenWidth
        {
            get
            {
                if (this.Form != null)
                {
                    return System.Windows.Forms.Screen.GetBounds(this.Form).Width;
                }
                return -1;
            }
        }

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x06000015 RID: 21 RVA: 0x000022D8 File Offset: 0x000004D8
        public int ScreenHeight
        {
            get
            {
                if (this.Form != null)
                {
                    return System.Windows.Forms.Screen.GetBounds(this.Form).Height;
                }
                return -1;
            }
        }

        // Token: 0x1700000C RID: 12
        // (get) Token: 0x06000016 RID: 22 RVA: 0x00002305 File Offset: 0x00000505
        // (set) Token: 0x06000017 RID: 23 RVA: 0x00002323 File Offset: 0x00000523
        public string Title
        {
            get
            {
                if (this.Form != null)
                {
                    return this.Form.Text;
                }
                return "";
            }
            set
            {
                if (this.Form != null)
                {
                    this.Form.Text = value;
                }
            }
        }

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x06000018 RID: 24 RVA: 0x0000233C File Offset: 0x0000053C
        // (set) Token: 0x06000019 RID: 25 RVA: 0x00002356 File Offset: 0x00000556
        public bool AllowDrop
        {
            get
            {
                return this.Form != null && this.Form.AllowDrop;
            }
            set
            {
                if (this.Form != null)
                {
                    this.Form.AllowDrop = value;
                }
            }
        }

        // Token: 0x14000001 RID: 1
        // (add) Token: 0x0600001A RID: 26 RVA: 0x0000236F File Offset: 0x0000056F
        // (remove) Token: 0x0600001B RID: 27 RVA: 0x00002388 File Offset: 0x00000588
        public event EventHandler OnActivated
        {
            add
            {
                if (this.Form != null)
                {
                    this.Form.Activated += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this.Form.Activated -= value;
                }
            }
        }

        // Token: 0x14000002 RID: 2
        // (add) Token: 0x0600001C RID: 28 RVA: 0x000023A1 File Offset: 0x000005A1
        // (remove) Token: 0x0600001D RID: 29 RVA: 0x000023BA File Offset: 0x000005BA
        public event EventHandler OnDeactivate
        {
            add
            {
                if (this.Form != null)
                {
                    this.Form.Deactivate += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this.Form.Deactivate -= value;
                }
            }
        }

        // Token: 0x14000003 RID: 3
        // (add) Token: 0x0600001E RID: 30 RVA: 0x000023D3 File Offset: 0x000005D3
        // (remove) Token: 0x0600001F RID: 31 RVA: 0x000023EC File Offset: 0x000005EC
        public event DragEventHandler OnDragDrop
        {
            add
            {
                if (this.Form != null)
                {
                    this.Form.DragDrop += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this.Form.DragDrop -= value;
                }
            }
        }

        // Token: 0x14000004 RID: 4
        // (add) Token: 0x06000020 RID: 32 RVA: 0x00002405 File Offset: 0x00000605
        // (remove) Token: 0x06000021 RID: 33 RVA: 0x0000241E File Offset: 0x0000061E
        public event DragEventHandler OnDragEnter
        {
            add
            {
                if (this.Form != null)
                {
                    this.Form.DragEnter += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this.Form.DragEnter -= value;
                }
            }
        }

        // Token: 0x14000005 RID: 5
        // (add) Token: 0x06000022 RID: 34 RVA: 0x00002437 File Offset: 0x00000637
        // (remove) Token: 0x06000023 RID: 35 RVA: 0x00002450 File Offset: 0x00000650
        public event EventHandler OnDragLeave
        {
            add
            {
                if (this.Form != null)
                {
                    this.Form.DragLeave += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this.Form.DragLeave -= value;
                }
            }
        }

        // Token: 0x14000006 RID: 6
        // (add) Token: 0x06000024 RID: 36 RVA: 0x00002469 File Offset: 0x00000669
        // (remove) Token: 0x06000025 RID: 37 RVA: 0x00002482 File Offset: 0x00000682
        public event DragEventHandler OnDragOver
        {
            add
            {
                if (this.Form != null)
                {
                    this.Form.DragOver += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this.Form.DragOver -= value;
                }
            }
        }

        // Token: 0x14000007 RID: 7
        // (add) Token: 0x06000026 RID: 38 RVA: 0x0000249B File Offset: 0x0000069B
        // (remove) Token: 0x06000027 RID: 39 RVA: 0x000024B4 File Offset: 0x000006B4
        public event EventHandler OnMove
        {
            add
            {
                if (this.Form != null)
                {
                    this.Form.Move += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this.Form.Move -= value;
                }
            }
        }

        // Token: 0x14000008 RID: 8
        // (add) Token: 0x06000028 RID: 40 RVA: 0x000024CD File Offset: 0x000006CD
        // (remove) Token: 0x06000029 RID: 41 RVA: 0x000024E6 File Offset: 0x000006E6
        public event KeyEventHandler OnKeyDown
        {
            add
            {
                if (this.Form != null)
                {
                    this.Form.KeyDown += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this.Form.KeyDown -= value;
                }
            }
        }

        // Token: 0x14000009 RID: 9
        // (add) Token: 0x0600002A RID: 42 RVA: 0x000024FF File Offset: 0x000006FF
        // (remove) Token: 0x0600002B RID: 43 RVA: 0x00002518 File Offset: 0x00000718
        public event KeyEventHandler OnKeyUp
        {
            add
            {
                if (this.Form != null)
                {
                    this.Form.KeyUp += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this.Form.KeyUp -= value;
                }
            }
        }

        // Token: 0x1400000A RID: 10
        // (add) Token: 0x0600002C RID: 44 RVA: 0x00002531 File Offset: 0x00000731
        // (remove) Token: 0x0600002D RID: 45 RVA: 0x0000254A File Offset: 0x0000074A
        public event MouseEventHandler OnLeftMouseDown
        {
            add
            {
                if (this.Form != null)
                {
                    this.Form._LeftMouseDown += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this.Form._LeftMouseDown -= value;
                }
            }
        }

        // Token: 0x1400000B RID: 11
        // (add) Token: 0x0600002E RID: 46 RVA: 0x00002563 File Offset: 0x00000763
        // (remove) Token: 0x0600002F RID: 47 RVA: 0x0000257C File Offset: 0x0000077C
        public event MouseEventHandler OnLeftMouseUp
        {
            add
            {
                if (this.Form != null)
                {
                    this.Form._LeftMouseUp += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this.Form._LeftMouseUp -= value;
                }
            }
        }

        // Token: 0x1400000C RID: 12
        // (add) Token: 0x06000030 RID: 48 RVA: 0x00002595 File Offset: 0x00000795
        // (remove) Token: 0x06000031 RID: 49 RVA: 0x000025A9 File Offset: 0x000007A9
        public event MouseEventHandler OnLeftDoubleClick
        {
            add
            {
                if (this.Form != null)
                {
                    this._LeftDoubleClick += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this._LeftDoubleClick -= value;
                }
            }
        }

        // Token: 0x1400000D RID: 13
        // (add) Token: 0x06000032 RID: 50 RVA: 0x000025BD File Offset: 0x000007BD
        // (remove) Token: 0x06000033 RID: 51 RVA: 0x000025D6 File Offset: 0x000007D6
        public event MouseEventHandler OnRightMouseDown
        {
            add
            {
                if (this.Form != null)
                {
                    this.Form._RightMouseDown += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this.Form._RightMouseDown -= value;
                }
            }
        }

        // Token: 0x1400000E RID: 14
        // (add) Token: 0x06000034 RID: 52 RVA: 0x000025EF File Offset: 0x000007EF
        // (remove) Token: 0x06000035 RID: 53 RVA: 0x00002608 File Offset: 0x00000808
        public event MouseEventHandler OnRightMouseUp
        {
            add
            {
                if (this.Form != null)
                {
                    this.Form._RightMouseUp += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this.Form._RightMouseUp -= value;
                }
            }
        }

        // Token: 0x1400000F RID: 15
        // (add) Token: 0x06000036 RID: 54 RVA: 0x00002621 File Offset: 0x00000821
        // (remove) Token: 0x06000037 RID: 55 RVA: 0x00002635 File Offset: 0x00000835
        public event MouseEventHandler OnRightDoubleClick
        {
            add
            {
                if (this.Form != null)
                {
                    this._RightDoubleClick += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this._RightDoubleClick -= value;
                }
            }
        }

        // Token: 0x14000010 RID: 16
        // (add) Token: 0x06000038 RID: 56 RVA: 0x00002649 File Offset: 0x00000849
        // (remove) Token: 0x06000039 RID: 57 RVA: 0x00002662 File Offset: 0x00000862
        public event MouseEventHandler OnMiddleMouseDown
        {
            add
            {
                if (this.Form != null)
                {
                    this.Form._MiddleMouseDown += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this.Form._MiddleMouseDown -= value;
                }
            }
        }

        // Token: 0x14000011 RID: 17
        // (add) Token: 0x0600003A RID: 58 RVA: 0x0000267B File Offset: 0x0000087B
        // (remove) Token: 0x0600003B RID: 59 RVA: 0x00002694 File Offset: 0x00000894
        public event MouseEventHandler OnMiddleMouseUp
        {
            add
            {
                if (this.Form != null)
                {
                    this.Form._MiddleMouseUp += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this.Form._MiddleMouseUp -= value;
                }
            }
        }

        // Token: 0x14000012 RID: 18
        // (add) Token: 0x0600003C RID: 60 RVA: 0x000026AD File Offset: 0x000008AD
        // (remove) Token: 0x0600003D RID: 61 RVA: 0x000026C1 File Offset: 0x000008C1
        public event MouseEventHandler OnMiddleDoubleClick
        {
            add
            {
                if (this.Form != null)
                {
                    this._MiddleDoubleClick += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this._MiddleDoubleClick -= value;
                }
            }
        }

        // Token: 0x14000013 RID: 19
        // (add) Token: 0x0600003E RID: 62 RVA: 0x000026D5 File Offset: 0x000008D5
        // (remove) Token: 0x0600003F RID: 63 RVA: 0x000026EE File Offset: 0x000008EE
        public event MouseEventHandler OnMouseWheel
        {
            add
            {
                if (this.Form != null)
                {
                    this.Form.MouseWheel += value;
                }
            }
            remove
            {
                if (this.Form != null)
                {
                    this.Form.MouseWheel -= value;
                }
            }
        }

        // Token: 0x14000014 RID: 20
        // (add) Token: 0x06000040 RID: 64 RVA: 0x00002708 File Offset: 0x00000908
        // (remove) Token: 0x06000041 RID: 65 RVA: 0x00002740 File Offset: 0x00000940
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private MouseEventHandler _LeftDoubleClick = null;

        // Token: 0x14000015 RID: 21
        // (add) Token: 0x06000042 RID: 66 RVA: 0x00002778 File Offset: 0x00000978
        // (remove) Token: 0x06000043 RID: 67 RVA: 0x000027B0 File Offset: 0x000009B0
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private MouseEventHandler _RightDoubleClick = null;

        // Token: 0x14000016 RID: 22
        // (add) Token: 0x06000044 RID: 68 RVA: 0x000027E8 File Offset: 0x000009E8
        // (remove) Token: 0x06000045 RID: 69 RVA: 0x00002820 File Offset: 0x00000A20
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private MouseEventHandler _MiddleDoubleClick = null;

        // Token: 0x06000046 RID: 70 RVA: 0x00002858 File Offset: 0x00000A58
        private void Awake()
        {
            this.CheckInstance();
            if (MascotMaker.mainWindowHandle == IntPtr.Zero)
            {
                Process currentProcess = Process.GetCurrentProcess();
                MascotMaker.SetForegroundWindow(currentProcess.Handle);
                MascotMaker.mainWindowHandle = MascotMaker.GetActiveWindow();
            }
            this.cam = base.transform.GetComponent<Camera>();
            this.rend = base.transform.GetComponent<Renderer>();
            this.cam.clearFlags = CameraClearFlags.Color;
            this.Form = new MascotMaker.MascotForm();
            this.Form.Show();
            this.formDummy = new Form();
            this.formDummy.FormBorderStyle = FormBorderStyle.None;
            this.formDummy.Opacity = 0.0;
            this.formDummy.Show();
            base.Invoke("closeDummy", 0.01f);
            if (this.PlayOnAwake)
            {
                this.isRender = true;
            }
            else
            {
                this.isRender = false;
                this.Form.Hide();
            }
            this.mascotMakerMaterial = (Material)Resources.Load("MascotMakerMaterial", typeof(Material));
            this.mascotMakerMaterialChromakey = (Material)Resources.Load("MascotMakerMaterialChromakey", typeof(Material));
            this.mascotMakerTexture = new RenderTexture((int)this.MascotFormSize.x, (int)this.MascotFormSize.y, 24, RenderTextureFormat.ARGB32);
            this.mascotMakerTexture.antiAliasing = (int)this.AntiAliasing;
            this.mascotFormSizePre = this.MascotFormSize;
            this.antiAliasingPre = this.AntiAliasing;
            this.ChromaKeyColor = new UnityEngine.Color(this.ChromaKeyColor.r, this.ChromaKeyColor.g, this.ChromaKeyColor.b, 0f);
            this.cam.backgroundColor = new UnityEngine.Color(this.ChromaKeyColor.r, this.ChromaKeyColor.g, this.ChromaKeyColor.b, 0f);
            this.mascotMakerMaterialChromakey.color = new UnityEngine.Color(this.ChromaKeyColor.r, this.ChromaKeyColor.g, this.ChromaKeyColor.b, 0f);
            this.ChromaKeyRange = Mathf.Clamp(this.ChromaKeyRange, 0.002f, 1f);
            this.chromaKeyRangePre = this.ChromaKeyRange;
            this.mascotMakerMaterialChromakey.SetFloat("_Amount", this.chromaKeyRangePre);
            if (this.ChromaKeyCompositing)
            {
                this.rend.material = this.mascotMakerMaterialChromakey;
            }
            else
            {
                this.rend.material = this.mascotMakerMaterial;
            }
            this.rend.sharedMaterial.mainTexture = this.mascotMakerTexture;
            this.cameraTexture = new Texture2D(this.mascotMakerTexture.width, this.mascotMakerTexture.height, TextureFormat.ARGB32, false);
            this.cam.targetTexture = this.mascotMakerTexture;
            CvSize size = new CvSize(this.mascotMakerTexture.width, this.mascotMakerTexture.height);
            this.img = Cv.CreateImage(size, BitDepth.U8, 4);
            this.scaleVector = new Vector3(1f, -1f, 1f);
            this.isMouseHover = false;
            this.Form._LeftMouseDown += this.LeftMouseDown;
            this.Form._LeftMouseUp += this.LeftMouseUp;
            this.Form._RightMouseUp += this.RightMouseUp;
            this.Form._MiddleMouseUp += this.MiddleMouseUp;
            this.offsetX = 0;
            this.offsetY = 0;
            this.isLeftMouseDown = false;
            this.frameColor = new CvScalar(255.0, 0.0, 0.0, 128.0);
        }

        // Token: 0x06000047 RID: 71 RVA: 0x00002C20 File Offset: 0x00000E20
        private void LeftMouseDown(object sender, MouseEventArgs e)
        {
            this.offsetX = this.Form.Left - System.Windows.Forms.Cursor.Position.X;
            this.offsetY = this.Form.Top - System.Windows.Forms.Cursor.Position.Y;
            this.isLeftMouseDown = true;
        }

        // Token: 0x06000048 RID: 72 RVA: 0x00002C74 File Offset: 0x00000E74
        private void LeftMouseUp(object sender, MouseEventArgs e)
        {
            float time = Time.time;
            if (time - this.LeftUpPreviousTime < 0.4f && this._LeftDoubleClick != null)
            {
                this._LeftDoubleClick(this.Form, new MouseEventArgs(MouseButtons.Left, 2, System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y, 0));
            }
            this.LeftUpPreviousTime = Time.time;
            this.isLeftMouseDown = false;
        }

        // Token: 0x06000049 RID: 73 RVA: 0x00002CF0 File Offset: 0x00000EF0
        private void RightMouseUp(object sender, MouseEventArgs e)
        {
            float time = Time.time;
            if (time - this.RightUpPreviousTime < 0.4f && this._RightDoubleClick != null)
            {
                this._RightDoubleClick(this.Form, new MouseEventArgs(MouseButtons.Right, 2, System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y, 0));
            }
            this.RightUpPreviousTime = Time.time;
        }

        // Token: 0x0600004A RID: 74 RVA: 0x00002D64 File Offset: 0x00000F64
        private void MiddleMouseUp(object sender, MouseEventArgs e)
        {
            float time = Time.time;
            if (time - this.MiddleUpPreviousTime < 0.4f && this._MiddleDoubleClick != null)
            {
                this._MiddleDoubleClick(this.Form, new MouseEventArgs(MouseButtons.Middle, 2, System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y, 0));
            }
            this.MiddleUpPreviousTime = Time.time;
        }

        // Token: 0x0600004B RID: 75 RVA: 0x00002DD6 File Offset: 0x00000FD6
        private void closeDummy()
        {
            if (this.formDummy != null)
            {
                this.formDummy.Close();
                this.formDummy = null;
            }
        }

        // Token: 0x0600004C RID: 76 RVA: 0x00002DF5 File Offset: 0x00000FF5
        private bool CheckInstance()
        {
            if (MascotMaker.instance == null)
            {
                MascotMaker.instance = this;
                return true;
            }
            if (MascotMaker.Instance == this)
            {
                return true;
            }
            UnityEngine.Object.Destroy(this);
            return false;
        }

        // Token: 0x0600004D RID: 77 RVA: 0x00002E28 File Offset: 0x00001028
        private void UpdateCore()
        {
            if (this.isRender)
            {
                if (this.mascotFormSizePre != this.MascotFormSize || this.antiAliasingPre != this.AntiAliasing)
                {
                    if (this.MascotFormSize.x < 1f)
                    {
                        this.MascotFormSize = new Vector2(1f, (float)((int)this.MascotFormSize.y));
                    }
                    if (this.MascotFormSize.y < 1f)
                    {
                        this.MascotFormSize = new Vector2((float)((int)this.MascotFormSize.x), 1f);
                    }
                    int antiAliasing = (int)this.AntiAliasing;
                    if (antiAliasing != 1 && antiAliasing != 2 && antiAliasing != 4 && antiAliasing != 8)
                    {
                        this.AntiAliasing = MascotMaker.AntiAliasingType.None;
                    }
                    this.ChangeRenderTexture(this.MascotFormSize, this.AntiAliasing);
                }
                if (this.ChromaKeyColor.r != this.cam.backgroundColor.r || this.ChromaKeyColor.g != this.cam.backgroundColor.g || this.ChromaKeyColor.b != this.cam.backgroundColor.b)
                {
                    this.ChromaKeyColor = new UnityEngine.Color(this.ChromaKeyColor.r, this.ChromaKeyColor.g, this.ChromaKeyColor.b, 0f);
                    this.cam.backgroundColor = new UnityEngine.Color(this.ChromaKeyColor.r, this.ChromaKeyColor.g, this.ChromaKeyColor.b, 0f);
                    this.mascotMakerMaterialChromakey.color = new UnityEngine.Color(this.ChromaKeyColor.r, this.ChromaKeyColor.g, this.ChromaKeyColor.b, 0f);
                }
                if (this.ChromaKeyRange != this.chromaKeyRangePre)
                {
                    this.ChromaKeyRange = Mathf.Clamp(this.ChromaKeyRange, 0.002f, 1f);
                    this.chromaKeyRangePre = this.ChromaKeyRange;
                    if (this.mascotMakerMaterialChromakey)
                    {
                        this.mascotMakerMaterialChromakey.SetFloat("_Amount", this.chromaKeyRangePre);
                    }
                }
                RenderTexture active = RenderTexture.active;
                RenderTexture.active = this.mascotMakerTexture;
                this.cam.Render();
                this.cameraTexture.ReadPixels(new Rect(0f, 0f, (float)this.mascotMakerTexture.width, (float)this.mascotMakerTexture.height), 0, 0);
                GL.Clear(true, true, this.mascotMakerMaterialChromakey.color);
                this.cameraTexture.Apply();
                RenderTexture.active = active;
                this.cameraTexturePixels = this.cameraTexture.GetPixels32();
                this.cameraTexturePixelsHandle = GCHandle.Alloc(this.cameraTexturePixels, GCHandleType.Pinned);
                this.cameraTexturePixelsPtr = this.cameraTexturePixelsHandle.AddrOfPinnedObject();
                this.img.ImageData = this.cameraTexturePixelsPtr;
                if (this.ShowMascotFormOutline)
                {
                    CvPoint[] array = new CvPoint[]
                    {
                        new CvPoint(0, 0),
                        new CvPoint(this.mascotMakerTexture.width - 1, 0),
                        new CvPoint(this.mascotMakerTexture.width - 1, this.mascotMakerTexture.height - 1),
                        new CvPoint(0, this.mascotMakerTexture.height - 1)
                    };
                    this.img.PolyLine(new CvPoint[][]
                    {
                        array
                    }, true, this.frameColor, 2);
                }
                this.img.CvtColor(this.img, ColorConversion.BgraToRgba);
                this.Form.Repaint(this.img.ToBitmap(), this.opacity);
                this.cameraTexturePixelsHandle.Free();
                if (this.TopMost != this.Form.TopMost)
                {
                    this.Form.TopMost = this.TopMost;
                }
            }
            if (this.Form.HitTestCount != this.Form.HitTestCountTmp)
            {
                this.isMouseHover = true;
                this.Form.HitTestCountTmp = this.Form.HitTestCount;
            }
            else
            {
                this.isMouseHover = false;
            }
            if (this.DragMove && this.isLeftMouseDown)
            {
                this.Form.Left = System.Windows.Forms.Cursor.Position.X + this.offsetX;
                this.Form.Top = System.Windows.Forms.Cursor.Position.Y + this.offsetY;
            }
        }

        // Token: 0x0600004E RID: 78 RVA: 0x000032D0 File Offset: 0x000014D0
        private void Update()
        {
            if (this.UpdateFunc == MascotMaker.UpdateFuncType.Update)
            {
                this.UpdateCore();
            }
        }

        // Token: 0x0600004F RID: 79 RVA: 0x000032E4 File Offset: 0x000014E4
        private void LateUpdate()
        {
            if (this.UpdateFunc == MascotMaker.UpdateFuncType.LateUpdate)
            {
                this.UpdateCore();
            }
        }

        // Token: 0x06000050 RID: 80 RVA: 0x000032F8 File Offset: 0x000014F8
        private void ChangeRenderTexture(Vector2 renderTextureSize, MascotMaker.AntiAliasingType antiAliasingType)
        {
            if (this.mascotMakerTexture != null)
            {
                this.cam.targetTexture = null;
                this.mascotMakerTexture.Release();
            }
            this.mascotMakerTexture = new RenderTexture((int)renderTextureSize.x, (int)renderTextureSize.y, 24, RenderTextureFormat.ARGB32);
            this.mascotMakerTexture.antiAliasing = (int)antiAliasingType;
            this.mascotFormSizePre = renderTextureSize;
            this.antiAliasingPre = antiAliasingType;
            this.rend.sharedMaterial.mainTexture = this.mascotMakerTexture;
            this.cameraTexture.Resize((int)renderTextureSize.x, (int)renderTextureSize.y, TextureFormat.ARGB32, false);
            this.cam.targetTexture = this.mascotMakerTexture;
            CvSize size = new CvSize((int)renderTextureSize.x, (int)renderTextureSize.y);
            this.img = Cv.CreateImage(size, BitDepth.U8, 4);
            this.Form.Size = new Size((int)renderTextureSize.x, (int)renderTextureSize.y);
        }

        // Token: 0x06000051 RID: 81 RVA: 0x000033F0 File Offset: 0x000015F0
        private void OnDestroy()
        {
            if (this.formDummy != null)
            {
                this.formDummy.Close();
                this.formDummy = null;
            }
            if (this.Form != null)
            {
                this.Form.Close();
                this.Form = null;
            }
            if (this.rend.sharedMaterial.mainTexture != null)
            {
                this.rend.sharedMaterial.mainTexture = null;
            }
        }

        // Token: 0x06000052 RID: 82 RVA: 0x00003464 File Offset: 0x00001664
        private void OnApplicationQuit()
        {
            this.isRender = false;
            if (this.formDummy != null)
            {
                this.formDummy.Close();
                this.formDummy = null;
            }
            if (this.Form != null)
            {
                this.Form.Close();
                this.Form = null;
            }
            MascotMaker.UnregisterClass("Mono.WinForms.1.0", IntPtr.Zero);
        }

        // Token: 0x06000053 RID: 83 RVA: 0x000034C2 File Offset: 0x000016C2
        private void OnPreCull()
        {
            this.cam.ResetWorldToCameraMatrix();
            this.cam.ResetProjectionMatrix();
            this.cam.projectionMatrix = this.cam.projectionMatrix * Matrix4x4.Scale(this.scaleVector);
        }

        // Token: 0x06000054 RID: 84 RVA: 0x00003500 File Offset: 0x00001700
        private void OnPreRender()
        {
            GL.invertCulling = true;
        }

        // Token: 0x06000055 RID: 85 RVA: 0x00003508 File Offset: 0x00001708
        private void OnPostRender()
        {
            GL.invertCulling = false;
        }

        // Token: 0x06000056 RID: 86 RVA: 0x00003510 File Offset: 0x00001710
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (this.ChromaKeyCompositing)
            {
                if (this.mascotMakerMaterialChromakey != null)
                {
                    UnityEngine.Graphics.Blit(source, destination, this.mascotMakerMaterialChromakey);
                }
            }
            else if (this.mascotMakerMaterial != null)
            {
                UnityEngine.Graphics.Blit(source, destination, this.mascotMakerMaterial);
            }
        }

        // Token: 0x06000057 RID: 87 RVA: 0x00003569 File Offset: 0x00001769
        public void Show()
        {
            if (!this.isRender)
            {
                this.isRender = true;
                this.Form.Show();
            }
        }

        // Token: 0x06000058 RID: 88 RVA: 0x00003588 File Offset: 0x00001788
        public void Hide()
        {
            if (this.isRender)
            {
                this.isRender = false;
                this.Form.Hide();
            }
        }

        // Token: 0x04000001 RID: 1
        private static MascotMaker instance;

        // Token: 0x04000002 RID: 2
        private byte opacity = byte.MaxValue;

        // Token: 0x04000003 RID: 3
        private static IntPtr mainWindowHandle = IntPtr.Zero;

        // Token: 0x04000004 RID: 4
        private bool isMouseHover;

        // Token: 0x04000005 RID: 5
        public bool PlayOnAwake = true;

        // Token: 0x04000006 RID: 6
        public bool TopMost = true;

        // Token: 0x04000007 RID: 7
        public Vector2 MascotFormSize = new Vector2(480f, 640f);

        // Token: 0x04000008 RID: 8
        private Vector2 mascotFormSizePre = new Vector2(480f, 640f);

        // Token: 0x04000009 RID: 9
        public MascotMaker.AntiAliasingType AntiAliasing = MascotMaker.AntiAliasingType.FourSamples;

        // Token: 0x0400000A RID: 10
        private MascotMaker.AntiAliasingType antiAliasingPre = MascotMaker.AntiAliasingType.FourSamples;

        // Token: 0x0400000B RID: 11
        public MascotMaker.UpdateFuncType UpdateFunc = MascotMaker.UpdateFuncType.Update;

        // Token: 0x0400000C RID: 12
        public bool ShowMascotFormOutline = false;

        // Token: 0x0400000D RID: 13
        private CvScalar frameColor;

        // Token: 0x0400000E RID: 14
        public bool ChromaKeyCompositing = false;

        // Token: 0x0400000F RID: 15
        public UnityEngine.Color ChromaKeyColor;

        // Token: 0x04000010 RID: 16
        public float ChromaKeyRange = 0.002f;

        // Token: 0x04000011 RID: 17
        private float chromaKeyRangePre;

        // Token: 0x04000012 RID: 18
        private RenderTexture mascotMakerTexture;

        // Token: 0x04000013 RID: 19
        private Material mascotMakerMaterial;

        // Token: 0x04000014 RID: 20
        private Material mascotMakerMaterialChromakey;

        // Token: 0x04000015 RID: 21
        private Texture2D cameraTexture;

        // Token: 0x04000016 RID: 22
        private Color32[] cameraTexturePixels;

        // Token: 0x04000017 RID: 23
        private GCHandle cameraTexturePixelsHandle;

        // Token: 0x04000018 RID: 24
        private IntPtr cameraTexturePixelsPtr;

        // Token: 0x04000019 RID: 25
        private MascotMaker.MascotForm Form;

        // Token: 0x0400001A RID: 26
        [NonSerialized]
        private IplImage img = null;

        // Token: 0x0400001B RID: 27
        private Vector3 scaleVector;

        // Token: 0x0400001C RID: 28
        private bool isRender = false;

        // Token: 0x04000020 RID: 32
        private float LeftUpPreviousTime = float.MinValue;

        // Token: 0x04000021 RID: 33
        private float RightUpPreviousTime = float.MinValue;

        // Token: 0x04000022 RID: 34
        private float MiddleUpPreviousTime = float.MinValue;

        // Token: 0x04000023 RID: 35
        public bool DragMove = true;

        // Token: 0x04000024 RID: 36
        private int offsetX;

        // Token: 0x04000025 RID: 37
        private int offsetY;

        // Token: 0x04000026 RID: 38
        private bool isLeftMouseDown;

        // Token: 0x04000027 RID: 39
        private Form formDummy;

        // Token: 0x04000028 RID: 40
        private Camera cam;

        // Token: 0x04000029 RID: 41
        private Renderer rend;

        // Token: 0x02000003 RID: 3
        public enum AntiAliasingType
        {
            // Token: 0x0400002B RID: 43
            None = 1,
            // Token: 0x0400002C RID: 44
            TwoSamples,
            // Token: 0x0400002D RID: 45
            FourSamples = 4,
            // Token: 0x0400002E RID: 46
            EightSamples = 8
        }

        // Token: 0x02000004 RID: 4
        public enum UpdateFuncType
        {
            // Token: 0x04000030 RID: 48
            Update = 1,
            // Token: 0x04000031 RID: 49
            LateUpdate
        }

        // Token: 0x02000005 RID: 5
        private class MascotForm : Form
        {
            // Token: 0x0600005A RID: 90 RVA: 0x000035B4 File Offset: 0x000017B4
            public MascotForm()
            {
                this.Text = "";
                this.AllowDrop = false;
                base.FormBorderStyle = FormBorderStyle.None;
                base.TopMost = false;
                base.ShowInTaskbar = false;
                base.MaximizeBox = false;
                base.MinimizeBox = false;
                this.DoubleBuffered = true;
                this.blend = default(MascotMaker.MascotForm.BLENDFUNC);
                this.blend.BlendOp = 0;
                this.blend.BlendFlags = 0;
                this.blend.AlphaFormat = 1;
                base.Capture = true;
                base.FormClosed += new FormClosedEventHandler(this.OnFormClosed);
                base.MouseDown += this.Form_MouseDown;
                base.MouseUp += this.Form_MouseUp;
                this.HitTestCount = 0;
            }

            // Token: 0x0600005B RID: 91
            [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
            private static extern int UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref MascotMaker.MascotForm.Vector2 pptDst, ref MascotMaker.MascotForm.Vector2 psize, IntPtr hdcSrc, ref MascotMaker.MascotForm.Vector2 pprSrc, int crKey, ref MascotMaker.MascotForm.BLENDFUNC pblend, int dwFlags);

            // Token: 0x0600005C RID: 92
            [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
            private static extern IntPtr GetDC(IntPtr hWnd);

            // Token: 0x0600005D RID: 93
            [DllImport("user32.dll", ExactSpelling = true)]
            private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

            // Token: 0x0600005E RID: 94
            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            private static extern IntPtr CreateCompatibleDC(IntPtr hDC);

            // Token: 0x0600005F RID: 95
            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            private static extern int DeleteDC(IntPtr hdc);

            // Token: 0x06000060 RID: 96
            [DllImport("gdi32.dll", ExactSpelling = true)]
            private static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

            // Token: 0x06000061 RID: 97
            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            private static extern int DeleteObject(IntPtr hObject);

            // Token: 0x06000062 RID: 98 RVA: 0x0000368D File Offset: 0x0000188D
            private void OnFormClosed(object sender, EventArgs e)
            {
                base.MouseDown -= this.Form_MouseDown;
                base.MouseUp -= this.Form_MouseUp;
            }

            // Token: 0x06000063 RID: 99 RVA: 0x000036B4 File Offset: 0x000018B4
            private void Form_MouseDown(object sender, MouseEventArgs e)
            {
                MouseEventHandler mouseEventHandler = null;
                MouseButtons button = e.Button;
                if (button != MouseButtons.Left)
                {
                    if (button != MouseButtons.Right)
                    {
                        if (button == MouseButtons.Middle)
                        {
                            mouseEventHandler = this._MiddleMouseDown;
                        }
                    }
                    else
                    {
                        mouseEventHandler = this._RightMouseDown;
                    }
                }
                else
                {
                    mouseEventHandler = this._LeftMouseDown;
                }
                if (mouseEventHandler != null)
                {
                    mouseEventHandler(this, e);
                }
            }

            // Token: 0x06000064 RID: 100 RVA: 0x00003728 File Offset: 0x00001928
            private void Form_MouseUp(object sender, MouseEventArgs e)
            {
                MouseEventHandler mouseEventHandler = null;
                MouseButtons button = e.Button;
                if (button != MouseButtons.Left)
                {
                    if (button != MouseButtons.Right)
                    {
                        if (button == MouseButtons.Middle)
                        {
                            mouseEventHandler = this._MiddleMouseUp;
                        }
                    }
                    else
                    {
                        mouseEventHandler = this._RightMouseUp;
                    }
                }
                else
                {
                    mouseEventHandler = this._LeftMouseUp;
                }
                if (mouseEventHandler != null)
                {
                    mouseEventHandler(this, e);
                }
            }

            // Token: 0x14000017 RID: 23
            // (add) Token: 0x06000065 RID: 101 RVA: 0x0000379C File Offset: 0x0000199C
            // (remove) Token: 0x06000066 RID: 102 RVA: 0x000037D4 File Offset: 0x000019D4
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public MouseEventHandler _LeftMouseDown;

            // Token: 0x14000018 RID: 24
            // (add) Token: 0x06000067 RID: 103 RVA: 0x0000380C File Offset: 0x00001A0C
            // (remove) Token: 0x06000068 RID: 104 RVA: 0x00003844 File Offset: 0x00001A44
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public MouseEventHandler _LeftMouseUp;

            // Token: 0x14000019 RID: 25
            // (add) Token: 0x06000069 RID: 105 RVA: 0x0000387C File Offset: 0x00001A7C
            // (remove) Token: 0x0600006A RID: 106 RVA: 0x000038B4 File Offset: 0x00001AB4
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public MouseEventHandler _RightMouseDown;

            // Token: 0x1400001A RID: 26
            // (add) Token: 0x0600006B RID: 107 RVA: 0x000038EC File Offset: 0x00001AEC
            // (remove) Token: 0x0600006C RID: 108 RVA: 0x00003924 File Offset: 0x00001B24
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public MouseEventHandler _RightMouseUp;

            // Token: 0x1400001B RID: 27
            // (add) Token: 0x0600006D RID: 109 RVA: 0x0000395C File Offset: 0x00001B5C
            // (remove) Token: 0x0600006E RID: 110 RVA: 0x00003994 File Offset: 0x00001B94
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public MouseEventHandler _MiddleMouseDown;

            // Token: 0x1400001C RID: 28
            // (add) Token: 0x0600006F RID: 111 RVA: 0x000039CC File Offset: 0x00001BCC
            // (remove) Token: 0x06000070 RID: 112 RVA: 0x00003A04 File Offset: 0x00001C04
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public MouseEventHandler _MiddleMouseUp;

            // Token: 0x06000071 RID: 113 RVA: 0x00003A3C File Offset: 0x00001C3C
            public void Repaint(Bitmap bitmap, byte opacity)
            {
                if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                {
                    throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");
                }
                IntPtr dc = MascotMaker.MascotForm.GetDC(IntPtr.Zero);
                IntPtr intPtr = MascotMaker.MascotForm.CreateCompatibleDC(dc);
                IntPtr intPtr2 = IntPtr.Zero;
                IntPtr hObject = IntPtr.Zero;
                intPtr2 = bitmap.GetHbitmap(System.Drawing.Color.FromArgb(0));
                hObject = MascotMaker.MascotForm.SelectObject(intPtr, intPtr2);
                MascotMaker.MascotForm.Vector2 vector = new MascotMaker.MascotForm.Vector2(bitmap.Width, bitmap.Height);
                MascotMaker.MascotForm.Vector2 vector2 = new MascotMaker.MascotForm.Vector2(0, 0);
                MascotMaker.MascotForm.Vector2 vector3 = new MascotMaker.MascotForm.Vector2(base.Left, base.Top);
                this.blend.SourceConstantAlpha = opacity;
                MascotMaker.MascotForm.UpdateLayeredWindow(base.Handle, dc, ref vector3, ref vector, intPtr, ref vector2, 0, ref this.blend, 2);
                if (intPtr2 != IntPtr.Zero)
                {
                    MascotMaker.MascotForm.SelectObject(intPtr, hObject);
                    MascotMaker.MascotForm.DeleteObject(intPtr2);
                }
                MascotMaker.MascotForm.DeleteDC(intPtr);
                MascotMaker.MascotForm.ReleaseDC(IntPtr.Zero, dc);
            }

            // Token: 0x06000072 RID: 114 RVA: 0x00003B20 File Offset: 0x00001D20
            protected override void WndProc(ref Message m)
            {
                if (m.Msg == 132)
                {
                    this.HitTestCount++;
                    if (this.HitTestCount >= 100000)
                    {
                        this.HitTestCount = -1;
                    }
                }
                m.Result = (IntPtr)1;
                base.WndProc(ref m);
            }

            // Token: 0x1700000E RID: 14
            // (get) Token: 0x06000073 RID: 115 RVA: 0x00003B78 File Offset: 0x00001D78
            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams createParams = base.CreateParams;
                    createParams.ExStyle |= 524288;
                    return createParams;
                }
            }

            // Token: 0x04000032 RID: 50
            private const int ULW_COLORKEY = 1;

            // Token: 0x04000033 RID: 51
            private const int ULW_ALPHA = 2;

            // Token: 0x04000034 RID: 52
            private const int ULW_OPAQUE = 4;

            // Token: 0x04000035 RID: 53
            private const byte AC_SRC_OVER = 0;

            // Token: 0x04000036 RID: 54
            private const byte AC_SRC_ALPHA = 1;

            // Token: 0x04000037 RID: 55
            private MascotMaker.MascotForm.BLENDFUNC blend;

            // Token: 0x04000038 RID: 56
            public int HitTestCount;

            // Token: 0x04000039 RID: 57
            public int HitTestCountTmp;

            // Token: 0x02000006 RID: 6
            private struct Vector2
            {
                // Token: 0x06000074 RID: 116 RVA: 0x00003B9F File Offset: 0x00001D9F
                public Vector2(int x, int y)
                {
                    this.x = x;
                    this.y = y;
                }

                // Token: 0x04000040 RID: 64
                public int x;

                // Token: 0x04000041 RID: 65
                public int y;
            }

            // Token: 0x02000007 RID: 7
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            private struct BGRA
            {
                // Token: 0x04000042 RID: 66
                public byte B;

                // Token: 0x04000043 RID: 67
                public byte G;

                // Token: 0x04000044 RID: 68
                public byte R;

                // Token: 0x04000045 RID: 69
                public byte A;
            }

            // Token: 0x02000008 RID: 8
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            private struct BLENDFUNC
            {
                // Token: 0x04000046 RID: 70
                public byte BlendOp;

                // Token: 0x04000047 RID: 71
                public byte BlendFlags;

                // Token: 0x04000048 RID: 72
                public byte SourceConstantAlpha;

                // Token: 0x04000049 RID: 73
                public byte AlphaFormat;
            }
        }
    }
}
