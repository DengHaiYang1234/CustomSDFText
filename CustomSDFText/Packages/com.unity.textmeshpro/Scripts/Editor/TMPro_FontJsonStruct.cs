using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

namespace TMPro.EditorUtilities
{
    [Serializable]
    public struct TMPro_FontJsonStruct
    {
        public FontFaceInfo faceInfo;
        public int characterTableCount;
        public CharacterTable[] characterTable;
        public int glyphAdjustmentTableCount;
        public GlyphAdjustmentTable[] glyphAdjustmentTable;
    }

    [Serializable]
    public struct FontFaceInfo
    {
        //
        // Summary:
        //     The Baseline is an imaginary line upon which all glyphs appear to rest on.
        public float baseline;
        //
        // Summary:
        //     The position of the strikethrough.
        public float strikethroughOffset;
        //
        // Summary:
        //     The thickness of the underline.
        public float underlineThickness;
        //
        // Summary:
        //     The position of the underline.
        public float underlineOffset;
        //
        // Summary:
        //     The relative size / scale of subscript characters.
        public float subscriptSize;
        //
        // Summary:
        //     The position of characters using subscript.
        public float subscriptOffset;
        //
        // Summary:
        //     The relative size / scale of superscript characters.
        public float superscriptSize;
        //
        // Summary:
        //     The position of characters using superscript.
        public float superscriptOffset;
        //
        // Summary:
        //     The Descent line is typically located at the bottom of the glyph with the lowest
        //     descender in the typeface.
        public float descentLine;
        //
        // Summary:
        //     The width of the tab character.
        public float tabWidth;
        //
        // Summary:
        //     The Mean line is typically located at the top of lowercase letters.
        public float meanLine;
        //
        // Summary:
        //     The Cap line is typically located at the top of capital letters.
        public float capLine;
        //
        // Summary:
        //     The Ascent line is typically located at the top of the tallest glyph in the typeface.
        public float ascentLine;
        //
        // Summary:
        //     The line height represents the distance between consecutive lines of text.
        public float lineHeight;
        //
        // Summary:
        //     The relative scale of the typeface.
        public float scale;
        //
        // Summary:
        //     The point size used for sampling the typeface.
        public int pointSize;
        //
        // Summary:
        //     The style name of the typeface which defines both the visual style and weight
        //     of the typeface.
        public string styleName;
        //
        // Summary:
        //     The name of the font typeface also known as family name.
        public string familyName;
        //
        // Summary:
        //     The thickness of the strikethrough.
        public float strikethroughThickness;

        public float padding;
        public int atlasWidth;
        public int atlasHeight;
    }

    [Serializable]
    public struct CharacterTable
    {
        public uint unicode;
        public uint glyphIndex;
        public float scale;
        public FontGlyphRect glyphRect;
        public FontGlyphMetrics glyphMetrics;
    }

    [Serializable]
    public struct FontGlyphRect
    {
        public float x;
        public float y;
        public float width;
        public float height;
    }

    [Serializable]
    public struct FontGlyphMetrics
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
    }

    [Serializable]
    public struct GlyphAdjustmentTable
    {
        public uint glyphIndex;
        public GlyphAdjustmentRecord firstAdjustmentRecord;
        public GlyphAdjustmentRecord secondAdjustmentRecord;
    }

    [Serializable]
    public struct GlyphAdjustmentRecord
    {
        public uint glyphIndex;
        public float xOffset;
        public float yOffset;
        public float xAdvance;
    }

}
