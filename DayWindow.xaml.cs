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
using System.Speech;
using System.Speech.Synthesis;
using System.Media;

namespace MafiaManager
{
    /// <summary>
    /// Interaction logic for DayWindow.xaml
    /// </summary>
    /// 
    /*
     * Window for handling the day game logic (voting who to kill off)
    */
    public partial class DayWindow : Window
    {
        MainWindow mainWindow;
        SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
        public bool clickedVote;
        private SoundPlayer Morning;
        private SoundPlayer Background;
        private SoundPlayer Scream; 

        public DayWindow(MainWindow _mainWindow)
        {
            InitializeComponent();
            Morning = new SoundPlayer("./Music/rooster_crowing-7027.wav");
            Background = new SoundPlayer("./Music/price-of-freedom-33106.wav");
            Scream = new SoundPlayer("./Music/Wilhelm-Scream.wav");

            this.mainWindow = _mainWindow;

            mainWindow.Speak("Another day has arrived, and the townspeople of EJ attempt to discover who the killer is.");
            displayMessages();
        }



        private async Task displayMessages()
        {
            // this pulls in the player list, then creates a combobox to select who they vote to eliminate.
            // since the box is the same for everyone we just make it once and then just show it to each person
            var players = mainWindow.players;
            List<string> whotolynch = new List<string>();
            Background.PlayLooping();
            for (int j = 0; j < mainWindow.playercount; j++)
            {
                if (players[j].alive)
                {
                    // populate combo box
                    ComboBoxItem newItem = new ComboBoxItem();
                    newItem.Content = players[j].name;
                    voteBox.Items.Add(newItem);
                }
            }
            ComboBoxItem novote = new ComboBoxItem();
            novote.Content = "No one";
            voteBox.Items.Add(novote);
            //asks each player to vote for who to eliminate and stores the choice
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].alive)
                {
                    clickedVote = false;
                    MessageBox.Show($"If {players[i].name} could please vote for who to eliminate.");

                    // wait for user to click vote
                    while (!clickedVote)
                    {
                        await Task.Delay(25);
                    }

                    // get content of vote box, store in lynch
                    string lynch = voteBox.SelectionBoxItem.ToString();
                    whotolynch.Add(lynch);
                    voteBox.SelectedIndex = -1;
                    voteBox.Text = "";
                }
            }
            //handle the voting results here
            //I'm sure there's a better way to do this, but I couldn't get them working
            //this shoves them into group objects that can be counted to see how often they occur
           
            var groupings = whotolynch.GroupBy(x => x);

            //stores the choice with the most votes and the choice with the second most votes
            //if the first and second place have the same number of votes, we know there was a tie and we don't eliminate anyone
            string winner = "";
            int winner_count = 0;
            string runup = "";
            int runup_count = 0;

            //just logic to handle the place where it gets stored
            foreach (var group in groupings)
            {
                if (group.Count() > winner_count)
                {
                    runup = winner;
                    runup_count = winner_count;

                    winner = group.Key;
                    winner_count = group.Count();
                }
                else if (group.Count() > runup_count)
                {
                    runup = group.Key;
                    runup_count = group.Count();
                }
            }

            Background.Stop();
            //since these two results have the same outcome, we just OR them together
            if (winner_count == runup_count || winner == "No one")
            {
                mainWindow.Speak("You have not agreed on a person to kill, therefore no one is eliminated.");
                this.Close();

                NightWindow nightWindow = new NightWindow(mainWindow);
                nightWindow.Show();
            }
            else
            {       
                Scream.Play();
                mainWindow.Speak($"You have voted to eliminate to {winner}, they are publically executed for their alleged crimes.");
                this.Close();
        
                for (int i = 0; i < mainWindow.playercount; i++)
                {
                    if (players[i].name == winner)
                    { players[i].alive = false; }
                }
                Scream.Stop();

                if (mainWindow.checkWin())
                {
                    return;
                }

                NightWindow nightWindow = new NightWindow(mainWindow);
                nightWindow.Show();
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            clickedVote = true;
        }

    }
}