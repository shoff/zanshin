/// <reference path="Rank.cs.d.ts" />
/// <reference path="Group.cs.d.ts" />
/// <reference path="Topic.cs.d.ts" />

declare module server {
	interface Forum {
		/** Gets or sets the forum identifier. */
		forumId: number;
		/** Gets or sets the post count. */
		postCount: number;
		/** Gets or sets the topic count. */
		topicCount: number;
		/** Gets or sets a value indicating whether [allow HTML]. */
		allowHtml: boolean;
		/** Gets or sets a value indicating whether [allow bb code]. */
		allowBBCode: boolean;
		/** Gets or sets a value indicating whether [allow sigs]. */
		allowSigs: boolean;
		/** Gets or sets the posts per page. */
		postsPerPage: number;
		/** Gets or sets the topics per page. */
		topicsPerPage: number;
		/** Gets or sets the hot topic threashold. */
		hotTopicThreashold: number;
		/** Gets or sets a value indicating whether this instance is private. */
		isPrivate: boolean;
		/** Gets or sets the required roles. */
		requiredRoles: string[];
		/** Gets or sets the name. */
		name: string;
		/** Gets or sets the forum description. */
		forumDescription: string;
		/** Gets or sets the forum password. */
		forumPassword: string;
		/** Gets or sets the forum image location, relative tothe root directory, of an additional image to associatewith this forum. */
		forumImage: string;
		/** Gets or sets a value indicating whether [allow indexing]. */
		allowIndexing: boolean;
		/** Gets or sets a value indicating whether [display active topics]. */
		displayActiveTopics: boolean;
		/** Gets or sets the date created. */
		dateCreated: Date;
		/** Gets or sets the row version. */
		rowVersion: any[];
		/** Gets or sets the category identifier. */
		categoryId: number;
		/** Gets or sets the forum category. */
		forumCategory: {
		/** Gets or sets the category identifier. */
			categoryId: number;
		/** Gets or sets the name. */
			name: string;
		/** Gets or sets the date created. */
			dateCreated: Date;
		/** Gets or sets the date updated. */
			lastUpdated: Date;
		/** Gets or sets the category order. */
			categoryOrder: number;
		/** Gets or sets the category description. */
			categoryDescription: string;
		/** Gets or sets the forum count. */
			forumCount: number;
		/** Gets or sets the row version. */
			rowVersion: any[];
		/** Gets or sets the forums. */
			forums: server.Forum[];
		/** Gets or sets the tags. */
			tags: any[];
		};
		/** Gets or sets the moderator identifier. */
		moderatorId: number;
		/** Gets or sets the forum moderator. */
		forumModerator: {
		/** Gets or sets the name of the user. */
			userName: string;
		/** Gets or sets the display name. */
			displayName: string;
		/** Gets or sets the password. */
			password: string;
		/** Gets or sets the password last changed date. */
			passwordLastChangedDate: Date;
		/** Gets or sets the maximum days between password change. */
			maximumDaysBetweenPasswordChange: number;
		/** Gets or sets the post count. */
			postCount: number;
		/** Gets or sets the topic count. */
			topicCount: number;
		/** Gets or sets the email. */
			email: string;
		/** Gets or sets the tagline. */
			tagline: string;
		/** Gets or sets the location. */
			location: string;
		/** Gets or sets the last search. */
			lastSearch: string;
		/** Gets or sets the rank identifier. */
			rankId: number;
		/** Gets or sets the rank. */
			rank: server.Rank;
		/** Gets or sets the joined date. */
			joinedDate: Date;
		/** Gets or sets the last login. */
			lastLogin: Date;
		/** Gets or sets the karma. */
			karma: number;
		/** Gets or sets the user icon. */
			userIcon: {
		/** Gets or sets the avatar identifier. */
				avatarId: number;
		/** Gets or sets the file. */
				file: string;
		/** Gets or sets the name. */
				name: string;
		/** Gets or sets the type of the MIME. */
				mimeType: string;
		/** Gets or sets the date created. */
				dateCreated: Date;
		/** Gets or sets a value indicating whether this  is display. */
				display: boolean;
		/** Gets or sets the weight. */
				weight: number;
		/** Gets or sets the user count. */
				userCount: number;
		/** Gets or sets the tags. */
				tags: any[];
			};
		/** Gets or sets the avatar identifier. */
			avatarId: number;
		/** Gets or sets a value indicating whether this  is active. */
			active: boolean;
		/** Gets or sets the row version. */
			rowVersion: any[];
		/** Gets or sets the notes. */
			notes: string;
		/** Gets or sets the user profile identifier. */
			userProfileId: number;
		/** Gets or sets the profile. */
			profile: {
		/** Gets or sets the user profile identifier. */
				userProfileId: number;
		/** Gets or sets the birth day. */
				birthDay: Date;
		/** Gets or sets the location. */
				location: string;
		/** Gets or sets the latitude. */
				latitude: number;
		/** Gets or sets the longitude. */
				longitude: number;
		/** Gets or sets the sig. */
				sig: string;
		/** Gets or sets a value indicating whether [allow HTML sig]. */
				allowHtmlSig: boolean;
		/** Gets or sets the facebook page. */
				facebookPage: string;
		/** Gets or sets the name of the skype user. */
				skypeUserName: string;
		/** Gets or sets the name of the twitter. */
				twitterName: string;
		/** Gets or sets the home page. */
				homePage: string;
			};
		/** Gets or sets the roles. */
			groups: server.Group[];
		/** Gets or sets the messages. */
			messages: any[];
		/** Gets or sets the tags. */
			tags: any[];
		};
		/** Gets or sets the moderator group identifier. */
		moderatorGroupId: number;
		/** Gets or sets the moderator group. */
		moderatorGroup: server.Group;
		/** Gets or sets the topics. */
		topics: server.Topic[];
		/** Gets or sets the last updated. */
		lastUpdated: Date;
		/** Gets or sets the tags. */
		tags: any[];
		/** Gets or sets the paged topics. */
		pagedTopics: {
			startingIndex: number;
			pageArray: string[];
			pageArraySize: number;
			pageNumber: number;
			currentPage: server.Topic[];
			pageSize: number;
			totalItems: number;
			totalPages: number;
			hasPreviousPage: boolean;
			hasNextPage: boolean;
		};
	}
}
