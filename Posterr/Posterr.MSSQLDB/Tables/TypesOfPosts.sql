CREATE TABLE [dbo].[TypesOfPosts]
(
	[Id] int not null
	,[Description] nvarchar(40) not null
	,constraint [pk_TypesOfPosts] primary key clustered([Id])
)
