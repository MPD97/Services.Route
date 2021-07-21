docker build -t service.route . ;
docker tag service.route mateusz9090/route:local ;
docker push mateusz9090/route:local