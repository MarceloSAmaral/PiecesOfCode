CREATE TABLE [dbo].[UsersPostsStats]
(
        [ReferenceDate] datetime not null
        ,[UserId] uniqueidentifier not null
        ,[PostsCounter] int not null
		,constraint [PK_UsersPostsStats] primary key clustered([ReferenceDate], [UserId])
		,constraint [FK_UsersPostsStats-UserId] foreign key ([UserId]) references [dbo].[Users]([Id])
);
GO