docker compose -p citus up --scale worker=2 -d

docker exec citus_master psql --user postgres -f "/sql_scripts/setup.sql"
