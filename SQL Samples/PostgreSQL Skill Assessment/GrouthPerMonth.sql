create temporary table TResult
(
	month_of_posts date not null
	,amount_of_posts integer not null
	,percent_growth numeric(9,2) null
);

insert into TResult(month_of_posts,amount_of_posts)
select
	date_trunc('month',posts.created_at)
	,count(*)
from
	posts
group by
	date_trunc('month',posts.created_at);

update TResult set
	percent_growth = ((TResult.amount_of_posts - previous_month.amount_of_posts)* 100.00) / previous_month.amount_of_posts
from
	TResult as previous_month
where
	TResult.amount_of_posts != 0
	and (TResult.month_of_posts = (previous_month.month_of_posts + interval '1 month'));


select
	month_of_posts as "date"
	,amount_of_posts as "count"
	,LTrim(to_char(percent_growth,'9990.0')) || '%' as percent_growth
from
	TResult
order by
	"date"

