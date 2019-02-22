using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExampleApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ContinueButton_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MapPage();
        }

        private void ChooseButton_OnClicked(object sender, EventArgs e)
        {
            
        }
    }
}
