using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace RINGTONEDEMOFINALMIX123dfghfw
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> files = new List<string>();
        private MediaPlayer player = new MediaPlayer();
        private int currentTrackIndex = -1;
        private DispatcherTimer timer = new DispatcherTimer();
        private bool isReplayAction = false;


        public MainWindow()
        {
            InitializeComponent();

            player.MediaEnded += MediaPlayer_MediaEnded;
            player.MediaOpened += MediaPlayer_MediaOpened;

            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (player.Source != null && player.NaturalDuration.HasTimeSpan)
            {
                slider.Value = player.Position.TotalMilliseconds;

                TimeSpan currentPosition = player.Position;
                current_time.Text = $"{currentPosition.Hours:D2}:{currentPosition.Minutes:D2}:{currentPosition.Seconds:D2}";
            }
        }



        private void MediaPlayer_MediaOpened(object sender, EventArgs e)
        {
            if (player.NaturalDuration.HasTimeSpan)
            {
                slider.Minimum = 0;
                slider.Maximum = player.NaturalDuration.TimeSpan.TotalMilliseconds;
                slider.TickFrequency = 1;
                slider.IsSnapToTickEnabled = true;
                slider.ValueChanged -= slider_ValueChanged;
                slider.ValueChanged += slider_ValueChanged;

                timer.Start();

                TimeSpan totalDuration = player.NaturalDuration.TimeSpan;
                music_duration.Text = $"{totalDuration.Hours:D2}:{totalDuration.Minutes:D2}:{totalDuration.Seconds:D2}";
            }
        }



        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.Position = TimeSpan.FromMilliseconds(slider.Value);
        }
        private void volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.Volume = volume.Value / 100f;
        }




        private void Open_Button(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog fileDialog = new CommonOpenFileDialog
            {
                Multiselect = true,
                DefaultExtension = ".mp3"
            };
            fileDialog.Filters.Add(new CommonFileDialogFilter("MP3 and FLAC files", "*.mp3;*.flac"));
            var result = fileDialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                foreach (string filename in fileDialog.FileNames)
                {
                    listbox.Items.Add(System.IO.Path.GetFileName(filename));
                    files.Add(filename);
                }

                currentTrackIndex = 0;
                PlaySong();
            }
        }
        
        private void Back_Button(object sender, RoutedEventArgs e)
        {
            int index = currentTrackIndex;

            if (index == 0)
            {
                index = files.Count - 1;
            }
            else
            {
                index--;
            }

            currentTrackIndex = index;
            PlaySong();
        }

        private void Next_Button(object sender, RoutedEventArgs e)
        {
            int index = currentTrackIndex;

            if (index == files.Count - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }

            currentTrackIndex = index;
            PlaySong();
        }
        private void Play_Button(object sender, RoutedEventArgs e)
        {
            int index = listbox.SelectedIndex;
            if (index >= 0)
            {
                currentTrackIndex = index;
                PlaySong();
            }
        }

        private void Pause_Button(object sender, RoutedEventArgs e)
        {
            player.Pause();
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            if (isReplayAction)
            {
                player.Position = TimeSpan.Zero;
            }
            else if (currentTrackIndex < files.Count - 1)
            {
                currentTrackIndex++;
                PlaySong();
            }
        }

        private void Repeat_Button(object sender, RoutedEventArgs e)
        {
            isReplayAction = !isReplayAction;
            if (isReplayAction)
            {
                ReplayButton.Label = "Повтор вкл.";
            }
            else
            {
                ReplayButton.Label = "Повтор выкл.";
            }
        }

        private void PlaySong()
        {
            if (currentTrackIndex >= 0 && currentTrackIndex < files.Count)
            {
                listbox.SelectedIndex = currentTrackIndex;
                if (isReplayAction && player.Position == TimeSpan.Zero && currentTrackIndex != 0)
                {
                    currentTrackIndex--;
                }
                else
                {
                    player.Open(new Uri(files[currentTrackIndex]));
                    player.Play();
                    slider.Value = 0;
                }
            }
        }

        private void Label_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("https://www.youtube.com/watch?v=itJ_DJVKAW0") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine("x");
            }
        }

        private void History_Button(object sender, RoutedEventArgs e)
        {

        }
    }

}
