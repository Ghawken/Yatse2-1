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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Net;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.Script;
using Plugin;
//using Jayrock.Json;

using System.Xml;
using System.Xml.Serialization;
using System.Linq;


namespace Remote.Emby.Api
{
  class XbmcVideoLibrary : IApiVideoLibrary
  {
    private readonly Xbmc _parent;

    public XbmcVideoLibrary(Xbmc parent)
    {
      _parent = parent;
    }

    public Collection<ApiTvSeason> GetTvSeasons()
    {
      var seasons = new Collection<ApiTvSeason>();
    /*
      var properties = new Jayrock.Json.JsonArray(new[] { "title" });
      var param = new JsonObject();
      param["properties"] = properties;
      
        
      var result = (JsonObject)_parent.JsonCommand("VideoLibrary.GetTVShows", param);
      
        
      if (result != null)
      {
        if (result.Contains("tvshows"))
        {
          foreach (JsonObject show in (JsonArray)result["tvshows"])
          {
            var properties2 =
              new JsonArray(new[] { "tvshowid", "fanart", "thumbnail", "season", "showtitle", "episode" });
            var param2 = new JsonObject();
            param2["properties"] = properties2;
            param2["tvshowid"] = (long)(JsonNumber)show["tvshowid"];
            var result2 = (JsonObject)_parent.JsonCommand("VideoLibrary.GetSeasons", param2);
            if (result2 == null) continue;
            if (!result2.Contains("seasons")) continue;
            foreach (JsonObject genre in (JsonArray)result2["seasons"])
            {
              try
              {
                var tvShow = new ApiTvSeason
                  {
                    SeasonNumber = (long)(JsonNumber)genre["season"],
                    IdShow = (long)(JsonNumber)genre["tvshowid"],
                    Show = genre["showtitle"].ToString(),
                    Thumb = genre["thumbnail"].ToString(),
                    EpisodeCount = (long)(JsonNumber)genre["episode"],
                    Fanart = genre["fanart"].ToString(),
                    Hash = Xbmc.Hash(genre["thumbnail"].ToString())
                  };
                seasons.Add(tvShow);
              }
              catch (Exception)
              {
              }
            }
          }
        }
      }
    */
      return seasons;
    }

    public Collection<ApiTvEpisode> GetTvEpisodes()
    {
      var episodes = new Collection<ApiTvEpisode>();
      /*
      var properties = new JsonArray(new[] { "title", "plot", "season", "episode", "showtitle", "tvshowid", "fanart", "thumbnail", "rating", "playcount", "firstaired" });
      var param = new JsonObject();
      param["properties"] = properties;
      var result = (JsonObject)_parent.JsonCommand("VideoLibrary.GetEpisodes", param);
      if (result != null)
      {
        if (result.Contains("episodes"))
        {
          foreach (JsonObject genre in (JsonArray)result["episodes"])
          {
            try
            {
              var tvShow = new ApiTvEpisode
                {
                  Title = genre["title"].ToString(),
                  Plot = genre["plot"].ToString(),
                  Rating = genre["rating"].ToString(),
                  Mpaa = "",
                  Date = genre["firstaired"].ToString(),
                  Director = "",
                  PlayCount = 0,
                  Studio = "",
                  IdEpisode = (long)(JsonNumber)genre["episodeid"],
                  IdShow = (long)(JsonNumber)genre["tvshowid"],
                  Season = (long)(JsonNumber)genre["season"],
                  Episode = (long)(JsonNumber)genre["episode"],
                  Path = "",
                  ShowTitle = genre["showtitle"].ToString(),
                  Thumb = genre["thumbnail"].ToString(),
                  Fanart = genre["fanart"].ToString(),
                  Hash = Xbmc.Hash(genre["thumbnail"].ToString())
                };
              episodes.Add(tvShow);
            }
            catch (Exception)
            {
            }
          }
        }
      }
        */
      return episodes;
    }

    public Collection<ApiTvShow> GetTvShows()
    {
     
        var shows = new Collection<ApiTvShow>();
     /*
      var properties = new JsonArray(new[] { "title", "plot", "genre", "fanart", "thumbnail", "rating", "mpaa", "studio", "playcount", "premiered", "episode" });
      var param = new JsonObject();
      param["properties"] = properties;
      var result = (JsonObject)_parent.JsonCommand("VideoLibrary.GetTVShows", param);
      if (result != null)
      {
        if (result.Contains("tvshows"))
        {
          foreach (JsonObject genre in (JsonArray)result["tvshows"])
          {
            try
            {
              var tvShow = new ApiTvShow
                {
                  Title = genre["title"].ToString(),
                  Plot = genre["plot"].ToString(),
                  Rating = genre["rating"].ToString(),
                  IdScraper = "",
                  Mpaa = genre["mpaa"].ToString(),
                  Genre = _parent.JsonArrayToString((JsonArray)genre["genre"]),
                  Studio = _parent.JsonArrayToString((JsonArray)genre["studio"]),
                  IdShow = (long)(JsonNumber)genre["tvshowid"],
                  TotalCount = (long)(JsonNumber)genre["episode"],
                  Path = "",
                  Premiered = genre["premiered"].ToString(),
                  Thumb = genre["thumbnail"].ToString(),
                  Fanart = genre["fanart"].ToString(),
                  Hash = Xbmc.Hash(genre["thumbnail"].ToString())
                };
              shows.Add(tvShow);
            }
            catch (Exception)
            {
            }
          }
        }
      }
      */
      return shows;
    }

    public string GetMainSelection(string param)
    {
        try
        {

            _parent.Log("Getting Main Selection Result" + _parent.IP);
            string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items";

            var request = WebRequest.CreateHttp(NPurl);

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
                   
                    var ItemData = deserializer.Deserialize<MainSelection.Root>(json);
                    _parent.Log("---------------Get Main Selection:  Issue: Results.Count: " + ItemData.Items.Count);

                    foreach (var id in ItemData.Items)
                    {
                       
                        
                        if (id.Name == param)
                        {
                            _parent.Log("----------- Get Main Selection Run ---" + param + " ID Result equals:  " + id.Id);
                            return id.Id;
                        }
                    }

                }
            }


            return null;
        }
        catch (Exception ex)
        {
            _parent.Log("ERROR in Main Selection obtaining: "+ex);
            return "";

        }
    }
    public SingleMovieItem.Rootobject GetSingleMovieItem(string itemId)
    {
        try
        {

            _parent.Log("Getting Single Movie Data" + _parent.IP);
            string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items/"+itemId;

            var request = WebRequest.CreateHttp(NPurl);

            request.Method = "get";
            request.Timeout = 5000;
            _parent.Log("Single Movie Selection: " + _parent.IP + ":" + _parent.Port);

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
                    _parent.Log("--------------GETTING Single Movie Selection Result ------" + json);

                    var deserializer = new JavaScriptSerializer();

                    var ItemData = deserializer.Deserialize<SingleMovieItem.Rootobject>(json);
                    _parent.Log("---------------Get Single Movie Selection:  Issue: Results.Taglines: " + ItemData.Taglines);

                    return ItemData;

                }
            }


            return null;
        }
        catch (Exception ex)
        {
            _parent.Log("ERROR in Single Movie Selection obtaining: " + ex);
            return null;

        }
    }

    public Collection<ApiMovie> GetMovies()
    {
      var movies = new Collection<ApiMovie>();
      var MovieId = GetMainSelection("Movies");

      try
      {

          _parent.Log("Getting Main Movie Database Result" + _parent.IP);
          string NPurl = "http://" + _parent.IP + ":" + _parent.Port + "/emby/Users/" + Globals.CurrentUserID + "/Items?ParentId=" + MovieId;

          var request = WebRequest.CreateHttp(NPurl);

          request.Method = "get";
          request.Timeout = 20000;
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
                  _parent.Log("--------------GETTING All Movies Results ------" + json);

                  var deserializer = new JavaScriptSerializer();

                  var ItemData = deserializer.Deserialize<Movies.Rootobject>(json);

                  _parent.Log("---------------Get Worlds Result:  Issue: Results.Count: " + ItemData.Items.Count);

                  foreach (var id in ItemData.Items)
                  {
                      SingleMovieItem.Rootobject Movieitem = GetSingleMovieItem(id.Id);
                      
                      string newDirector = Movieitem.People.First(i => i.Type == "Director").ToString();
                      
                      var movie = new ApiMovie
                      {
                          Title = Movieitem.Name,
                          Plot = Movieitem.Overview,
                          Votes = Movieitem.VoteCount.ToString(),
                          Rating = id.OfficialRating,
                          Year = id.ProductionYear,
                          Tagline = Movieitem.Taglines.FirstOrDefault(),
                          IdScraper = Movieitem.ProviderIds.Imdb,
                          Length = id.RunTimeTicks.ToString(),
                          Mpaa = id.OfficialRating,
                          Genre = Movieitem.Genres.FirstOrDefault(),
                          Director = newDirector,
                          OriginalTitle = id.Name,
                          Studio = Movieitem.Studios.FirstOrDefault().ToString(),
                          IdFile = 0,
                          IdMovie = 123,
                          FileName = Movieitem.Path,
                          Path = Movieitem.Path,
                          PlayCount = Movieitem.UserData.PlayCount,
                          Thumb = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + id.Id + "/Images/Primary",
                          Fanart = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + id.Id + "/Images/Backdrop",
                          Hash = Xbmc.Hash(id.Id)
                      };
                      movies.Add(movie);

                  }

                  /*
                                    _nowPlaying.FanartURL = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.PrimaryItemId + "/Images/Backdrop";
                                    _nowPlaying.ThumbURL = "http://" + _parent.IP + ":" + _parent.Port + "/Items/" + server.PrimaryItemId + "/Images/Primary";
                  */

              }

          }


      }
      catch (Exception ex)
      {
          _parent.Log("ERROR in Main Movies obtaining: " + ex);


      }


      /*

      var properties = new JsonArray(new[] { "title", "plot", "genre", "year", "fanart", "thumbnail", "playcount", "studio", "rating", "runtime", "mpaa", "originaltitle", "director", "votes" });
      var param = new JsonObject();
      param["properties"] = properties;
      var result = (JsonObject)_parent.JsonCommand("VideoLibrary.GetMovies", param);
      if (result != null)
      {
        if (result.Contains("movies"))
        {
          foreach (JsonObject genre in (JsonArray)result["movies"])
          {
            try
            {
              var t = TimeSpan.FromSeconds((long)(JsonNumber)genre["runtime"]);
              var duration = string.Format("{0:D2}:{1:D2}", t.Hours, t.Minutes);
              var movie = new ApiMovie
                {

                  Title = genre["title"].ToString(),
                  Plot = genre["plot"].ToString(),
                  Votes = genre["votes"].ToString(),
                  Rating = genre["rating"].ToString(),
                  Year = (long)(JsonNumber)genre["year"],
                  IdScraper = "",
                  Length = duration,
                  Mpaa = genre["mpaa"].ToString(),
                  Genre = _parent.JsonArrayToString((JsonArray)genre["genre"]),
                  Director = _parent.JsonArrayToString((JsonArray)genre["director"]),
                  OriginalTitle = genre["originaltitle"].ToString(),
                  Studio = _parent.JsonArrayToString((JsonArray)genre["studio"]),
                  IdFile = 0,
                  IdMovie = (long)(JsonNumber)genre["movieid"],
                  FileName = "",
                  Path = "",
                  PlayCount = 0,
                  Thumb = genre["thumbnail"].ToString(),
                  Fanart = genre["fanart"].ToString(),
                  Hash = Xbmc.Hash(genre["thumbnail"].ToString())
                };
              movies.Add(movie);
            }
            catch (Exception)
            {
            }
          }
        }
      }
     */
      return movies;
    }
  }
}
