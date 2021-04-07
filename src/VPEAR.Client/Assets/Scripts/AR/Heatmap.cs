using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using VPEAR.Core.Wrappers;

public static class Heatmap
{
    public static float[,] ScaleBilinear(float scaleX, float scaleY, float[,] values)
    {
        var width = values.GetLength(0);
        var height = values.GetLength(0);
        var newWidth = (int)(width * scaleX);
        var newHeight = (int)(height * scaleY);
        var result = new float[newWidth, newHeight];

        for (var x = 0; x < newWidth; x++)
        {
            for (var y = 0; y < newHeight; y++)
            {
                var gx = ((float)x) / newWidth * (width - 1);
                var gy = ((float)y) / newHeight * (height - 1);
                var gxi = (int)gx;
                var gyi = (int)gy;
                var v00 = values[gxi, gyi];
                var v10 = values[gxi + 1, gyi];
                var v01 = values[gxi, gyi + 1];
                var v11 = values[gxi + 1, gyi + 1];

                result[x, y] = Blerp(v00, v10, v01, v11, gx - gxi, gy - gyi);
            }
        };

        return result;
    }

    public static Sprite CreateHeatmapSprite(
        float minValue,
        float maxValue,
        float[,] values,
        ColorScale colorScale,
        FilterMode filterMode = FilterMode.Point,
        TextureWrapMode wrapMode = TextureWrapMode.Clamp,
        float pixelsPerUnit = 1f)
    {
        var width = values.GetLength(0);
        var height = values.GetLength(1);
        var texture = new Texture2D(width, height);
        var colorArray = new Color32[width * height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                colorArray[x + y * width] = GetColor(minValue, maxValue, values[x, y], GetColorScale(colorScale));
            }
        };

        texture.SetPixels32(colorArray);
        texture.Apply();
        texture.filterMode = filterMode;
        texture.wrapMode = wrapMode;

        return Sprite.Create(texture, new Rect(0f, 0f, width, height), Vector2.zero, pixelsPerUnit);
    }

    public static Color32[] CreateHeatmapColors(
        float minValue,
        float maxValue,
        float[,] values,
        ColorScale colorScale)
    {
        var width = values.GetLength(0);
        var height = values.GetLength(1);
        var colorArray = new Color32[width * height];
        var colors = GetColorScale(colorScale);

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                colorArray[x + y * width] = GetColor(minValue, maxValue, values[x, y], colors);
            }
        };

        return colorArray;
    }

    public static Texture2D CreateHeatmapTexture(
        int width,
        int height,
        Color32[] colors,
        FilterMode filterMode = FilterMode.Point,
        TextureWrapMode wrapMode = TextureWrapMode.Clamp)
    {
        var texture = new Texture2D(width, height);

        texture.SetPixels32(colors);
        texture.Apply();
        texture.filterMode = filterMode;
        texture.wrapMode = wrapMode;

        return texture;
    }

    public static float[,] CreateHeatmapValues(
        int width,
        int height,
        TimeSpan deltaMinutes,
        GetFrameResponse current,
        IList<GetFrameResponse> history)
    {
        var values = new float[width, height];
        var slice = GetSortedSlice(deltaMinutes, current, history);

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                values[x, y] = (float)slice
                    .Select(frame => frame.Readings[x][y])
                    .Average();
            }
        };

        return values;
    }

    private static float Lerp(float s, float e, float t)
    {
        return s + (e - s) * t;
    }

    private static float Blerp(float v00, float v10, float v01, float v11, float tx, float ty)
    {
        return Lerp(Lerp(v00, v10, tx), Lerp(v01, v11, tx), ty);
    }

    private static IOrderedEnumerable<GetFrameResponse> GetSortedSlice(
        TimeSpan delta,
        GetFrameResponse current,
        IEnumerable<GetFrameResponse> history)
    {
        var minDate = current.Time - delta;
        var maxDate = current.Time;

        return history.Where(frame => minDate <= frame.Time && maxDate >= frame.Time)
            .OrderBy(frame => frame.Time);
    }

    private static Color32 GetColor(float min, float max, float value, Texture2D colors)
    {
        // if value is smaller min, set value to min
        if (value < min)
        {
            value = min;
        }

        // if value is greater thresh, set value to max
        if (value > max)
        {
            value = max;
        }

        var x = (int)(colors.width / max * value);
        var y = 0;
        var color = colors.GetPixel(x, y);

        if (value == min)
        {
            color.a = 0.1f;
        }

        return color;
    }

    private static Texture2D GetColorScale(ColorScale colorScale)
    {
        switch (colorScale)
        {
            case ColorScale.Jet:
                return JetTexture;
            case ColorScale.Plasma:
                return PlasmaTexture;
            case ColorScale.Viridis:
                return ViridisTexture;
            default:
                throw new ArgumentOutOfRangeException(nameof(colorScale));
        }
    }

    private static Texture2D JetTexture => Resources.Load("Images/Jet") as Texture2D;

    private static Texture2D PlasmaTexture => Resources.Load("Images/Plasma") as Texture2D;

    private static Texture2D ViridisTexture => Resources.Load("Images/Viridis") as Texture2D;
}
