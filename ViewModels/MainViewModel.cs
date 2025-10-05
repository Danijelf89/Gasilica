using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Gasilica.ViewModels
{
    public class MainViewModel : BaseViewModel, IDataErrorInfo
    {
        private DispatcherTimer countdownTimer;
        private TimeSpan remainingTime;
        private MainWindow _window;

        public MainViewModel(MainWindow window)
        {
            _window = window;
        }

        public string _hours;
        public string Hours
        {
            get
            {
                return _hours;
            }
            set
            {
                //if (string.IsNullOrEmpty(value))
                //{
                //    _hours = "0";
                //}

                //else
                //{
                    _hours = value;
                //}
                OnPropertyChanged();
                OnPropertyChanged("Minutes");

                if (int.TryParse(Hours.ToString(), out int parsed))
                {
                    ConvertedHours = parsed;
                }
            }
        }

        public bool HasErrors()
        {
            // Ovde nabroji property-je koje želiš da proveriš
            string[] propertiesToValidate = { nameof(Hours), nameof(Minutes) };

            foreach (string property in propertiesToValidate)
            {
                string error = this[property];
                if (!string.IsNullOrEmpty(error))
                    return true;
            }

            return false;
        }


        public int _convertedHours;
        public int ConvertedHours
        {
            get
            {
                return _convertedHours;
            }
            set
            {
                _convertedHours = value;
                OnPropertyChanged();
            }
        }

        public string _minutes;
        public string Minutes
        {
            get
            {
                return _minutes;
            }
            set
            {
                //if(string.IsNullOrEmpty(value))
                //{
                //    _minutes = "0";
                //}

                //else
                //{
                    _minutes = value;
                //}
               

                if (int.TryParse(Minutes.ToString(), out int parsed))
                {
                    ConvertedMinutes = parsed;
                }

                OnPropertyChanged();
                OnPropertyChanged("Hours");
            }
        }



        public int _convertedMinutes;
        public int ConvertedMinutes
        {
            get
            {
                return _convertedMinutes;
            }
            set
            {
                _convertedMinutes = value;
                OnPropertyChanged();
            }
        }

        public void Start()
        {
            remainingTime = new TimeSpan(ConvertedHours, ConvertedMinutes, 0);

            if (HasErrors())
            {
                MessageBox.Show("Unesite validno vreme!");
                return;
            }

            // Prikaži preostalo vreme odmah
            //UpdateCountdownLabel();

            // Postavi timer da "otkucava" svake sekunde
            countdownTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            countdownTimer.Tick += CountdownTimer_Tick;
            countdownTimer.Start();

          
        }

        public string Error => null; // Ne koristi se često

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Hours):
                        

                        if (!string.IsNullOrEmpty(Hours) &&  !int.TryParse(Hours.ToString(), out int parsed))
                            return "Dozvoljeni su samo brojevi.";

                        if ((string.IsNullOrEmpty(Hours) || Hours == "0") && (string.IsNullOrEmpty(Minutes) || Minutes == "0"))
                            return "Sati ili minute moraj da se unesu.";
                        break;
                    case nameof(Minutes):


                        if (!string.IsNullOrEmpty(Minutes) && !int.TryParse(Minutes.ToString(), out int parsedMiin))
                            return "Dozvoljeni su samo brojevi.";
                        if ((string.IsNullOrEmpty(Hours) || Hours == "0") && (string.IsNullOrEmpty(Minutes) || Minutes == "0"))
                            return "Sati ili minute moraj da se unesu.";
                        break;
                }



                return null;
            }
        }

        bool soundOn = false;

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if(soundOn)
            {
                SystemSounds.Beep.Play();
            }
           
            if (remainingTime.TotalSeconds > 0)
            {
                remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
                //UpdateCountdownLabel();
            }
            // Sada ažuriraj UI na main threadu
            _window.Dispatcher.Invoke(() =>
            {
                _window.OdborjavanjeTxt.Text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                    remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);

                // Menjaj boju teksta ako je manje od 30 sekundi
                if (remainingTime.TotalSeconds <= 30)
                {
                    if(_window.Toogle.IsChecked == true)
                    {
                        soundOn = true;
                    }
                    else
                    {
                        soundOn = false;
                    }

                        _window.OdborjavanjeTxt.Foreground = Brushes.Red;
                }
            });

            // Ako je vreme isteklo – zaustavi timer i ugasi računar
            if (remainingTime.TotalSeconds <= 0)
            {
                countdownTimer.Stop();
                ShutdownComputer();
            }
        }

        public void Stop()
        {
            countdownTimer.Stop();
        }

        private void ShutdownComputer()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "shutdown",
                    Arguments = "/s /t 0",
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška prilikom gašenja: " + ex.Message);
            }
        }
    }
}
