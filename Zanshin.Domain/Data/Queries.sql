
-- update topic post counts
update dbo.Topics set dbo.Topics.PostCount = (select count(*) from dbo.Posts p where p.TopicId = Topics.TopicId)

-- update posts and assign a random user to it
update dbo.Posts set dbo.Posts.UserId =  (ABS(CHECKSUM(NewId())) % 100) + 1

-- update post topic starter id
update dbo.Topics set dbo.Topics.UserId = (select Top(1) dbo.Posts.Userid from dbo.Posts where dbo.Posts.TopicId = dbo.Topics.TopicId)

-- update topic starter names to correct user name
update dbo.topics set topics.TopicStarterName = (select u.DisplayName  from dbo.users u where u.id = dbo.topics.UserId)

-- update user post counts
update dbo.Users set dbo.Users.PostCount = (select count(*) from dbo.Posts p where p.UserId = dbo.Users.Id)

-- update user topic counts
update dbo.Users set dbo.Users.TopicCount = (select count(*) from dbo.Topics t where t.UserId = dbo.Users.Id)

-- update user ranks
  update dbo.users set users.RankId = 2 where dbo.Users.PostCount > 50
  update dbo.users set users.RankId = 3 where dbo.Users.PostCount > 100
  update dbo.users set users.RankId = 4 where dbo.Users.PostCount > 250
  update dbo.users set users.RankId = 5 where dbo.Users.PostCount > 500
  update dbo.users set users.RankId = 6 where dbo.Users.PostCount > 1000