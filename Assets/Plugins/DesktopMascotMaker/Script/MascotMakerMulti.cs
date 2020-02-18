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
    // Token: 0x02000009 RID: 9
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("DesktopMascotMaker/MascotMakerMulti")]
    public class MascotMakerMulti : MonoBehaviour
    {
        // Token: 0x06000076 RID: 118
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern short UnregisterClass(string lpClassName, IntPtr hInstance);

        // Token: 0x06000077 RID: 119
        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        // Token: 0x06000078 RID: 120
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x06000079 RID: 121 RVA: 0x00003C7F File Offset: 0x00001E7F
        // (set) Token: 0x0600007A RID: 122 RVA: 0x00003C87 File Offset: 0x00001E87
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

        // Token: 0x17000010 RID: 16
        // (get) Token: 0x0600007B RID: 123 RVA: 0x00003C9C File Offset: 0x00001E9C
        public static IntPtr MainWindowHandle
        {
            get
            {
                return MascotMakerMulti.mainWindowHandle;
            }
        }

        // Token: 0x17000011 RID: 17
        // (get) Token: 0x0600007C RID: 124 RVA: 0x00003CA3 File Offset: 0x00001EA3
        public bool IsMouseHover
        {
            get
            {
                return this.isMouseHover;
            }
        }

        // Token: 0x17000012 RID: 18
        // (get) Token: 0x0600007D RID: 125 RVA: 0x00003CAB File Offset: 0x00001EAB
        // (set) Token: 0x0600007E RID: 126 RVA: 0x00003CC5 File Offset: 0x00001EC5
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

        // Token: 0x17000013 RID: 19
        // (get) Token: 0x0600007F RID: 127 RVA: 0x00003CDE File Offset: 0x00001EDE
        // (set) Token: 0x06000080 RID: 128 RVA: 0x00003CF8 File Offset: 0x00001EF8
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

        // Token: 0x17000014 RID: 20
        // (get) Token: 0x06000081 RID: 129 RVA: 0x00003D11 File Offset: 0x00001F11
        // (set) Token: 0x06000082 RID: 130 RVA: 0x00003D31 File Offset: 0x00001F31
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

        // Token: 0x17000015 RID: 21
        // (get) Token: 0x06000083 RID: 131 RVA: 0x00003D4A File Offset: 0x00001F4A
        // (set) Token: 0x06000084 RID: 132 RVA: 0x00003D64 File Offset: 0x00001F64
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

        // Token: 0x17000016 RID: 22
        // (get) Token: 0x06000085 RID: 133 RVA: 0x00003D7E File Offset: 0x00001F7E
        // (set) Token: 0x06000086 RID: 134 RVA: 0x00003D98 File Offset: 0x00001F98
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

        // Token: 0x17000017 RID: 23
        // (get) Token: 0x06000087 RID: 135 RVA: 0x00003DB4 File Offset: 0x00001FB4
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

        // Token: 0x17000018 RID: 24
        // (get) Token: 0x06000088 RID: 136 RVA: 0x00003DE4 File Offset: 0x00001FE4
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

        // Token: 0x17000019 RID: 25
        // (get) Token: 0x06000089 RID: 137 RVA: 0x00003E11 File Offset: 0x00002011
        // (set) Token: 0x0600008A RID: 138 RVA: 0x00003E2F File Offset: 0x0000202F
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

        // Token: 0x1700001A RID: 26
        // (get) Token: 0x0600008B RID: 139 RVA: 0x00003E48 File Offset: 0x00002048
        // (set) Token: 0x0600008C RID: 140 RVA: 0x00003E62 File Offset: 0x00002062
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

        // Token: 0x1400001D RID: 29
        // (add) Token: 0x0600008D RID: 141 RVA: 0x00003E7B File Offset: 0x0000207B
        // (remove) Token: 0x0600008E RID: 142 RVA: 0x00003E94 File Offset: 0x00002094
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

        // Token: 0x1400001E RID: 30
        // (add) Token: 0x0600008F RID: 143 RVA: 0x00003EAD File Offset: 0x000020AD
        // (remove) Token: 0x06000090 RID: 144 RVA: 0x00003EC6 File Offset: 0x000020C6
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

        // Token: 0x1400001F RID: 31
        // (add) Token: 0x06000091 RID: 145 RVA: 0x00003EDF File Offset: 0x000020DF
        // (remove) Token: 0x06000092 RID: 146 RVA: 0x00003EF8 File Offset: 0x000020F8
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

        // Token: 0x14000020 RID: 32
        // (add) Token: 0x06000093 RID: 147 RVA: 0x00003F11 File Offset: 0x00002111
        // (remove) Token: 0x06000094 RID: 148 RVA: 0x00003F2A File Offset: 0x0000212A
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

        // Token: 0x14000021 RID: 33
        // (add) Token: 0x06000095 RID: 149 RVA: 0x00003F43 File Offset: 0x00002143
        // (remove) Token: 0x06000096 RID: 150 RVA: 0x00003F5C File Offset: 0x0000215C
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

        // Token: 0x14000022 RID: 34
        // (add) Token: 0x06000097 RID: 151 RVA: 0x00003F75 File Offset: 0x00002175
        // (remove) Token: 0x06000098 RID: 152 RVA: 0x00003F8E File Offset: 0x0000218E
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

        // Token: 0x14000023 RID: 35
        // (add) Token: 0x06000099 RID: 153 RVA: 0x00003FA7 File Offset: 0x000021A7
        // (remove) Token: 0x0600009A RID: 154 RVA: 0x00003FC0 File Offset: 0x000021C0
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

        // Token: 0x14000024 RID: 36
        // (add) Token: 0x0600009B RID: 155 RVA: 0x00003FD9 File Offset: 0x000021D9
        // (remove) Token: 0x0600009C RID: 156 RVA: 0x00003FF2 File Offset: 0x000021F2
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

        // Token: 0x14000025 RID: 37
        // (add) Token: 0x0600009D RID: 157 RVA: 0x0000400B File Offset: 0x0000220B
        // (remove) Token: 0x0600009E RID: 158 RVA: 0x00004024 File Offset: 0x00002224
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

        // Token: 0x14000026 RID: 38
        // (add) Token: 0x0600009F RID: 159 RVA: 0x0000403D File Offset: 0x0000223D
        // (remove) Token: 0x060000A0 RID: 160 RVA: 0x00004056 File Offset: 0x00002256
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

        // Token: 0x14000027 RID: 39
        // (add) Token: 0x060000A1 RID: 161 RVA: 0x0000406F File Offset: 0x0000226F
        // (remove) Token: 0x060000A2 RID: 162 RVA: 0x00004088 File Offset: 0x00002288
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

        // Token: 0x14000028 RID: 40
        // (add) Token: 0x060000A3 RID: 163 RVA: 0x000040A1 File Offset: 0x000022A1
        // (remove) Token: 0x060000A4 RID: 164 RVA: 0x000040B5 File Offset: 0x000022B5
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

        // Token: 0x14000029 RID: 41
        // (add) Token: 0x060000A5 RID: 165 RVA: 0x000040C9 File Offset: 0x000022C9
        // (remove) Token: 0x060000A6 RID: 166 RVA: 0x000040E2 File Offset: 0x000022E2
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

        // Token: 0x1400002A RID: 42
        // (add) Token: 0x060000A7 RID: 167 RVA: 0x000040FB File Offset: 0x000022FB
        // (remove) Token: 0x060000A8 RID: 168 RVA: 0x00004114 File Offset: 0x00002314
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

        // Token: 0x1400002B RID: 43
        // (add) Token: 0x060000A9 RID: 169 RVA: 0x0000412D File Offset: 0x0000232D
        // (remove) Token: 0x060000AA RID: 170 RVA: 0x00004141 File Offset: 0x00002341
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

        // Token: 0x1400002C RID: 44
        // (add) Token: 0x060000AB RID: 171 RVA: 0x00004155 File Offset: 0x00002355
        // (remove) Token: 0x060000AC RID: 172 RVA: 0x0000416E File Offset: 0x0000236E
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

        // Token: 0x1400002D RID: 45
        // (add) Token: 0x060000AD RID: 173 RVA: 0x00004187 File Offset: 0x00002387
        // (remove) Token: 0x060000AE RID: 174 RVA: 0x000041A0 File Offset: 0x000023A0
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

        // Token: 0x1400002E RID: 46
        // (add) Token: 0x060000AF RID: 175 RVA: 0x000041B9 File Offset: 0x000023B9
        // (remove) Token: 0x060000B0 RID: 176 RVA: 0x000041CD File Offset: 0x000023CD
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

        // Token: 0x1400002F RID: 47
        // (add) Token: 0x060000B1 RID: 177 RVA: 0x000041E1 File Offset: 0x000023E1
        // (remove) Token: 0x060000B2 RID: 178 RVA: 0x000041FA File Offset: 0x000023FA
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

        // Token: 0x14000030 RID: 48
        // (add) Token: 0x060000B3 RID: 179 RVA: 0x00004214 File Offset: 0x00002414
        // (remove) Token: 0x060000B4 RID: 180 RVA: 0x0000424C File Offset: 0x0000244C
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private MouseEventHandler _LeftDoubleClick = null;

        // Token: 0x14000031 RID: 49
        // (add) Token: 0x060000B5 RID: 181 RVA: 0x00004284 File Offset: 0x00002484
        // (remove) Token: 0x060000B6 RID: 182 RVA: 0x000042BC File Offset: 0x000024BC
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private MouseEventHandler _RightDoubleClick = null;

        // Token: 0x14000032 RID: 50
        // (add) Token: 0x060000B7 RID: 183 RVA: 0x000042F4 File Offset: 0x000024F4
        // (remove) Token: 0x060000B8 RID: 184 RVA: 0x0000432C File Offset: 0x0000252C
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private MouseEventHandler _MiddleDoubleClick = null;

        // Token: 0x060000B9 RID: 185 RVA: 0x00004364 File Offset: 0x00002564
        private void Awake()
        {
            if (MascotMakerMulti.mainWindowHandle == IntPtr.Zero)
            {
                Process currentProcess = Process.GetCurrentProcess();
                MascotMakerMulti.SetForegroundWindow(currentProcess.Handle);
                MascotMakerMulti.mainWindowHandle = MascotMakerMulti.GetActiveWindow();
            }
            this.cam = base.transform.GetComponent<Camera>();
            this.rend = base.transform.GetComponent<Renderer>();
            this.cam.clearFlags = CameraClearFlags.Color;
            this.Form = new MascotMakerMulti.MascotForm();
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

        // Token: 0x060000BA RID: 186 RVA: 0x00004724 File Offset: 0x00002924
        private void LeftMouseDown(object sender, MouseEventArgs e)
        {
            this.offsetX = this.Form.Left - System.Windows.Forms.Cursor.Position.X;
            this.offsetY = this.Form.Top - System.Windows.Forms.Cursor.Position.Y;
            this.isLeftMouseDown = true;
        }

        // Token: 0x060000BB RID: 187 RVA: 0x00004778 File Offset: 0x00002978
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

        // Token: 0x060000BC RID: 188 RVA: 0x000047F4 File Offset: 0x000029F4
        private void RightMouseUp(object sender, MouseEventArgs e)
        {
            float time = Time.time;
            if (time - this.RightUpPreviousTime < 0.4f && this._RightDoubleClick != null)
            {
                this._RightDoubleClick(this.Form, new MouseEventArgs(MouseButtons.Right, 2, System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y, 0));
            }
            this.RightUpPreviousTime = Time.time;
        }

        // Token: 0x060000BD RID: 189 RVA: 0x00004868 File Offset: 0x00002A68
        private void MiddleMouseUp(object sender, MouseEventArgs e)
        {
            float time = Time.time;
            if (time - this.MiddleUpPreviousTime < 0.4f && this._MiddleDoubleClick != null)
            {
                this._MiddleDoubleClick(this.Form, new MouseEventArgs(MouseButtons.Middle, 2, System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y, 0));
            }
            this.MiddleUpPreviousTime = Time.time;
        }

        // Token: 0x060000BE RID: 190 RVA: 0x000048DA File Offset: 0x00002ADA
        private void closeDummy()
        {
            if (this.formDummy != null)
            {
                this.formDummy.Close();
                this.formDummy = null;
            }
        }

        // Token: 0x060000BF RID: 191 RVA: 0x000048FC File Offset: 0x00002AFC
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
                        this.AntiAliasing = MascotMakerMulti.AntiAliasingType.None;
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
                if (this.mascotMakerTexture != null)
                {
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
                }
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

        // Token: 0x060000C0 RID: 192 RVA: 0x00004DB5 File Offset: 0x00002FB5
        private void Update()
        {
            if (this.UpdateFunc == MascotMakerMulti.UpdateFuncType.Update)
            {
                this.UpdateCore();
            }
        }

        // Token: 0x060000C1 RID: 193 RVA: 0x00004DC9 File Offset: 0x00002FC9
        private void LateUpdate()
        {
            if (this.UpdateFunc == MascotMakerMulti.UpdateFuncType.LateUpdate)
            {
                this.UpdateCore();
            }
        }

        // Token: 0x060000C2 RID: 194 RVA: 0x00004DE0 File Offset: 0x00002FE0
        private void ChangeRenderTexture(Vector2 renderTextureSize, MascotMakerMulti.AntiAliasingType antiAliasingType)
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

        // Token: 0x060000C3 RID: 195 RVA: 0x00004ED8 File Offset: 0x000030D8
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

        // Token: 0x060000C4 RID: 196 RVA: 0x00004F4C File Offset: 0x0000314C
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
            MascotMakerMulti.UnregisterClass("Mono.WinForms.1.0", IntPtr.Zero);
        }

        // Token: 0x060000C5 RID: 197 RVA: 0x00004FAA File Offset: 0x000031AA
        private void OnPreCull()
        {
            this.cam.ResetWorldToCameraMatrix();
            this.cam.ResetProjectionMatrix();
            this.cam.projectionMatrix = this.cam.projectionMatrix * Matrix4x4.Scale(this.scaleVector);
        }

        // Token: 0x060000C6 RID: 198 RVA: 0x00004FE8 File Offset: 0x000031E8
        private void OnPreRender()
        {
            GL.invertCulling = true;
        }

        // Token: 0x060000C7 RID: 199 RVA: 0x00004FF0 File Offset: 0x000031F0
        private void OnPostRender()
        {
            GL.invertCulling = false;
        }

        // Token: 0x060000C8 RID: 200 RVA: 0x00004FF8 File Offset: 0x000031F8
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

        // Token: 0x060000C9 RID: 201 RVA: 0x00005051 File Offset: 0x00003251
        public void Show()
        {
            if (!this.isRender)
            {
                this.isRender = true;
                this.Form.Show();
            }
        }

        // Token: 0x060000CA RID: 202 RVA: 0x00005070 File Offset: 0x00003270
        public void Hide()
        {
            if (this.isRender)
            {
                this.isRender = false;
                this.Form.Hide();
            }
        }

        // Token: 0x0400004A RID: 74
        private byte opacity = byte.MaxValue;

        // Token: 0x0400004B RID: 75
        private static IntPtr mainWindowHandle = IntPtr.Zero;

        // Token: 0x0400004C RID: 76
        private bool isMouseHover;

        // Token: 0x0400004D RID: 77
        public bool PlayOnAwake = true;

        // Token: 0x0400004E RID: 78
        public bool TopMost = true;

        // Token: 0x0400004F RID: 79
        public Vector2 MascotFormSize = new Vector2(480f, 640f);

        // Token: 0x04000050 RID: 80
        private Vector2 mascotFormSizePre = new Vector2(480f, 640f);

        // Token: 0x04000051 RID: 81
        public MascotMakerMulti.AntiAliasingType AntiAliasing = MascotMakerMulti.AntiAliasingType.FourSamples;

        // Token: 0x04000052 RID: 82
        private MascotMakerMulti.AntiAliasingType antiAliasingPre = MascotMakerMulti.AntiAliasingType.FourSamples;

        // Token: 0x04000053 RID: 83
        public MascotMakerMulti.UpdateFuncType UpdateFunc = MascotMakerMulti.UpdateFuncType.Update;

        // Token: 0x04000054 RID: 84
        public bool ShowMascotFormOutline = false;

        // Token: 0x04000055 RID: 85
        private CvScalar frameColor;

        // Token: 0x04000056 RID: 86
        public bool ChromaKeyCompositing = false;

        // Token: 0x04000057 RID: 87
        public UnityEngine.Color ChromaKeyColor;

        // Token: 0x04000058 RID: 88
        public float ChromaKeyRange = 0.002f;

        // Token: 0x04000059 RID: 89
        private float chromaKeyRangePre;

        // Token: 0x0400005A RID: 90
        private RenderTexture mascotMakerTexture;

        // Token: 0x0400005B RID: 91
        private Material mascotMakerMaterial;

        // Token: 0x0400005C RID: 92
        private Material mascotMakerMaterialChromakey;

        // Token: 0x0400005D RID: 93
        private Texture2D cameraTexture;

        // Token: 0x0400005E RID: 94
        private Color32[] cameraTexturePixels;

        // Token: 0x0400005F RID: 95
        private GCHandle cameraTexturePixelsHandle;

        // Token: 0x04000060 RID: 96
        private IntPtr cameraTexturePixelsPtr;

        // Token: 0x04000061 RID: 97
        private MascotMakerMulti.MascotForm Form;

        // Token: 0x04000062 RID: 98
        [NonSerialized]
        private IplImage img = null;

        // Token: 0x04000063 RID: 99
        private Vector3 scaleVector;

        // Token: 0x04000064 RID: 100
        private bool isRender = false;

        // Token: 0x04000068 RID: 104
        private float LeftUpPreviousTime = float.MinValue;

        // Token: 0x04000069 RID: 105
        private float RightUpPreviousTime = float.MinValue;

        // Token: 0x0400006A RID: 106
        private float MiddleUpPreviousTime = float.MinValue;

        // Token: 0x0400006B RID: 107
        public bool DragMove = true;

        // Token: 0x0400006C RID: 108
        private int offsetX;

        // Token: 0x0400006D RID: 109
        private int offsetY;

        // Token: 0x0400006E RID: 110
        private bool isLeftMouseDown;

        // Token: 0x0400006F RID: 111
        private Camera cam;

        // Token: 0x04000070 RID: 112
        private Renderer rend;

        // Token: 0x04000071 RID: 113
        private Form formDummy;

        // Token: 0x0200000A RID: 10
        public enum AntiAliasingType
        {
            // Token: 0x04000073 RID: 115
            None = 1,
            // Token: 0x04000074 RID: 116
            TwoSamples,
            // Token: 0x04000075 RID: 117
            FourSamples = 4,
            // Token: 0x04000076 RID: 118
            EightSamples = 8
        }

        // Token: 0x0200000B RID: 11
        public enum UpdateFuncType
        {
            // Token: 0x04000078 RID: 120
            Update = 1,
            // Token: 0x04000079 RID: 121
            LateUpdate
        }

        // Token: 0x0200000C RID: 12
        private class MascotForm : Form
        {
            // Token: 0x060000CC RID: 204 RVA: 0x0000509C File Offset: 0x0000329C
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
                this.blend = default(MascotMakerMulti.MascotForm.BLENDFUNC);
                this.blend.BlendOp = 0;
                this.blend.BlendFlags = 0;
                this.blend.AlphaFormat = 1;
                base.Capture = true;
                base.FormClosed += new FormClosedEventHandler(this.OnFormClosed);
                base.MouseDown += this.Form_MouseDown;
                base.MouseUp += this.Form_MouseUp;
                this.HitTestCount = 0;
            }

            // Token: 0x060000CD RID: 205
            [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
            private static extern int UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref MascotMakerMulti.MascotForm.Vector2 pptDst, ref MascotMakerMulti.MascotForm.Vector2 psize, IntPtr hdcSrc, ref MascotMakerMulti.MascotForm.Vector2 pprSrc, int crKey, ref MascotMakerMulti.MascotForm.BLENDFUNC pblend, int dwFlags);

            // Token: 0x060000CE RID: 206
            [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
            private static extern IntPtr GetDC(IntPtr hWnd);

            // Token: 0x060000CF RID: 207
            [DllImport("user32.dll", ExactSpelling = true)]
            private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

            // Token: 0x060000D0 RID: 208
            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            private static extern IntPtr CreateCompatibleDC(IntPtr hDC);

            // Token: 0x060000D1 RID: 209
            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            private static extern int DeleteDC(IntPtr hdc);

            // Token: 0x060000D2 RID: 210
            [DllImport("gdi32.dll", ExactSpelling = true)]
            private static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

            // Token: 0x060000D3 RID: 211
            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            private static extern int DeleteObject(IntPtr hObject);

            // Token: 0x060000D4 RID: 212 RVA: 0x00005175 File Offset: 0x00003375
            private void OnFormClosed(object sender, EventArgs e)
            {
                base.MouseDown -= this.Form_MouseDown;
                base.MouseUp -= this.Form_MouseUp;
            }

            // Token: 0x060000D5 RID: 213 RVA: 0x0000519C File Offset: 0x0000339C
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

            // Token: 0x060000D6 RID: 214 RVA: 0x00005210 File Offset: 0x00003410
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

            // Token: 0x14000033 RID: 51
            // (add) Token: 0x060000D7 RID: 215 RVA: 0x00005284 File Offset: 0x00003484
            // (remove) Token: 0x060000D8 RID: 216 RVA: 0x000052BC File Offset: 0x000034BC
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public MouseEventHandler _LeftMouseDown;

            // Token: 0x14000034 RID: 52
            // (add) Token: 0x060000D9 RID: 217 RVA: 0x000052F4 File Offset: 0x000034F4
            // (remove) Token: 0x060000DA RID: 218 RVA: 0x0000532C File Offset: 0x0000352C
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public MouseEventHandler _LeftMouseUp;

            // Token: 0x14000035 RID: 53
            // (add) Token: 0x060000DB RID: 219 RVA: 0x00005364 File Offset: 0x00003564
            // (remove) Token: 0x060000DC RID: 220 RVA: 0x0000539C File Offset: 0x0000359C
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public MouseEventHandler _RightMouseDown;

            // Token: 0x14000036 RID: 54
            // (add) Token: 0x060000DD RID: 221 RVA: 0x000053D4 File Offset: 0x000035D4
            // (remove) Token: 0x060000DE RID: 222 RVA: 0x0000540C File Offset: 0x0000360C
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public MouseEventHandler _RightMouseUp;

            // Token: 0x14000037 RID: 55
            // (add) Token: 0x060000DF RID: 223 RVA: 0x00005444 File Offset: 0x00003644
            // (remove) Token: 0x060000E0 RID: 224 RVA: 0x0000547C File Offset: 0x0000367C
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public MouseEventHandler _MiddleMouseDown;

            // Token: 0x14000038 RID: 56
            // (add) Token: 0x060000E1 RID: 225 RVA: 0x000054B4 File Offset: 0x000036B4
            // (remove) Token: 0x060000E2 RID: 226 RVA: 0x000054EC File Offset: 0x000036EC
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public MouseEventHandler _MiddleMouseUp;

            // Token: 0x060000E3 RID: 227 RVA: 0x00005524 File Offset: 0x00003724
            public void Repaint(Bitmap bitmap, byte opacity)
            {
                if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                {
                    throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");
                }
                IntPtr dc = MascotMakerMulti.MascotForm.GetDC(IntPtr.Zero);
                IntPtr intPtr = MascotMakerMulti.MascotForm.CreateCompatibleDC(dc);
                IntPtr intPtr2 = IntPtr.Zero;
                IntPtr hObject = IntPtr.Zero;
                intPtr2 = bitmap.GetHbitmap(System.Drawing.Color.FromArgb(0));
                hObject = MascotMakerMulti.MascotForm.SelectObject(intPtr, intPtr2);
                MascotMakerMulti.MascotForm.Vector2 vector = new MascotMakerMulti.MascotForm.Vector2(bitmap.Width, bitmap.Height);
                MascotMakerMulti.MascotForm.Vector2 vector2 = new MascotMakerMulti.MascotForm.Vector2(0, 0);
                MascotMakerMulti.MascotForm.Vector2 vector3 = new MascotMakerMulti.MascotForm.Vector2(base.Left, base.Top);
                this.blend.SourceConstantAlpha = opacity;
                MascotMakerMulti.MascotForm.UpdateLayeredWindow(base.Handle, dc, ref vector3, ref vector, intPtr, ref vector2, 0, ref this.blend, 2);
                if (intPtr2 != IntPtr.Zero)
                {
                    MascotMakerMulti.MascotForm.SelectObject(intPtr, hObject);
                    MascotMakerMulti.MascotForm.DeleteObject(intPtr2);
                }
                MascotMakerMulti.MascotForm.DeleteDC(intPtr);
                MascotMakerMulti.MascotForm.ReleaseDC(IntPtr.Zero, dc);
            }

            // Token: 0x060000E4 RID: 228 RVA: 0x00005608 File Offset: 0x00003808
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

            // Token: 0x1700001B RID: 27
            // (get) Token: 0x060000E5 RID: 229 RVA: 0x00005660 File Offset: 0x00003860
            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams createParams = base.CreateParams;
                    createParams.ExStyle |= 524288;
                    return createParams;
                }
            }

            // Token: 0x0400007A RID: 122
            private const int ULW_COLORKEY = 1;

            // Token: 0x0400007B RID: 123
            private const int ULW_ALPHA = 2;

            // Token: 0x0400007C RID: 124
            private const int ULW_OPAQUE = 4;

            // Token: 0x0400007D RID: 125
            private const byte AC_SRC_OVER = 0;

            // Token: 0x0400007E RID: 126
            private const byte AC_SRC_ALPHA = 1;

            // Token: 0x0400007F RID: 127
            private MascotMakerMulti.MascotForm.BLENDFUNC blend;

            // Token: 0x04000080 RID: 128
            public int HitTestCount;

            // Token: 0x04000081 RID: 129
            public int HitTestCountTmp;

            // Token: 0x0200000D RID: 13
            private struct Vector2
            {
                // Token: 0x060000E6 RID: 230 RVA: 0x00005687 File Offset: 0x00003887
                public Vector2(int x, int y)
                {
                    this.x = x;
                    this.y = y;
                }

                // Token: 0x04000088 RID: 136
                public int x;

                // Token: 0x04000089 RID: 137
                public int y;
            }

            // Token: 0x0200000E RID: 14
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            private struct BGRA
            {
                // Token: 0x0400008A RID: 138
                public byte B;

                // Token: 0x0400008B RID: 139
                public byte G;

                // Token: 0x0400008C RID: 140
                public byte R;

                // Token: 0x0400008D RID: 141
                public byte A;
            }

            // Token: 0x0200000F RID: 15
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            private struct BLENDFUNC
            {
                // Token: 0x0400008E RID: 142
                public byte BlendOp;

                // Token: 0x0400008F RID: 143
                public byte BlendFlags;

                // Token: 0x04000090 RID: 144
                public byte SourceConstantAlpha;

                // Token: 0x04000091 RID: 145
                public byte AlphaFormat;
            }
        }
    }
}
