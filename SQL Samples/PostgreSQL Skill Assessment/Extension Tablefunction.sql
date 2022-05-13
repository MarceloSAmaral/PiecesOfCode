$$ language plpgsql;
CREATE EXTENSION tablefunc;
select
  products."name"
  ,ctDetails.good
  ,ctDetails.ok
  ,ctDetails.bad
from
  products
  inner join
  (
    SELECT
    *
    FROM crosstab('
select
    details.product_id
    ,details.detail
    ,count(1) as ct
from
    details
group by
    details.product_id
    ,details.detail  
order by 1
',$$VALUES ('good'), ('ok'),('bad')$$)
    AS ct(product_id integer, "good" integer, "ok" integer, "bad" integer)
  ) as ctDetails on (products.id = ctDetails.product_id)
order by
  products."name";
