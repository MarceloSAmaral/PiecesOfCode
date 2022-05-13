insert into [dbo].[Users]
(
[Id]
,[UserName]
,[JoinDate]
)
select
	'00000000-0000-0000-0001-000000000001'
	,'Myself'
	,convert(datetime,'2022-01-25 08:00:00',120);
GO

insert into [dbo].[Users]
(
[Id]
,[UserName]
,[JoinDate]
)
select
	'00000000-0000-0000-0001-000000000002'
	,'Tom'
	,convert(datetime,'2022-01-26 08:00:00',120);
GO
insert into [dbo].[Users]
(
[Id]
,[UserName]
,[JoinDate]
)
select
	'00000000-0000-0000-0001-000000000003'
	,'Dick'
	,convert(datetime,'2022-01-26 08:00:00',120);
GO
insert into [dbo].[Users]
(
[Id]
,[UserName]
,[JoinDate]
)
select
	'00000000-0000-0000-0001-000000000004'
	,'Harry'
	,convert(datetime,'2022-01-26 08:00:00',120);
GO

declare @fruits table
(
	fruitId int not null
	,fruitName nvarchar(40) not null
)

insert @fruits
(
fruitID, fruitName
) 
values (1,'Orange')
,(2,'Apple')
,(3,'Banana')
,(4,'Avocado')


declare @referenceDate datetime
set @referenceDate = getdate()
insert [dbo].[Posts]
(
Id
,UserId
,PostType
,PostedAt
,Content
)
select
	newid() as Id
	,[dbo].[Users].[Id] as UserId
	,1 as PostType
	,dateadd(MINUTE,-1 * TFruits.fruitId, @referenceDate) as PostedAt
	,TFruits.fruitName
from
	[dbo].[Users]
	,@fruits as TFruits;

insert into [dbo].[UsersStats]
(
UserId
,NumberOfPosts
,NumberOfFollowing
,NumberOfFollowers
,LastUpdate
)
select
	[dbo].[Users].[Id]
	,4 /*From script*/
	,0
	,0
	,[dbo].[Users].[JoinDate]
from
	[dbo].[Users];

insert into [dbo].[UsersPostsStats]
(
ReferenceDate
,UserId
,PostsCounter
)
select
	convert(datetime, convert(varchar(10),@referenceDate,120),120)
	,[dbo].[Users].[Id]
	,4 /*From script*/
from
	[dbo].[Users];
GO