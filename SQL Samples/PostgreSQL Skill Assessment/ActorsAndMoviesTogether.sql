create temporary table TActorsTogether
(
	first_actor_id integer not null
	,second_actor_id integer not null
);

create temporary table TMoviesTogether
(
	first_actor_id integer not null
	,second_actor_id integer not null
	,film_id integer not null
);
 
insert into TActorsTogether(first_actor_id,second_actor_id)
select
	first_actor_film.actor_id as first_actor_id
	,second_actor_film.actor_id as second_actor_id
from
	film_actor as first_actor_film
	inner join film_actor as second_actor_film
	on (
		first_actor_film.film_id = second_actor_film.film_id
		and first_actor_film.actor_id < /*actor_id of the first_actor should be lower then actor_id of the second_actor*/ second_actor_film.actor_id
	)
group by
	first_actor_film.actor_id
	,second_actor_film.actor_id
order by
	count(*) desc
limit 1;

insert into TMoviesTogether(first_actor_id,second_actor_id,film_id)
select
	TActorsTogether.first_actor_id
	,TActorsTogether.second_actor_id
	,first_actor_film.film_id
from
	TActorsTogether
	inner join film_actor as first_actor_film on (TActorsTogether.first_actor_id = first_actor_film.actor_id)
	inner join film_actor as second_actor_film
	on (
		TActorsTogether.second_actor_id = second_actor_film.actor_id
		and first_actor_film.film_id = second_actor_film.film_id
	);
 
select
	first_actor.first_name || ' ' || first_actor.last_name as first_actor
	,second_actor.first_name || ' ' || second_actor.last_name as second_actor
	,film.title
from
	TMoviesTogether
	inner join actor as first_actor on (TMoviesTogether.first_actor_id = first_actor.actor_id)
	inner join actor as second_actor on (TMoviesTogether.second_actor_id = second_actor.actor_id)
	inner join film on (TMoviesTogether.film_id = film.film_id)
order by
  film.title


