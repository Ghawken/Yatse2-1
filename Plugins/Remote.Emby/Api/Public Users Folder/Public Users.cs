using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Emby.Api.Public_Users_Folder
{
    public class PublicUsers
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto", IsNullable = false)]
        public partial class ArrayOfUserDto
        {

            private ArrayOfUserDtoUserDto[] userDtoField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("UserDto")]
            public ArrayOfUserDtoUserDto[] UserDto
            {
                get
                {
                    return this.userDtoField;
                }
                set
                {
                    this.userDtoField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto")]
        public partial class ArrayOfUserDtoUserDto
        {

            private ArrayOfUserDtoUserDtoConfiguration configurationField;

            private string connectLinkTypeField;

            private string connectUserIdField;

            private string connectUserNameField;

            private bool hasConfiguredEasyPasswordField;

            private bool hasConfiguredPasswordField;

            private bool hasPasswordField;

            private string idField;

            private System.DateTime lastActivityDateField;

            private System.DateTime lastLoginDateField;

            private string nameField;

            private object offlinePasswordField;

            private object offlinePasswordSaltField;

            private ArrayOfUserDtoUserDtoPolicy policyField;

            private string primaryImageAspectRatioField;

            private string primaryImageTagField;

            private string serverIdField;

            private object serverNameField;

            /// <remarks/>
            public ArrayOfUserDtoUserDtoConfiguration Configuration
            {
                get
                {
                    return this.configurationField;
                }
                set
                {
                    this.configurationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string ConnectLinkType
            {
                get
                {
                    return this.connectLinkTypeField;
                }
                set
                {
                    this.connectLinkTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string ConnectUserId
            {
                get
                {
                    return this.connectUserIdField;
                }
                set
                {
                    this.connectUserIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string ConnectUserName
            {
                get
                {
                    return this.connectUserNameField;
                }
                set
                {
                    this.connectUserNameField = value;
                }
            }

            /// <remarks/>
            public bool HasConfiguredEasyPassword
            {
                get
                {
                    return this.hasConfiguredEasyPasswordField;
                }
                set
                {
                    this.hasConfiguredEasyPasswordField = value;
                }
            }

            /// <remarks/>
            public bool HasConfiguredPassword
            {
                get
                {
                    return this.hasConfiguredPasswordField;
                }
                set
                {
                    this.hasConfiguredPasswordField = value;
                }
            }

            /// <remarks/>
            public bool HasPassword
            {
                get
                {
                    return this.hasPasswordField;
                }
                set
                {
                    this.hasPasswordField = value;
                }
            }

            /// <remarks/>
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            public System.DateTime LastActivityDate
            {
                get
                {
                    return this.lastActivityDateField;
                }
                set
                {
                    this.lastActivityDateField = value;
                }
            }

            /// <remarks/>
            public System.DateTime LastLoginDate
            {
                get
                {
                    return this.lastLoginDateField;
                }
                set
                {
                    this.lastLoginDateField = value;
                }
            }

            /// <remarks/>
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object OfflinePassword
            {
                get
                {
                    return this.offlinePasswordField;
                }
                set
                {
                    this.offlinePasswordField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object OfflinePasswordSalt
            {
                get
                {
                    return this.offlinePasswordSaltField;
                }
                set
                {
                    this.offlinePasswordSaltField = value;
                }
            }

            /// <remarks/>
            public ArrayOfUserDtoUserDtoPolicy Policy
            {
                get
                {
                    return this.policyField;
                }
                set
                {
                    this.policyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string PrimaryImageAspectRatio
            {
                get
                {
                    return this.primaryImageAspectRatioField;
                }
                set
                {
                    this.primaryImageAspectRatioField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string PrimaryImageTag
            {
                get
                {
                    return this.primaryImageTagField;
                }
                set
                {
                    this.primaryImageTagField = value;
                }
            }

            /// <remarks/>
            public string ServerId
            {
                get
                {
                    return this.serverIdField;
                }
                set
                {
                    this.serverIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object ServerName
            {
                get
                {
                    return this.serverNameField;
                }
                set
                {
                    this.serverNameField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto")]
        public partial class ArrayOfUserDtoUserDtoConfiguration
        {

            private string audioLanguagePreferenceField;

            private bool displayCollectionsViewField;

            private bool displayFoldersViewField;

            private bool displayMissingEpisodesField;

            private bool displayUnairedEpisodesField;

            private bool enableCinemaModeField;

            private bool enableLocalPasswordField;

            private object excludeFoldersFromGroupingField;

            private bool groupMoviesIntoBoxSetsField;

            private object groupedFoldersField;

            private bool hidePlayedInLatestField;

            private bool includeTrailersInSuggestionsField;

            private object latestItemsExcludesField;

            private object orderedViewsField;

            private object plainFolderViewsField;

            private bool playDefaultAudioTrackField;

            private string subtitleLanguagePreferenceField;

            private string subtitleModeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration", IsNullable = true)]
            public string AudioLanguagePreference
            {
                get
                {
                    return this.audioLanguagePreferenceField;
                }
                set
                {
                    this.audioLanguagePreferenceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool DisplayCollectionsView
            {
                get
                {
                    return this.displayCollectionsViewField;
                }
                set
                {
                    this.displayCollectionsViewField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool DisplayFoldersView
            {
                get
                {
                    return this.displayFoldersViewField;
                }
                set
                {
                    this.displayFoldersViewField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool DisplayMissingEpisodes
            {
                get
                {
                    return this.displayMissingEpisodesField;
                }
                set
                {
                    this.displayMissingEpisodesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool DisplayUnairedEpisodes
            {
                get
                {
                    return this.displayUnairedEpisodesField;
                }
                set
                {
                    this.displayUnairedEpisodesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool EnableCinemaMode
            {
                get
                {
                    return this.enableCinemaModeField;
                }
                set
                {
                    this.enableCinemaModeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool EnableLocalPassword
            {
                get
                {
                    return this.enableLocalPasswordField;
                }
                set
                {
                    this.enableLocalPasswordField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration", IsNullable = true)]
            public object ExcludeFoldersFromGrouping
            {
                get
                {
                    return this.excludeFoldersFromGroupingField;
                }
                set
                {
                    this.excludeFoldersFromGroupingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool GroupMoviesIntoBoxSets
            {
                get
                {
                    return this.groupMoviesIntoBoxSetsField;
                }
                set
                {
                    this.groupMoviesIntoBoxSetsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public object GroupedFolders
            {
                get
                {
                    return this.groupedFoldersField;
                }
                set
                {
                    this.groupedFoldersField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool HidePlayedInLatest
            {
                get
                {
                    return this.hidePlayedInLatestField;
                }
                set
                {
                    this.hidePlayedInLatestField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool IncludeTrailersInSuggestions
            {
                get
                {
                    return this.includeTrailersInSuggestionsField;
                }
                set
                {
                    this.includeTrailersInSuggestionsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public object LatestItemsExcludes
            {
                get
                {
                    return this.latestItemsExcludesField;
                }
                set
                {
                    this.latestItemsExcludesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public object OrderedViews
            {
                get
                {
                    return this.orderedViewsField;
                }
                set
                {
                    this.orderedViewsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public object PlainFolderViews
            {
                get
                {
                    return this.plainFolderViewsField;
                }
                set
                {
                    this.plainFolderViewsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool PlayDefaultAudioTrack
            {
                get
                {
                    return this.playDefaultAudioTrackField;
                }
                set
                {
                    this.playDefaultAudioTrackField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration", IsNullable = true)]
            public string SubtitleLanguagePreference
            {
                get
                {
                    return this.subtitleLanguagePreferenceField;
                }
                set
                {
                    this.subtitleLanguagePreferenceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public string SubtitleMode
            {
                get
                {
                    return this.subtitleModeField;
                }
                set
                {
                    this.subtitleModeField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto")]
        public partial class ArrayOfUserDtoUserDtoPolicy
        {

            private object accessSchedulesField;

            private object blockUnratedItemsField;

            private object blockedChannelsField;

            private object blockedMediaFoldersField;

            private object blockedTagsField;

            private bool enableAllChannelsField;

            private bool enableAllDevicesField;

            private bool enableAllFoldersField;

            private bool enableAudioPlaybackTranscodingField;

            private bool enableContentDeletionField;

            private bool enableContentDownloadingField;

            private bool enableLiveTvAccessField;

            private bool enableLiveTvManagementField;

            private bool enableMediaPlaybackField;

            private bool enablePublicSharingField;

            private bool enableRemoteControlOfOtherUsersField;

            private bool enableSharedDeviceControlField;

            private bool enableSyncField;

            private bool enableSyncTranscodingField;

            private bool enableUserPreferenceAccessField;

            private bool enableVideoPlaybackTranscodingField;

            private object enabledChannelsField;

            private object enabledDevicesField;

            private string[] enabledFoldersField;

            private byte invalidLoginAttemptCountField;

            private bool isAdministratorField;

            private bool isDisabledField;

            private bool isHiddenField;

            private object maxParentalRatingField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public object AccessSchedules
            {
                get
                {
                    return this.accessSchedulesField;
                }
                set
                {
                    this.accessSchedulesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public object BlockUnratedItems
            {
                get
                {
                    return this.blockUnratedItemsField;
                }
                set
                {
                    this.blockUnratedItemsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users", IsNullable = true)]
            public object BlockedChannels
            {
                get
                {
                    return this.blockedChannelsField;
                }
                set
                {
                    this.blockedChannelsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users", IsNullable = true)]
            public object BlockedMediaFolders
            {
                get
                {
                    return this.blockedMediaFoldersField;
                }
                set
                {
                    this.blockedMediaFoldersField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public object BlockedTags
            {
                get
                {
                    return this.blockedTagsField;
                }
                set
                {
                    this.blockedTagsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableAllChannels
            {
                get
                {
                    return this.enableAllChannelsField;
                }
                set
                {
                    this.enableAllChannelsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableAllDevices
            {
                get
                {
                    return this.enableAllDevicesField;
                }
                set
                {
                    this.enableAllDevicesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableAllFolders
            {
                get
                {
                    return this.enableAllFoldersField;
                }
                set
                {
                    this.enableAllFoldersField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableAudioPlaybackTranscoding
            {
                get
                {
                    return this.enableAudioPlaybackTranscodingField;
                }
                set
                {
                    this.enableAudioPlaybackTranscodingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableContentDeletion
            {
                get
                {
                    return this.enableContentDeletionField;
                }
                set
                {
                    this.enableContentDeletionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableContentDownloading
            {
                get
                {
                    return this.enableContentDownloadingField;
                }
                set
                {
                    this.enableContentDownloadingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableLiveTvAccess
            {
                get
                {
                    return this.enableLiveTvAccessField;
                }
                set
                {
                    this.enableLiveTvAccessField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableLiveTvManagement
            {
                get
                {
                    return this.enableLiveTvManagementField;
                }
                set
                {
                    this.enableLiveTvManagementField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableMediaPlayback
            {
                get
                {
                    return this.enableMediaPlaybackField;
                }
                set
                {
                    this.enableMediaPlaybackField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnablePublicSharing
            {
                get
                {
                    return this.enablePublicSharingField;
                }
                set
                {
                    this.enablePublicSharingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableRemoteControlOfOtherUsers
            {
                get
                {
                    return this.enableRemoteControlOfOtherUsersField;
                }
                set
                {
                    this.enableRemoteControlOfOtherUsersField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableSharedDeviceControl
            {
                get
                {
                    return this.enableSharedDeviceControlField;
                }
                set
                {
                    this.enableSharedDeviceControlField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableSync
            {
                get
                {
                    return this.enableSyncField;
                }
                set
                {
                    this.enableSyncField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableSyncTranscoding
            {
                get
                {
                    return this.enableSyncTranscodingField;
                }
                set
                {
                    this.enableSyncTranscodingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableUserPreferenceAccess
            {
                get
                {
                    return this.enableUserPreferenceAccessField;
                }
                set
                {
                    this.enableUserPreferenceAccessField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableVideoPlaybackTranscoding
            {
                get
                {
                    return this.enableVideoPlaybackTranscodingField;
                }
                set
                {
                    this.enableVideoPlaybackTranscodingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public object EnabledChannels
            {
                get
                {
                    return this.enabledChannelsField;
                }
                set
                {
                    this.enabledChannelsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public object EnabledDevices
            {
                get
                {
                    return this.enabledDevicesField;
                }
                set
                {
                    this.enabledDevicesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            [System.Xml.Serialization.XmlArrayItemAttribute(Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays", IsNullable = false)]
            public string[] EnabledFolders
            {
                get
                {
                    return this.enabledFoldersField;
                }
                set
                {
                    this.enabledFoldersField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public byte InvalidLoginAttemptCount
            {
                get
                {
                    return this.invalidLoginAttemptCountField;
                }
                set
                {
                    this.invalidLoginAttemptCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool IsAdministrator
            {
                get
                {
                    return this.isAdministratorField;
                }
                set
                {
                    this.isAdministratorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool IsDisabled
            {
                get
                {
                    return this.isDisabledField;
                }
                set
                {
                    this.isDisabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool IsHidden
            {
                get
                {
                    return this.isHiddenField;
                }
                set
                {
                    this.isHiddenField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users", IsNullable = true)]
            public object MaxParentalRating
            {
                get
                {
                    return this.maxParentalRatingField;
                }
                set
                {
                    this.maxParentalRatingField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users", IsNullable = false)]
        public partial class EnabledFolders
        {

            private string[] stringField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("string", Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
            public string[] @string
            {
                get
                {
                    return this.stringField;
                }
                set
                {
                    this.stringField = value;
                }
            }
        }




    }
}
