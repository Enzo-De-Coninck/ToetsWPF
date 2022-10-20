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
using Microsoft.Win32;

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
            KaartOpslaan.IsEnabled = true;
            KaartAfdrukken.IsEnabled = true;
        }

        private void Geboorte_Click(object sender, RoutedEventArgs e)
        {
            DeStackPanel.Visibility = Visibility.Visible;
            String stringPath = @"C:\Users\User\Desktop\C#\ToetsWPF\ToetsWPF\images\geboortekaart.jpg";
            Uri imageUri = new Uri(stringPath, UriKind.Relative);
            BitmapImage imageBitmap = new BitmapImage(imageUri);
            deAchtergrond.ImageSource = imageBitmap;
            KaartOpslaan.IsEnabled = true;
            KaartAfdrukken.IsEnabled = true;
        }

        private void Nieuw()
        {
            DeStackPanel.Visibility = Visibility.Hidden;
            KaartAfdrukken.IsEnabled = false;
            KaartOpslaan.IsEnabled = false;
            ComboBoxKleuren.SelectedIndex = 0;
            ComboBoxLettertype.SelectedIndex = 0;
            
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

        private Ellipse sleepbal = new Ellipse();
        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            sleepbal = (Ellipse)sender;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject sleepKleur = new DataObject("deKleur", sleepbal.Fill);
                DragDrop.DoDragDrop(sleepbal, sleepKleur, DragDropEffects.Move);
            }
        }

        
        private void Ellipse_Drop(object sender, DragEventArgs e)
        {
            
            if (e.Data.GetDataPresent("deKleur"))
            {

                Brush gesleepteKleur = (Brush)e.Data.GetData("deKleur");
                Ellipse bal = new Ellipse();
                bal.Fill = gesleepteKleur;
                bal.Height = 40;
                bal.Width = 40;
                bal.Stroke = Brushes.Black;
                bal.StrokeThickness = 5;
                Point punt = e.GetPosition(deCanvas);
                double posX = punt.X -20;
                double posY = punt.Y -20;
                Canvas.SetLeft(bal, posX);
                Canvas.SetTop(bal, posY);
                deCanvas.Children.Add(bal);
            }
        }

        private void OpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.FileName = "Wenskaart";
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Wenskaarten |*.txt";
                if (dlg.ShowDialog() == true)
                {
                    using (StreamReader bestand = new StreamReader(dlg.FileName))
                    {
                        deAchtergrond.ImageSource = new ImageSourceConverter().ConvertFromString(bestand.ReadLine()) as ImageSource;
                        int tijdelijk = int.Parse(bestand.ReadLine());
                        for (int i = 0; i < tijdelijk; i++)
                        {
                            Ellipse bol = new Ellipse();
                            bol.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(bestand.ReadLine());
                            double canvasLeft = Convert.ToDouble(bestand.ReadLine());
                            double canvasTop = Convert.ToDouble(bestand.ReadLine());
                            Canvas.SetLeft(bol, canvasLeft);
                            Canvas.SetTop(bol, canvasTop);
                        }
                        TextBoxWens.Text = Console.ReadLine();
                        TypeConverter convertLettertype = TypeDescriptor.GetConverter(typeof(FontFamily));
                        TextBoxWens.FontFamily = (FontFamily)convertLettertype.ConvertFromString(bestand.ReadLine());
                        TextBoxWens.FontSize = Convert.ToDouble(bestand.ReadLine());    

                        Bestandslocatie.Content = System.IO.Path.GetFullPath(dlg.FileName).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("openen mislukt: " + ex.Message);
            }
        }

        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = "Wenskaart";
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Wenskaarten |*.txt";
                if (dlg.ShowDialog() == true)
                {
                    using (StreamWriter bestand = new StreamWriter(dlg.FileName))
                    {
                        bestand.WriteLine(deAchtergrond.ImageSource.ToString());
                        bestand.WriteLine(deCanvas.Children.Count);
                        foreach (Ellipse bol in deCanvas.Children)
                        {
                            bestand.WriteLine(bol.Fill.ToString());
                            bestand.WriteLine(Canvas.GetLeft(bol));
                            bestand.WriteLine(Canvas.GetTop(bol));
                        }
                        bestand.WriteLine(TextBoxWens.Text.ToString());
                        bestand.WriteLine(TextBoxWens.FontFamily.ToString());
                        bestand.WriteLine(TextBoxWens.FontSize.ToString());
                        Bestandslocatie.Content = System.IO.Path.GetFullPath(dlg.FileName).ToString();
                    }
                }
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("opslaan mislukt: " + ex.Message);
            }
        }
    }
}
