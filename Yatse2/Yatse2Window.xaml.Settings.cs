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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Microsoft.Win32;
using Setup;
using Yatse2.Libs;

namespace Yatse2
{
    public partial class Yatse2Window
    {
        private static void Donate()
        {
            const string url = @"https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=";
            Process.Start(new ProcessStartInfo(url));
        }

        private static void WebSite()
        {
            const string url = @"https://github.com/Ghawken/Yatse2-1";
            Process.Start(new ProcessStartInfo(url));
        }

        private void btn_Settings_CheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            CheckUpdate(true);
        }

        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Donate();
        }

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WebSite();
        }

        private void btn_Settings_SelectImagesDirectory_Click(object sender, RoutedEventArgs e)
        {
            ResetTimer();
            using (var dialog = new FolderBrowserDialog() )
            {
                dialog.SelectedPath = txt_Settings_ImagesDirectory.Text;
                var result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    txt_Settings_ImagesDirectory.Text = dialog.SelectedPath;
                }
           }
        }

        private void btn_Home_Settings_Click(object sender, RoutedEventArgs e)
        {
            if (_currentGrid == grd_Settings) return;
            chk_Settings_SecondScreen.IsChecked = _config.SecondScreen;
            chk_Settings_Topmost.IsChecked = _config.Topmost;
            chk_Settings_KeepFocus.IsChecked = _config.KeepFocus;
            //chk_Settings_MinimiseAlways.IsChecked = _config.MinimiseAlways;
            chk_Settings_ForceResolution.IsChecked = _config.ForceResolution;
            chk_Settings_Diaporama.IsChecked = _config.Diaporama;
            chk_Settings_Dimming.IsChecked = _config.Dimming;
            chk_Settings_Currently.IsChecked = _config.Currently;
            chk_Settings_CurrentlyMovie.IsChecked = _config.CurrentlyMovie;
            chk_Settings_HideCursor.IsChecked = _config.HideCursor;

            chk_Settings_UseBanner.IsChecked = _config.UseBanner;
            chk_Settings_ShowOverlay.IsChecked = _config.ShowOverlay;
            chk_Settings_ShowEmptyMusicGenre.IsChecked = _config.ShowEmptyMusicGenre;
            chk_Settings_ShowEndTime.IsChecked = _config.ShowEndTime;

            chk_Settings_DisableAnimations.IsChecked = _config.DisableAnimations;
            chk_Settings_Debug.IsChecked = _config.Debug;
            chk_Settings_HideWatched.IsChecked = _config.HideWatched;
            chk_Settings_RefreshOnConnect.IsChecked = _config.RefreshOnConnect;
            //chk_Settings_AnimatedMusicCover.IsChecked = _config.AnimatedMusicCover;
            chk_Settings_DimmingOnlyVideo.IsChecked = _config.DimmingOnlyVideo;
            chk_Settings_CheckUpdate.IsChecked = _config.CheckForUpdate;

            chk_Settings_HideCompilationArtists.IsChecked = _config.HideCompilationArtists;
            chk_Settings_GenreToArtists.IsChecked = _config.GenreToArtists;
            chk_Settings_MusicFanartRotation.IsChecked = _config.MusicFanartRotation;

            txt_Settings_ImagesDirectory.Text = _config.ImageDirectory;
            txt_Settings_WeatherLocId.Text = _config.WeatherLoc;

            chk_Settings_MouseMode.IsChecked = _config.MouseMode;

            chk_Settings_DebugTrace.IsChecked = _config.DebugTrace;
            chk_Settings_CropCacheImage.IsChecked = _config.CropCacheImage;
            chk_Settings_IgnoreSortTokens.IsChecked = _config.IgnoreSortTokens;
            chk_Settings_StartWithWindows.IsChecked = _config.StartWithWindows;

            LoadSettingsResolutions();

            lst_Settings_Skin.Items.Clear();
            var skins = Directory.GetDirectories(Helper.SkinPath);
            Logger.Instance().Log("Yatse2", "Detected skins : " + skins.Length);
            foreach (var skin in skins)
            {
                var skinname = skin.Replace(Helper.SkinPath, "");
                var index = lst_Settings_Skin.Items.Add(skinname);
                Logger.Instance().Log("Yatse2", "Adding skin : " + skinname );
                if (skinname == _config.Skin)
                {
                    lst_Settings_Skin.SelectedIndex = index;
                }
            }

            lst_Settings_HomePage.Items.Clear();
            var en = _yatse2Pages.GetEnumerator();
            while (en.MoveNext())
            {
                var index = lst_Settings_HomePage.Items.Add(en.Key.ToString());
                if (en.Key.ToString() == _config.Homepage)
                    lst_Settings_HomePage.SelectedIndex = index;
            }

            lst_Settings_DefaultPlay.Items.Clear();
            en = _yatse2PlayModes.GetEnumerator();
            while (en.MoveNext())
            {
                var index = lst_Settings_DefaultPlay.Items.Add(GetLocalizedString(Convert.ToInt32(en.Value.ToString())));
                if (Convert.ToInt32(en.Key.ToString()) == _config.DefaultPlayMode)
                    lst_Settings_DefaultPlay.SelectedIndex = index;
            }

            lst_Settings_WeatherUnit.Items.Clear();
            lst_Settings_WeatherUnit.Items.Add("°C");
            lst_Settings_WeatherUnit.Items.Add("°F");
            lst_Settings_WeatherUnit.SelectedIndex = _config.WeatherUnit == "c" ? 0 : 1;

            LoadSettingsLangs();

            ShowGrid(grd_Settings);
        }

        private void LoadSettingsResolutions()
        {
            var screens = Screen.AllScreens;
            var modes = ScreenResolution.EnumModes(screens.Length == 1 ? 0 : 1);
            if (screens.Length == 1)
                Logger.Instance().Log("Yatse2", "Detected main screen resolutions : " + modes.Length);
            else
                Logger.Instance().Log("Yatse2", "Detected secondary screen resolutions : " + modes.Length);

            Logger.Instance().TraceDump("Yatse2", modes);

            lst_Settings_Resolution.Items.Clear();
            foreach (var mode in modes.Where(mode => mode.DMBitsPerPel == _config.MinDMBitsPerPel && mode.DMPelsWidth >= _config.MinDMPelsWidth))
            {
                var index = lst_Settings_Resolution.Items.Add(new ScreenRes(mode));
                Logger.Instance().Trace("Yatse2", "Detected resolution : " + lst_Settings_Resolution.Items[index]);
                if (mode == _config.Resolution)
                {
                    lst_Settings_Resolution.SelectedIndex = index;
                }
            }
            Logger.Instance().Log("Yatse2", "Added resolutions : " + lst_Settings_Resolution.Items.Count);
        }

        private void LoadSettingsLangs()
        {
            lst_Settings_Language.Items.Clear();
            var langs = Directory.GetFiles(Helper.LangPath, "*.xaml");
            Logger.Instance().Log("Yatse2", "Detected language files : " + langs.Length);
            var langVersion = new Regex(@"Version:(\d+)");
            foreach (var lang in langs)
            {
                var langname = lang.Replace(Helper.LangPath, "").Replace(".xaml", "");
                var index = lst_Settings_Language.Items.Add(langname);
                var data = File.ReadAllText(lang);
                var m = langVersion.Match(data);
                if (m.Success)
                    Logger.Instance().Log("Yatse2", "Adding lang : " + langname + " | Version : " + m.Groups[1]);
                else
                    Logger.Instance().Log("Yatse2", "Adding lang : " + langname + " | No version");

                if (langname == _config.Language)
                {
                    lst_Settings_Language.SelectedIndex = index;
                }
            }
        }

        private void GetSettingsVars()
        {

            _config.IsConfigured = true;
            try
            {
                // ReSharper disable PossibleInvalidOperationException
                _config.SecondScreen = (bool)chk_Settings_SecondScreen.IsChecked;
                _config.Topmost = (bool)chk_Settings_Topmost.IsChecked;
                _config.KeepFocus = (bool)chk_Settings_KeepFocus.IsChecked;
                _config.ForceResolution = (bool)chk_Settings_ForceResolution.IsChecked;
                _config.Diaporama = (bool)chk_Settings_Diaporama.IsChecked;
                _config.Dimming = (bool)chk_Settings_Dimming.IsChecked;
                _config.Currently = (bool)chk_Settings_Currently.IsChecked;
                _config.CurrentlyMovie = (bool)chk_Settings_CurrentlyMovie.IsChecked;
                _config.HideCursor = (bool)chk_Settings_HideCursor.IsChecked;
                _config.UseBanner = (bool)chk_Settings_UseBanner.IsChecked;
                _config.ShowOverlay = (bool)chk_Settings_ShowOverlay.IsChecked;
                _config.ShowEmptyMusicGenre = (bool)chk_Settings_ShowEmptyMusicGenre.IsChecked;
                _config.DisableAnimations = (bool)chk_Settings_DisableAnimations.IsChecked;
                _config.ShowEndTime = (bool)chk_Settings_ShowEndTime.IsChecked;
                _config.Debug = (bool)chk_Settings_Debug.IsChecked;
                _config.HideWatched = (bool)chk_Settings_HideWatched.IsChecked;
                _config.RefreshOnConnect = (bool)chk_Settings_RefreshOnConnect.IsChecked;
                _config.DimmingOnlyVideo = (bool)chk_Settings_DimmingOnlyVideo.IsChecked;
                _config.HideCompilationArtists = (bool)chk_Settings_HideCompilationArtists.IsChecked;
                _config.GenreToArtists = (bool)chk_Settings_GenreToArtists.IsChecked;
                _config.MusicFanartRotation = (bool)chk_Settings_MusicFanartRotation.IsChecked;
                _config.CheckForUpdate = (bool)chk_Settings_CheckUpdate.IsChecked;
                _config.MouseMode = (bool)chk_Settings_MouseMode.IsChecked;
                _config.CropCacheImage = (bool)chk_Settings_CropCacheImage.IsChecked;
                _config.DebugTrace = (bool)chk_Settings_DebugTrace.IsChecked;
                _config.IgnoreSortTokens = (bool) chk_Settings_IgnoreSortTokens.IsChecked;
                _config.StartWithWindows = (bool)chk_Settings_StartWithWindows.IsChecked;
                // ReSharper restore PossibleInvalidOperationException
            }
            catch (InvalidOperationException) { }

            _config.ImageDirectory = txt_Settings_ImagesDirectory.Text;
            _config.WeatherLoc = txt_Settings_WeatherLocId.Text;
            _config.WeatherUnit = lst_Settings_WeatherUnit.SelectedIndex == 0 ? "c" : "f";
            if (lst_Settings_Language.SelectedItem != null)
                _config.Language = lst_Settings_Language.SelectedItem.ToString();
            if (lst_Settings_Skin.SelectedItem != null)
                _config.Skin = lst_Settings_Skin.SelectedItem.ToString();

            var en = _yatse2Pages.GetEnumerator();
            while (en.MoveNext())
            {
                if (en.Key.ToString() == lst_Settings_HomePage.SelectedItem.ToString())
                    _config.Homepage = en.Key.ToString();
            }

            en = _yatse2PlayModes.GetEnumerator();
            while (en.MoveNext())
            {
                if (GetLocalizedString(Convert.ToInt32(en.Value.ToString())) == lst_Settings_DefaultPlay.SelectedItem.ToString())
                    _config.DefaultPlayMode = Convert.ToInt32(en.Key.ToString());
            }

            var resolution = (ScreenRes)lst_Settings_Resolution.SelectedItem;
            if (resolution != null)
            {
                _config.Resolution = resolution.Mode;
            }
        }

        private void btn_Settings_SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            GetSettingsVars();

            Logger.Instance().Debug = _config.Debug;
            Logger.Instance().DebugTrace = _config.DebugTrace;

            Cursor = _config.HideCursor ? System.Windows.Input.Cursors.None : System.Windows.Input.Cursors.Arrow;

            _config.Save(_configFile);
            InitWeather();

            Helper.Instance.CurrentSkin = _config.Skin;
            _yatse2Properties.Skin = _config.Skin;
            _yatse2Properties.Language = _config.Language;
            RefreshDictionaries();

            RefreshHeader();

            if (_config.DefaultRemote < 1)
            {
                ShowGrid(grd_Remotes);
            }

            ShowPopup(GetLocalizedString(98));
            Change_Display_Settings(null, null);

            if (!_config.DisableAnimations)
            {
                trp_Transition.Transition = TryFindResource("GridTransition") as FluidKit.Controls.Transition;
            }
            else
            {
                trp_Transition.Transition = new FluidKit.Controls.NoTransition();
            }

            if (_config.StartWithWindows)
            {
                var rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (rkApp != null) rkApp.SetValue("Yatse2", System.Windows.Forms.Application.ExecutablePath);
            }
            else
            {
                var rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (rkApp != null) rkApp.DeleteValue("Yatse2", false);
            }

            _audioGenresDataSource.Clear();
            _audioArtistsDataSource.Clear();
            _audioArtistsDataSource.Clear();
            _moviesDataSource.Clear();
            _tvShowsDataSource.Clear();
        }

        private void btn_Settings_WeatherLocId_Click(object sender, RoutedEventArgs e)
        {
            grd_Settings_Weather.Visibility = Visibility.Visible;
            //_yatse2Properties.ShowSettingsWeather = true;
        }

        private void btn_Settings_Weather_SelectLocId_Click(object sender, RoutedEventArgs e)
        {
            if (lst_Settings_Weather_LocationId.SelectedItem != null)
                txt_Settings_WeatherLocId.Text = ((WeatherLocation)lst_Settings_Weather_LocationId.SelectedItem).LocId;
            grd_Settings_Weather.Visibility = Visibility.Hidden;
            //_yatse2Properties.ShowSettingsWeather = false;
        }

        private void txt_Settings_Weather_WeatherLocId_TextChanged(object sender, TextChangedEventArgs e)
        {
            lst_Settings_Weather_LocationId.Items.Clear();
            if (txt_Settings_Weather_WeatherLocId.Text.Length < 3) return;

            var cities = Weather.SearchCity(txt_Settings_Weather_WeatherLocId.Text);
            foreach (var weatherLocation in cities)
            {
                lst_Settings_Weather_LocationId.Items.Add(weatherLocation);
            }
        }

        private void btn_Settings_Weather_Cancel_Click(object sender, RoutedEventArgs e)
        {
            grd_Settings_Weather.Visibility = Visibility.Hidden;
            //_yatse2Properties.ShowSettingsWeather = false;
        }

       
    }
}