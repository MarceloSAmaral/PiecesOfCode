CREATE TABLE [dbo].[Posts]
(
	[Id] uniqueidentifier not null
    ,[UserId] uniqueidentifier not null
    ,[PostType] int not null
    ,[Content] nvarchar(1554) null
    ,[PostedAt] datetime not null
    ,[RepostFrom] uniqueidentifier null
    ,[QuoteFrom] uniqueidentifier null
    ,constraint [PK_Posts] primary key nonclustered ([Id])
    ,constraint [FK_Posts-UserId] foreign key ([UserId]) references [dbo].[Users](Id)
    ,constraint [FK_Posts-RepostFrom] foreign key ([RepostFrom]) references [dbo].[Posts](Id)
    ,constraint [FK_Posts-QuoteFrom] foreign key ([QuoteFrom]) references [dbo].[Posts](Id)
);
GO

create clustered index [CX_Posts] on [dbo].[Posts]([PostedAt] asc);
GO

