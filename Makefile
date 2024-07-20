default:
	docker-compose up -d 

dev:
	docker-compose -f docker-compose-dev.yml up -d 
   
stop:
	docker-compose down