

 

/// <reference path="Enums.ts" />

declare module Zanshin.Domain.Entities.Forum {
	interface Forum {
		forumId: number;
		postCount: number;
		topicCount: number;
		allowHtml: boolean;
		allowBBCode: boolean;
		allowSigs: boolean;
		postsPerPage: number;
		topicsPerPage: number;
		hotTopicThreashold: number;
		isPrivate: boolean;
		requiredRoles: string[];
		name: string;
		forumDescription: string;
		forumPassword: string;
		forumImage: string;
		allowIndexing: boolean;
		displayActiveTopics: boolean;
		dateCreated: Date;
		rowVersion: number[];
		categoryId: number;
		forumCategory: Zanshin.Domain.Entities.Forum.Category;
		moderatorId: number;
		forumModerator: Zanshin.Domain.Entities.Identity.User;
		moderatorGroupId: number;
		moderatorGroup: Zanshin.Domain.Entities.Forum.Group;
		topics: Zanshin.Domain.Entities.Forum.Topic[];
		lastUpdated: Date;
		tags: Zanshin.Domain.Entities.Tag[];
		pagedTopics: Zanshin.Domain.Collections.SerializablePagination<Zanshin.Domain.Entities.Forum.Topic>;
	}
	interface Category {
		categoryId: number;
		name: string;
		dateCreated: Date;
		lastUpdated: Date;
		categoryOrder: number;
		categoryDescription: string;
		forumCount: number;
		rowVersion: number[];
		forums: Zanshin.Domain.Entities.Forum.Forum[];
		tags: Zanshin.Domain.Entities.Tag[];
	}
	interface Post {
		postId: number;
		subject: string;
		userId: number;
		poster: Zanshin.Domain.Entities.Identity.User;
		topicId: number;
		postTopic: Zanshin.Domain.Entities.Forum.Topic;
		content: string;
		replyToPostId: number;
		replyToPost: Zanshin.Domain.Entities.Forum.Post;
		forumId: number;
		postKarma: number;
		tags: Zanshin.Domain.Entities.Tag[];
		dateCreated: Date;
		lastUpdated: Date;
	}
	interface Rank {
		rankId: number;
		rankName: string;
		imageUrl: string;
		requiredPostCount: number;
		specialRank: boolean;
	}
	interface Group {
		groupId: number;
		groupName: string;
		founderId: number;
		groupDescription: string;
		displayGroupInLegend: boolean;
		groupRecievePrivateMessages: boolean;
		groupColor: string;
		memberCount: number;
		adminCount: number;
		administrators: Zanshin.Domain.Entities.Identity.User[];
		members: Zanshin.Domain.Entities.Identity.User[];
	}
	interface Topic {
		topicId: number;
		subject: string;
		createdDate: Date;
		lastPostDate: Date;
		forumId: number;
		postCount: number;
		userId: number;
		createdBy: Zanshin.Domain.Entities.Identity.User;
		forumName: string;
		topicStarterName: string;
		topicIcon: string;
		views: number;
		sticky: boolean;
		locked: boolean;
		moved: boolean;
		movedToTopicId: number;
		rowVersion: number[];
		movedReason: string;
		tags: Zanshin.Domain.Entities.Tag[];
		posts: Zanshin.Domain.Entities.Forum.Post[];
		pagedPosts: Zanshin.Domain.Collections.SerializablePagination<Zanshin.Domain.Entities.Forum.Post>;
	}
}
declare module Zanshin.Domain.Entities {
	interface Tag {
		tagId: number;
		text: string;
		dateCreated: Date;
		posts: Zanshin.Domain.Entities.Forum.Post[];
		forums: Zanshin.Domain.Entities.Forum.Forum[];
		topics: Zanshin.Domain.Entities.Forum.Topic[];
		categories: Zanshin.Domain.Entities.Forum.Category[];
		users: Zanshin.Domain.Entities.Identity.User[];
		websites: Zanshin.Domain.Entities.Website[];
		avatars: Zanshin.Domain.Entities.Avatar[];
	}
	interface Avatar {
		avatarId: number;
		file: string;
		name: string;
		mimeType: string;
		dateCreated: Date;
		display: boolean;
		weight: number;
		userCount: number;
		tags: Zanshin.Domain.Entities.Tag[];
	}
	interface PrivateMessage {
		privateMessageId: number;
		image: string;
		subject: string;
		message: string;
		fromUserId: number;
		fromUser: Zanshin.Domain.Entities.Identity.User;
		toUserId: number;
		toUser: Zanshin.Domain.Entities.Identity.User;
		dateSent: Date;
		dateSeen: Date;
	}
	interface Website {
		websiteId: number;
		name: string;
		tags: Zanshin.Domain.Entities.Tag[];
	}
	interface Log {
		logId: number;
		dateCreated: Date;
		logLevel: string;
		logger: string;
		message: string;
		messageId: string;
		windowsUserName: string;
		callSite: string;
		threadId: string;
		exception: string;
		stackTrace: string;
	}
	interface GroupMessage {
		groupMessageId: number;
		image: string;
		subject: string;
		message: string;
		fromUserId: number;
		fromUser: Zanshin.Domain.Entities.Identity.User;
		dateSent: Date;
		readBy: Zanshin.Domain.Entities.MessageReadByUser[];
	}
	interface MessageReadByUser {
		groupMessageId: number;
		message: Zanshin.Domain.Entities.GroupMessage;
		userId: number;
		user: Zanshin.Domain.Entities.Identity.User;
	}
}
declare module Zanshin.Domain.Entities.Identity {
	interface User extends Microsoft.AspNet.Identity.EntityFramework.IdentityUser<number, Zanshin.Domain.Entities.Identity.UserLogin, Zanshin.Domain.Entities.Identity.UserRole, Zanshin.Domain.Entities.Identity.UserClaim> {
		userName: string;
		displayName: string;
		password: string;
		passwordLastChangedDate: Date;
		maximumDaysBetweenPasswordChange: number;
		postCount: number;
		topicCount: number;
		email: string;
		tagline: string;
		location: string;
		lastSearch: string;
		rankId: number;
		rank: Zanshin.Domain.Entities.Forum.Rank;
		joinedDate: Date;
		lastLogin: Date;
		karma: number;
		userIcon: Zanshin.Domain.Entities.Avatar;
		avatarId: number;
		active: boolean;
		rowVersion: number[];
		notes: string;
		userProfileId: number;
		profile: Zanshin.Domain.Entities.Identity.UserProfile;
		groups: Zanshin.Domain.Entities.Forum.Group[];
		messages: Zanshin.Domain.Entities.PrivateMessage[];
		tags: Zanshin.Domain.Entities.Tag[];
	}
	interface UserRole extends Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<number> {
	}
	interface UserClaim extends Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<number> {
		userClaimId: number;
		user: Zanshin.Domain.Entities.Identity.User;
	}
	interface UserLogin extends Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<number> {
		userLoginId: number;
		user: Zanshin.Domain.Entities.Identity.User;
	}
	interface UserProfile {
		userProfileId: number;
		birthDay: Date;
		location: string;
		latitude: number;
		longitude: number;
		sig: string;
		allowHtmlSig: boolean;
		facebookPage: string;
		skypeUserName: string;
		twitterName: string;
		homePage: string;
	}
}
declare module Microsoft.AspNet.Identity.EntityFramework {
	interface IdentityUser<TKey, TLogin, TRole, TClaim> {
		email: string;
		emailConfirmed: boolean;
		passwordHash: string;
		securityStamp: string;
		phoneNumber: string;
		phoneNumberConfirmed: boolean;
		twoFactorEnabled: boolean;
		lockoutEndDateUtc: Date;
		lockoutEnabled: boolean;
		accessFailedCount: number;
		roles: TRole[];
		claims: TClaim[];
		logins: TLogin[];
		id: TKey;
		userName: string;
	}
	interface IdentityUserRole<TKey> {
		userId: TKey;
		roleId: TKey;
	}
	interface IdentityUserClaim<TKey> {
		id: number;
		userId: TKey;
		claimType: string;
		claimValue: string;
	}
	interface IdentityUserLogin<TKey> {
		loginProvider: string;
		providerKey: string;
		userId: TKey;
	}
}
declare module Zanshin.Domain.Collections {
	interface SerializablePagination<T> extends Zanshin.Domain.Collections.BasePagination {
		startingIndex: number;
		pageArray: string[];
		pageArraySize: number;
		pageNumber: number;
		currentPage: T[];
		pageSize: number;
		totalItems: number;
		totalPages: number;
		hasPreviousPage: boolean;
		hasNextPage: boolean;
	}
	interface BasePagination {
		totalPages: number;
		pageArraySize: number;
		pageNumber: number;
	}
}
declare module Zanshin.Domain.Models.Account {
	interface LoginViewModel {
		email: string;
		password: string;
		rememberMe: boolean;
	}
}


