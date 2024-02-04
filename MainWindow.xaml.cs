using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech;
using System.Speech.Synthesis;
using System.Media;

namespace MafiaManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    /*
     * The MainWindow object contains the start window code (selecting number of players and clicking start)
     * MainWindow also contains functionality shared by other windows such as TTS (through MainWindow.Speak())
    */
    public partial class MainWindow : Window
    {
        public int playercount;
        public bool doctor;
        public bool sheriff;
        public List<Player> players;
        SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
        private SoundPlayer Mafiawin;
        private SoundPlayer Townwin;


        public MainWindow()
        {
            InitializeComponent();
            Mafiawin = new SoundPlayer("./Music/19-Game-Over.wav");
            Townwin = new SoundPlayer("./Music/17-Victory-Celebration.wav");
        }

        // Called after content is rendered to the main window
        private void OnWindowLoad(object sender, EventArgs e)
        {
            SpeakAsync("Welcome to Malicious Mafia Manager! Please select the number of players to continue.");
        }

        public void Speak(string text)
        {
            speechSynthesizer.Speak(text);
        }

        public async Task SpeakAsync(string text)
        {
            await Task.Run(() => speechSynthesizer.Speak(text)); 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // NEW: get player count from slider
            playercount = Convert.ToInt32(PlayerCountSlider.Value);

            // OLD: get player count from combo box
            /*
            var selected = PlayerCountBox.SelectionBoxItem.ToString();
            int selectedInt = 0;
            int.TryParse(selected, out selectedInt);
            playercount = selectedInt;
            */

            generatePlayers();
        }

        public bool checkWin()
        {
            
            int mafia_count = 0;
            int town_count = 0;
            for (int i = 0; i < playercount; i++)
            {
                if (players[i].role == "mafia" && players[i].alive == true)
                {
                    mafia_count++;
                }
                else if (players[i].role != "mafia" && players[i].alive == true)
                {
                    town_count++;
                }
            }
            if (mafia_count == town_count)
            {
                //call end window for mafia victory
                Mafiawin.PlayLooping();
                //MessageBox.Show("MAFIA WIN");
                MafiaWinWindow winWindow = new MafiaWinWindow(this);
                winWindow.Show();
                return true;
            }
            else if (mafia_count == 0)
            {
                //call end window for town victory
                Townwin.PlayLooping();
                //MessageBox.Show("TOWNPEOPLE WIN");
                TownWinWindow winWindow = new TownWinWindow();
                winWindow.Show();
                return true;
            }
            else { return false; }
        }
        
        
        
        
        // async because we want the application to continue running while we handle getting the user input
        // in the pop-up window. genereatePlayers() runs on a different thread than the main thread
        public async Task<List<Player>> generatePlayers()
        {
            //create playerlist
            players = new List<Player>();

            //logic for assinging player roles
            for (int i = 0; i < playercount; i++)
            {
                players.Add(new Player("null", "citizen"));
            }

            int numMafia = (int)(playercount / 4.0);
            Random rand = new Random();
            for (int i = 0; i < numMafia; i++)
            {
                // assign mafia
                int indexOfNewMafia = rand.Next() % playercount;
                while (players[indexOfNewMafia].role != "citizen")
                {
                    indexOfNewMafia = rand.Next() % playercount;
                }
                players[indexOfNewMafia].role = "mafia";
            }

            if (doctor)
            {
                int indexOfNewDoctor = rand.Next() % playercount;
                while (players[indexOfNewDoctor].role != "citizen")
                {
                    indexOfNewDoctor = rand.Next() % playercount;
                }
                players[indexOfNewDoctor].role = "doctor";
            }
            if (sheriff)
            {
                int indexOfNewSheriff = rand.Next() % playercount;
                while (players[indexOfNewSheriff].role != "citizen")
                {
                    indexOfNewSheriff = rand.Next() % playercount;
                }
                players[indexOfNewSheriff].role = "sheriff";
            }

            for (int i = 0; i < playercount; i++)
            {
                // open window for getting player name
                PlayerInfoWindow newWindow = new PlayerInfoWindow();
                newWindow.Show();

                // go on to next window once newWindow.isOpen returns false
                while (newWindow.isOpen)
                {
                    // delay for some time until we check the loop condition again
                    await Task.Delay(25);
                }

                // get player name and assign to players[i]
                players[i].name = newWindow.playerName;
                MessageBox.Show($"Your name is {players[i].name} and your role is {players[i].role}.");
            }

            NightWindow nightWindow = new NightWindow(this);
            nightWindow.Show();


            return players;
        }

        // The function called when the player count slider's value is changed
        private void PlayerCountChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PlayerCountLabel.Content = $"Number of players: {PlayerCountSlider.Value}";
        }

        private void Sheriff_BoxUnchecked(object sender, RoutedEventArgs e)
        {
            sheriff = false;
        }
        private void Sheriff_BoxChecked(object sender, RoutedEventArgs e)
        {
            sheriff = true;
        }

        private void Doctor_BoxUnchecked(object sender, RoutedEventArgs e)
        {
            doctor = false;
        }
        private void Doctor_BoxChecked(object sender, RoutedEventArgs e)
        {
            doctor = true;
        }
    }
}

// ------------------- COMMENT WASTELAND --------------------------------------------------------
/*
            MessageBoxResult result;
result = MessageBox.Show("Hello World!", "Testing!", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Yes);

class PlayerCount : ObservableCollection<string>
{
    public PlayerCount()
    {
        Add("test");
    }
}

            MessageBox.Show(selectedInt.ToString());
            Application.Current.Properties["NumPlayers"] = selectedInt;

        private void NumPlayersChanged(object sender, SelectionChangedEventArgs e)
        {
            // get selection
            // set Application.Current.Properties["NumPlayers"] equal to selection value
            MessageBox.Show("Selection changed");
        }
*/