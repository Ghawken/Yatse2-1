using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Emby.Api.Sessions
{
    public class Sessions
    {


        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session", IsNullable = false)]
        public partial class ArrayOfSessionInfoDto
        {

            private ArrayOfSessionInfoDtoSessionInfoDto[] sessionInfoDtoField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("SessionInfoDto")]
            public ArrayOfSessionInfoDtoSessionInfoDto[] SessionInfoDto
            {
                get
                {
                    return this.sessionInfoDtoField;
                }
                set
                {
                    this.sessionInfoDtoField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session")]
        public partial class ArrayOfSessionInfoDtoSessionInfoDto
        {

            private object additionalUsersField;

            private object appIconUrlField;

            private string applicationVersionField;

            private string clientField;

            private string deviceIdField;

            private string deviceNameField;

            private string idField;

            private System.DateTime lastActivityDateField;

            private object nowPlayingItemField;

            private object nowViewingItemField;

            private ArrayOfSessionInfoDtoSessionInfoDtoPlayState playStateField;

            private string[] playableMediaTypesField;

            private object queueableMediaTypesField;

            private string[] supportedCommandsField;

            private bool supportsRemoteControlField;

            private object transcodingInfoField;

            private string userIdField;

            private string userNameField;

            private string userPrimaryImageTagField;

            /// <remarks/>
            public object AdditionalUsers
            {
                get
                {
                    return this.additionalUsersField;
                }
                set
                {
                    this.additionalUsersField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object AppIconUrl
            {
                get
                {
                    return this.appIconUrlField;
                }
                set
                {
                    this.appIconUrlField = value;
                }
            }

            /// <remarks/>
            public string ApplicationVersion
            {
                get
                {
                    return this.applicationVersionField;
                }
                set
                {
                    this.applicationVersionField = value;
                }
            }

            /// <remarks/>
            public string Client
            {
                get
                {
                    return this.clientField;
                }
                set
                {
                    this.clientField = value;
                }
            }

            /// <remarks/>
            public string DeviceId
            {
                get
                {
                    return this.deviceIdField;
                }
                set
                {
                    this.deviceIdField = value;
                }
            }

            /// <remarks/>
            public string DeviceName
            {
                get
                {
                    return this.deviceNameField;
                }
                set
                {
                    this.deviceNameField = value;
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
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object NowPlayingItem
            {
                get
                {
                    return this.nowPlayingItemField;
                }
                set
                {
                    this.nowPlayingItemField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object NowViewingItem
            {
                get
                {
                    return this.nowViewingItemField;
                }
                set
                {
                    this.nowViewingItemField = value;
                }
            }

            /// <remarks/>
            public ArrayOfSessionInfoDtoSessionInfoDtoPlayState PlayState
            {
                get
                {
                    return this.playStateField;
                }
                set
                {
                    this.playStateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute(Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays", IsNullable = false)]
            public string[] PlayableMediaTypes
            {
                get
                {
                    return this.playableMediaTypesField;
                }
                set
                {
                    this.playableMediaTypesField = value;
                }
            }

            /// <remarks/>
            public object QueueableMediaTypes
            {
                get
                {
                    return this.queueableMediaTypesField;
                }
                set
                {
                    this.queueableMediaTypesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute(Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays", IsNullable = false)]
            public string[] SupportedCommands
            {
                get
                {
                    return this.supportedCommandsField;
                }
                set
                {
                    this.supportedCommandsField = value;
                }
            }

            /// <remarks/>
            public bool SupportsRemoteControl
            {
                get
                {
                    return this.supportsRemoteControlField;
                }
                set
                {
                    this.supportsRemoteControlField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object TranscodingInfo
            {
                get
                {
                    return this.transcodingInfoField;
                }
                set
                {
                    this.transcodingInfoField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string UserId
            {
                get
                {
                    return this.userIdField;
                }
                set
                {
                    this.userIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string UserName
            {
                get
                {
                    return this.userNameField;
                }
                set
                {
                    this.userNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string UserPrimaryImageTag
            {
                get
                {
                    return this.userPrimaryImageTagField;
                }
                set
                {
                    this.userPrimaryImageTagField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session")]
        public partial class ArrayOfSessionInfoDtoSessionInfoDtoPlayState
        {

            private object audioStreamIndexField;

            private bool canSeekField;

            private bool isMutedField;

            private bool isPausedField;

            private object mediaSourceIdField;

            private object playMethodField;

            private object positionTicksField;

            private string repeatModeField;

            private object subtitleStreamIndexField;

            private object volumeLevelField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object AudioStreamIndex
            {
                get
                {
                    return this.audioStreamIndexField;
                }
                set
                {
                    this.audioStreamIndexField = value;
                }
            }

            /// <remarks/>
            public bool CanSeek
            {
                get
                {
                    return this.canSeekField;
                }
                set
                {
                    this.canSeekField = value;
                }
            }

            /// <remarks/>
            public bool IsMuted
            {
                get
                {
                    return this.isMutedField;
                }
                set
                {
                    this.isMutedField = value;
                }
            }

            /// <remarks/>
            public bool IsPaused
            {
                get
                {
                    return this.isPausedField;
                }
                set
                {
                    this.isPausedField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object MediaSourceId
            {
                get
                {
                    return this.mediaSourceIdField;
                }
                set
                {
                    this.mediaSourceIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object PlayMethod
            {
                get
                {
                    return this.playMethodField;
                }
                set
                {
                    this.playMethodField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object PositionTicks
            {
                get
                {
                    return this.positionTicksField;
                }
                set
                {
                    this.positionTicksField = value;
                }
            }

            /// <remarks/>
            public string RepeatMode
            {
                get
                {
                    return this.repeatModeField;
                }
                set
                {
                    this.repeatModeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object SubtitleStreamIndex
            {
                get
                {
                    return this.subtitleStreamIndexField;
                }
                set
                {
                    this.subtitleStreamIndexField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object VolumeLevel
            {
                get
                {
                    return this.volumeLevelField;
                }
                set
                {
                    this.volumeLevelField = value;
                }
            }
        }


    }
}
