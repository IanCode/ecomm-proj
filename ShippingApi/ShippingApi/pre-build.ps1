# Script to check if the couchbase container is running and optionally pull, run, and configure the container.

$containerId = $(docker ps -qf "name=ce-6.5")

if($containerId)
{
	echo "The container ce-6.5 is already running"
}
else
{
	docker run -d --name ce-6.5 -p 8091-8096:8091-8096 -p 11210-11211:11210-11211 couchbase:community-6.5.0
	echo "Started container ce-6.5"

	#cluster and bucket initialization were not working unless I waited for a little bit.
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2

	docker exec -it ce-6.5 couchbase-cli cluster-init --cluster-username Administrator --cluster-password password --cluster-name myNewCE6.5 --services data,query,index,fts
	echo "Initialized new cluster"

	docker exec -it ce-6.5 couchbase-cli bucket-create --cluster localhost --username Administrator --password password --bucket default --bucket-type couchbase --bucket-ramsize 100
	echo "Created a new bucket"

	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2
	echo "Waiting..."
	Start-Sleep -s 2

	docker exec -it ce-6.5 /opt/couchbase/bin/cbq -e http://localhost:8091/ -u=Administrator -p=password --script="CREATE PRIMARY INDEX ON default;"
	docker exec -it ce-6.5 /opt/couchbase/bin/cbq -e http://localhost:8091/ -u=Administrator -p=password --script="CREATE INDEX adaptive_default ON default(DISTINCT PAIRS(self));"
	echo "Created indexes"

	Start-Sleep -s 3

	#uploads json data file to docker container
	docker cp Data/products.json ce-6.5:/products.json
	#docker cp [OPTIONS] CONTAINER:SRC_PATH DEST_PATH|-

	docker exec -it ce-6.5 cbimport json -c localhost -u Administrator -p password -b default -d file://products.json -g %productId% -f list
	#cbimport json -c couchbase://127.0.0.1 -u Administrator -p password \ -b default -d file:///data/lines.json -f lines -g key::%name% -t 4
	echo "Loaded Test Data"
}