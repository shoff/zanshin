declare module server {
	interface Rank {
		/** Gets or sets the rank identifier. */
		rankId: number;
		/** Gets or sets the name of the rank. */
		rankName: string;
		/** Gets or sets the image URL. */
		imageUrl: string;
		/** Gets or sets the required post count. */
		requiredPostCount: number;
		/** Gets or sets a value indicating whether [special rank]. */
		specialRank: boolean;
	}
}
