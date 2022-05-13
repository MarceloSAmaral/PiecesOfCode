CREATE TABLE [dbo].[UsersFollowings]
(
        [UserId] uniqueidentifier not null
        ,[FollowsThisId] uniqueidentifier not null
		,[Since] datetime not null
		,constraint [PK_UsersFollowings] primary key clustered([UserId],[FollowsThisId])
		,constraint [FK_UsersFollowings-UserId] foreign key ([UserId]) references [dbo].[Users]([Id])
		,constraint [FK_UsersFollowings-FollowsThisId] foreign key ([FollowsThisId]) references [dbo].[Users]([Id])
);
GO