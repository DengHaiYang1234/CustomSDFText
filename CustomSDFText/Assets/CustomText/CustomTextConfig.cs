using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

public class TFontGlyphMetrics
{
    //
    // Summary:
    //     The width of the glyph.
    public float width;
    //
    // Summary:
    //     The height of the glyph.
    public float height;
    //
    // Summary:
    //     The horizontal distance from the current drawing position (origin) relative to
    //     the element's left bounding box edge (bbox).
    public float xOffset;
    //
    // Summary:
    //     The vertical distance from the current baseline relative to the element's top
    //     bounding box edge (bbox).
    public float yOffset;
    //
    // Summary:
    //     The horizontal distance to increase (left to right) or decrease (right to left)
    //     the drawing position relative to the origin of the text element.
    public float xAdvance;

    public TFontGlyphMetrics(float width, float height, float xOffset, float yOffset, float xAdvance)
    {
        this.width = width;
        this.height = height;
        this.xOffset = xOffset;
        this.yOffset = yOffset;
        this.xAdvance = xAdvance;
    }

    public TFontGlyphMetrics(TFontGlyphMetrics m)
    {
        this.width = m.width;
        this.height = m.height;
        this.xOffset = m.xOffset;
        this.yOffset = m.yOffset;
        this.xAdvance = m.xAdvance;
    }
}

public class TFontGlyph
{
    public Rect rect;
    public TFontGlyphMetrics metrics;
    public Dictionary<uint, float> kerning;


    public TFontGlyph(Rect rect, float width, float height, float xOffset, float yOffset, float xAdvance)
    {
        this.rect = rect;
        metrics = new TFontGlyphMetrics(width, height, xOffset, yOffset, xAdvance);
    }

    public float GetKerning(uint previousChar)
    {
        float k = 0f;

        if (kerning == null)
            return k;

        kerning.TryGetValue(previousChar, out k);
        return k;
    }
}

public class TFontGlyphAdjustmentRecord
{
    public uint glyphIndex;
    public float xAdvance;
}

public class TFontFace
{
    public float baseline, strikethroughOffset, underlineThickness, underlineOffset, subscriptSize, subscriptOffset, superscriptSize,
                superscriptOffset, descentLine, tabWidth, meanLine, capLine, ascentLine, lineHeight, scale,
                strikethroughThickness, padding;

    public int atlasWidth, atlasHeight, pointSize;

    public string styleName, familyName;


    public TFontFace(float baseline, float strikethroughOffset, float underlineThickness, float underlineOffset,
                    float subscriptSize, float subscriptOffset, float superscriptSize, float superscriptOffset,
                    float descentLine, float tabWidth, float meanLine,
                    float capLine, float ascentLine, float lineHeight, float scale, int pointSize,
                    string styleName, string familyName, float strikethroughThickness, int atlasWidth, int atlasHeight, float padding)
    {
        this.baseline = baseline;
        this.strikethroughOffset = strikethroughOffset;
        this.underlineThickness = underlineThickness;
        this.underlineOffset = underlineOffset;
        this.subscriptSize = subscriptSize;
        this.subscriptOffset = subscriptOffset;
        this.superscriptSize = superscriptSize;
        this.superscriptOffset = superscriptOffset;
        this.descentLine = descentLine;
        this.tabWidth = tabWidth;
        this.meanLine = meanLine;
        this.capLine = capLine;
        this.ascentLine = ascentLine;
        this.lineHeight = lineHeight;
        this.scale = scale;
        this.pointSize = pointSize;
        this.styleName = styleName;
        this.familyName = familyName;
        this.strikethroughThickness = strikethroughThickness;
        this.atlasWidth = atlasWidth;
        this.atlasHeight = atlasHeight;
        this.padding = padding;
    }
}

public class CustomTextConfig
{
    public static CustomTextConfig m_Instance;
    public static CustomTextConfig Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new CustomTextConfig();
                m_Instance.Init();
            }


            return m_Instance;
        }
    }

    public Dictionary<uint, TFontGlyph> Glyphs { get { return m_GlyphDictionary; } }
    public TFontFace TFontFace { get { return m_TFontFace; } }

    private Dictionary<uint, TFontGlyph> m_GlyphDictionary = new Dictionary<uint, TFontGlyph>();
    private Dictionary<uint, TFontGlyphAdjustmentRecord[]> m_KerningDictionary = new Dictionary<uint, TFontGlyphAdjustmentRecord[]>();
    private TFontFace m_TFontFace;

    


    public void Init()
    {
        string path = "/Users/denghaiyang/workSpace/TestWork/Assets/TextMesh Pro/Resources/Fonts & Materials/Songti SDF";
        m_GlyphDictionary.Clear();
        m_KerningDictionary.Clear();

        var content = "";
        using (var sr = File.OpenText(path))
        {
            content = sr.ReadToEnd();
        }


        var jsonStruct = JsonUtility.FromJson<TMPro_FontJsonStruct>(content);

        m_TFontFace = new TFontFace(
            jsonStruct.faceInfo.baseline,
            jsonStruct.faceInfo.strikethroughOffset,
            jsonStruct.faceInfo.underlineThickness,
            jsonStruct.faceInfo.underlineOffset,
            jsonStruct.faceInfo.subscriptSize,
            jsonStruct.faceInfo.subscriptOffset,
            jsonStruct.faceInfo.superscriptSize,
            jsonStruct.faceInfo.superscriptOffset,
            jsonStruct.faceInfo.descentLine,
            jsonStruct.faceInfo.tabWidth,
            jsonStruct.faceInfo.meanLine,
            jsonStruct.faceInfo.capLine,
            jsonStruct.faceInfo.ascentLine,
            jsonStruct.faceInfo.lineHeight,
            jsonStruct.faceInfo.scale,
            jsonStruct.faceInfo.pointSize,
            jsonStruct.faceInfo.styleName,
            jsonStruct.faceInfo.familyName,
            jsonStruct.faceInfo.strikethroughThickness,
            jsonStruct.faceInfo.atlasWidth,
            jsonStruct.faceInfo.atlasHeight,
            jsonStruct.faceInfo.padding
        );

        if (jsonStruct.glyphAdjustmentTableCount > 0)
        {
            for (int i = 0; i < jsonStruct.glyphAdjustmentTableCount; i++)
            {
                var item = jsonStruct.glyphAdjustmentTable[i];

                if (m_KerningDictionary.ContainsKey(item.glyphIndex))
                    continue;

                m_KerningDictionary.Add(item.glyphIndex, new TFontGlyphAdjustmentRecord[2]
                {
                    new TFontGlyphAdjustmentRecord()
                    {
                        glyphIndex = item.firstAdjustmentRecord.glyphIndex,
                        xAdvance = item.firstAdjustmentRecord.xAdvance,
                    },
                    new TFontGlyphAdjustmentRecord()
                    {
                        glyphIndex = item.secondAdjustmentRecord.glyphIndex,
                        xAdvance = item.secondAdjustmentRecord.xAdvance,
                    },
                });
            }
        }

        if (jsonStruct.characterTableCount > 0)
        {
            for (int i = 0; i < jsonStruct.characterTableCount; i++)
            {
                var item = jsonStruct.characterTable[i];

                if (m_GlyphDictionary.ContainsKey(item.unicode))
                    continue;

                TFontGlyph m_TFontGlyph = new TFontGlyph(
                    new Rect(item.glyphRect.x, item.glyphRect.y, item.glyphRect.width, item.glyphRect.height),

                    item.glyphMetrics.width, item.glyphMetrics.height, item.glyphMetrics.xOffset, item.glyphMetrics.yOffset, item.glyphMetrics.xAdvance
                );

                if (m_KerningDictionary.ContainsKey(item.glyphIndex))
                {
                    var kerning = m_KerningDictionary[item.glyphIndex];
                    m_TFontGlyph.kerning = new Dictionary<uint, float>()
                    {
                        {kerning[1].glyphIndex,kerning[1].xAdvance}
                    };
                }
                m_GlyphDictionary.Add(item.unicode, m_TFontGlyph);
            }
        }
    }
}
