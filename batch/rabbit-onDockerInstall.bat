docker run -d --hostname finchattermq --name finchatter-rabbit -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=usr -e RABBITMQ_DEFAULT_PASS=Qwerty123$ rabbitmq:3-management 
pause