USE [Zanshin]
GO
/****** Object:  Table [dbo].[AvatarJoinTag]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AvatarJoinTag](
	[Avatar_AvatarId] [int] NOT NULL,
	[Tag_TagId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.AvatarJoinTag] PRIMARY KEY CLUSTERED 
(
	[Avatar_AvatarId] ASC,
	[Tag_TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Avatars]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Avatars](
	[AvatarId] [int] IDENTITY(1,1) NOT NULL,
	[File] [nvarchar](512) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[MimeType] [nvarchar](30) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[Display] [bit] NOT NULL,
	[Weight] [int] NOT NULL,
	[UserCount] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Avatars] PRIMARY KEY CLUSTERED 
(
	[AvatarId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Categories]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[CategoryOrder] [int] NOT NULL,
	[CategoryDescription] [nvarchar](128) NULL,
	[ForumCount] [int] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_dbo.Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CategoryJoinTag]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryJoinTag](
	[Category_CategoryId] [int] NOT NULL,
	[Tag_TagId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.CategoryJoinTag] PRIMARY KEY CLUSTERED 
(
	[Category_CategoryId] ASC,
	[Tag_TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ForumJoinTag]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ForumJoinTag](
	[Forum_ForumId] [int] NOT NULL,
	[Tag_TagId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ForumJoinTag] PRIMARY KEY CLUSTERED 
(
	[Forum_ForumId] ASC,
	[Tag_TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Forums]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Forums](
	[ForumId] [int] IDENTITY(1,1) NOT NULL,
	[PostCount] [int] NOT NULL,
	[TopicCount] [int] NOT NULL,
	[AllowHtml] [bit] NOT NULL,
	[AllowBBCode] [bit] NOT NULL,
	[AllowSigs] [bit] NOT NULL,
	[PostsPerPage] [int] NOT NULL,
	[TopicsPerPage] [int] NOT NULL,
	[HotTopicThreashold] [int] NOT NULL,
	[IsPrivate] [bit] NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[ForumDescription] [nvarchar](128) NULL,
	[ForumPassword] [nvarchar](128) NULL,
	[ForumImage] [nvarchar](512) NULL,
	[AllowIndexing] [bit] NOT NULL,
	[DisplayActiveTopics] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[ModeratorId] [int] NOT NULL,
	[ModeratorGroupId] [int] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Forums] PRIMARY KEY CLUSTERED 
(
	[ForumId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GeoLocations]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeoLocations](
	[GeoLocationId] [int] IDENTITY(1,1) NOT NULL,
	[Address] [nvarchar](16) NOT NULL,
	[Country] [nvarchar](60) NULL,
	[CountryCode] [nvarchar](8) NULL,
	[Region] [nvarchar](40) NULL,
	[RegionName] [nvarchar](max) NULL,
	[City] [nvarchar](256) NULL,
	[Zip] [nvarchar](max) NULL,
	[Latitude] [nvarchar](max) NULL,
	[Longitude] [nvarchar](max) NULL,
	[TimeZone] [nvarchar](max) NULL,
	[Isp] [nvarchar](max) NULL,
	[Organization] [nvarchar](max) NULL,
	[As] [nvarchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
	[LastSeen] [datetime] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.GeoLocations] PRIMARY KEY CLUSTERED 
(
	[GeoLocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GroupMessages]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupMessages](
	[GroupMessageId] [int] IDENTITY(1,1) NOT NULL,
	[Image] [nvarchar](100) NULL,
	[Subject] [nvarchar](255) NOT NULL,
	[Message] [nvarchar](4000) NOT NULL,
	[FromUserId] [int] NOT NULL,
	[DateSent] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.GroupMessages] PRIMARY KEY CLUSTERED 
(
	[GroupMessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Groups]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[GroupId] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [nvarchar](20) NOT NULL,
	[FounderId] [int] NULL,
	[GroupDescription] [nvarchar](256) NULL,
	[DisplayGroupInLegend] [bit] NOT NULL,
	[GroupRecievePrivateMessages] [bit] NOT NULL,
	[GroupColor] [nvarchar](7) NULL,
	[MemberCount] [int] NOT NULL,
	[AdminCount] [int] NOT NULL,
	[User_Id] [int] NULL,
 CONSTRAINT [PK_dbo.Groups] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LocationCounts]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocationCounts](
	[LocationCountId] [int] IDENTITY(1,1) NOT NULL,
	[GeoLocationId] [int] NOT NULL,
	[HitCount] [int] NOT NULL,
 CONSTRAINT [PK_dbo.LocationCounts] PRIMARY KEY CLUSTERED 
(
	[LocationCountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MessagesReadByUsers]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MessagesReadByUsers](
	[GroupMessageId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.MessagesReadByUsers] PRIMARY KEY CLUSTERED 
(
	[GroupMessageId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PostJoinTag]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PostJoinTag](
	[Post_PostId] [int] NOT NULL,
	[Tag_TagId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.PostJoinTag] PRIMARY KEY CLUSTERED 
(
	[Post_PostId] ASC,
	[Tag_TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Posts]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Posts](
	[PostId] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [nvarchar](60) NOT NULL,
	[UserId] [int] NOT NULL,
	[TopicId] [int] NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[ReplyToPostId] [int] NULL,
	[ForumId] [int] NOT NULL,
	[PostKarma] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Posts] PRIMARY KEY CLUSTERED 
(
	[PostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PrivateMessages]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrivateMessages](
	[PrivateMessageId] [int] IDENTITY(1,1) NOT NULL,
	[Image] [nvarchar](100) NULL,
	[Subject] [nvarchar](255) NOT NULL,
	[Message] [nvarchar](4000) NOT NULL,
	[FromUserId] [int] NOT NULL,
	[ToUserId] [int] NOT NULL,
	[DateSent] [datetime] NOT NULL,
	[DateSeen] [datetime] NULL,
 CONSTRAINT [PK_dbo.PrivateMessages] PRIMARY KEY CLUSTERED 
(
	[PrivateMessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Ranks]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ranks](
	[RankId] [int] IDENTITY(1,1) NOT NULL,
	[RankName] [nvarchar](128) NULL,
	[ImageUrl] [nvarchar](512) NULL,
	[RequiredPostCount] [int] NOT NULL,
	[SpecialRank] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Ranks] PRIMARY KEY CLUSTERED 
(
	[RankId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tags]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tags](
	[TagId] [int] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](20) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Tags] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TopicJoinTag]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TopicJoinTag](
	[Topic_TopicId] [int] NOT NULL,
	[Tag_TagId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.TopicJoinTag] PRIMARY KEY CLUSTERED 
(
	[Topic_TopicId] ASC,
	[Tag_TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Topics]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Topics](
	[TopicId] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [nvarchar](255) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastPostDate] [datetime] NOT NULL,
	[ForumId] [int] NOT NULL,
	[PostCount] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[ForumName] [nvarchar](max) NULL,
	[TopicStarterName] [nvarchar](max) NULL,
	[TopicIcon] [nvarchar](max) NULL,
	[Views] [int] NOT NULL,
	[Sticky] [bit] NOT NULL,
	[Locked] [bit] NOT NULL,
	[Moved] [bit] NOT NULL,
	[MovedToTopicId] [int] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[MovedReason] [nvarchar](60) NULL,
 CONSTRAINT [PK_dbo.Topics] PRIMARY KEY CLUSTERED 
(
	[TopicId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserClaims]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserClaims](
	[UserClaimId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.UserClaims] PRIMARY KEY CLUSTERED 
(
	[UserClaimId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserGroups]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserGroups](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.UserGroups] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserJoinTag]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserJoinTag](
	[Id] [int] NOT NULL,
	[TagId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.UserJoinTag] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserLogins]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [int] NOT NULL,
	[UserLoginId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.UserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserProfiles]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfiles](
	[UserProfileId] [int] IDENTITY(1,1) NOT NULL,
	[BirthDay] [datetime] NULL,
	[Location] [nvarchar](200) NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
	[Sig] [nvarchar](512) NULL,
	[AllowHtmlSig] [bit] NOT NULL,
	[FacebookPage] [nvarchar](512) NULL,
	[SkypeUserName] [nvarchar](256) NULL,
	[TwitterName] [nvarchar](50) NULL,
	[HomePage] [nvarchar](512) NULL,
 CONSTRAINT [PK_dbo.UserProfiles] PRIMARY KEY CLUSTERED 
(
	[UserProfileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[DisplayName] [nvarchar](20) NULL,
	[Password] [nvarchar](256) NULL,
	[PasswordLastChangedDate] [datetime] NOT NULL,
	[MaximumDaysBetweenPasswordChange] [int] NOT NULL,
	[PostCount] [int] NOT NULL,
	[TopicCount] [int] NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[Tagline] [nvarchar](512) NULL,
	[Location] [nvarchar](80) NULL,
	[LastSearch] [nvarchar](256) NULL,
	[RankId] [int] NULL,
	[JoinedDate] [datetime] NOT NULL,
	[LastLogin] [datetime] NOT NULL,
	[Karma] [int] NOT NULL,
	[AvatarId] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
	[Notes] [nvarchar](512) NULL,
	[UserProfileId] [int] NOT NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[Group_GroupId] [int] NULL,
	[Group_GroupId1] [int] NULL,
 CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WebsiteJoinTag]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebsiteJoinTag](
	[Website_WebsiteId] [int] NOT NULL,
	[Tag_TagId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.WebsiteJoinTag] PRIMARY KEY CLUSTERED 
(
	[Website_WebsiteId] ASC,
	[Tag_TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Websites]    Script Date: 9/17/2015 2:43:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Websites](
	[WebsiteId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](60) NOT NULL,
 CONSTRAINT [PK_dbo.Websites] PRIMARY KEY CLUSTERED 
(
	[WebsiteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[AvatarJoinTag]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AvatarJoinTag_dbo.Avatars_Avatar_AvatarId] FOREIGN KEY([Avatar_AvatarId])
REFERENCES [dbo].[Avatars] ([AvatarId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AvatarJoinTag] CHECK CONSTRAINT [FK_dbo.AvatarJoinTag_dbo.Avatars_Avatar_AvatarId]
GO
ALTER TABLE [dbo].[AvatarJoinTag]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AvatarJoinTag_dbo.Tags_Tag_TagId] FOREIGN KEY([Tag_TagId])
REFERENCES [dbo].[Tags] ([TagId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AvatarJoinTag] CHECK CONSTRAINT [FK_dbo.AvatarJoinTag_dbo.Tags_Tag_TagId]
GO
ALTER TABLE [dbo].[CategoryJoinTag]  WITH CHECK ADD  CONSTRAINT [FK_dbo.CategoryJoinTag_dbo.Categories_Category_CategoryId] FOREIGN KEY([Category_CategoryId])
REFERENCES [dbo].[Categories] ([CategoryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CategoryJoinTag] CHECK CONSTRAINT [FK_dbo.CategoryJoinTag_dbo.Categories_Category_CategoryId]
GO
ALTER TABLE [dbo].[CategoryJoinTag]  WITH CHECK ADD  CONSTRAINT [FK_dbo.CategoryJoinTag_dbo.Tags_Tag_TagId] FOREIGN KEY([Tag_TagId])
REFERENCES [dbo].[Tags] ([TagId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CategoryJoinTag] CHECK CONSTRAINT [FK_dbo.CategoryJoinTag_dbo.Tags_Tag_TagId]
GO
ALTER TABLE [dbo].[ForumJoinTag]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ForumJoinTag_dbo.Forums_Forum_ForumId] FOREIGN KEY([Forum_ForumId])
REFERENCES [dbo].[Forums] ([ForumId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ForumJoinTag] CHECK CONSTRAINT [FK_dbo.ForumJoinTag_dbo.Forums_Forum_ForumId]
GO
ALTER TABLE [dbo].[ForumJoinTag]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ForumJoinTag_dbo.Tags_Tag_TagId] FOREIGN KEY([Tag_TagId])
REFERENCES [dbo].[Tags] ([TagId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ForumJoinTag] CHECK CONSTRAINT [FK_dbo.ForumJoinTag_dbo.Tags_Tag_TagId]
GO
ALTER TABLE [dbo].[Forums]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Forums_dbo.Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([CategoryId])
GO
ALTER TABLE [dbo].[Forums] CHECK CONSTRAINT [FK_dbo.Forums_dbo.Categories_CategoryId]
GO
ALTER TABLE [dbo].[Forums]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Forums_dbo.Groups_ModeratorGroupId] FOREIGN KEY([ModeratorGroupId])
REFERENCES [dbo].[Groups] ([GroupId])
GO
ALTER TABLE [dbo].[Forums] CHECK CONSTRAINT [FK_dbo.Forums_dbo.Groups_ModeratorGroupId]
GO
ALTER TABLE [dbo].[Forums]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Forums_dbo.Users_ModeratorId] FOREIGN KEY([ModeratorId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Forums] CHECK CONSTRAINT [FK_dbo.Forums_dbo.Users_ModeratorId]
GO
ALTER TABLE [dbo].[GeoLocations]  WITH CHECK ADD  CONSTRAINT [FK_dbo.GeoLocations_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[GeoLocations] CHECK CONSTRAINT [FK_dbo.GeoLocations_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[GroupMessages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.GroupMessages_dbo.Users_FromUserId] FOREIGN KEY([FromUserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[GroupMessages] CHECK CONSTRAINT [FK_dbo.GroupMessages_dbo.Users_FromUserId]
GO
ALTER TABLE [dbo].[Groups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Groups_dbo.Users_User_Id] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Groups] CHECK CONSTRAINT [FK_dbo.Groups_dbo.Users_User_Id]
GO
ALTER TABLE [dbo].[LocationCounts]  WITH CHECK ADD  CONSTRAINT [FK_dbo.LocationCounts_dbo.GeoLocations_GeoLocationId] FOREIGN KEY([GeoLocationId])
REFERENCES [dbo].[GeoLocations] ([GeoLocationId])
GO
ALTER TABLE [dbo].[LocationCounts] CHECK CONSTRAINT [FK_dbo.LocationCounts_dbo.GeoLocations_GeoLocationId]
GO
ALTER TABLE [dbo].[MessagesReadByUsers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.MessagesReadByUsers_dbo.GroupMessages_GroupMessageId] FOREIGN KEY([GroupMessageId])
REFERENCES [dbo].[GroupMessages] ([GroupMessageId])
GO
ALTER TABLE [dbo].[MessagesReadByUsers] CHECK CONSTRAINT [FK_dbo.MessagesReadByUsers_dbo.GroupMessages_GroupMessageId]
GO
ALTER TABLE [dbo].[MessagesReadByUsers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.MessagesReadByUsers_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[MessagesReadByUsers] CHECK CONSTRAINT [FK_dbo.MessagesReadByUsers_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[PostJoinTag]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PostJoinTag_dbo.Posts_Post_PostId] FOREIGN KEY([Post_PostId])
REFERENCES [dbo].[Posts] ([PostId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PostJoinTag] CHECK CONSTRAINT [FK_dbo.PostJoinTag_dbo.Posts_Post_PostId]
GO
ALTER TABLE [dbo].[PostJoinTag]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PostJoinTag_dbo.Tags_Tag_TagId] FOREIGN KEY([Tag_TagId])
REFERENCES [dbo].[Tags] ([TagId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PostJoinTag] CHECK CONSTRAINT [FK_dbo.PostJoinTag_dbo.Tags_Tag_TagId]
GO
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Posts_dbo.Posts_ReplyToPostId] FOREIGN KEY([ReplyToPostId])
REFERENCES [dbo].[Posts] ([PostId])
GO
ALTER TABLE [dbo].[Posts] CHECK CONSTRAINT [FK_dbo.Posts_dbo.Posts_ReplyToPostId]
GO
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Posts_dbo.Topics_TopicId] FOREIGN KEY([TopicId])
REFERENCES [dbo].[Topics] ([TopicId])
GO
ALTER TABLE [dbo].[Posts] CHECK CONSTRAINT [FK_dbo.Posts_dbo.Topics_TopicId]
GO
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Posts_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Posts] CHECK CONSTRAINT [FK_dbo.Posts_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[PrivateMessages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PrivateMessages_dbo.Users_FromUserId] FOREIGN KEY([FromUserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[PrivateMessages] CHECK CONSTRAINT [FK_dbo.PrivateMessages_dbo.Users_FromUserId]
GO
ALTER TABLE [dbo].[PrivateMessages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PrivateMessages_dbo.Users_ToUserId] FOREIGN KEY([ToUserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[PrivateMessages] CHECK CONSTRAINT [FK_dbo.PrivateMessages_dbo.Users_ToUserId]
GO
ALTER TABLE [dbo].[TopicJoinTag]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TopicJoinTag_dbo.Tags_Tag_TagId] FOREIGN KEY([Tag_TagId])
REFERENCES [dbo].[Tags] ([TagId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TopicJoinTag] CHECK CONSTRAINT [FK_dbo.TopicJoinTag_dbo.Tags_Tag_TagId]
GO
ALTER TABLE [dbo].[TopicJoinTag]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TopicJoinTag_dbo.Topics_Topic_TopicId] FOREIGN KEY([Topic_TopicId])
REFERENCES [dbo].[Topics] ([TopicId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TopicJoinTag] CHECK CONSTRAINT [FK_dbo.TopicJoinTag_dbo.Topics_Topic_TopicId]
GO
ALTER TABLE [dbo].[Topics]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Topics_dbo.Forums_ForumId] FOREIGN KEY([ForumId])
REFERENCES [dbo].[Forums] ([ForumId])
GO
ALTER TABLE [dbo].[Topics] CHECK CONSTRAINT [FK_dbo.Topics_dbo.Forums_ForumId]
GO
ALTER TABLE [dbo].[Topics]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Topics_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Topics] CHECK CONSTRAINT [FK_dbo.Topics_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[UserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserClaims_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserClaims] CHECK CONSTRAINT [FK_dbo.UserClaims_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[UserGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserGroups_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserGroups] CHECK CONSTRAINT [FK_dbo.UserGroups_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[UserJoinTag]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserJoinTag_dbo.Tags_TagId] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tags] ([TagId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserJoinTag] CHECK CONSTRAINT [FK_dbo.UserJoinTag_dbo.Tags_TagId]
GO
ALTER TABLE [dbo].[UserJoinTag]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserJoinTag_dbo.Users_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserJoinTag] CHECK CONSTRAINT [FK_dbo.UserJoinTag_dbo.Users_Id]
GO
ALTER TABLE [dbo].[UserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserLogins_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserLogins] CHECK CONSTRAINT [FK_dbo.UserLogins_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Users_dbo.Avatars_AvatarId] FOREIGN KEY([AvatarId])
REFERENCES [dbo].[Avatars] ([AvatarId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_dbo.Users_dbo.Avatars_AvatarId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Users_dbo.Groups_Group_GroupId] FOREIGN KEY([Group_GroupId])
REFERENCES [dbo].[Groups] ([GroupId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_dbo.Users_dbo.Groups_Group_GroupId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Users_dbo.Groups_Group_GroupId1] FOREIGN KEY([Group_GroupId1])
REFERENCES [dbo].[Groups] ([GroupId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_dbo.Users_dbo.Groups_Group_GroupId1]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Users_dbo.Ranks_RankId] FOREIGN KEY([RankId])
REFERENCES [dbo].[Ranks] ([RankId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_dbo.Users_dbo.Ranks_RankId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Users_dbo.UserProfiles_UserProfileId] FOREIGN KEY([UserProfileId])
REFERENCES [dbo].[UserProfiles] ([UserProfileId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_dbo.Users_dbo.UserProfiles_UserProfileId]
GO
ALTER TABLE [dbo].[WebsiteJoinTag]  WITH CHECK ADD  CONSTRAINT [FK_dbo.WebsiteJoinTag_dbo.Tags_Tag_TagId] FOREIGN KEY([Tag_TagId])
REFERENCES [dbo].[Tags] ([TagId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WebsiteJoinTag] CHECK CONSTRAINT [FK_dbo.WebsiteJoinTag_dbo.Tags_Tag_TagId]
GO
ALTER TABLE [dbo].[WebsiteJoinTag]  WITH CHECK ADD  CONSTRAINT [FK_dbo.WebsiteJoinTag_dbo.Websites_Website_WebsiteId] FOREIGN KEY([Website_WebsiteId])
REFERENCES [dbo].[Websites] ([WebsiteId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WebsiteJoinTag] CHECK CONSTRAINT [FK_dbo.WebsiteJoinTag_dbo.Websites_Website_WebsiteId]
GO
