using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPEAR.Core.Wrappers;

public class Heatmap
{
    private readonly int _width;
    private readonly int _height;
    private readonly float[,] _values;
    private readonly IEnumerable<GetFrameResponse> _history;
    private bool _update;
    private TimeSpan _delta;
    private GetFrameResponse _current;

    public Heatmap(int width, int height, TimeSpan delta, IEnumerable<GetFrameResponse> history)
    {
        _width = width;
        _height = height;
        _values = new float[width, height];
        _update = true;
        _delta = delta;
        _current = history.DefaultIfEmpty(new GetFrameResponse()).Last();
        _history = history;

        Update();
    }

    public TimeSpan Delta
    {
        get { return _delta; }

        set
        {
            _delta = value;
            _update = true;
        }
    }

    public GetFrameResponse Current
    {
        get { return _current; }

        set
        {
            if (_history.Contains(value))
            {
                _current = value;
                _update = true;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }

    public float[,] Values
    {
        get { return _values; }
    }

    public void Update()
    {
        if (!_update)
        {
            return;
        }

        var slice = GetSortedSlice(_delta, _current, _history);

        Parallel.For(0, _width, x =>
        {
            for (var y = 0; y < _height; y++)
            {
                _values[x, y] = (float)slice.Select(frame => frame.Readings[x][y]).Average();
            }
        });

        _update = false;
    }

    public float[,] ScaleBilinear(float scaleX, float scaleY)
    {
        Update();

        var newWidth = (int)(_width * scaleX);
        var newHeight = (int)(_height * scaleY);
        var result = new float[newWidth, newHeight];

        Parallel.For(0, newWidth, x =>
        {
            for (var y = 0; y < newHeight; y++)
            {
                var gx = ((float)x) / newWidth * (_width - 1);
                var gy = ((float)y) / newHeight * (_height - 1);
                var gxi = (int)gx;
                var gyi = (int)gy;
                var v00 = _values[gxi, gyi];
                var v10 = _values[gxi + 1, gyi];
                var v01 = _values[gxi, gyi + 1];
                var v11 = _values[gxi + 1, gyi + 1];

                result[x, y] = Blerp(v00, v10, v01, v11, gx - gxi, gy - gyi);
            }
        });

        return result;
    }

    private static float Lerp(float s, float e, float t)
    {
        return s + (e - s) * t;
    }

    private static float Blerp(float v00, float v10, float v01, float v11, float tx, float ty)
    {
        return Lerp(Lerp(v00, v10, tx), Lerp(v01, v11, tx), ty);
    }

    private static IEnumerable<GetFrameResponse> GetSortedSlice(TimeSpan delta, GetFrameResponse current, IEnumerable<GetFrameResponse> history)
    {
        var minDate = current.Time - delta;
        var maxDate = current.Time;

        return history.Where(frame => minDate <= frame.Time && maxDate >= frame.Time)
            .OrderBy(frame => frame.Time);
    }
}
