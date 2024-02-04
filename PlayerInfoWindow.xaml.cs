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
using System.Windows.Shapes;

namespace MafiaManager
{
    /// <summary>
    /// Interaction logic for popup_window.xaml
    /// Pops up and asks for the user to input their name, then returns the name so it can be added to the list
    /// </summary>
    public partial class PlayerInfoWindow : Window
    {
        public bool isOpen { get; set; }
        public string playerName;


        public PlayerInfoWindow()
        {
            InitializeComponent();
            isOpen = true;
        }
        public void closewindow(object sender, RoutedEventArgs e)
        {
            playerName = textBox.Text;
            isOpen = false;
            this.Close();
        }
        private void clicked(object sender, KeyboardFocusChangedEventArgs e)
        {
            textBox.Text = "";
        }
    }
}
