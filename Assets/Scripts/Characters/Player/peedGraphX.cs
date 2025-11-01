using UnityEngine;

/// <summary>
/// Rigidbody2D의 X속도를 화면에 그래프로 표시하는 디버그용 스크립트.
/// Player 오브젝트에 붙이면 자동으로 작동.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class SpeedGraphX : MonoBehaviour
{
    [Header("Sampling")]
    [Tooltip("버퍼에 보관할 샘플 수 (그래프의 가로 해상도)")]
    public int sampleCount = 300;
    [Tooltip("FixedUpdate에서 샘플링할지 여부 (물리 프레임 기준)")]
    public bool sampleInFixedUpdate = true;

    [Header("Graph Rect (pixels)")]
    public Vector2 anchor = new Vector2(24, 24);
    public Vector2 size = new Vector2(420, 160);

    [Header("Y Scale")]
    public bool manualYMax = false;
    public float yMax = 10f;
    public float autoHeadroom = 1.1f;

    [Header("Style")]
    public Color bgColor = new Color(0, 0, 0, 0.35f);
    public Color frameColor = new Color(1, 1, 1, 0.8f);
    public Color lineColor = new Color(1f, 0.5f, 0.3f, 0.95f);
    public int lineWidth = 2;

    [Header("Grid")]
    public bool showGrid = true;
    public Color gridColor = new Color(1, 1, 1, 0.12f);
    public int gridCols = 6;
    public int gridRows = 4;

    [Header("Readout")]
    public bool showNumber = true;

    Rigidbody2D rb;
    float[] bufX;
    int head;
    Material lineMat;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bufX = new float[sampleCount];
        CreateLineMaterial();
    }

    void FixedUpdate()
    {
        if (sampleInFixedUpdate) Sample();
    }

    void Update()
    {
        if (!sampleInFixedUpdate)
            Sample();
    }

    void Sample()
    {
        bufX[head] = rb.velocity.x;
        head = (head + 1) % sampleCount;
    }

    float CurrentMaxY()
    {
        if (manualYMax) return Mathf.Max(0.001f, yMax);
        float max = 0f;
        for (int i = 0; i < sampleCount; i++)
            max = Mathf.Max(max, Mathf.Abs(bufX[i]));
        if (max <= 0.001f) max = 1f;
        return max * autoHeadroom;
    }

    void CreateLineMaterial()
    {
        if (lineMat != null) return;
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        lineMat = new Material(shader) { hideFlags = HideFlags.HideAndDontSave };
        lineMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        lineMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        lineMat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        lineMat.SetInt("_ZWrite", 0);
    }

    void OnGUI()
    {
        if (showNumber)
        {
            float vx = rb.velocity.x;
            var rect = new Rect(anchor.x, anchor.y + size.y + 6, 140, 28);
            GUI.color = new Color(0,0,0,0.35f);
            GUI.Box(rect, GUIContent.none);
            GUI.color = Color.white;
            GUI.Label(rect, $"vx : {vx:0.00}");
        }
    }

    void OnRenderObject()
    {
        if (lineMat == null) CreateLineMaterial();
        lineMat.SetPass(0);
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, Screen.width, Screen.height, 0);

        float x = anchor.x, y = anchor.y, w = size.x, h = size.y;

        // 배경
        DrawFilledRect(x, y, w, h, bgColor);

        // 그리드
        if (showGrid) DrawGrid();

        // 프레임
        DrawRect(x, y, w, h, frameColor, 1);

        // X속도 그래프
        float ymax = CurrentMaxY();
        PlotBuffer(bufX, lineColor, ymax);

        GL.PopMatrix();
    }

    void DrawGrid()
    {
        float x = anchor.x, y = anchor.y, w = size.x, h = size.y;
        GL.Begin(GL.LINES);
        GL.Color(gridColor);
        for (int c = 1; c < gridCols; c++)
        {
            float px = x + w * c / gridCols;
            GL.Vertex3(px, y, 0);
            GL.Vertex3(px, y + h, 0);
        }
        for (int r = 1; r < gridRows; r++)
        {
            float py = y + h * r / gridRows;
            GL.Vertex3(x, py, 0);
            GL.Vertex3(x + w, py, 0);
        }
        GL.End();
    }

    void PlotBuffer(float[] buf, Color col, float ymax)
    {
        if (ymax <= 0.0001f) return;
        float x = anchor.x, y = anchor.y, w = size.x, h = size.y;
        int passes = Mathf.Max(1, lineWidth);

        for (int p = 0; p < passes; p++)
        {
            float offset = p - (passes - 1) * 0.5f;
            GL.Begin(GL.LINES);
            GL.Color(col);

            for (int i = 0; i < sampleCount - 1; i++)
            {
                int a = (head + i) % sampleCount;
                int b = (head + i + 1) % sampleCount;

                float xa = x + (w * i / (sampleCount - 1));
                float xb = x + (w * (i + 1) / (sampleCount - 1));

                float ya = y + h - Mathf.Clamp01((buf[a] + ymax) / (2 * ymax)) * h + offset;
                float yb = y + h - Mathf.Clamp01((buf[b] + ymax) / (2 * ymax)) * h + offset;

                GL.Vertex3(xa, ya, 0);
                GL.Vertex3(xb, yb, 0);
            }
            GL.End();
        }
    }

    void DrawRect(float x, float y, float w, float h, Color col, int thickness = 1)
    {
        for (int i = 0; i < thickness; i++)
        {
            GL.Begin(GL.LINES);
            GL.Color(col);
            GL.Vertex3(x + i, y + i, 0); GL.Vertex3(x + w - i, y + i, 0);
            GL.Vertex3(x + w - i, y + i, 0); GL.Vertex3(x + w - i, y + h - i, 0);
            GL.Vertex3(x + w - i, y + h - i, 0); GL.Vertex3(x + i, y + h - i, 0);
            GL.Vertex3(x + i, y + h - i, 0); GL.Vertex3(x + i, y + i, 0);
            GL.End();
        }
    }

    void DrawFilledRect(float x, float y, float w, float h, Color col)
    {
        GL.Begin(GL.QUADS);
        GL.Color(col);
        GL.Vertex3(x, y, 0);
        GL.Vertex3(x + w, y, 0);
        GL.Vertex3(x + w, y + h, 0);
        GL.Vertex3(x, y + h, 0);
        GL.End();
    }
}
