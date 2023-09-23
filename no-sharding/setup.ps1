docker compose up -d

docker exec postgresql-b-no-sharding createdb -U postgres books
docker exec postgresql-b-no-sharding psql --user postgres -d books -f "/sql_scripts/setup.sql"
