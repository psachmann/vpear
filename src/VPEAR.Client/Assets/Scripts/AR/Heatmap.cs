using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VPEAR.Core.Wrappers;

public static class Heatmap
{
    public enum InterpolationMehtod : byte
    {
        Bilinear = 0,
        Bicosine,
        Bicubic
    }

    public static double[,] Scale(double scale, double[,] values, InterpolationMehtod mehtod)
    {
        var width = values.GetLength(0);
        var height = values.GetLength(1);
        var newWidth = (int)(width * scale);
        var newHeight = (int)(height * scale);
        var result = new double[newWidth, newHeight];

        for (var x = 0; x < newWidth; x++)
        {
            for (var y = 0; y < newHeight; y++)
            {
                result[x, y] = mehtod switch
                {
                    InterpolationMehtod.Bilinear => InterpolateBilinear(x, y, width, height, newWidth, newHeight, values),
                    InterpolationMehtod.Bicosine => InterpolateBicosine(x, y, width, height, newWidth, newHeight, values),
                    InterpolationMehtod.Bicubic => InterpolateBicubic(x, y, width, height, newWidth, newHeight, values),
                    _ => throw new ArgumentOutOfRangeException(nameof(mehtod)),
                };
            }
        };

        return result;
    }

    public static Color32[] CreateHeatmapColors(
        double minValue,
        double maxValue,
        double[,] values,
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
        FilterMode filterMode = FilterMode.Trilinear,
        TextureWrapMode wrapMode = TextureWrapMode.Clamp)
    {
        var texture = new Texture2D(width, height);

        texture.SetPixels32(colors);
        texture.Apply();
        texture.filterMode = filterMode;
        texture.wrapMode = wrapMode;

        return texture;
    }

    public static double[,] CreateHeatmapValues(
        int width,
        int height,
        TimeSpan deltaMinutes,
        GetFrameResponse current,
        IList<GetFrameResponse> history)
    {
        var values = new double[width, height];
        var slice = GetSortedSlice(deltaMinutes, current, history);

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                // calculating the median for the recorded readings
                values[x, y] = slice
                    .Select(frame => frame.Readings[x][y])
                    .OrderBy(value => value)
                    .ElementAt(slice.Count() / 2);
            }
        };

        return values;
    }

    private static double InterpolateBilinear(int x, int y, int width, int height, int newWidth, int newHeight, double[,] values)
    {
        var gx = ((double)x) / newWidth * (width - 1);
        var gy = ((double)y) / newHeight * (height - 1);
        var gxi = (int)gx;
        var gyi = (int)gy;
        var v00 = values[gxi, gyi];
        var v10 = values[gxi + 1, gyi];
        var v01 = values[gxi, gyi + 1];
        var v11 = values[gxi + 1, gyi + 1];

        return InterpolateLinear(InterpolateLinear(v00, v10, gx - gxi), InterpolateLinear(v01, v11, gx - gxi), gy - gyi);
    }

    private static double InterpolateLinear(double a, double b, double x)
    {
        return a + (b - a) * x;
    }

    private static double InterpolateBicosine(int x, int y, int width, int height, int newWidth, int newHeight, double[,] values)
    {
        var gx = ((double)x) / newWidth * (width - 1);
        var gy = ((double)y) / newHeight * (height - 1);
        var gxi = (int)gx;
        var gyi = (int)gy;
        var v00 = values[gxi, gyi];
        var v10 = values[gxi + 1, gyi];
        var v01 = values[gxi, gyi + 1];
        var v11 = values[gxi + 1, gyi + 1];

        return InterpolateCosine(InterpolateCosine(v00, v10, gx - gxi), InterpolateCosine(v01, v11, gx - gxi), gy - gyi);
    }

    private static double InterpolateCosine(double a, double b, double x)
    {
        var ft = x * Math.PI;
        var f = (1 - Math.Cos(ft)) * 0.5;

        return a * (1 - f) + b * f;
    }

    private static double InterpolateBicubic(int x, int y, int width, int height, int newWidth, int newHeight, double[,] values)
    {
        var dx = ((double)x) / newWidth * (width - 3);
        var dy = ((double)y) / newHeight * (height - 3);
        var x0 = (int)dx;
        var x1 = x0 + 1;
        var x2 = x0 + 2;
        var x3 = x0 + 3;
        var y0 = (int)dy;
        var y1 = y0 + 1;
        var y2 = y0 + 2;
        var y3 = y0 + 3;
        var fracX = dx - x0;
        var fracY = dy - y0;
        var v0 = InterpolateCubic(values[x0, y0], values[x1, y0], values[x2, y0], values[x3, y0], fracX);
        var v1 = InterpolateCubic(values[x0, y1], values[x1, y1], values[x2, y1], values[x3, y1], fracX);
        var v2 = InterpolateCubic(values[x0, y2], values[x1, y2], values[x2, y2], values[x3, y2], fracX);
        var v3 = InterpolateCubic(values[x0, y3], values[x1, y3], values[x2, y3], values[x3, y3], fracX);

        return InterpolateCubic(v0, v1, v2, v3, fracY);
    }

    private static double InterpolateCubic(double v00, double v10, double v01, double v11, double fracX)
    {
        var a = (v11 - v01) - (v00 - v10);
        var b = (v00 - v10) - a;
        var c = v01 - v00;
        var d = v10;

        return a * fracX * fracX * fracX + b * fracX * fracX + c * fracX + d;
    }

    private static IEnumerable<GetFrameResponse> GetSortedSlice(
        TimeSpan delta,
        GetFrameResponse current,
        IEnumerable<GetFrameResponse> history)
    {
        var minDate = current.Time - delta;
        var maxDate = current.Time;

        return history.Where(frame => minDate <= frame.Time && maxDate >= frame.Time)
            .OrderBy(frame => frame.Time);
    }

    private static Color32 GetColor(double min, double max, double value, Texture2D colors)
    {
        // if value is smaller min, set value to min
        if (value < min)
        {
            value = min;
        }

        // if value is greater max, set value to max
        if (value > max)
        {
            value = max;
        }

        var x = (int)Map(value, min, max, 0, colors.width);

        if (x >= colors.width)
        {
            x = colors.width - 1;
        }

        var color = colors.GetPixel(x, 0);

        if (value < (max * 0.01))
        {
            color.a = 0.1f;
        }

        return color;
    }

    private static double Map(double value, double fromSource, double toSource, double fromTarget, double toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }

    public static Texture2D GetColorScale(ColorScale colorScale)
    {
        return colorScale switch
        {
            ColorScale.Jet => JetTexture,
            ColorScale.Plasma => PlasmaTexture,
            ColorScale.Viridis => ViridisTexture,
            _ => throw new ArgumentOutOfRangeException(nameof(colorScale)),
        };
    }

    private static Texture2D JetTexture => Resources.Load("Images/Jet") as Texture2D;

    private static Texture2D PlasmaTexture => Resources.Load("Images/Plasma") as Texture2D;

    private static Texture2D ViridisTexture => Resources.Load("Images/Viridis") as Texture2D;
}
