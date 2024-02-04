using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Microsoft.Win32;
using MafiaManager;


namespace MafiaManager
{
    /// <summary>
    /// Interaction logic for NightWindow.xaml
    /// </summary>
    public partial class NightWindow : Window
    {
        private MainWindow mainWindow; // keep a reference to the main window object
        private bool clickedShow; // keeps track of when to continue o
        private SoundPlayer Sound;
        private SoundPlayer nightfall;
        private SoundPlayer death;
        private SoundPlayer doctor;



        public NightWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();

            // Here we Initialize the Sound player with the path to the embedded resource 
            Sound = new SoundPlayer("./Music/sword-taking-out-of-sheath-98778.wav");
            nightfall = new SoundPlayer("./Music/mixkit-night-crickets-near-the-swamp-1782.wav");
            death = new SoundPlayer("./Music/mixkit-horror-ambience-2482.wav");
            doctor = new SoundPlayer("./Music/healing.wav");


            // As night falls, the background music playes 

            // changed from speechSynthesizer.Speak() to mainWindow.Speak() in an effort to make
            // TTS run asynchronously
            mainWindow.SpeakAsync("And so, another night falls on the sleepy town of Eric Johnston. Don't let the mafia bite.");

            displayMessages();
        }

        /*
        * Display messages for each player and allow mafia to choose who to kill
        */
        private async Task displayMessages()
        {
            var players = mainWindow.players;
            List<string> whotokill = new List<string>();
            string whotosave = "";
            death.PlayLooping();
            for (int i = 0; i < mainWindow.playercount; i++)
            {
                clickedShow = false;
                if (players[i].alive)
                {
                    // Change message field to show player name
                    informant.Content = $"Message for {mainWindow.players[i].name}";

                    // Wait until user clicks "Show Message"
                    while (!clickedShow)
                    {
                        await Task.Delay(25);
                    }

                    // Call appropriate function depending on role
                    // e.g. "mafiaRole" or "citizenRole"
                    if (players[i].role == "mafia")
                    {
                        Task<string> task = mafiaRole(players);

                        // store who was chosen by mafia in whotokill list
                        string kill = await task;
                        whotokill.Add(kill);
                    }
                    else if (players[i].role == "citizen")
                    {
                        citizenRole();
                    }
                    else if (players[i].role == "doctor")
                    {
                        Task<string> task = doctorRole(players);

                        whotosave = await task;
                    }
                    else if (players[i].role == "sheriff")
                    {
                        Task<string> task = sheriffRole(players);
                        string nullstring = await task;
                    }
                }
            }

            death.Stop();
            // Handle killing of selected player (choose from whotokill list)
            Random rand = new Random();
            int index = rand.Next(0, whotokill.Count);
            string nameofdead = whotokill[index];

            if (nameofdead != whotosave)
            {
                for (int i = 0; i < mainWindow.playercount; i++)
                {
                    if (players[i].name == nameofdead)
                    { players[i].alive = false; }
                }
                // When the button is pushed the sound should be played 
                Sound.Play();
                // changed from speechSynthesizer.Speak() to mainWindow.Speak() in an effort to make
                // TTS run asynchronously
                this.Close();
                mainWindow.Speak($"It appears that in the night, {nameofdead} was murdered!");
            }
            else if (nameofdead == whotosave)
            {
                doctor.Play();
                this.Close();
                mainWindow.Speak($"{nameofdead} died. Or they would have, if they hadn't been saved at the last moment by the doctor!");
                doctor.Stop();
            }

            if (mainWindow.checkWin())
            {
                return;
            }

            DayWindow dayWindow = new DayWindow(mainWindow);
            dayWindow.Show();

        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            clickedShow = true;
        }


        private async Task<string> mafiaRole(List<Player> players)
        {
            // we will return playername, the name of the player we choose to kill
            string playername = "";

            // create new KillWindow
            KillWindow killWindow = new KillWindow("mafia");

            // populate killWindow combobox
            for (int i = 0; i < mainWindow.playercount; i++)
            {
                if (players[i].role != "mafia" && players[i].alive)
                {

                    ComboBoxItem newItem = new ComboBoxItem();
                    newItem.Content = players[i].name;
                    killWindow.comboBox.Items.Add(newItem);
                }
            }

            // Show kill window and wait for user to make their selection
            killWindow.Show();
            while (killWindow.windowIsOpen)
            {
                await Task.Delay(25);
            }

            // return kill choice (playername)
            playername = killWindow.comboBox.SelectionBoxItem.ToString();
            return playername;
        }
        private async Task<string> doctorRole(List<Player> players)
        {
            // we will return playername, the name of the player we choose to save
            string playername = "";

            // create new KillWindow(that is used to save! Code reuse!)
            KillWindow killWindow = new KillWindow("doctor");

            // populate killWindow combobox
            for (int i = 0; i < mainWindow.playercount; i++)
            {
                if (players[i].alive)
                {

                    ComboBoxItem newItem = new ComboBoxItem();
                    newItem.Content = players[i].name;
                    killWindow.comboBox.Items.Add(newItem);
                }
            }

            // Show window and wait for user to make their selection
            killWindow.Show();
            while (killWindow.windowIsOpen)
            {
                await Task.Delay(25);
            }

            // return save choice (playername)
            playername = killWindow.comboBox.SelectionBoxItem.ToString();
            return playername;
        }

        private async Task<string> sheriffRole(List<Player> players)
        {
            // create new KillWindow(that is used to investigate! Code reuse again!)
            KillWindow killWindow = new KillWindow("sheriff");

            // populate killWindow combobox
            for (int i = 0; i < mainWindow.playercount; i++)
            {
                if (players[i].role != "sheriff")
                {

                    ComboBoxItem newItem = new ComboBoxItem();
                    newItem.Content = players[i].name;
                    killWindow.comboBox.Items.Add(newItem);
                }
            }

            // Show window and wait for user to make their selection
            killWindow.Show();
            while (killWindow.windowIsOpen)
            {
                await Task.Delay(25);
            }

            // we will check the role of the player whose name is selected
            string playername = "";
            playername = killWindow.comboBox.SelectionBoxItem.ToString();

            for (int i = 0; i < mainWindow.playercount; i++)
            {
                if (players[i].name == playername)
                {
                    MessageBox.Show($"{playername}'s role is {players[i].role}. Crazy!");
                }
            }
            return "done";
        }
        void citizenRole()
        {
            MessageBox.Show("You fear for your life, but unable to do anything about it you decide to sleep.", "Darkness falls on the sleepy town of EJ");
        }
    }
}