using Guna.UI2.WinForms;

namespace Client.UI;

public class ColorHelper
{
    public static void SetPanelColor(Guna2Panel panel, int r, int g, int b)
    {
        panel.BackColor = Color.FromArgb(r, g, b);

        var label = panel.Controls.OfType<Label>().FirstOrDefault();
        if (label == null) return;

        label.BackColor = Color.FromArgb(r, g, b);
        label.ForeColor = Color.White;
    }
}