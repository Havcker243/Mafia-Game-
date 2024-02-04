using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Numerics;
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
    /// Interaction logic for KillWindow.xaml
    /// </summary>
    /// 
    /*
     * Window that pops up when mafia click "Show Message"
     * Contains dropdown that allows mafia to vote for who to kill
    */
    public partial class KillWindow : Window
    {
        public bool windowIsOpen = true;
        public KillWindow(string role)
        {
            InitializeComponent();
            //I realized that this type of box was used for all the roles, so I just dynamically change the window based on what role they have
            //It defaults to "kill", but will swap to "save" for doctor and "investigate" for sheriff
            if (role == "doctor")
            {
                confirm.Content = "Save";
            }
            else if (role == "sheriff")
            {
                confirm.Content = "Investigate";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            windowIsOpen = false;
            this.Close();
        }
    }
}
