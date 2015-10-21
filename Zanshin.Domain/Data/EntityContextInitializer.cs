
namespace Zanshin.Domain.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Security;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using NLog;
    using Zanshin.Domain.Entities;
    using Zanshin.Domain.Entities.Forum;
    using Zanshin.Domain.Entities.Identity;
    using Zanshin.Domain.Extensions;
    using Zanshin.Domain.Interfaces;

    /// <summary>
    /// </summary>
    public sealed class EntityContextInitializer : DropCreateDatabaseIfModelChanges<DataContext>, ISingletonLifestyle
    {
        private static readonly Logger logger = LogManager.GetLogger("EntityContextInitializer");
        private static readonly Random random = new Random((int)DateTime.Now.Ticks);
        private static readonly List<User> users = new List<User>();
        private static Group adminGroup;
        private static Group userGroup;
        private static Avatar avatar;
        private static List<Rank> ranks;

        protected override void Seed(DataContext context)
        {
            context.Configuration.AutoDetectChangesEnabled = false;
            CreateAvatars(context);
            CreateRanks(context);
            CreateUsers(context, avatar.AvatarId);
            CreateGroups(context);
            CreateCategories(context);
            UpdateForumCounts(context);
            context.Commit();
        }

        private static void CreateRanks(DataContext context)
        {
            ranks = new List<Rank>
            {
                new Rank { RankName = "Board Noob", RequiredPostCount = 0 }, // 1
                new Rank { RankName = "Active Poster", RequiredPostCount = 51 }, // 2
                new Rank { RankName = "Valued Contributor", RequiredPostCount = 101 }, // 3
                new Rank { RankName = "Board Veteran", RequiredPostCount = 251 }, // 4
                new Rank { RankName = "Board General", RequiredPostCount = 501 }, // 5
                new Rank { RankName = "Elite Poster", RequiredPostCount = 1001 }, // 6
                new Rank { RankName = "Admin", RequiredPostCount = 0, SpecialRank = true },
                new Rank { RankName = "Moderator", RequiredPostCount = 0, SpecialRank = true },
            };
            ranks.ForEach(x => context.Ranks.Add(x));
            context.Commit();
        }

        private void UpdateForumCounts(DataContext context)
        {
            List<Forum> forums = context.SetEntity<Forum>().Include("Topics").Include("Topics.Posts").OrderBy(f => f.ForumId).ToList();

            foreach (var forum in forums)
            {
                int forumPostCount = 0;
                foreach (var topic in forum.Topics)
                {
                    int count = topic.Posts.Count();
                    topic.PostCount = count;
                    forumPostCount += count;
                    context.SetModified(topic);

                    var topicUser = users.FirstOrDefault(x => x.Id == topic.UserId);

                    if (topicUser != null)
                    {
                        topicUser.TopicCount++;
                        context.SetModified(topicUser);
                    }

                    foreach (var post in topic.Posts)
                    {
                        var uid = post.UserId;
                        var postUser = users.FirstOrDefault(x => x.Id == uid);
                        if (postUser != null)
                        {
                            postUser.PostCount++;
                            context.SetModified(postUser);
                        }
                    }
                }
                forum.PostCount = forumPostCount;
                context.SetModified(forum);
                context.Commit();
            }

            foreach (var user in users)
            {
                if (user.PostCount >= 1001)
                {
                    user.RankId = 6;
                }
                else if (user.PostCount >= 501)
                {
                    user.RankId = 5;
                }
                else if (user.PostCount >= 251)
                {
                    user.RankId = 4;
                }
                else if (user.PostCount >= 101)
                {
                    user.RankId = 3;
                }
                else if (user.PostCount >= 51)
                {
                    user.RankId = 2;
                }
                else
                {
                    user.RankId = 1;
                }
                context.SetModified(user);
            }
            context.Commit();
        }

        private static void CreateUsers(DataContext context, int avatarId)
        {
            User adminUser = new User
            {
                Active = true,
                DisplayName = "Webmaster",
                AvatarId = avatarId,
                Email = "webmaster@Zanshins.com",
                JoinedDate = DateTime.Now,
                Karma = 0,
                LastLogin = DateTime.Now,
                UserName = "admin",
                Password = "P@ssword1".Sha256Hash(),
                PasswordLastChangedDate = DateTime.Now,
                MaximumDaysBetweenPasswordChange = 180,
                Profile =
                    new UserProfile
                    {
                        BirthDay = DateTime.Parse("02/13/1969"),
                        Location = "Coon Rapids, MN",
                        SkypeUserName = "smhoff256@gmail.com",
                        Sig = "this too shall pass"
                    }
            };

            context.Users.Add(adminUser);
            context.Commit();
            users.Add(adminUser);
            User adminUser1 = new User
            {
                Active = true,
                DisplayName = "Webmaster1",
                AvatarId = avatarId,
                Email = "webmaster1@Zanshins.com",
                JoinedDate = DateTime.Now,
                Karma = 0,
                LastLogin = DateTime.Now,
                UserName = "admin1",
                Password = "P@ssword1".Sha256Hash(),
                PasswordLastChangedDate = DateTime.Now,
                MaximumDaysBetweenPasswordChange = 180,
                Profile =
                    new UserProfile
                    {
                        BirthDay = DateTime.Parse("02/13/1969"),
                        Location = "Coon Rapids, MN",
                        SkypeUserName = "smhoff256@gmail.com",
                        Sig = "this too shall pass"
                    }
            };
            context.Users.Add(adminUser1);
            context.Commit();
            users.Add(adminUser1);

            CreateUsers(context);
            logger.Debug("Completed adding all users.");
        }

        private static void CreateGroups(DataContext context)
        {
            adminGroup = new Group
            {
                DisplayGroupInLegend = false,
                FounderId = users[0].Id,
                GroupColor = "#990000",
                GroupDescription = "Default Admin Group",
                GroupName = "Administrators",
                GroupRecievePrivateMessages = true
            };

            adminGroup.Administrators.Add(users[0]);
            context.Groups.Add(adminGroup);

            userGroup = new Group
            {
                DisplayGroupInLegend = true,
                FounderId = users[0].Id,
                GroupColor = "#000",
                GroupDescription = "Default user group",
                GroupName = "Users",
                GroupRecievePrivateMessages = false
            };
            context.Groups.Add(userGroup);
            context.Commit();

            logger.Debug("Groups added to database.");
        }

        private static void CreateAvatars(DataContext context)
        {
            avatar = new Avatar
            {
                DateCreated = DateTime.Now,
                Display = true,
                File = "/images/avatars/noimage.gif",
                MimeType = "gif",
                Name = "noimage",
                UserCount = 1,
                Weight = 0
            };

            context.Avatars.Add(avatar);
            context.Commit();
            logger.Debug("Avatar added to database.");
        }


        private static void CreateForums(DataContext context, int categoryId)
        {
            for (int i = 0; i < 4; i++)
            {
                int topicCount = random.Next(100);

                var forum = new Forum
                {
                    AllowBBCode = true,
                    AllowHtml = false,
                    AllowSigs = true,
                    CategoryId = categoryId,
                    DateCreated = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    ForumDescription = "Example Forum",
                    TopicCount = topicCount,
                    PostCount = 0,
                    ModeratorId = users[0].Id,
                    HotTopicThreashold = 20,
                    IsPrivate = false,
                    PostsPerPage = 20,
                    ForumImage = GetImage(categoryId, i+1),
                    TopicsPerPage = 20,
                    Name = string.Format("Welcome Forum {0} - {1}", i, categoryId),
                    ModeratorGroupId = adminGroup.GroupId
                };

                context.Forums.Add(forum);
                context.Commit();
                logger.Debug("forum {0} created", forum.Name);

                CreateTopics(context, forum.ForumId, forum.Name, topicCount);
            }
        }

        private static string GetImage(int categoryId, int i)
        {
            string[] images = {
                                  "http://lorempixel.com/200/80/business",
                                  "http://lorempixel.com/200/80/abstract",
                                  "http://lorempixel.com/200/80/city",
                                  "http://lorempixel.com/200/80/people",
                                  "http://lorempixel.com/200/80/transport",
                                  "http://lorempixel.com/200/80/animals",
                                  "http://lorempixel.com/200/80/food",
                                  "http://lorempixel.com/200/80/nature",
                                  "http://lorempixel.com/200/80/nightlife",
                                  "http://lorempixel.com/200/80/sports",
                                  "http://lorempixel.com/200/80/cats",
                                  "http://lorempixel.com/200/80/fashion",
                                  "http://lorempixel.com/200/80/technics",
                                  "http://lorempixel.com/200/80/people",
                                  "http://lorempixel.com/200/80/people",
                                  "http://lorempixel.com/200/80/"};

            switch (categoryId)
            {
                case 1:
                    return images[i];
                case 2:
                    return images[i + 4];
                case 3:
                    return images[i + 8];
                default:
                    return images[i + 12];
            }

        }

        private static void CreateCategories(DataContext context)
        {
            Category category = new Category
            {
                CategoryDescription = "General Discussion",
                CategoryOrder = 0,
                DateCreated = DateTime.Now,
                LastUpdated = DateTime.Now,
                ForumCount = 4,
                Name = "General"
            };

            context.Categories.Add(category);
            context.Commit();

            // logger.Debug(string.Format("Category {0} added to the database.", category.CategoryId));
            CreateForums(context, category.CategoryId);

            Category category1 = new Category
            {
                CategoryDescription = "File bug reports here",
                CategoryOrder = 0,
                DateCreated = DateTime.Now,
                LastUpdated = DateTime.Now,
                ForumCount = 4,
                Name = "Board Issues & Bug Reports"
            };

            context.Categories.Add(category1);
            context.Commit();

            // logger.Debug(string.Format("Category {0} added to the database.", category.CategoryId));
            CreateForums(context, category1.CategoryId);

            Category category2 = new Category
            {
                CategoryDescription = "A place to request new features",
                CategoryOrder = 0,
                DateCreated = DateTime.Now,
                LastUpdated = DateTime.Now,
                ForumCount = 4,
                Name = "Feature and Board Requests"
            };

            context.Categories.Add(category2);
            context.Commit();

            // logger.Debug(string.Format("Category {0} added to the database.", category.CategoryId));
            CreateForums(context, category2.CategoryId);

            Category category3 = new Category
            {
                CategoryDescription = "Information about Kendo training",
                CategoryOrder = 0,
                DateCreated = DateTime.Now,
                LastUpdated = DateTime.Now,
                ForumCount = 4,
                Name = "Training and Waza"
            };

            context.Categories.Add(category3);
            context.Commit();

            // logger.Debug(string.Format("Category {0} added to the database.", category.CategoryId));
            CreateForums(context, category3.CategoryId);
        }

        private static void CreateTopics(DataContext context, int forumId, string forumName, int topicCount)
        {
            for (int i = 0; i < topicCount; i++)
            {
                int postCount = random.Next(1, 30);
                var user = users[random.Next(0, users.Count - 1)];
                int userId = user.Id;

                Topic topic = new Topic
                {
                    CreatedDate = DateTime.Now,
                    ForumName = forumName,
                    ForumId = forumId,
                    LastPostDate = DateTime.Now,
                    Subject = string.Format("Auto generated Topic no. {0}", i),
                    UserId = userId,
                    TopicStarterName = user.UserName,
                    Locked = random.Next() % 15 == 0,
                };

                context.Topics.Add(topic);
                context.Commit();
                for (int p = 0; p < postCount; p++)
                {
                    topic.Posts.Add(CreatePost(context, topic, forumId, userId));
                }
                context.Commit();
            }
            logger.Debug(string.Format("Topics for Forum with Id {0} added to database", forumId));
        }

        private static Post CreatePost(DataContext context, Topic topic, int forumId, int userId)
        {
            Post post = new Post
            {
                Subject = "Post number 1",
                Content = LoremIpsum.GetLoremIpsum(512),
                DateCreated = DateTime.Now,
                UserId = userId,
                TopicId = topic.TopicId,

                //PostTopic = topic,
                ForumId = forumId,
                LastUpdated = DateTime.Now,
            };

            //var tag = CreateTags(context, userId);
            //post.Tags.Add(tag);
            context.Posts.Add(post);
            return post;
        }

        /// <summary>
        ///   Extracts the filename.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <returns></returns>
        /// <exception cref="IOException">An I/O error occurred.</exception>
        /// <exception cref="ArgumentException">Attempted to set to an empty string ("").</exception>
        /// <exception cref="DirectoryNotFoundException">Attempted to set a local path that cannot be found.</exception>
        /// <exception cref="SecurityException">The caller does not have the appropriate permission.</exception>
        public string ExtractFilename(string filepath)
        {
            // If path ends with a "\", it's a path only so return String.Empty.
            if (filepath.Trim().EndsWith(@"\"))
            {
                return String.Empty;
            }

            // Determine where last backslash is. 
            int position = filepath.LastIndexOf('\\');

            // If there is no backslash, assume that this is a filename. 
            if (position == -1)
            {
                // Determine whether file exists in the current directory. 
                if (File.Exists(Environment.CurrentDirectory + Path.DirectorySeparatorChar + filepath))
                {
                    return filepath;
                }
                return String.Empty;
            }

            // Determine whether file exists using filepath. 
            if (File.Exists(filepath))
            {
                // Return filename without file path. 
                return filepath.Substring(position + 1);
            }

            return String.Empty;
        }

        private static async void CreateUsers(DataContext context)
        {
            List<string> usedNames = new List<string>();

            //string imageFolder = AppDomain.CurrentDomain.BaseDirectory + "\\images\\avatars\\";

            RandomMe randomMe = new RandomMe();

            for (int i = 0; i < 100; i++)
            {
                RandomUser randomUser = await randomMe.GetUser();

                while ((usedNames.Contains(randomUser.ThumbnailUrl)) || (!randomUser.Valid))
                {
                    randomUser = await randomMe.GetUser();
                }

                usedNames.Add(randomUser.ThumbnailUrl);

                User user = new User();

                user.UserIcon = new Avatar
                {
                    DateCreated = DateTime.Now,
                    Display = true,
                    File = "/images/avatars/" + randomUser.Thumbnail,
                    MimeType = "jpeg",
                    Name = randomUser.Thumbnail,
                    UserCount = 1,
                    Weight = 0
                };
                user.UserName = (randomUser.FirstName + "_" + randomUser.LastName);
                user.DisplayName = randomUser.DisplayName;
                user.Email = (string)randomUser.Item["user"]["email"];
                user.JoinedDate = DateTime.Now.AddDays(-random.Next(10, 120));
                user.LastLogin = DateTime.Now.AddDays(-random.Next(10, 120));
                user.Password = (string)randomUser.Item["user"]["sha256"];
                user.PasswordLastChangedDate = DateTime.Now.AddDays(-random.Next(10, 120));
                user.MaximumDaysBetweenPasswordChange = 180;
                user.Karma = random.Next(0, 5608);
                user.Profile = new UserProfile
                {
                    BirthDay = randomUser.DateOfBirth
                };

                context.Users.Add(user);
                users.Add(user);
                try
                {
                    ((DbContext)context).SaveChanges();
                }
                catch (Exception e)
                {
                    logger.Error(e);
                    throw;
                }

                //context.Commit();
            }
        }
    }

    public class RandomMe
    {
        private readonly string imageFolder = AppDomain.CurrentDomain.BaseDirectory + "\\images\\avatars\\";

        internal async Task<RandomUser> GetUser()
        {
            RandomUser randomUser = new RandomUser();
            HttpClient webClient = new HttpClient();

            var json = await webClient.GetStringAsync("http://api.randomuser.me/");
            dynamic result = JsonConvert.DeserializeObject<dynamic>(json);
            dynamic item = result.results[0];

            randomUser.Gender = (string)item["user"]["gender"];
            randomUser.FirstName = (string)item["user"]["name"]["first"];
            randomUser.LastName = (string)item["user"]["name"]["last"];
            randomUser.DateOfBirth = UnixTimeStampToDateTime((string)item["user"]["dob"]);
            randomUser.Registered = UnixTimeStampToDateTime((string)item["user"]["registered"]);
            randomUser.UserName = (string)item["user"]["username"];
            randomUser.Password = (string)item["user"]["sha256"];

            randomUser.ThumbnailUrl = (string)item["user"]["picture"]["thumbnail"];
            var thumbArray = randomUser.ThumbnailUrl.Split('/');
            randomUser.Thumbnail = "thumb_" + randomUser.Gender + "_" + thumbArray[thumbArray.Length - 1];

            randomUser.MediumUrl = (string)item["user"]["picture"]["medium"];
            var mediumArray = randomUser.MediumUrl.Split('/');
            randomUser.Medium = "med_" + randomUser.Gender + "_" + mediumArray[mediumArray.Length - 1];

            randomUser.LargeUrl = (string)item["user"]["picture"]["large"];
            var largeArray = randomUser.LargeUrl.Split('/');
            randomUser.Large = "large_" + randomUser.Gender + "_" + largeArray[largeArray.Length - 1];

            await GetImageFile(randomUser.ThumbnailUrl, randomUser.Thumbnail);
            await GetImageFile(randomUser.MediumUrl, randomUser.Medium);
            await GetImageFile(randomUser.LargeUrl, randomUser.Large);
            randomUser.Item = item;
            randomUser.Validate();
            return randomUser;
        }

        private async Task GetImageFile(string url, string fileName)
        {
            using (HttpClient webClient = new HttpClient())
            {
                using (var stream = await webClient.GetStreamAsync(url))
                {
                    using (var fileStream = File.Create(this.imageFolder + fileName))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
            }
        }

        private DateTime UnixTimeStampToDateTime(string unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            double uts = double.Parse(unixTimeStamp);
            dtDateTime = dtDateTime.AddSeconds(uts).ToLocalTime();
            return dtDateTime;
        }
    }

    internal class RandomUser
    {
        private bool valid = true;

        public dynamic Item
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public DateTime DateOfBirth
        {
            get;
            set;
        }

        public string DisplayName
        {
            get;
            set;
        }

        public string Gender
        {
            get;
            set;
        }

        public string ThumbnailUrl
        {
            get;
            set;
        }

        public string MediumUrl
        {
            get;
            set;
        }

        public string LargeUrl
        {
            get;
            set;
        }

        public string Thumbnail
        {
            get;
            set;
        }

        public string Medium
        {
            get;
            set;
        }

        public string Large
        {
            get;
            set;
        }

        public DateTime Registered
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public bool Valid
        {
            get
            {
                return this.valid;
            }
        }

        public void Validate()
        {
            if (Item == null)
            {
                valid = false;
                return;
            }

            if ((string.IsNullOrEmpty(FirstName)) || (string.IsNullOrEmpty(LastName)))
            {
                valid = false;
                return;
            }
            DisplayName = FirstName;

            while (DisplayName.Length < 4)
            {
                DisplayName += "_";
            }
        }
    }

    /// <summary>
    /// </summary>
    public static class LoremIpsum
    {
        // ReSharper disable once CSharpWarnings::CS1591
        public const int LorumIpsumDefaultLength = 4545;

        #region lorem ipsum 4545 bytes

        private static readonly string loremIpsum =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In et lacus in nunc vulputate auctor. Ut suscipit urna non purus pulvinar vel malesuada justo ultrices. Vestibulum luctus tortor quis purus malesuada aliquam. Quisque lacinia nibh tristique erat rhoncus cursus. Suspendisse potenti. Integer eget urna vitae purus convallis ullamcorper. Donec sit amet velit augue. Curabitur odio enim, dictum at venenatis eget, iaculis in nisl. Aliquam adipiscing enim quis risus porttitor volutpat. Duis luctus semper tortor eu dapibus. Suspendisse sed lorem leo, vitae varius dui. Nam auctor rutrum urna eget euismod. Curabitur nec sem at magna gravida porttitor id a felis. Curabitur vitae lacus lacus, sed sollicitudin leo. Integer scelerisque tempor ante, ut viverra ligula malesuada vel. Quisque tincidunt aliquet turpis eu accumsan. Nunc vehicula ornare dui at pulvinar. Ut at nibh in turpis suscipit pulvinar sed eget enim. Nam mattis convallis risus, vel malesuada risus luctus vitae. Vivamus rhoncus iaculis eleifend. Aliquam et lorem nulla. Ut nec dolor tortor, a venenatis orci. Donec eu lectus a sem pulvinar imperdiet eu sed lectus. In ultrices, eros ut tincidunt iaculis, eros enim aliquam velit, a fermentum nisl justo in tellus. Aliquam tortor nisl, varius non bibendum at, vehicula in neque. Aliquam elit metus, facilisis ut elementum vel, egestas non justo. Fusce posuere sodales magna nec congue. Proin dapibus vestibulum sem, vel dapibus ligula porta sit amet. Aliquam erat volutpat. Nulla et ligula nibh, in auctor arcu. Etiam porttitor vehicula mauris non gravida. Nam porta hendrerit malesuada. Morbi malesuada pellentesque quam, sed feugiat dui pharetra et. Vestibulum posuere dolor et libero fermentum lacinia. Donec augue orci, sollicitudin in egestas ut, sagittis eget diam. Pellentesque ultrices elit ut lorem accumsan nec rhoncus lacus condimentum. Nunc varius mi et nulla tincidunt sit amet dapibus arcu adipiscing. Nam urna dui, tempus id tristique vel, malesuada at ipsum. Quisque non sem velit, a pulvinar lorem. Proin commodo luctus nibh sed faucibus. Fusce placerat nunc at urna sodales consectetur. Proin bibendum, odio non vulputate sagittis, orci ante condimentum justo, eget vestibulum sapien enim et nisl. In et risus mi. Etiam auctor, purus non placerat ullamcorper, dui tellus tempor eros, quis fermentum orci lorem quis sapien. Vivamus eu volutpat elit. Suspendisse iaculis lacinia enim in tincidunt. Cras gravida lacinia diam, vel interdum eros mollis a. In mollis ipsum quis nulla porttitor sit amet ullamcorper sapien rhoncus. Suspendisse sed justo eget libero vehicula egestas id eget nisi. Fusce gravida, lectus nec egestas sollicitudin, quam leo ullamcorper felis, feugiat posuere tortor magna at nulla. Fusce lectus nisi, viverra vel fringilla sit amet, fermentum ac elit. Aenean venenatis, augue vel dictum aliquam, odio metus luctus erat, nec sodales est lorem et turpis. Etiam nec enim sit amet turpis fermentum ullamcorper. Aliquam luctus convallis nisi eget feugiat. Cras arcu magna, pretium in vulputate vel, consectetur sed magna. Etiam nulla augue, tincidunt id gravida ut, elementum quis nulla. Pellentesque ultricies sem vitae enim placerat at eleifend metus semper. Suspendisse non massa lectus. Nullam tincidunt adipiscing orci sit amet adipiscing. Ut eros nulla, rutrum ut blandit id, tempus a dui. Aliquam vitae nulla lacus. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Nullam molestie arcu vitae arcu euismod sed tempor elit sodales. Etiam est nisl, porta eget elementum ut, pharetra eget neque. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Vivamus orci ante, porttitor vel tincidunt vel, bibendum sed odio. Ut lorem erat, blandit bibendum accumsan id, gravida sed magna. Aenean porttitor congue arcu, id molestie purus malesuada vitae. Vivamus eget ipsum odio. Maecenas accumsan nibh quis orci sagittis vitae tristique tortor ornare. Nunc quis nibh non sem mattis facilisis sed vitae lorem. Etiam tincidunt faucibus orci, eu consequat dui commodo vel. Proin ante elit, eleifend id elementum ac, cursus et erat. Mauris ac felis vel arcu pharetra lobortis. Morbi dictum nunc a metus vulputate eget placerat purus rhoncus. Morbi ac sodales sapien. Etiam pulvinar rhoncus lorem non euismod. Fusce mi libero, interdum ut dictum sed, feugiat et diam. Nunc nec eros placerat eros interdum laoreet id vel quam. Sed quis eros a tortor feugiat rutrum sed eu lorem. Proin non augue at dui gravida bibendum.";

        #endregion

        /// <summary>
        ///   Returns the generated text as a char array.
        /// </summary>
        /// <returns></returns>
        private static List<char> LoremIpsumCharArray()
        {
            return loremIpsum.ToCharArray().ToList();
        }

        /// <summary>
        ///   Gets the lorem ipsum.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string GetLoremIpsum(int length)
        {
            List<char> charList = new List<char>();
            List<char> ipchar = LoremIpsumCharArray();

            // less than 4545 characters
            if (length <= LorumIpsumDefaultLength)
            {
                // lol this sucks
                return new string(ipchar.GetRange(0, length).ToArray());
            }

            // just add the char array into the list 
            // until we break the count barrier
            while (length - charList.Count > LorumIpsumDefaultLength)
            {
                charList.AddRange(LoremIpsumCharArray());
            }

            // number of characters left to fetch.
            int i = length % LorumIpsumDefaultLength;
            int j = 0;
            do
            {
                charList.Add(ipchar[j]);
                j++;
            }
            while (j < i);
            return new string(charList.ToArray());
        }

        /// <summary>
        ///   Gets a default chunk of LoremIpsum that is 4545 characters long.
        /// </summary>
        /// <returns><c>string</c> of steaming lorem ipsum</returns>
        public static string GetLoremIpsum()
        {
            return GetLoremIpsum(LorumIpsumDefaultLength);
        }

        /// <summary>
        ///   Ases the byte array.
        /// </summary>
        /// <returns></returns>
        public static byte[] AsByteArray()
        {
            return Encoding.ASCII.GetBytes(loremIpsum);
        }

        /// <summary>
        ///   Ases the stream.
        /// </summary>
        /// <returns></returns>
        public static MemoryStream AsStream()
        {
            return new MemoryStream(AsByteArray());
        }
    }
}