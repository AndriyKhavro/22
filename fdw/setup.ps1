docker compose up -d

docker exec postgresql-b createdb -U postgres books
docker exec postgresql-b1 createdb -U postgres books
docker exec postgresql-b2 createdb -U postgres books

docker exec postgresql-b1 psql --user postgres -d books -f "/sql_scripts/setup.sql"
docker exec postgresql-b2 psql --user postgres -d books -f "/sql_scripts/setup.sql"
docker exec postgresql-b psql --user postgres -d books -f "/sql_scripts/setup.sql"

docker exec postgresql-b1 psql --user postgres -d books -c 'ALTER TABLE books ADD CONSTRAINT category_id_check CHECK ( category_id = 1 )'
docker exec postgresql-b2 psql --user postgres -d books -c 'ALTER TABLE books ADD CONSTRAINT category_id_check CHECK ( category_id = 2 )'
