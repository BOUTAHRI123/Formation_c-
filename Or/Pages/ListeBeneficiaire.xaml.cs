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

namespace Or.Pages
{
    /// <summary>
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class ListeBeneficiaire : PageFunction<long>
    {
        private long CarteId;
        public ListeBeneficiaire(long numCarte)
        {
            InitializeComponent();
            CarteId = numCarte;
            Donnees();


        }


        public void Donnees()
        {

        }
         private void Retour_Click(object sender, RoutedEventArgs e)
        {
            OnReturn(null);
        }
        public void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //GridView gridView = listView.View as GridView;
            //if (gridView != null)
            //{
               // double totalWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;
               // gridView.Columns[0].Width = totalWidth * 0.10; // 10%
               // gridView.Columns[1].Width = totalWidth * 0.45; // 40%
               // gridView.Columns[2].Width = totalWidth * 0.45; // 20%
            //}
        }
    }
}
