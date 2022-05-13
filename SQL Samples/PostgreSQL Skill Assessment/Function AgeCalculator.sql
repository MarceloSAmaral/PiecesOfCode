create function AGECALCULATOR(date_of_birth in date)
	returns integer
	as
$$
declare
	age_in_years integer;
begin
	age_in_years = date_part('year',age(NOW(), date_of_birth));
	return age_in_years;
end;
