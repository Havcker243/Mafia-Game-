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
    /// Interaction logic for MafiaWinWindow.xaml
    /// </summary>
    public partial class MafiaWinWindow : Window
    {
        private MainWindow mainWindow;

        public MafiaWinWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            MafiaWereBox.Text = "The mafia were: ";
            int numMafia = (int)(mainWindow.playercount / 4.0);
            int counter = 0;
            for (int i = 0; i < mainWindow.playercount; i++)
            {
                if (mainWindow.players[i].role == "mafia")
                {
                    if (counter != 0) // don't add a comma for first mafia
                    {
                        MafiaWereBox.Text += ", ";
                    }

                    MafiaWereBox.Text += mainWindow.players[i].name;
                    counter++;
                }
            }
        }
    }
}
