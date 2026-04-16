namespace Client.UI;

public class LoginAnimator(Form form)
{
    private bool _running;

    public async Task StartAsync(CancellationToken ct = default)
    {
        _running = true;

        int r = 0, g = 50, b = 50;
        form.BackColor = Color.FromArgb(r, g, b);

        while (_running && !ct.IsCancellationRequested)
        {
            for (var i = 0; i < 50; i++)
            {
                r++;
                g--;
                form.BackColor = Color.FromArgb(r, g, b);
                await Task.Delay(100, ct);
            }

            for (var i = 0; i < 50; i++)
            {
                g++;
                b--;
                form.BackColor = Color.FromArgb(r, g, b);
                await Task.Delay(100, ct);
            }

            for (var i = 0; i < 50; i++)
            {
                b++;
                r--;
                form.BackColor = Color.FromArgb(r, g, b);
                await Task.Delay(100, ct);
            }
        }
    }

    public void Stop()
    {
        _running = false;
    }
}