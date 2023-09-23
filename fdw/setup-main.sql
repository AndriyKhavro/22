CREATE EXTENSION postgres_fdw;

CREATE SERVER books_1_server 
FOREIGN DATA WRAPPER postgres_fdw 
OPTIONS(host 'postgresql-b1', port '5432', dbname 'books');

CREATE USER MAPPING FOR postgres
SERVER books_1_server
OPTIONS (user 'postgres', password 'postgres');

CREATE SERVER books_2_server 
FOREIGN DATA WRAPPER postgres_fdw 
OPTIONS(host 'postgresql-b2', port '5432', dbname 'books');

CREATE USER MAPPING FOR postgres
SERVER books_2_server
OPTIONS (user 'postgres', password 'postgres');

CREATE FOREIGN TABLE books_1 (
id bigint not null,
category_id  int not null,
author character varying not null,
title character varying not null,
year int not null )
SERVER books_1_server
OPTIONS (schema_name 'public', table_name 'books');

CREATE FOREIGN TABLE books_2 (
id bigint not null,
category_id  int not null,
author character varying not null,
title character varying not null,
year int not null )
SERVER books_2_server
OPTIONS (schema_name 'public', table_name 'books');

CREATE VIEW books AS
	SELECT * FROM books_1
		UNION ALL
	SELECT * FROM books_2;

CREATE RULE books_insert_to_1 AS ON INSERT TO books
WHERE ( category_id = 1 )
DO INSTEAD INSERT INTO books_1 VALUES (NEW.*);

CREATE RULE books_insert_to_2 AS ON INSERT TO books
WHERE ( category_id = 2 )
DO INSTEAD INSERT INTO books_2 VALUES (NEW.*);

-- these rules are required to avoid this error:
-- MessageText: cannot insert into view "books"
-- Hint: To enable inserting into the view, provide an INSTEAD OF INSERT trigger or an unconditional ON INSERT DO INSTEAD rule.
CREATE RULE books_insert AS ON INSERT TO books
DO INSTEAD NOTHING;
CREATE RULE books_update AS ON UPDATE TO books
DO INSTEAD NOTHING;
CREATE RULE books_delete AS ON DELETE TO books
DO INSTEAD NOTHING;

