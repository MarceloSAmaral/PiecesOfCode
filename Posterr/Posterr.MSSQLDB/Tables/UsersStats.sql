CREATE TABLE [dbo].[UsersStats]
(
	[UserId]  uniqueidentifier not null
	,[LastUpdate] datetime not null
	,[NumberOfFollowers] int not null
	,[NumberOfFollowing] int not null
	,[NumberOfPosts] int not null
	,constraint [PK_UsersStats] primary key clustered([UserId])
	,constraint [FK_UsersStats-UserId] foreign key ([UserId]) references [dbo].[Users]([Id])
);
GO