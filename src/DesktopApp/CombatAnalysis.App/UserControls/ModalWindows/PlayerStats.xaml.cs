using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CombatAnalysis.App.UserControls.ModalWindows;

/// <summary>
/// Interaction logic for PlayerStats.xaml
/// </summary>
public partial class PlayerStats : UserControl
{
    public PlayerStats()
    {
        InitializeComponent();
    }

    // IsOpen property
    public static readonly DependencyProperty IsOpenProperty =
        DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(PlayerStats), new PropertyMetadata(false));

    public bool IsOpen
    {
        get => (bool)GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    // Modal Width
    public static readonly DependencyProperty ModalWidthProperty =
        DependencyProperty.Register(nameof(ModalWidth), typeof(double), typeof(PlayerStats), new PropertyMetadata(400.0));

    public double ModalWidth
    {
        get => (double)GetValue(ModalWidthProperty);
        set => SetValue(ModalWidthProperty, value);
    }

    // Modal Height
    public static readonly DependencyProperty ModalHeightProperty =
        DependencyProperty.Register(nameof(ModalHeight), typeof(double), typeof(PlayerStats), new PropertyMetadata(300.0));

    public double ModalHeight
    {
        get => (double)GetValue(ModalHeightProperty);
        set => SetValue(ModalHeightProperty, value);
    }

    // Title
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(PlayerStats), new PropertyMetadata(string.Empty));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    // Modal content
    public static readonly DependencyProperty ContentProperty =
        DependencyProperty.Register(nameof(Content), typeof(object), typeof(PlayerStats), new PropertyMetadata(null));

    public new object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    // Overlay background
    public static readonly DependencyProperty OverlayBackgroundProperty =
        DependencyProperty.Register(nameof(OverlayBackground), typeof(Brush), typeof(PlayerStats),
            new PropertyMetadata(new SolidColorBrush(Color.FromArgb(128, 0, 0, 0))));

    public Brush OverlayBackground
    {
        get => (Brush)GetValue(OverlayBackgroundProperty);
        set => SetValue(OverlayBackgroundProperty, value);
    }

    // Modal background
    public static readonly DependencyProperty ModalBackgroundProperty =
        DependencyProperty.Register(nameof(ModalBackground), typeof(Brush), typeof(PlayerStats),
            new PropertyMetadata(Brushes.White));

    public Brush ModalBackground
    {
        get => (Brush)GetValue(ModalBackgroundProperty);
        set => SetValue(ModalBackgroundProperty, value);
    }
}
