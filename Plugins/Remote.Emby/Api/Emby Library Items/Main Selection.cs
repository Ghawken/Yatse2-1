using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Emby.Api.MainSelection
{
    public class UserData
    {
        public double PlayedPercentage { get; set; }
        public int UnplayedItemCount { get; set; }
        public int PlaybackPositionTicks { get; set; }
        public int PlayCount { get; set; }
        public bool IsFavorite { get; set; }
        public bool Played { get; set; }
        public string Key { get; set; }
    }

    public class ImageTags
    {
        public string Primary { get; set; }
        public string Disc { get; set; }
        public string Thumb { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public string ServerId { get; set; }
        public string Id { get; set; }
        public bool HasDynamicCategories { get; set; }
        public string PlayAccess { get; set; }
        public bool IsFolder { get; set; }
        public string Type { get; set; }
        public UserData UserData { get; set; }
        public int RecursiveItemCount { get; set; }
        public int ChildCount { get; set; }
        public string CollectionType { get; set; }
        public ImageTags ImageTags { get; set; }
        public IList<string> BackdropImageTags { get; set; }
        public string LocationType { get; set; }
    }

    public class Root
    {
        public IList<Item> Items { get; set; }
        public int TotalRecordCount { get; set; }
    }

}
