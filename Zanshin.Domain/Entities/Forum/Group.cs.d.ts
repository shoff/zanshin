declare module server {
	interface Group {
		/** Gets or sets the group identifier. */
		groupId: number;
		/** Gets or sets the name of the group. */
		groupName: string;
		/** Gets or sets the founder identifier. */
		founderId: number;
		/** Gets or sets the group description. */
		groupDescription: string;
		/** Gets or sets a value indicating whether [display group in legend]. */
		displayGroupInLegend: boolean;
		/** Gets or sets a value indicating whether [group receive private messages]. */
		groupRecievePrivateMessages: boolean;
		/** Gets or sets the color of the group. */
		groupColor: string;
		/** Gets or sets the member count. */
		memberCount: number;
		/** Gets or sets the admin count. */
		adminCount: number;
		/** Gets or sets the administrators. */
		administrators: any[];
		/** Gets or sets the users. */
		members: any[];
	}
}
