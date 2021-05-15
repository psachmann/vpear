using NUnit.Framework;
using System;
using System.Linq;

public class HeatmapTest
{
    [Test]
    public void CreateHeatmapColorsTest()
    {
        var values = new double[10, 10];
        var result = Heatmap.CreateHeatmapColors(0.0, 100.0, values, ColorScale.Jet);

        Assert.NotNull(result);
        Assert.AreEqual(10 * 10, result.Length);
    }

    [Test]
    public void CreateHeatmapTextureTest()
    {
        var values = new double[10, 10];
        var colors = Heatmap.CreateHeatmapColors(0.0, 100.0, values, ColorScale.Jet);
        var result = Heatmap.CreateHeatmapTexture(10, 10, colors);

        Assert.NotNull(result);
        Assert.AreEqual(10, result.width);
        Assert.AreEqual(10, result.height);
    }

    [Test]
    public void CreateHeatmapValuesTest()
    {
        var values = new ARFeature().GetState() as ARState;
        var result = Heatmap.CreateHeatmapValues(
            64,
            27,
            TimeSpan.FromMinutes(120.0),
            values.History.Last(),
            values.History);

        Assert.NotNull(result);
        Assert.AreEqual(64, result.GetLength(0));
        Assert.AreEqual(27, result.GetLength(1));
    }

    [Test]
    public void GetColorsSuccessTest()
    {
        var colorScales = Enum.GetValues(typeof(ColorScale));

        foreach (var colorScale in colorScales)
        {
            var result = Heatmap.GetColorScale((ColorScale)colorScale);

            Assert.NotNull(result);
        }
        
    }

    [Test]
    public void GetColorsFailureTest()
    {
        Assert.Catch<ArgumentOutOfRangeException>(() => Heatmap.GetColorScale((ColorScale)12));
    }

    [Test]
    public void ScaleTest()
    {
        var values = new double[10, 10];
        var result = Heatmap.Scale(4.0, values, Heatmap.InterpolationMehtod.Bilinear);

        Assert.AreEqual(10 * 4, result.GetLength(0));
        Assert.AreEqual(10 * 4, result.GetLength(1));

        result = Heatmap.Scale(4.0, values, Heatmap.InterpolationMehtod.Bicosine);

        Assert.AreEqual(10 * 4, result.GetLength(0));
        Assert.AreEqual(10 * 4, result.GetLength(1));

        result = Heatmap.Scale(4.0, values, Heatmap.InterpolationMehtod.Bicubic);

        Assert.AreEqual(10 * 4, result.GetLength(0));
        Assert.AreEqual(10 * 4, result.GetLength(1));
    }
}
