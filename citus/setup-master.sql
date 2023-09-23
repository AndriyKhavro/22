CREATE TABLE IF NOT EXISTS books  (
id bigint not null,
category_id  int not null,
author character varying not null,
title character varying not null,
year int not null );

SELECT create_distributed_table('books', 'category_id');
