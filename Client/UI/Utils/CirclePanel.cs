using System.Drawing.Drawing2D;

namespace Client.UI.Utils;

public class CirclePanel : Panel
{
    public CirclePanel()
    {
        SetStyle(ControlStyles.UserPaint |
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw, true);

        UpdateStyles();
        BackColor = Color.Transparent;
    }

    protected override void OnResize(EventArgs eventargs)
    {
        base.OnResize(eventargs);

        var size = Math.Min(Width, Height);
        Width = size;
        Height = size;

        var path = new GraphicsPath();
        path.AddEllipse(0, 0, size - 1, size - 1);

        Region = new Region(path);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

        using var brush = new SolidBrush(BackColor);
        e.Graphics.FillEllipse(brush, 0, 0, Width - 1, Height - 1);
    }
}