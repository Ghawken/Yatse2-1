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

using System.Collections.ObjectModel;
using Plugin;
using Jayrock.Json;
using System;
using System.Net;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.Script;

namespace Remote.Emby.Api
{
  class XbmcAudioLibrary : IApiAudioLibrary
  {
    private readonly Xbmc _parent;

    public XbmcAudioLibrary(Xbmc parent)
    {
      _parent = parent;
    }
    public string GetMainSelection(string param)
    {
        try
        {
            _parent.Log("Getting Music Selection Result" + _parent.IP);
            string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Views";
            var request = WebRequest.CreateHttp(NPurl);
            var MusicID = "";
            request.Method = "get";
            request.Timeout = 10000;
            _parent.Log("Main Selection: " + _parent.IP + ":" + _parent.Port);
            var authString = _parent.GetAuthString();
            request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
            request.Headers.Add("X-Emby-Authorization", authString);
            request.ContentType = "application/json; charset=utf-8";
            request.Accept = "application/json; charset=utf-8";
            var response = request.GetResponse();
            if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
            {
                System.IO.Stream dataStream = response.GetResponseStream();
                System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
                using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    string json = sr.ReadToEnd();
                    _parent.Log("--------------GETTING Main Selection Result ------" + json);
                    var deserializer = new JavaScriptSerializer();
                    var ItemData = deserializer.Deserialize<MainSelectionforMusic.Rootobject>(json);
                    _parent.Log("---------------Get Main Selection:  Issue: Results.Count: " + ItemData.Items.Length);
                    foreach (var id in ItemData.Items)
                    {
                        if (id.Name == "Music")
                        {
                            _parent.Log("----------- Get Main Selection Run ---" + param + " ID Result equals:  " + id.Id);
                            MusicID = id.Id;
                        }
                    }

                }
            }

            // Do again to get Album, Genre etc results
            // these come from param - above is fixed to Music
            // Options to pass are Latest, Playlists, Albums, Album Artists, Songs, Genres
            _parent.Log("Getting Music Next Selection  Result" + _parent.IP);
            NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?parentId="+ MusicID;
            var request2 = WebRequest.CreateHttp(NPurl);
            request2.Method = "get";
            request2.Timeout = 10000;
            _parent.Log("Main Selection: " + _parent.IP + ":" + _parent.Port);
            //var authString = _parent.GetAuthString();
            request2.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
            request2.Headers.Add("X-Emby-Authorization", authString);
            request2.ContentType = "application/json; charset=utf-8";
            request2.Accept = "application/json; charset=utf-8";
            var response2 = request2.GetResponse();
            if (((HttpWebResponse)response2).StatusCode == HttpStatusCode.OK)
            {
                System.IO.Stream dataStream = response2.GetResponseStream();
                System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
                using (var sr = new System.IO.StreamReader(response2.GetResponseStream()))
                {
                    string json = sr.ReadToEnd();
                    _parent.Log("--------------GETTING Music Next Selection Result ------" + json);
                    var deserializer = new JavaScriptSerializer();
                    var ItemData = deserializer.Deserialize<MusicSelection.Rootobject>(json);
                    _parent.Log("---------------Get Next  Selection:  Issue: Results.Count: " + ItemData.TotalRecordCount);
                    foreach (var id in ItemData.Items)
                    {
                        if (id.Name == param)
                        {
                            _parent.Log("----------- Next Music Next Selection Run ---" + param + " ID Result equals:  " + id.Id);
                            return id.Id; 
                        }
                    }

                }
            }




            return null;
        }
        catch (Exception ex)
        {
            _parent.Log("ERROR in Main Music Selection obtaining: " + ex);
            return "";

        }
    }
    public Collection<ApiAudioGenre> GetGenres()
    {

      var genreID = GetMainSelection("Genres"); 
      var genres = new Collection<ApiAudioGenre>();
     
      try
      {
          _parent.Log("Getting Music Genres: Parent IP: " + _parent.IP);
          string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?ParentId="+genreID;
          var request = WebRequest.CreateHttp(NPurl);
          request.Method = "get";
          request.Timeout = 5000;
          _parent.Log("Genre Music Selection: " + _parent.IP + ":" + _parent.Port);
          var authString = _parent.GetAuthString();
          request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
          request.Headers.Add("X-Emby-Authorization", authString);
          request.ContentType = "application/json; charset=utf-8";
          request.Accept = "application/json; charset=utf-8";

          var response = request.GetResponse();

          if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
          {

              System.IO.Stream dataStream = response.GetResponseStream();
              System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

              using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
              {
                  string json = sr.ReadToEnd();
                  _parent.Log("--------------GETTING Music Genres Selection Result ------" + json);

                  var deserializer = new JavaScriptSerializer();
                  var ItemData = deserializer.Deserialize<MusicGenres.Rootobject>(json);
                  _parent.Log("---------------Get Music Genres:  Issue: Results.Record Count: " + ItemData.TotalRecordCount);

                  foreach (var genre in ItemData.Items)
                  {
                      try
                      {
                          var gen = new ApiAudioGenre
                          {
                              IdGenre = Xbmc.IDtoNumber(genre.Id),
                              Name = genre.Name ?? "",
                              AlbumCount = genre.ChildCount,
                              Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Primary" ?? ""
                          };
                          genres.Add(gen);
                      }
                      catch (Exception ex)
                      {
                          _parent.Log("Music Genres Exception Caught " + ex);
                      }
                  }

              }
          }
      }
      catch (Exception Ex)
      {
          _parent.Log("Another Music Genres exception" + Ex);
      }
      return genres;
    }

    public Collection<ApiAudioArtist> GetArtists()
    {
      var artists = new Collection<ApiAudioArtist>();


          var AlbumArtistsID = GetMainSelection("Album Artists");
         

          try
          {
              _parent.Log("Getting Album ARtists: Parent IP: " + _parent.IP);
              string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?ParentId=" + AlbumArtistsID;
              var request = WebRequest.CreateHttp(NPurl);
              request.Method = "get";
              request.Timeout = 5000;
              _parent.Log("Genre Music Selection: " + _parent.IP + ":" + _parent.Port);
              var authString = _parent.GetAuthString();
              request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
              request.Headers.Add("X-Emby-Authorization", authString);
              request.ContentType = "application/json; charset=utf-8";
              request.Accept = "application/json; charset=utf-8";

              var response = request.GetResponse();

              if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
              {

                  System.IO.Stream dataStream = response.GetResponseStream();
                  System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);

                  using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                  {
                      string json = sr.ReadToEnd();
                      _parent.Log("--------------GETTING Album Artists Genres Selection Result ------" + json);

                      var deserializer = new JavaScriptSerializer();
                      var ItemData = deserializer.Deserialize<AlbumArtists.Rootobject>(json);
                      _parent.Log("---------------Get Album Artists :  Issue: Results.Record Count: " + ItemData.TotalRecordCount);

                      foreach (var genre in ItemData.Items)
                      {
                          _parent.Trace("-----------  Get ALbum ARtists : ID " + genre.Id);
                          _parent.Trace("------------- Get ALbum ARtists :Name " + genre.Name);
                          
                          try
                          {
                              var artist = new ApiAudioArtist
                                {
                                    IdArtist = Xbmc.IDtoNumber(genre.Id),
                                    Name = genre.Name ?? "",
                                    Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Primary" ?? "",
                                    Fanart = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + genre.Id + "/Images/Backdrop" ?? "",
                                    Biography = ""
                                };
                              artists.Add(artist);
                          }

                          catch (Exception ex)
                          {
                              _parent.Log("Music Genres Exception Caught " + ex);
                          }
                      }

                  }
              }
          }
          catch (Exception Ex)
          {
              _parent.Log("Another Album Artists  exception" + Ex);
          }
          return artists;
      }




    public Collection<ApiAudioAlbum> GetAlbums()
    {
      var albums = new Collection<ApiAudioAlbum>();

      var properties = new JsonArray(new[] { "title", "thumbnail", "genre", "genreid", "artist", "year" });
      var param = new JsonObject();
      param["properties"] = properties;
      var result = (JsonObject)_parent.JsonCommand("AudioLibrary.GetAlbums", param);
      if (result != null)
      {
        if (result.Contains("albums"))
        {
          foreach (JsonObject genre in (JsonArray)result["albums"])
          {
            try
            {
              var album = new ApiAudioAlbum
                {
                  IdAlbum = (long)(JsonNumber)genre["albumid"],
                  Title = genre["title"].ToString(),
                  IdGenre = 0,
                  IdArtist = 0,
                  Artist = _parent.JsonArrayToString((JsonArray)genre["artist"]),
                  Genre = _parent.JsonArrayToString((JsonArray)genre["genre"]),
                  Year = (long)(JsonNumber)genre["year"],
                  Thumb = genre["thumbnail"].ToString()
                };
              albums.Add(album);
            }
            catch (Exception)
            {
            }
          }
        }
      }
      return albums;
    }

    public Collection<ApiAudioSong> GetSongs()
    {
      var songs = new Collection<ApiAudioSong>();

      var properties = new JsonArray(new[] { "title", "thumbnail", "genre", "genreid", "artist", "year", "duration", "album", "albumid", "track" });
      var param = new JsonObject();
      param["properties"] = properties;
      var result = (JsonObject)_parent.JsonCommand("AudioLibrary.GetSongs", param);
      if (result != null)
      {
        if (result.Contains("songs"))
        {
          foreach (JsonObject genre in (JsonArray)result["songs"])
          {
            try
            {
              var song = new ApiAudioSong
                {
                  IdSong = (long)(JsonNumber)genre["songid"],
                  Title = genre["title"].ToString(),
                  Track = (long)(JsonNumber)genre["track"],
                  Duration = (long)(JsonNumber)genre["duration"],
                  Year = (long)(JsonNumber)genre["year"],
                  FileName = "",
                  IdAlbum = (long)(JsonNumber)genre["albumid"],
                  Album = genre["album"].ToString(),
                  Path = "",
                  IdArtist = 0,
                  Artist = _parent.JsonArrayToString((JsonArray)genre["artist"]),
                  IdGenre = 0,
                  Genre = _parent.JsonArrayToString((JsonArray)genre["genre"]),
                  Thumb = genre["thumbnail"].ToString(),
                };
              songs.Add(song);
            }
            catch (Exception)
            {
            }
          }
        }
      }

      return songs;
    }

  }
}
