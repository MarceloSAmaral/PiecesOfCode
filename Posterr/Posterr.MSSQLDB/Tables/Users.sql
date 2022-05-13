CREATE TABLE [dbo].[Users]
(
	[Id] uniqueidentifier not null
	,[UserName] nvarchar(28) not null
	,[JoinDate] datetime not null
	,constraint [PK_Users] primary key nonclustered ([Id])
	,constraint [UQ_Users-UserName] unique ([UserName])
	,constraint [ck_Users-UserName] check ([UserName] not like '%[^a-zA-Z0-9]%')
);
GO

create clustered index [CX_Users] on [dbo].[Users]([UserName]);
GO;