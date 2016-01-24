// ------------------------------------------------------------------------
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
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace Remote.Emby.Api
{
    public class MpcHcRemote
    {
        private readonly Xbmc _parent;
        private class CommandInfo
        {
            public string Command { get; set; }
            public string Parameter { get; set; }
        }

        public void AsyncCommand(string cmd,string parameter)
        {
            var commandInfo = new CommandInfo { Command = cmd, Parameter = parameter};

            var bw = new BackgroundWorker();
            bw.DoWork += AsyncCommandWorker;
            bw.RunWorkerAsync(commandInfo);
        }
        public void AsyncGeneralCommand(string cmd, string parameter)
        {
            var commandInfo = new CommandInfo { Command = cmd, Parameter = parameter };

            var bw = new BackgroundWorker();
            bw.DoWork += AsyncCommandGeneralWorker;
            bw.RunWorkerAsync(commandInfo);
        }

        private void AsyncCommandGeneralWorker(object sender, DoWorkEventArgs e)
        {
            var commandInfo = (CommandInfo)e.Argument;
            CommandGeneral(commandInfo.Command,commandInfo.Parameter);
        }

        private void AsyncCommandWorker(object sender, DoWorkEventArgs e)
        {
            var commandInfo = (CommandInfo)e.Argument;
            Command(commandInfo.Command, commandInfo.Parameter);
        }


        public bool Command(string cmd,string parameter)
        {
            HttpWebRequest request;
            var returnContent = false;
            var authString = _parent.GetAuthString();

            var uri = @"http://" + _parent.IP + ":" + _parent.Port+"/emby/Sessions/"+Globals.SessionIDClient+"/Playing/";
            
            if (!String.IsNullOrEmpty(cmd))
            {
                uri += cmd;
            }


            _parent.Log(" ---------EMBY PLAY COMMAND: TESTING URL:" + uri+":::::");

            try
            {
                request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
                request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
                request.Headers.Add("X-Emby-Authorization", authString);
                request.ContentType = "application/json";

                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] data = encoder.GetBytes("");


                request.ContentLength = data.Length;
                request.Expect = "application/json";

                request.Method = "POST";
                request.Timeout = 1000;
                _parent.Log("Emby COMMAND  : " + cmd);
                _parent.Trace(uri);


                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                            using (var reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                var reqContent = reader.ReadToEnd();
                                _parent.Trace(reqContent);
                                returnContent = true;
                            }
                    }
                }
            }
            catch (WebException e)
            {
                _parent.Log("ERROR - EMBY  Command : " + cmd + " - " + e.Message);
                if (e.Status == WebExceptionStatus.Timeout)
                {

                   // _parent.MpcLoaded = false;
                }
            }
            return returnContent;
        }

        public bool CommandGeneral(string cmd, string parameter)
        {
            HttpWebRequest request;
            var returnContent = false;
            var authString = _parent.GetAuthString();

            var uri = @"http://" + _parent.IP + ":" + _parent.Port + "/emby/Sessions/" + Globals.SessionIDClient + "/Command/";

            if (!String.IsNullOrEmpty(cmd))
            {
                uri += cmd;
            }


            _parent.Log(" ---------EMBY GENERAL COMMAND COMMAND: TESTING URL:" + uri + ":::::");

            try
            {
                request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
                request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
                request.Headers.Add("X-Emby-Authorization", authString);
                request.ContentType = "application/json";

                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] data = encoder.GetBytes("");


                request.ContentLength = data.Length;
                request.Expect = "application/json";

                request.Method = "POST";
                request.Timeout = 1000;
                _parent.Log("Emby COMMAND  : " + cmd);
                _parent.Trace(uri);


                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                            using (var reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                var reqContent = reader.ReadToEnd();
                                _parent.Trace(reqContent);
                                returnContent = true;
                            }
                    }
                }
            }
            catch (WebException e)
            {
                _parent.Log("ERROR - EMBY  Command : " + cmd + " - " + e.Message);
                if (e.Status == WebExceptionStatus.Timeout)
                {

                    // _parent.MpcLoaded = false;
                }
            }
            return returnContent;
        }


        public string GetStatus()
        {
            HttpWebRequest request;
            var returnContent = "";

            var uri = @"http://" + _parent.IP + ":32400/system/players/"+_parent.ClientIPAddress+"/playback";
            try
            {
                request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
                request.Method = "GET";
                request.Timeout = 1000;
                _parent.Trace(uri);
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                            using (var reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                var reqContent = reader.ReadToEnd();
                                _parent.Trace(reqContent);
                                returnContent = reqContent;
                                returnContent = returnContent.Replace("OnStatus(", "").TrimEnd(')').Replace("'", "\"").Replace("\\\"", "'").Replace(", ", ",");
                            }
                    }
                }
            }
            catch (WebException e)
            {
                _parent.Log("ERROR - MPCCOMMAND : Status" + " - " + e.Message);
                if (e.Status == WebExceptionStatus.Timeout)
                {
                       _parent.Log("ERROR - MPCCOMMAND : Web Exception Thrown and MpcLoad now false");  
                      
                }
                
            }
            return returnContent;
        }
        public MpcHcRemote(Xbmc parent)
        {
            _parent = parent;
        }

        public void SetVolume(int volumepercent)
        {
            if (_parent.MpcLoaded)
                AsyncCommand("-2", "volume=" + Convert.ToString(volumepercent, CultureInfo.InvariantCulture));
        }

        public void SeekPercentage(int percent)
        {
            if (_parent.MpcLoaded)
                AsyncCommand("-1", "percent=" + Convert.ToString(percent, CultureInfo.InvariantCulture));
        }

        public void SkipPrevious()
        {
            if (_parent.MpcLoaded)
                AsyncCommand("920", "");
        }

        public void SkipNext()
        {
            if (_parent.MpcLoaded)
                AsyncCommand("921", "");
        }

        public void ToggleMute()
        {
            AsyncGeneralCommand("ToggleMute", "");
        }

        public void Return()
        {
            if (_parent.MpcLoaded)
                AsyncCommand("933", "");
        }

        public void Enter()
        {
            if (_parent.MpcLoaded)
                AsyncCommand("932", "");
        }

        public void Info()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("info");
        }

        public void Home()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("start");
        }

        public void Video()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("myvideo");
        }

        public void Music()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("mymusic");
        }

        public void Pictures()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("mypictures");
        }

        public void Tv()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("mytv");
        }

        public void VolUp()
        {
            if (_parent.MpcLoaded)
                AsyncCommand("907", "");
        }

        public void VolDown()
        {
            if (_parent.MpcLoaded)
                AsyncCommand("908", "");
        }

        public void Menu()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("menu");
        }
               
        public void Title()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("title");
        }

        public void Down()
        {
            if (_parent.MpcLoaded)
                AsyncCommand("931", "");
        }

        public void Up()
        {
            if (_parent.MpcLoaded)
                AsyncCommand("930", "");
        }

        public void Left()
        {
            if (_parent.MpcLoaded)
                AsyncCommand("928", "");
        }

        public void Right()
        {
            if (_parent.MpcLoaded)
                AsyncCommand("929", "");
        }

        public void Mute()
        {
            if (_parent.MpcLoaded)
                AsyncCommand("909", "");
        }

        public void PlayDrive()
        {
            if (_parent.MpcLoaded)
                AsyncCommand("951", "");
        }
                
        public void EjectDrive()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventAction("EjectTray");
        }

        public void Subtitles()
        {
            if (_parent.MpcLoaded)
                AsyncCommand("953", "");
        }

        public void Previous()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("skipPrevious");
        }

        public void Rewind()
        {
            if (_parent.MpcLoaded)
                AsyncCommand("stepBack", "");
        }

        public void Play()
        {
            //if (_parent.MpcLoaded)
               
            
            
            AsyncCommand("Unpause", "");
        }

        public void Pause()
        {
            AsyncCommand("Pause", "");
        }

        public void Stop()
        {
            //if (_parent.MpcLoaded)
                AsyncCommand("Stop", "");
        }

        public void Forward()
        {
            //if (_parent.MpcLoaded)
                AsyncCommand("stepForward", "");
        }

        public void Next()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("skipNext");
        }

        public void One()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("one");
        }

        public void Two()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("two");
        }

        public void Three()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("three");
        }

        public void Four()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("four");
        }

        public void Five()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("five");
        }

        public void Six()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("six");
        }

        public void Seven()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("seven");
        }

        public void Eight()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("eight");
        }

        public void Nine()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("nine");
        }

        public void Zero()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("zero");
        }

        public void Star()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("star");
        }

        public void Hash()
        {
            if (_parent.MpcLoaded)
                _parent.AsyncEventButton("hash");
        }

        public static void ParseCSVFields(ArrayList result, string data)
        {
            var pos = -1;
            while (pos < data.Length)
                result.Add(ParseCSVField(data, ref pos));
        }

        private static string ParseCSVField(string data, ref int startSeparatorPosition)
        {
            if (startSeparatorPosition == data.Length - 1)
            {
                startSeparatorPosition++;
                return "";
            }
            var fromPos = startSeparatorPosition + 1;
            if (data[fromPos] == '"')
            {
                if (fromPos == data.Length - 1)
                {
                    return "\"";
                }
                var nextSingleQuote = FindSingleQuote(data, fromPos + 1);
                startSeparatorPosition = nextSingleQuote + 1;
                return data.Substring(fromPos + 1, nextSingleQuote - fromPos - 1).Replace("\"\"", "\"");
            }
            var nextComma = data.IndexOf(',', fromPos);
            if (nextComma == -1)
            {
                startSeparatorPosition = data.Length;
                return data.Substring(fromPos);
            }
            startSeparatorPosition = nextComma;
            return data.Substring(fromPos, nextComma - fromPos);
        }

        private static int FindSingleQuote(string data, int startFrom)
        {
            var i = startFrom - 1;
            while (++i < data.Length)
                if (data[i] == '"')
                {
                    if (i < data.Length - 1 && data[i + 1] == '"')
                    {
                        i++;
                        continue;
                    }
                    return i;
                }
            return i;
        }
    }
}
