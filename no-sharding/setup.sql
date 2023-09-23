CREATE TABLE IF NOT EXISTS books  (
id bigint not null,
category_id  int not null,
author character varying not null,
title character varying not null,
year int not null );

CREATE INDEX IF NOT EXISTS books_category_id_idx ON books USING btree(category_id);
