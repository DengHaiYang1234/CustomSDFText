using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class CustomText : MonoBehaviour
{
    public string Text;
    public float Size = 32;
    public bool IsOrthographic;
    public bool UpdadeConfig = true;
    public Mesh m_Mesh;
    private List<Vector3> m_Vertices = new List<Vector3>();
    private List<Vector2> m_UVs = new List<Vector2>();
    private List<Vector2> m_UVs2 = new List<Vector2>();
    private List<Color> m_Colors = new List<Color>();
    private List<int> m_Triangles = new List<int>();

    public float Width { get; protected set; }
    public float Height { get; protected set; }

    private string m_Text;
    private float m_Size;
    private bool m_IsOrthographic, m_UpdadeConfig;


    public bool IsDirty
    {
        get
        {
            return (Text != m_Text) | (Size != m_Size) | (IsOrthographic != m_IsOrthographic) | (m_UpdadeConfig != UpdadeConfig);
        }
    }



    void LateUpdate()
    {
        if (IsDirty)
        {
            RebuildMesh();

            m_Text = Text;
            m_Size = Size;
            m_IsOrthographic = IsOrthographic;
        }
    }

    public void RebuildMesh()
    {
        if (UpdadeConfig)
        {
            UpdadeConfig = false;
            CustomTextConfig.Instance.Init();
        }

        if (m_Mesh == null)
        {
            m_Mesh = new Mesh();
        }

        m_Mesh.Clear();
        m_Vertices.Clear();
        m_UVs.Clear();
        m_UVs2.Clear();
        m_Colors.Clear();
        m_Triangles.Clear();

        Width = 0f;
        Height = 0f;
        DivisionText();

        m_Mesh.vertices = m_Vertices.ToArray();
        m_Mesh.uv = m_UVs.ToArray();
        m_Mesh.triangles = m_Triangles.ToArray();
        m_Mesh.colors = null;
        m_Mesh.uv2 = null;
        m_Mesh.normals = null;

        m_Mesh.RecalculateBounds();

        this.GetComponent<MeshFilter>().mesh = m_Mesh;
    }

    private void DivisionText()
    {
        float cursorX = 0, cursorY = 0;
        Text = Regex.Replace(Text, @"\r\n", "\n");
        string[] lines = Regex.Split(Text, @"\n");

        float pointSize = CustomTextConfig.Instance.TFontFace.pointSize;
        float lineHeight = CustomTextConfig.Instance.TFontFace.lineHeight;
        float baseScale = (float)Size / pointSize * (IsOrthographic ? 1 : 0.1f);

        foreach (string line in lines)
        {
            BlitString(line, cursorX, cursorY, baseScale);
        }

        cursorY += lineHeight * baseScale;

        Height = cursorY;
    }

    private float BlitString(string str, float cursorX, float cursorY, float baseScale)
    {
        TFontGlyph prevGlyph = null;


        foreach (char c in str)
        {
            uint charCode = (uint)c;
            if (!CustomTextConfig.Instance.Glyphs.ContainsKey(charCode))
            {
                continue;
            }
            TFontGlyph glyph = CustomTextConfig.Instance.Glyphs[charCode];

            float kerning = 0f;

            if (prevGlyph != null)
                kerning = prevGlyph.GetKerning(charCode) * Size;


            BlitQuad(cursorX, glyph, baseScale);

            cursorX += glyph.metrics.xAdvance * baseScale + kerning;
            prevGlyph = glyph;
        }

        if (cursorX > Width)
            Width = cursorX;

        return cursorX;
    }

    private void BlitQuad(float cursorX, TFontGlyph glyph, float baseScale)
    {

        float atlasWidth = CustomTextConfig.Instance.TFontFace.atlasWidth;
        float atlasHeight = CustomTextConfig.Instance.TFontFace.atlasHeight;

        float padding = 1.25f;

        Vector3 top_left;
        top_left.x = (glyph.metrics.xOffset - padding) * baseScale + cursorX;
        top_left.y = (glyph.metrics.yOffset + padding) * baseScale;
        top_left.z = 0;

        Vector3 bottom_left;
        bottom_left.x = top_left.x;
        bottom_left.y = top_left.y - ((glyph.metrics.height + padding * 2) * baseScale);
        bottom_left.z = 0;

        Vector3 top_right;
        top_right.x = bottom_left.x + ((glyph.metrics.width + padding * 2) * baseScale);
        top_right.y = top_left.y;
        top_right.z = 0;

        Vector3 bottom_right;
        bottom_right.x = top_right.x;
        bottom_right.y = bottom_left.y;
        bottom_right.z = 0;

        Vector2 uv_bottom_left;
        uv_bottom_left.x = (glyph.rect.x - padding) / atlasWidth;
        uv_bottom_left.y = (glyph.rect.y - padding) / atlasHeight;


        Vector2 uv_top_left;
        uv_top_left.x = uv_bottom_left.x;
        uv_top_left.y = (glyph.rect.y + padding + glyph.rect.height) / atlasHeight;


        Vector2 uv_top_right;
        uv_top_right.x = (glyph.rect.x + padding + glyph.rect.width) / atlasWidth;
        uv_top_right.y = uv_top_left.y;


        Vector2 uv_bottom_right;
        uv_bottom_right.x = uv_top_right.x;
        uv_bottom_right.y = uv_bottom_left.y;

        int index = m_Vertices.Count;

        m_Vertices.Add(bottom_left);
        m_Vertices.Add(bottom_right);
        m_Vertices.Add(top_left);
        m_Vertices.Add(top_right);

        m_UVs.Add(uv_bottom_left);
        m_UVs.Add(uv_bottom_right);
        m_UVs.Add(uv_top_left);
        m_UVs.Add(uv_top_right);

        m_Triangles.Add(index);
        m_Triangles.Add(index + 2);
        m_Triangles.Add(index + 1);
        m_Triangles.Add(index + 1);
        m_Triangles.Add(index + 2);
        m_Triangles.Add(index + 3);

    }
}
