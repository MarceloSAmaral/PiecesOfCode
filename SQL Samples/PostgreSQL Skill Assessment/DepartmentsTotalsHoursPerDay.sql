select
  DATE(TotalHours.date_of_work) as "day"
  ,department."name" as department_name
  ,TotalHours.amount_of_hours as "total_hours"
from
  department
  inner join
  (
    select
      timesheet.department_id
      ,date_trunc('day',timesheet.login) as date_of_work
      ,extract(epoch from sum(age(timesheet.logout,timesheet.login))) / 3600 as amount_of_hours
    from
      timesheet
    group by
      timesheet.department_id
      ,date_trunc('day',timesheet.login)
  ) as TotalHours on (TotalHours.department_id = department.id)
order by
  "day"
  ,department_name
