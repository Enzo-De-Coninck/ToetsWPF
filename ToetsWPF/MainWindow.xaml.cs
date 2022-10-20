using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.ComponentModel;
using System.IO;

namespace ToetsWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Nieuw();
            foreach(PropertyInfo info in typeof(Colors).GetProperties())
            {
                BrushConverter bc = new BrushConverter();
                SolidColorBrush deKleur = (SolidColorBrush)bc.ConvertFromString(info.Name);
                Kleur kleurke = new Kleur();
                kleurke.Borstel = deKleur;
                kleurke.Naam = info.Name;
                kleurke.Hex = deKleur.ToString();
                kleurke.Rood = deKleur.Color.R;
                kleurke.Groen = deKleur.Color.G;
                kleurke.Blauw = deKleur.Color.B;
                ComboBoxKleuren.Items.Add(kleurke);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBoxLettertype.Items.SortDescriptions.Add(new SortDescription("Source", ListSortDirection.Ascending));
            ComboBoxLettertype.SelectedItem = new FontFamily(TextBoxWens.FontFamily.ToString());
        }

        private void VergrootButton_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxWens.FontSize < 40)
            {
                TextBoxWens.FontSize++;
            }
        }

        private void VerkleinButton_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxWens.FontSize > 10)
            {
                TextBoxWens.FontSize--;
            }
        }

        private void Kerst_Click(object sender, RoutedEventArgs e)
        {
            DeStackPanel.Visibility = Visibility.Visible;
            String stringPath = @"C:\Users\User\Desktop\C#\ToetsWPF\ToetsWPF\images\kerstkaart.jpg";
            Uri imageUri = new Uri(stringPath, UriKind.Relative);
            BitmapImage imageBitmap = new BitmapImage(imageUri);
            deAchtergrond.ImageSource = imageBitmap;
        }

        private void Geboorte_Click(object sender, RoutedEventArgs e)
        {
            DeStackPanel.Visibility = Visibility.Visible;
            String stringPath = @"C:\Users\User\Desktop\C#\ToetsWPF\ToetsWPF\images\geboortekaart.jpg";
            Uri imageUri = new Uri(stringPath, UriKind.Relative);
            BitmapImage imageBitmap = new BitmapImage(imageUri);
            deAchtergrond.ImageSource = imageBitmap;
        }

        private void Nieuw()
        {
            DeStackPanel.Visibility = Visibility.Hidden;
            KaartAfdrukken.IsEnabled = false;
            KaartOpslaan.IsEnabled = false;
            
        }

        private void NewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Nieuw();
            Bestandslocatie.Content = "nieuw";
        }

        private void CloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Wilt u het programma afsluiten ?", "Afsluiten", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
                e.Cancel = true;
        }
    }
}
