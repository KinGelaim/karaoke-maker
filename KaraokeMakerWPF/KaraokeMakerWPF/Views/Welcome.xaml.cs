using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KaraokeMakerWPF.Views;

/// <summary>
/// Логика взаимодействия для Welcome.xaml
/// </summary>
public partial class Welcome : UserControl
{
    public static readonly DependencyProperty StartButtonProperty =
        DependencyProperty.Register(
            "StartButton",
            typeof(ICommand),
            typeof(Welcome),
            new UIPropertyMetadata(null));

    public ICommand StartButton
    {
        get { return (ICommand)GetValue(StartButtonProperty); }
        set { SetValue(StartButtonProperty, value); }
    }

    public Welcome()
    {
        InitializeComponent();
    }
}