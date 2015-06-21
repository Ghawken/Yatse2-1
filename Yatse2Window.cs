﻿// ------------------------------------------------------------------------
//    YATSE 2 - A touch screen remote controller for XBMC (.NET 3.5)
//    Copyright (C) 2010  Tolriq (http://yatse.leetzone.org)
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Plugin;
using Yatse2.Classes;
using Yatse2.Libs;
using Setup;
using System.Windows.Automation.Peers;
using System.Net.Sockets;


namespace Yatse2
{



    public partial class Yatse2Window : IDisposable
    {
        private const string Repository = @"http://yatse.leetzone.org/repository";
        private bool _allowBeta;
        private readonly Yatse2Config _config = new Yatse2Config();
        private readonly string _configFile = Helper.AppPath + @"Yatse2.xml";
        private readonly Yatse2DB _database = new Yatse2DB();
        private readonly Weather _weather = new Weather();
        private long _timerScreenSaver = 120;
        private bool _startLetterDrag;
        private Point _mouseDownPoint;
        private DateTime _mouseDownTime;

        public delegate void UpdateTextCallback(string message);

        private readonly Hashtable _yatse2Pages = new Hashtable
                            {
                                {"Home", "grd_Home"},
                                {"Movies", "grd_Movies"},
                                {"Tv Shows", "grd_TvShows"},
                                {"Music Artists", "grd_AudioArtists"},
                                {"Music Album", "grd_AudioAlbums"},
                                {"Music Genres", "grd_AudioGenres"},
                                {"Weather", "grd_Weather"}
                            };
        private readonly Hashtable _yatse2PlayModes = new Hashtable
                            {
                                {0, 139},
                                {1, 140}
                            };

        private bool _videoFavoritesFilter;
        private bool _audioFavoritesFilter;
        private bool _failedRemoteCheck;
        private bool _updatecheck;
        private Yatse2Properties _yatse2Properties;
        private MoviesCollection _moviesDataSource;
        private CollectionView _moviesCollectionView;
        private TvShowsCollection _tvShowsDataSource;
        private CollectionView _tvShowsCollectionView;
        private TvSeasonsCollection _tvSeasonsDataSource;
        private CollectionView _tvSeasonsCollectionView;
        private TvEpisodesCollection _tvEpisodesDataSource;
        private CollectionView _tvEpisodesCollectionView;
        private AudioAlbumsCollection _audioAlbumsDataSource;
        private CollectionView _audioAlbumsCollectionView;
        private AudioArtistsCollection _audioArtistsDataSource;
        private CollectionView _audioArtistsCollectionView;
        private AudioGenresCollection _audioGenresDataSource;
        private CollectionView _audioGenresCollectionView;
        private AudioSongsCollection _audioSongsDataSource;
        private System.Windows.Forms.NotifyIcon ni = null;

        private readonly Collection<string> _gridHistory = new Collection<string>();
        private long _currentRemoteId;
        private ApiConnection _remote;
        private IYatse2RemotePlugin _remotePlugin;
        private bool _remoteConnected;
        private bool _remoteLibraryRefreshed;
        private Yatse2Remote _remoteInfo;
        private Yatse2Remote _remoteInfoEdit;
        private Grid _currentGrid;
        private long _timer;
        private long _timerHeader;
        private bool _isScreenSaver;
        private bool _isfanart;
        private int _diaporamaCurrentImage;
        private int _fanartCurrentImage;
        private bool _showRemoteSelect;
        private bool _showHomePage;
        private bool _isPlaying;
        private bool _disableFocus;

        private string _filterMovie = "";
        private string _filterTvShow = "";
        private string _filterAudioGenre = "";
        private string _filterAudioAlbum = "";
        private string _filterAudioArtist = "";
        private bool _filteredArtists;
        private bool _filteredAlbums;
        private bool _setPov;


        public string GetLocalizedString(int id)
        {
            var ret = (string)TryFindResource("Localized_" + id);
            return ret ?? "Non localized string";
        }

        private void StartDiaporama()
        {
            switch (_config.DiaporamaMode)
            {
                case 0:
                    img_Diaporama1.Stretch = Stretch.None;
                    img_Diaporama2.Stretch = Stretch.None;
                    break;
                case 1:
                    img_Diaporama1.Stretch = Stretch.Uniform;
                    img_Diaporama2.Stretch = Stretch.Uniform;
                    break;
                case 2:
                    img_Diaporama1.Stretch = Stretch.UniformToFill;
                    img_Diaporama2.Stretch = Stretch.UniformToFill;
                    break;
                case 3:
                    img_Diaporama1.Stretch = Stretch.Fill;
                    img_Diaporama2.Stretch = Stretch.Fill;
                    break;
            }

            _yatse2Properties.DiaporamaImage1 = GetRandomImagePath(_config.ImageDirectory);
            _diaporamaCurrentImage = 1;
            var stbDiaporamaShow = (Storyboard)TryFindResource("stb_ShowDiaporama");
            if (stbDiaporamaShow != null)
            {
                stbDiaporamaShow.Begin(this);
                _isScreenSaver = true;
            }
        }

        private void SwitchDiaporama()
        {
            if (_diaporamaCurrentImage == 1)
            {
                _diaporamaCurrentImage = 2;
                _yatse2Properties.DiaporamaImage2 = GetRandomImagePath(_config.ImageDirectory);
                var stbDiaporamaSwap = (Storyboard)TryFindResource("stb_Diaporama_12_1");
                if (stbDiaporamaSwap != null)
                    stbDiaporamaSwap.Begin(this);
            }
            else
            {
                _diaporamaCurrentImage = 1;
                _yatse2Properties.DiaporamaImage1 = GetRandomImagePath(_config.ImageDirectory);
                var stbDiaporamaSwap = (Storyboard)TryFindResource("stb_Diaporama_21_1");
                if (stbDiaporamaSwap != null)
                    stbDiaporamaSwap.Begin(this);
            }
        }
        private void StartFanart()
        {
            
            switch (_config.DiaporamaMode)
            {
                case 0:
                    img_Diaporama1.Stretch = Stretch.None;
                    img_Diaporama2.Stretch = Stretch.None;
                    break;
                case 1:
                    img_Diaporama1.Stretch = Stretch.Uniform;
                    img_Diaporama2.Stretch = Stretch.Uniform;
                    break;
                case 2:
                    img_Diaporama1.Stretch = Stretch.UniformToFill;
                    img_Diaporama2.Stretch = Stretch.UniformToFill;
                    break;
                case 3:
                    img_Diaporama1.Stretch = Stretch.Fill;
                    img_Diaporama2.Stretch = Stretch.Fill;
                    break;
            }
          //  var appdatadirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
          //  _config.FanartDirectory = appdatadirectory + @"\Kodi\userdata\addon_data\script.artworkorganizer\";
         
       //     Logger.Instance().Log("FanART DEBUG", "Fanart Directory equals " + _config.FanartDirectory, true);
            



            _yatse2Properties.DiaporamaImage1 = GetRandomImagePathNew(_config.FanartDirectory);
            if (_yatse2Properties.DiaporamaImage1 == null)
            {
                //isfanart = false;
                var stbDiaporamaHide = (Storyboard)TryFindResource("stb_HideDiaporama");
                if (stbDiaporamaHide != null)
                {
                    stbDiaporamaHide.Begin(this);
                }
                return;
            }



            _fanartCurrentImage = 1;
            var stbDiaporamaShow = (Storyboard)TryFindResource("stb_ShowDiaporama");
            if (stbDiaporamaShow != null)
            {
                stbDiaporamaShow.Begin(this);
                _isfanart = true;
                _isScreenSaver = true;
            }
        }

        private void SwitchFanart()
        {
           /*( if (grd_Diaporama.Visibility == Visibility.Hidden )
            {
                var stbDiaporamaShow = (Storyboard)TryFindResource("stb_ShowDiaporama");
                if (stbDiaporamaShow != null)
                {
                    stbDiaporamaShow.Begin(this);
                }
            }*/
            
            if (_fanartCurrentImage == 1)
            {
                _fanartCurrentImage = 2;
                _yatse2Properties.DiaporamaImage2 = GetRandomImagePathNew(_config.FanartDirectory);
                var stbDiaporamaSwap = (Storyboard)TryFindResource("stb_Diaporama_12_1");
                if (stbDiaporamaSwap != null)
                    stbDiaporamaSwap.Begin(this);
            }
            else
            {
                _fanartCurrentImage = 1;
                _yatse2Properties.DiaporamaImage1 = GetRandomImagePathNew(_config.FanartDirectory);
                var stbDiaporamaSwap = (Storyboard)TryFindResource("stb_Diaporama_21_1");
                if (stbDiaporamaSwap != null)
                    stbDiaporamaSwap.Begin(this);
            }
        }
        private void InitDatabase()
        {            
            _database.SetDebug(_config.Debug);
            _database.Open(null,_config.IgnoreSortTokens,_config.SortTokens);
            
            var check = _database.CheckDBVersion();
            if (check == 1) return;
            if (check == 0)
            {
                Logger.Instance().Log("Yatse2", "Database Create");
                _database.CreateDatabase();
                return;
            }
            Logger.Instance().Log("Yatse2", "Database Update");
            _database.UpdateDatabase();
        }

        private void InitTimer()
        {
            Logger.Instance().Log("Yatse2", "Init Timer");
            var dispatchTimer = new DispatcherTimer(DispatcherPriority.Normal);
            dispatchTimer.Tick += Timer_Tick;
            dispatchTimer.Interval = new TimeSpan(0, 0, 1);
            dispatchTimer.Start();
        }

        private static bool GetUpdater()
        {
            using (var client = new WebClient())
            {
                try
                {
                    if (File.Exists(Helper.AppPath + @"Temp\Yatse2Setup.exe"))
                        File.Delete(Helper.AppPath + @"Temp\Yatse2Setup.exe");
                    client.DownloadFile(Repository + "/Download/File/Yatse2Setup.exe",
                                        Helper.AppPath + @"Temp\Yatse2Setup.exe");
                }
                catch (WebException)
                {
                    return false;
                }
            }
            return true;
        }

        static string RemoveDiacritics(string stIn)
        {
            var stFormD = stIn.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var t in stFormD)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(t);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(t);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        static string RemoveDiacriticsUpper(string stIn)
        {
            return RemoveDiacritics(stIn).ToUpperInvariant();
        }

        /*private static bool IsFavorite(object obj)
        {
            return ((Yatse2Media)obj).IsFavorite > 0;
        }*/

        public bool FilterMovies(object item)
        {
            var gg = item as Yatse2Movie;
            if (_videoFavoritesFilter)
                return (gg != null) && RemoveDiacriticsUpper(gg.Title).Contains(_filterMovie.ToUpperInvariant()) &&
                       (gg.IsFavorite > 0);

            return (gg != null) && RemoveDiacriticsUpper(gg.Title).Contains(_filterMovie.ToUpperInvariant());
        }

        public bool FilterTvShows(object item)
        {
            var gg = item as Yatse2TvShow;
            if (_videoFavoritesFilter)
                return (gg != null) && RemoveDiacriticsUpper(gg.Title).Contains(_filterTvShow.ToUpperInvariant()) &&
                       (gg.IsFavorite > 0);
            return (gg != null) && RemoveDiacriticsUpper(gg.Title).Contains(_filterTvShow.ToUpperInvariant());
        }

        public bool FilterAudioArtist(object item)
        {
            var gg = item as Yatse2AudioArtist;
            if (_audioFavoritesFilter)
                return (gg != null) && RemoveDiacriticsUpper(gg.Name).Contains(_filterAudioArtist.ToUpperInvariant()) &&
                       (gg.IsFavorite > 0);
            return (gg != null) && RemoveDiacriticsUpper(gg.Name).Contains(_filterAudioArtist.ToUpperInvariant());
        }

        public bool FilterAudioGenre(object item)
        {
            var gg = item as Yatse2AudioGenre;
            if (_audioFavoritesFilter)
                return (gg != null) && RemoveDiacriticsUpper(gg.Name).Contains(_filterAudioGenre.ToUpperInvariant()) &&
                       (gg.IsFavorite > 0);
            return (gg != null) && RemoveDiacriticsUpper(gg.Name).Contains(_filterAudioGenre.ToUpperInvariant());
        }

        public bool FilterAudioAlbum(object item)
        {
            var gg = item as Yatse2AudioAlbum;
            if (_audioFavoritesFilter)
                return (gg != null) && RemoveDiacriticsUpper(gg.Title).Contains(_filterAudioAlbum.ToUpperInvariant()) &&
                       (gg.IsFavorite > 0);
            return (gg != null) && RemoveDiacriticsUpper(gg.Title).Contains(_filterAudioAlbum.ToUpperInvariant());
        }

        private void ShowOkDialog(string message)
        {
            ModalDialog.ShowDialog(message);
        }

        private bool ShowYesNoDialog(string message)
        {
            return ModalDialog.ShowYesNoDialog(message);
        }
        
        private static void PreInit()
        {
            ServicePointManager.CheckCertificateRevocationList = false;
            ServicePointManager.DnsRefreshTimeout = 4 * 3600 * 1000;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.DefaultConnectionLimit = 10000; 
            Directory.CreateDirectory(Helper.LogPath);
            Directory.CreateDirectory(Helper.CachePath);
            Directory.CreateDirectory(Helper.CachePath + @"Video");
            Directory.CreateDirectory(Helper.CachePath + @"Music");
            Directory.CreateDirectory(Helper.CachePath + @"Weather");
            Directory.CreateDirectory(Helper.CachePath + @"Video\Thumbs");
            Directory.CreateDirectory(Helper.CachePath + @"Video\Fanarts");
            Directory.CreateDirectory(Helper.CachePath + @"Music\Thumbs");
            Directory.CreateDirectory(Helper.CachePath + @"Music\Artists");
            Directory.CreateDirectory(Helper.CachePath + @"Music\Fanarts");
        }

        private void StartServer()
        { 
            Logger.Instance().Log("SERVER", "IN STARTSERVER - Starting Server Thread... ", true);
            //Logger.Instance().LogDump("SERVER THREAD    : Attempting to start new Thread ", true);
            Thread t = new Thread(NewThread) { IsBackground = true };
            t.Start();
            
            Logger.Instance().Log("SERVER", "IN STARTSERVER after thread started... ", true);
        }

        private void NewThread()
        {

            IPAddress localAdd = IPAddress.Parse(_config.IPAddress);
            TcpListener listener = new TcpListener(IPAddress.Any, _config.IPPort);
            Logger.Instance().Log("SERVER", "Within New Thread running Listener... ", true);
            listener.Start();
            
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
               // Logger.Instance().LogDump("Yatse2 FANART    : Timer Result", _timer);
                
                Logger.Instance().LogDump("SERVER", "Data Received  " + dataReceived, true);
                _config.FanartCurrentPath = dataReceived;
                //  onfig.FanartCurrentPath = dataReceived;
                // Console.WriteLine("The resulting messages on the server" + dataReceived);
                //  nwStream.Write(buffer, 0, bytesRead);
               // Console.WriteLine("\n");
                client.Close();
            }
            //   listener.Stop();
        }

        private void UpdateText(string message)
        {
            Logger.Instance().Log("SERVER",message, true);
        }


        private void InitProperties()
        {

            _yatse2Properties = TryFindResource("Yatse2Properties") as Yatse2Properties;
            _moviesDataSource = TryFindResource("MoviesDataSource") as MoviesCollection;
            _moviesCollectionView =
                (CollectionView)CollectionViewSource.GetDefaultView(lst_Movies_flow.ItemsSource);
            _tvShowsDataSource = TryFindResource("TvShowsDataSource") as TvShowsCollection;
            _tvShowsCollectionView =
                (CollectionView)CollectionViewSource.GetDefaultView(lst_TvShows_flow.ItemsSource);
            _tvSeasonsDataSource = TryFindResource("TvSeasonsDataSource") as TvSeasonsCollection;
            _tvSeasonsCollectionView =
                (CollectionView)CollectionViewSource.GetDefaultView(lst_TvSeasons_flow.ItemsSource);
            _tvEpisodesDataSource = TryFindResource("TvEpisodesDataSource") as TvEpisodesCollection;
            _tvEpisodesCollectionView =
                (CollectionView)CollectionViewSource.GetDefaultView(lst_TvEpisodes_flow.ItemsSource);
            _audioAlbumsDataSource = TryFindResource("AudioAlbumsDataSource") as AudioAlbumsCollection;
            _audioAlbumsCollectionView =
                (CollectionView)CollectionViewSource.GetDefaultView(lst_AudioAlbums_flow.ItemsSource);
            _audioArtistsDataSource = TryFindResource("AudioArtistsDataSource") as AudioArtistsCollection;
            _audioArtistsCollectionView =
                (CollectionView)CollectionViewSource.GetDefaultView(lst_AudioArtists_flow.ItemsSource);
            _audioGenresDataSource = TryFindResource("AudioGenresDataSource") as AudioGenresCollection;
            _audioGenresCollectionView =
                (CollectionView)CollectionViewSource.GetDefaultView(lst_AudioGenres_flow.ItemsSource);
            _audioSongsDataSource = TryFindResource("AudioSongsDataSource") as AudioSongsCollection;

            _moviesCollectionView.Filter = new Predicate<object>(FilterMovies);
            _tvShowsCollectionView.Filter = new Predicate<object>(FilterTvShows);
            _audioAlbumsCollectionView.Filter = new Predicate<object>(FilterAudioAlbum);
            _audioArtistsCollectionView.Filter = new Predicate<object>(FilterAudioArtist);
            _audioGenresCollectionView.Filter = new Predicate<object>(FilterAudioGenre);
        }
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);

            //Loop through the running processes in with the same name 
            foreach (Process process in processes)
            {
                //Ignore the current process 
                if (process.Id != current.Id)
                {
                    //Make sure that the process is running from the exe file. 
                    if (Assembly.GetExecutingAssembly().Location.
                         Replace("/", "\\") == current.MainModule.FileName)
                    {
                        //Return the other process instance.  
                        return process;

                    }
                }
            }
            //No other instance was found, return null.  
            return null;
        }

        private void Init()
        {
            
            try
            {
                PreInit();
                // Attempting to start server socket on seperate thread
                
                Logger.Instance().Log("SERVER", "Starting Server Thread... ",true);
                


                var assem = Assembly.GetEntryAssembly();
                var assemName = assem.GetName();
                var ver = assemName.Version;

                Logger.Instance().Log("Yatse2","Starting build : " + ver.Build,true);

                Logger.Instance().Log("OSInfo", "Name = " + OSInfo.Name, true);
                Logger.Instance().Log("OSInfo", "Edition = " + OSInfo.Edition, true);
                Logger.Instance().Log("OSInfo", "Service Pack =" + OSInfo.ServicePack, true);
                Logger.Instance().Log("OSInfo", "Version = " + OSInfo.VersionString, true);
                Logger.Instance().Log("OSInfo", "Bits = " + OSInfo.RealBits, true);
                Logger.Instance().Log("OSInfo", "Culture = " + Thread.CurrentThread.CurrentCulture.Name, true);
                Logger.Instance().Log("Yatse 2 Debug :", "Checking for another instance", true);
              
                if (Yatse2Window.RunningInstance() != null)
                {
                    Logger.Instance().Log("NEW Yastse Debug:", "Duplicate Yatse2 Running Closing... ");
                    MessageBox.Show("Duplicate Instance of Yatse, Closing....");
                    Application.Current.Shutdown();

                    //TODO:
                    //Your application logic for duplicate 
                    //instances would go here.
                }
                //load local kodi source xml file to get directories base
                //trial
                LoadKodiSource();

                _config.Load(_configFile);
                _timerScreenSaver = _config.ScreensaverTimer;
                Logger.Instance().Debug = _config.Debug;
                Logger.Instance().DebugTrace = _config.DebugTrace;
                Logger.Instance().Log("Yatse2", "End load config");
                Logger.Instance().LogDump("Yatse2",_config);

                ApiHelper.Instance().LoadRemotePlugins(ver.Build);

                _currentRemoteId = _config.DefaultRemote;
                _currentGrid = grd_Home;


                if (_config.HideCursor)
                    Cursor = System.Windows.Input.Cursors.None;

                InitProperties();

                if (_yatse2Properties != null)
                {
                    Helper.Instance.CurrentSkin = _config.Skin;
                    _yatse2Properties.Skin = _config.Skin;
                    _yatse2Properties.Language = _config.Language;
                    _yatse2Properties.ShowHomeButton = false;
                    _yatse2Properties.Weather = new Yatse2Weather();
                    _yatse2Properties.Currently = new Yatse2Currently
                                                        {
                                                            IsNotMovieDetails = true,
                                                            IsNotMusicDetails = true,
                                                            IsNotTvDetails = true
                                                        };
                    RefreshDictionaries();
                }

                InitDatabase();
                InitWeather();
                InitRemote();
                InitTimer();
                RefreshRemotes();

                RefreshHeader();

                StartServer();

                
                System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
                string sPath2Icon = Path.Combine(Environment.CurrentDirectory, "Yatse2.ico");
                ni.Icon = new System.Drawing.Icon(sPath2Icon);
                

                ni.Visible = true;
                ni.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
                Logger.Instance().Log("NEW Yastse Debug:","Create new Taskbar Icon, make Visible, create Double Click event ");

                if (!_config.DisableAnimations)
                {
                    trp_Transition.Transition = TryFindResource("GridTransition") as FluidKit.Controls.Transition;
                }
                else
                {
                    trp_Transition.Transition = new FluidKit.Controls.NoTransition();
                }

                if (!_config.DisableResolutionDetection)
                {
                    Microsoft.Win32.SystemEvents.DisplaySettingsChanged += Change_Display_Settings;
                }
                Change_Display_Settings(null, null);

            }
            catch (Exception e)
            {
                Logger.Instance().LogException("Yaste2Init",e);
                Logger.Instance().Log("Yatse2Init","Forcing close");
                ni.Dispose();
                ni.Icon = null;
                
                Close();
            }

            Logger.Instance().Log("Yatse2", "End init", true);
           
        }

        //load Kodi Source xml and populate values to be checked against
        //working
        private void LoadKodiSource()
        {
            Logger.Instance().Log("Kodi Source", "Loading Kodi Source xml file", true);
            var appdatadirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            XmlDocument kodisource = new XmlDocument();
            kodisource.Load(@appdatadirectory+@"\Kodi\userdata\sources.xml");
            XmlNodeList KodiDirectories = kodisource.GetElementsByTagName("path");
            foreach (XmlNode node in KodiDirectories)
            {
                Logger.Instance().Log("Load Kodi Source", "Xml Data ==  " + node.InnerText, true);
            }

        }

        private void PositionScreen()
        {

            if (!_setPov)
            {
                _yatse2Properties.TvShowPosterPov = (lst_TvShows_flow.ActualWidth/lst_TvShows_flow.ActualHeight)/10*85;
                _yatse2Properties.VideoPov = (lst_Movies_flow.ActualWidth/lst_Movies_flow.ActualHeight)/10*85;
                _yatse2Properties.AudioGenresPov = (lst_AudioGenres_flow.ActualWidth/lst_AudioGenres_flow.ActualHeight)/
                                                   10*85;
                _yatse2Properties.AudioAlbumsPov = (lst_AudioAlbums_flow.ActualWidth/lst_AudioAlbums_flow.ActualHeight)/
                                                   10*85;
                _yatse2Properties.AudioArtistsPov = (lst_AudioArtists_flow.ActualWidth/
                                                     lst_AudioArtists_flow.ActualHeight)/10*85;
                _setPov = true;
            }

            if (_config.DisableScreenPositioning)
                return;
            if (_config.MouseMode)
                return;
            var dx = 1.0;
            var dy = 1.0;
            var temp = PresentationSource.FromVisual(this);
            if (temp != null)
            {
                if (temp.CompositionTarget != null)
                {
                    var m = temp.CompositionTarget.TransformToDevice;
                    dx = m.M11;
                    dy = m.M22;
                }
            }
            var screens = System.Windows.Forms.Screen.AllScreens;
            if (screens.Length == 1 || !_config.SecondScreen)
            {
                if (Top != 0 || Left != 0)
                {
                    Top = 0;
                    Left = 0;
                }
            }
            else
            {
                foreach (var scr in
                    screens.Where(scr => !scr.Primary).Where(scr => Top != (scr.Bounds.Top / dy) || Left != (scr.Bounds.Left / dx)))
                {
                    Top = scr.Bounds.Top / dy;
                    Left = scr.Bounds.Left / dx;
                    break;
                }
            }

        }

        private void CheckUpdate(bool showResult)
        {
            var assem = Assembly.GetEntryAssembly();
            var assemName = assem.GetName();
            var ver = assemName.Version;
            var platform = "x86";
            if (Tools.Is64BitProcess)
                platform = "x64";
            Logger.Instance().Log("Yatse2", "Checking for updates. Current version : " + ver, true);
            _allowBeta = File.Exists(Helper.AppPath + "Yatse2.beta");

            var repo = new RemoteRepository();
            repo.SetDebug(_config.Debug);
            if (!repo.LoadRepository(Repository, platform, Helper.AppPath + "Updates"))
            {
                if (showResult)
                    ShowOkDialog(GetLocalizedString(114));
                return;
            }
                
            var result = repo.UpdateTranslations(Helper.LangPath);
            if (result)
            {
                if (showResult)
                    ShowOkDialog(GetLocalizedString(115));
                RefreshDictionaries();
            }
            var versions = repo.GetBuildList(_allowBeta);
            if (versions == null)
            {
                Logger.Instance().Log("Yatse2", "Build list empty !", true);
                if (showResult)
                    ShowOkDialog(GetLocalizedString(114));
            }
            else
            {
                if (versions.Version.Count < 1)
                {
                    Logger.Instance().Log("Yatse2", "Build list empty !", true);
                    if (showResult)
                        ShowOkDialog(GetLocalizedString(114));
                }
                else
                {
                    var lastBuild = versions.Version[versions.Version.Count - 1];

                    if (ver.Build >= lastBuild.Build)
                    {
                        Logger.Instance().Log("Yatse2", "Version is up2date!", true);
                        if (showResult)
                            ShowOkDialog(GetLocalizedString(113));
                    }
                    else
                    {
                        Logger.Instance().Log("Yatse2", "Update available : " + lastBuild.Build, true);
                        var res = ShowYesNoDialog(GetLocalizedString(109));
                        if (res)
                        {
                            Directory.CreateDirectory(Helper.AppPath + "Temp");
                            if (GetUpdater())
                            {
                                Process.Start(Helper.AppPath + @"Temp\Yatse2Setup.exe");
                                Close();
                            }
                            else
                            {
                                ShowOkDialog(GetLocalizedString(101));
                            }
                        }

                    }
                }
            }
            repo.CleanTemporary();
        }
        static bool IsFileURI(String path)
        {
            return (String.Compare(path, 0, "smb:", 0, 3, StringComparison.OrdinalIgnoreCase) == 0);
        }

       /* Have to sort out this later - presently will not deal with none smb:\\ paths.
        * private static string MakeValidFileName(string name)
        {
            string illegal = "\"M\"\\a/ry/ h**ad:>> a\\/:*?\"| li*tt|le|| la\"mb.?";
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            illegal = r.Replace(illegal, "");
        }*/

        private void CheckFanArt()
        {
            var nowPlaying2 = _remote != null ? _remote.Player.NowPlaying(false) : new ApiCurrently();
            var FanartAlways = _config.FanartAlways;
            //_config.FanartDirectory = null;
            int numberofdirectoriesdeep = _config.FanartNumberDirectories;

            Logger.Instance().LogDump("Yatse2 FANART    : Check FanART Run & Current Menu prior", nowPlaying2.CurrentMenuLabel, true);

            if (grd_Diaporama.Visibility == Visibility.Hidden && nowPlaying2.CurrentMenuID != "10004")
            {
                var stbDiaporamaShow = (Storyboard)TryFindResource("stb_ShowDiaporama");
                if (stbDiaporamaShow != null)
                {
                    stbDiaporamaShow.Begin(this);
                }
            }



            if (FanartAlways == true)
            {
                //Logger.Instance().Log("SERVER", "Fanart Directory from Socket Server  " + _config.FanartCurrentPath, true);

                string CurrentPath = _config.FanartCurrentPath;
                var appdatadirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var FanartDirectory = appdatadirectory + @"\Kodi\userdata\"; //addon_data\script.artworkorganizer\";
                //_config.FanartDirectory = FanartDirectory + _config.FanartDirectoryTV;


               Logger.Instance().LogDump("SERVER", "Fanart Directory from Socket =  " + _config.FanartCurrentPath, true);
               //Logger.Instance().Log("SERVER", "Fanart Directory MAKEVALIDPATH equals " + MakeValidFileName(_config.FanartCurrentPath), true);

                // Current path from Socket is true path and video menu selected.
                // Need to sort out season 1 etc. naming as path from Kodi varies.
                // also if images selected want it to be slideshow of that current directory

                if (IsFileURI(CurrentPath) == true && nowPlaying2.CurrentMenuID == "10025")
                {
                    try
                    {
                        char[] MyChar = { 's', 'm', 'b', ':' };
                        string CurrentPath2 = CurrentPath.TrimStart(MyChar);
                        CurrentPath2 = Path.GetFullPath(CurrentPath2).Replace(@"/", @"\");
                        Logger.Instance().LogDump("SERVER", "Video Directory Socket returned path - CurrentPath2 equals  " + @CurrentPath2, true);
                        // Annoying and difficult - below splits the path into the first three directorys only ie. \\fileserver2012\tvss\Title of Show\  only
                        // Overcomes issues with Season 1/Season 2 etc directories and path to extrafanart
                        // Need to set number -->  will add config setting.
                        string CurrentPath3 = BreakDirectory(CurrentPath2, numberofdirectoriesdeep);
                        _config.FanartDirectory = @CurrentPath3 + @"extrafanart\";
                        Logger.Instance().LogDump("SERVER", "BreakDirectory Performed and equals  " + CurrentPath3, true);
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance().LogDump("SERVER", "Fanart Video Menu 10025 - Exception occured   " + ex, true);
                        _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryTV;
                    } 

                }

                if (IsFileURI(CurrentPath) == true && nowPlaying2.CurrentMenuID == "10002")
                {
                    try
                    {
                        char[] MyChar = { 's', 'm', 'b', ':' };
                        string CurrentPath2 = CurrentPath.TrimStart(MyChar);
                        Logger.Instance().LogDump("SERVER", "CurrentPath 2 equals:" + @CurrentPath2, true);
                        CurrentPath2 = Path.GetFullPath(@CurrentPath2).Replace(@"/", @"\");
                        Logger.Instance().LogDump("SERVER", "Image Directory Selected - smb equals  " + CurrentPath2, true);
                        _config.FanartDirectory = @CurrentPath2;
                        Logger.Instance().LogDump("SERVER", "Image Directory Selected & fanart equals  " + _config.FanartDirectory, true);
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance().LogDump("SERVER", "Fanart Image - Exception occured   " + ex, true);
                        _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryTV;
                        
                    }
                    
                    
                }

                // if no directory or no files afte above then move to default menu based settings                

                if (nowPlaying2.CurrentMenuID == "10025" && IsFileURI(CurrentPath) != true)
                    {
                        _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryMovie; // +@"MovieFanart\";
                    }
                    
                if (nowPlaying2.CurrentMenuID == "10501")
                    {
                        _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryMusic; // +@"ArtistFanart\";

                    }
                   
                  //  if (nowPlaying2.CurrentMenuID == "10002")
                  //  {
                  //      _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryMyImages; // +@"OwnFanart\";
                  //  }
                if (nowPlaying2.CurrentMenuID == "12600")
                    {
                        _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryWeather; // ppdatadirectory + @"\Kodi\userdata\addon_data\skin.aeonmq5.extrapack\backgrounds_weather\";
                    }
                if (nowPlaying2.CurrentMenuID == "10000")  //Equals the home menu
                    {
                       //
                        _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryTV; // ppdatadirectory + @"\Kodi\userdata\addon_data\skin.aeonmq5.extrapack\backgrounds_weather\";
                    }
                 if (nowPlaying2.CurrentMenuID == "10502")
                    {
                        _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryMusic; // +@"ArtistFanart\";

                    }

                    //If directory empty and fanart show being displayed - change to default - which is Movies
                if (GetRandomImagePathNew(_config.FanartDirectory) == null && grd_Diaporama.Visibility != Visibility.Hidden )
                    {
                        _config.FanartDirectory = FanartDirectory + _config.FanartDirectoryMovie;
                    }
                



                
                if (nowPlaying2.CurrentMenuID == "10004" && grd_Diaporama.Visibility != Visibility.Hidden)
                {

                    var stbDiaporamaHide = (Storyboard)TryFindResource("stb_HideDiaporama");
                    if (stbDiaporamaHide != null)
                    {
                        stbDiaporamaHide.Begin(this);
                    }

                }

            }
        }
        static string BreakDirectory(string path3, int dirnumber)
        {

            string[] directories = path3.Split(Path.DirectorySeparatorChar);
            string previousEntry = string.Empty;
            var count = -2;
            if (null != directories)
            {
                foreach (string direc in directories)
                {
                    count++;
                    string newEntry = previousEntry + Path.DirectorySeparatorChar + direc;
                    if (!string.IsNullOrEmpty(newEntry))
                    {
                        if (!newEntry.Equals(Convert.ToString(Path.DirectorySeparatorChar), StringComparison.OrdinalIgnoreCase))
                        {

                            previousEntry = newEntry;
                        }
                    }

                    if (count == dirnumber)
                    {
   
                        return @"\" + newEntry + @"\";
                    }
                }
            }
            return null;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            _timerHeader++;
            _timer++;
            Logger.Instance().LogDump("Yatse2 FANART    : Timer Result", _timer);
            UpdateRemote();
            Window glennwindow = Window.GetWindow(this);


            //if (_config.CheckForUpdate && !_updatecheck)
            //{
             //   _updatecheck = true;
              //  CheckUpdate(false);
            //}

            if (!_showHomePage)
            {
                ShowHome();
                _showHomePage = true;
            }

            if (_timerHeader > 15)
            {
                RefreshHeader();
                _timerHeader = 0;
            }
            var nowPlaying = _remote != null ? _remote.Player.NowPlaying(false) : new ApiCurrently();
            var GlennMinimise = (_config.MinimiseAlways);


            //Logger.Instance().Log("Yatse2", "About to CALL CheckFanARt");
           // CheckFanArt();
            //Logger.Instance().Log("Yatse2", "After CALL CheckFanARt");


            if ((_timer > _config.DimmingTimer) && _config.Dimming && (nowPlaying.IsPlaying))
            {
                if (!(!_yatse2Properties.Currently.IsTv && !_yatse2Properties.Currently.IsMovie && _config.DimmingOnlyVideo))
                {
                    if (grd_Dimming.Visibility != Visibility.Visible)
                    {
                        Logger.Instance().Log("Yatse2", "Start screen saver : Dimming here 2");
                        var stbDimmingShow = (Storyboard)TryFindResource("stb_ShowDimming");
                        if (stbDimmingShow != null)
                            stbDimmingShow.Begin(this);
                    }
                }
                Logger.Instance().LogDump("Yatse2 FANART    : ResetTimer Run from 2", _timer);
                ResetTimer();
            }

            if (!nowPlaying.IsPaused && !nowPlaying.IsPlaying)
            {
                if (grd_Dimming.Visibility == Visibility.Visible && glennwindow.WindowState == WindowState.Normal)
                {
                    var stbDimmingShow = (Storyboard)TryFindResource("stb_HideDimming");
                    if (stbDimmingShow != null)
                        stbDimmingShow.Begin(this);
                    Logger.Instance().LogDump("Yatse2 NEW DEBUG:", "Playback Paused or Playing - Dim on Undim");
                }

                if (glennwindow.WindowState == WindowState.Normal)
                //     if (this.Visibility == Visibility.Visible)
                {
                    if (GlennMinimise == true)
                    {
                        notifyIcon1_DoubleClick(null, null);
                        // this.ShowInTaskbar = false;
                        // this.WindowState = WindowState.Minimized;
                        // Hide();
                        // this.ShowInTaskbar = false;
                        // _config.MinimiseAlways = true;
                        Logger.Instance().LogDump("NEW Yastse  Debug    : DBL click tasbar event/Normal Window, Minimise Window and set MinimiseAlways to true ", _config.MinimiseAlways);
                        // this.Activate();

                    }
                    if (!_isfanart && GlennMinimise == false && _config.FanartAlways == true && nowPlaying.CurrentMenuID != "10004" && !nowPlaying.IsPaused && !nowPlaying.IsPlaying && ( _timer % _config.FanartTimer) == 0 ) 
                    {
                         Logger.Instance().LogDump("Yatse2 FANART    : StartFanART Run & Fanart Timer result", _timer);
                         CheckFanArt();
                         StartFanart();
                         Logger.Instance().LogDump("Yatse2 FANART    : StartFanART Finsihed & _timer result", _timer);
                         //Fanart Routine shoudl go here
                        
                    }
                }
            }
                
            if (nowPlaying.IsPaused)

            {
                    Logger.Instance().LogDump("Yatse2", "nowPlaying.Paused is called");    
                    if (grd_Dimming.Visibility == Visibility.Visible)
                    {
                        var stbDimmingShow = (Storyboard)TryFindResource("stb_HideDimming");
                        if (stbDimmingShow != null)
                            stbDimmingShow.Begin(this);
                        Logger.Instance().LogDump("Yatse2 NEW Debug:", "Playback Paused undim ",true);
                   ResetTimer();
                   }

           }

          if (nowPlaying.IsMuted)
          {
                    if (grd_Dimming.Visibility == Visibility.Visible)
                    {
                        var stbDimmingShow = (Storyboard)TryFindResource("stb_HideDimming");
                        if (stbDimmingShow != null)
                            stbDimmingShow.Begin(this);
                        Logger.Instance().LogDump("Yatse2 NEW Debug:", "Playback Muted undim ",true);
                    }
                    ResetTimer();
          }

          if (_timer > _timerScreenSaver && !nowPlaying.IsPaused && _config.FanartAlways == false )
          {
                    StartScreensaver();
          }


          if (_isScreenSaver && _diaporamaCurrentImage != 0 && (_timer % _config.DiaporamaTimer) == 0)
          {
                    SwitchDiaporama();
          }

          if (glennwindow.WindowState == WindowState.Normal && _isfanart && _fanartCurrentImage != 0 && (_timer % _config.FanartTimer) == 0)
          {
                    CheckFanArt();
                    SwitchFanart();
                    Logger.Instance().LogDump("Yatse2 FANART    : SWITCH FanART Run & FanartTimer result", _timer );
          }

          PositionScreen();

          CheckFirstLaunch();
          
        }
        

        private void StartScreensaver()
        {
            if (!_isScreenSaver)
            {
                _isScreenSaver = true;
                if (_config.Dimming && !_config.DimmingOnlyVideo)
                {
                    Logger.Instance().Log("Yatse2", "Start screen saver : Dimming Here as well");
                    var stbDimmingShow = (Storyboard)TryFindResource("stb_ShowDimming");
                    if (stbDimmingShow != null)
                        stbDimmingShow.Begin(this);
                }
                if (_config.Diaporama && (!_config.Dimming || _config.DimmingOnlyVideo))
                {
                    Logger.Instance().Log("Yatse2", "Start screen saver : Diaporama");
                    StartDiaporama();
                }
            }
            
            ResetTimer();
        }

        private void CheckFirstLaunch()
        {
            if (!_config.IsConfigured && _currentGrid.Name != "grd_Settings")
            {
                Logger.Instance().Log("Yatse2", "Not configured, Go to settings", true);
                btn_Home_Settings_Click(null, null);
            }
        }

        private void Change_Display_Settings(object sender, EventArgs e)
        {
            Logger.Instance().Log("Yatse2", "Dispay settings changed");
            Topmost = _config.Topmost;
            WindowStartupLocation = WindowStartupLocation.Manual;
            Activate();
            var dx = 1.0;
            var dy = 1.0;
            var temp = PresentationSource.FromVisual(this);
            if (temp != null)
            {
                if (temp.CompositionTarget != null)
                {
                    var m = temp.CompositionTarget.TransformToDevice;
                    dx = m.M11;
                    dy = m.M22;
                }
            }
            var screens = System.Windows.Forms.Screen.AllScreens;

    //        ni.BalloonTipTitle = "Minimise Setting";
    //        ni.BalloonTipText = " Minimise Always On";

        //    ni.DoubleClick +=

        //        delegate(object sender, EventArgs args)
         //       {
        //            this.Show();
        //            this.WindowState = WindowState.Normal;
        // //      

            if (screens.Length == 1 || !_config.SecondScreen)
            {
                if (_config.ForceResolution)
                {
                    var currentRes = ScreenResolution.GetDevmode(0, -1);
                    Logger.Instance().LogDump("CurrentResolutionMonoScreen", currentRes);
                    if (currentRes.DMPelsHeight != _config.Resolution.DMPelsHeight || currentRes.DMPelsWidth != _config.Resolution.DMPelsWidth || currentRes.DMBitsPerPel != _config.Resolution.DMBitsPerPel)
                    {
                        ScreenResolution.ChangeResolutionMode(0, _config.Resolution);
                        Logger.Instance().LogDump("ChangeResolutionMonoScreen", _config.Resolution);
                    }
                }
                Top = 0;
                Left = 0;
            }
            else
            {
                if (_config.ForceResolution)
                {
                    var currentRes = ScreenResolution.GetDevmode(1, -1);
                    Logger.Instance().LogDump("CurrentResolutionMultiScreen", currentRes);
                    if (currentRes.DMPelsHeight != _config.Resolution.DMPelsHeight || currentRes.DMPelsWidth != _config.Resolution.DMPelsWidth || currentRes.DMBitsPerPel != _config.Resolution.DMBitsPerPel)
                    {
                        ScreenResolution.ChangeResolutionMode(1, _config.Resolution);
                        Logger.Instance().LogDump("ChangeResolutionMultiScreen", _config.Resolution);
                    }
                }
                screens = System.Windows.Forms.Screen.AllScreens;
                foreach (var scr in screens.Where(scr => !scr.Primary))
                {
                    Top = scr.Bounds.Top / dy;
                    Left = scr.Bounds.Left / dx;
                    break;
                }
            }
            if (_config.Resolution.DMPelsWidth > 0)
            {
                Width = _config.Resolution.DMPelsWidth / dx;
                Height = _config.Resolution.DMPelsHeight / dy;
            }

            if (_config.Resolution.DMPelsHeight == 480)
            {
                _config.Hack480 = true;
                brd_Home_Video.Margin = new Thickness(0, 0, 100, 180);
                brd_Home_Music.Margin = new Thickness(0, 70, 100, 0);
                brd_Home_Other.Margin = new Thickness(0, 320, 100, 0);
            }
            else
            {
                brd_Home_Video.Margin = new Thickness(0, 0, 100, 250);
                brd_Home_Music.Margin = new Thickness(0, 70, 100, 0);
                brd_Home_Other.Margin = new Thickness(0, 390, 100, 0);
            }

            _setPov = false;
        }

        private void notifyIcon1_DoubleClick(object Sender, EventArgs e)
        {
            // Show the form when the user double clicks on the notify icon. 

            // Set the WindowState to normal if the form is minimized. 
            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
                //this.ShowInTaskbar = true;
                _config.MinimiseAlways = false;
                _config.FanartAlways = true;
                //this.Activate();

                

                Logger.Instance().LogDump("NEW Yastse Debug    : DBL click tasbar event/Min Window, Open Window and set MinimiseAlways to false ", _config.MinimiseAlways);
                return;
            }
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Minimized;
                //this.ShowInTaskbar = false;
                _config.MinimiseAlways = true;
                _config.FanartAlways = false;
                Logger.Instance().LogDump("NEW Yastse Debug    : DBL click tasbar event/Normal Window, Minimise Window and set MinimiseAlways to true ", _config.MinimiseAlways);
            }
            // Activate the form. 
            //this.Activate();



        }

        private void ShowHome()
        {
            
            var en = _yatse2Pages.GetEnumerator();
            while (en.MoveNext())
            {
                if (en.Key.ToString() != _config.Homepage)
                    continue;
                Logger.Instance().Log("Yatse2", "Home Page : " + en.Value);
                switch (en.Value.ToString())
                {
                    case "grd_Movies":
                        Load_Movies();
                        if (_moviesDataSource.Count < 1)
                            return;
                        break;
                    case "grd_TvShows" :
                        Load_TvShows();
                        if (_tvShowsDataSource.Count < 1)
                            return;
                        break;
                    case "grd_AudioAlbums":
                        Load_AudioAlbums();
                        if (_audioAlbumsDataSource.Count < 1)
                            return;
                        break;
                    case "grd_AudioArtists":
                        Load_AudioArtists();
                        if (_audioArtistsDataSource.Count < 1)
                            return;
                        break;
                    case "grd_AudioGenres":
                        Load_AudioGenres();
                        if (_audioGenresDataSource.Count < 1)
                            return;
                        break;
                    case "grd_Weather" :
                        RefreshWeather();
                        break;
                    default:
                        break;
                }

                foreach (Grid grid in trp_Transition.Items)
                {
                    if (grid.Name != en.Value.ToString()) continue;
                    ShowGrid(grid);
                    return;
                }
            }
        }

        private void GoBack()
        {
            if (_gridHistory.Count < 1)
                return;
            foreach (Grid grid in trp_Transition.Items)
            {
                if (grid.Name != _gridHistory[_gridHistory.Count-1]) continue;
                if (ShowGrid(grid, false) && _gridHistory.Count > 0)
                    _gridHistory.RemoveAt(_gridHistory.Count - 1);
                return;
            }
            
        }

        private void ShowGrid(Grid newGrid)
        {
            ShowGrid(newGrid, true);
        }

        private bool ShowGrid(Grid newGrid , bool history)
        {
            Logger.Instance().LogDump("Yatse2 FANART    : ResetTimer Run from ShowGrid1", _timer);
            ResetTimer();
            if ((_currentGrid.Name == newGrid.Name) || (trp_Transition.IsTransitioning)) return false;
            Logger.Instance().Log("Yatse2", "Show Grid : " + newGrid.Name);

            grd_PlayMenu.Visibility = Visibility.Hidden;
            grd_Filter.Visibility = Visibility.Hidden;
            grd_Settings_Weather.Visibility = Visibility.Hidden;
            grd_Settings_Remotes_Edit.Visibility = Visibility.Hidden;
            grd_Movies_Details.Visibility = Visibility.Hidden;
            grd_TvShows_Details.Visibility = Visibility.Hidden;
            grd_AudioAlbums_Details.Visibility = Visibility.Hidden;
            grd_Remote.Visibility = Visibility.Hidden;

            _yatse2Properties.ShowHomeButton = newGrid.Name != "grd_Home";

            _disableFocus = ((newGrid.Name == "grd_Settings") || (newGrid.Name == "grd_Remotes"));

            trp_Transition.ApplyTransition(_currentGrid.Name, newGrid.Name);

            if (newGrid.Name == "grd_Home")
                _gridHistory.Clear();
            else
                if (history)
                    if (_currentGrid.Name != "grd_Currently")
                        _gridHistory.Add(_currentGrid.Name);
            _currentGrid = newGrid;
            return true;
        }


        private void ResetTimer()
        {
            if (_config.KeepFocus && _remoteInfo != null && !_disableFocus)
            {
                if (_remote != null)
                    _remote.GiveFocus();
            }
            Logger.Instance().LogDump("Yatse2 FANART    : ResetTimer Run", _timer);
            _timer = 0;
        }

        private void RefreshHeader()
        {
            var now = DateTime.Now;
            _yatse2Properties.Date = now.ToString("dd MMMM yyy", CultureInfo.CurrentUICulture.DateTimeFormat);
            _yatse2Properties.Time = now.ToShortTimeString();
            
            var weatherData = _weather.GetWeatherData(_config.WeatherLoc);
            if (weatherData == null)
            {
                Logger.Instance().Log("Yatse2", "RefreshHeader : No weather data");
                return;
            }
            if (String.IsNullOrEmpty(_yatse2Properties.Weather.Day1Name))
                RefreshWeather();
            _yatse2Properties.Weather.LoadCurrentData(weatherData,_yatse2Properties.Skin);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _weather.Dispose();
                _database.Close();
                ni.Icon = null;
                ni.Dispose();
             }
        }


        private class FakeWindowsPeer : WindowAutomationPeer
        {
            public FakeWindowsPeer(Window window) : base(window)
            {
                
            }

            protected override List<AutomationPeer> GetChildrenCore()
            {
                return null;
            }

        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new FakeWindowsPeer(this);
        }

    }

}