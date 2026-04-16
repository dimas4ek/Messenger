using Guna.UI2.WinForms;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using Timer = System.Windows.Forms.Timer;

namespace Client.Forms;

public partial class VideoPlayerForm : Form
{
    private Guna2Button? _btnFullscreen;
    private Guna2Button? _btnPlayPause;
    private Guna2Button? _btnStop;
    private bool _isDraggingProgress;
    private Label? _lblTime;
    private LibVLC? _libVlc;
    private Media? _media;
    private MediaPlayer? _mediaPlayer;
    private Guna2Panel? _panelControls;
    private Guna2TrackBar? _progressBar;

    private VideoView? _videoView;
    private Guna2TrackBar? _volumeBar;

    public VideoPlayerForm()
    {
        InitializeComponent();
        BuildUi();
        InitVlc();
    }

    private void BuildUi()
    {
        Text = "Video Player";
        Size = new Size(900, 560);
        BackColor = Color.FromArgb(20, 20, 30);
        FormBorderStyle = FormBorderStyle.Sizable;

        _videoView = new VideoView();
        _videoView.Dock = DockStyle.Fill;
        _videoView.BackColor = Color.Black;
        Controls.Add(_videoView);

        _panelControls = new Guna2Panel();
        _panelControls.Dock = DockStyle.Bottom;
        _panelControls.Height = 90;
        _panelControls.FillColor = Color.FromArgb(30, 30, 45);
        _panelControls.BorderRadius = 0;
        Controls.Add(_panelControls);

        _progressBar = new Guna2TrackBar();
        _progressBar.Location = new Point(10, 8);
        _progressBar.Size = new Size(860, 18);
        _progressBar.Minimum = 0;
        _progressBar.Maximum = 1000;
        _progressBar.Value = 0;
        _progressBar.ThumbColor = Color.FromArgb(100, 149, 237);
        _progressBar.FillColor = Color.FromArgb(60, 60, 80);
        _progressBar.MouseDown += (s, e) => _isDraggingProgress = true;
        _progressBar.MouseUp += ProgressBar_MouseUp;
        _panelControls.Controls.Add(_progressBar);

        _btnPlayPause = CreateButton("▶", 10, 35);
        _btnPlayPause.Click += BtnPlayPause_Click;
        _panelControls.Controls.Add(_btnPlayPause);

        _btnStop = CreateButton("■", 100, 35);
        _btnStop.Click += BtnStop_Click;
        _panelControls.Controls.Add(_btnStop);

        var lblVol = new Label();
        lblVol.Text = "🔊";
        lblVol.Font = new Font("Segoe UI", 13);
        lblVol.ForeColor = Color.White;
        lblVol.Location = new Point(195, 38);
        lblVol.Size = new Size(30, 30);
        lblVol.BackColor = Color.Transparent;
        _panelControls.Controls.Add(lblVol);

        _volumeBar = new Guna2TrackBar();
        _volumeBar.Location = new Point(230, 42);
        _volumeBar.Size = new Size(120, 18);
        _volumeBar.Minimum = 0;
        _volumeBar.Maximum = 100;
        _volumeBar.Value = 80;
        _volumeBar.ThumbColor = Color.FromArgb(100, 149, 237);
        _volumeBar.ValueChanged += (s, e) =>
        {
            if (_mediaPlayer != null)
                _mediaPlayer.Volume = _volumeBar.Value;
        };
        _panelControls.Controls.Add(_volumeBar);

        _lblTime = new Label();
        _lblTime.Text = "00:00 / 00:00";
        _lblTime.Font = new Font("Segoe UI", 9);
        _lblTime.ForeColor = Color.FromArgb(180, 180, 200);
        _lblTime.Location = new Point(365, 42);
        _lblTime.Size = new Size(130, 20);
        _lblTime.BackColor = Color.Transparent;
        _panelControls.Controls.Add(_lblTime);

        _btnFullscreen = CreateButton("⛶", 760, 35);
        _btnFullscreen.Click += BtnFullscreen_Click;
        _panelControls.Controls.Add(_btnFullscreen);

        var timer = new Timer();
        timer.Interval = 500;
        timer.Tick += Timer_Tick;
        timer.Start();
    }

    private Guna2Button CreateButton(string text, int x, int y)
    {
        var btn = new Guna2Button();
        btn.Text = text;
        btn.Size = new Size(75, 38);
        btn.Location = new Point(x, y);
        btn.Font = new Font("Segoe UI", 14);
        btn.FillColor = Color.FromArgb(50, 50, 70);
        btn.HoverState.FillColor = Color.FromArgb(100, 149, 237);
        btn.PressedColor = Color.FromArgb(70, 110, 200);
        btn.ForeColor = Color.White;
        btn.BorderRadius = 8;
        btn.BorderColor = Color.FromArgb(80, 80, 110);
        btn.BorderThickness = 1;
        return btn;
    }

    private void InitVlc()
    {
        Core.Initialize();
        _libVlc = new LibVLC();
        _mediaPlayer = new MediaPlayer(_libVlc);
        _videoView?.MediaPlayer = _mediaPlayer;
        _mediaPlayer.Volume = 80;

        _media = new Media(_libVlc, "");
        _mediaPlayer.Play(_media);
        _btnPlayPause?.Text = "⏸";
    }

    private void BtnPlayPause_Click(object? sender, EventArgs e)
    {
        if (_mediaPlayer is { IsPlaying: true })
        {
            _mediaPlayer.Pause();
            _btnPlayPause?.Text = "▶";
        }
        else
        {
            _mediaPlayer?.Play();
            _btnPlayPause?.Text = "⏸";
        }
    }

    private void BtnStop_Click(object? sender, EventArgs e)
    {
        Task.Run(() => _mediaPlayer?.Stop());
        _btnPlayPause?.Text = "▶";
    }

    private void BtnFullscreen_Click(object? sender, EventArgs e)
    {
        if (FormBorderStyle == FormBorderStyle.None)
        {
            FormBorderStyle = FormBorderStyle.Sizable;
            WindowState = FormWindowState.Normal;
            _btnFullscreen?.Text = "⛶";
        }
        else
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            _btnFullscreen?.Text = "✕";
        }
    }

    private void ProgressBar_MouseUp(object? sender, MouseEventArgs e)
    {
        _isDraggingProgress = false;
        if (_mediaPlayer is not { Length: > 0 }) return;
        if (_progressBar == null) return;
        var pos = _progressBar.Value / 1000f;
        _mediaPlayer.Position = pos;
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (_mediaPlayer == null || _isDraggingProgress) return;

        if (_mediaPlayer.Length <= 0) return;
        _progressBar?.Value = (int)(_mediaPlayer.Position * 1000);

        var current = TimeSpan.FromMilliseconds(_mediaPlayer.Time);
        var total = TimeSpan.FromMilliseconds(_mediaPlayer.Length);
        _lblTime?.Text = $"{current:mm\\:ss} / {total:mm\\:ss}";
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _mediaPlayer?.Stop();
        _mediaPlayer?.Dispose();
        _libVlc?.Dispose();
        base.OnFormClosed(e);
    }
}