default:
	docker-compose up -d 

build:
	docker-compose up -d --build

dev:
	docker-compose -f docker-compose-dev.yml up -d 
   
stop:
	docker-compose down