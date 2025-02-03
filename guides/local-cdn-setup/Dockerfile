FROM nginx:alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY certs/ingweland_localhost.crt /etc/nginx/ssl/
COPY certs/ingweland_localhost.key /etc/nginx/ssl/
RUN mkdir -p /etc/nginx/ssl