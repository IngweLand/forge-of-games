# Create this directory structure:
```
nginx-https/
├── certs/
├── nginx.conf
├── Dockerfile
├── docker-compose.yml
├── index.html
└── www
```


# 1. certs
### a) PowerShell. Create PFX cert for Windows and install it into trusted root. Export crt and key for nginx (with openssl)
```powershell
# Run as administrator
$certName = "localhost"
$cert = New-SelfSignedCertificate -DnsName $certName -CertStoreLocation "cert:\LocalMachine\My" -FriendlyName "Ingweland Local Development Certificate" -NotAfter (Get-Date).AddYears(10)

# Export certificate to files
$pwd = ConvertTo-SecureString -String "password" -Force -AsPlainText
$certPath = "certs"
$certFileName = "ingweland_localhost"
New-Item -ItemType Directory -Force -Path $certPath
# Export PFX
Export-PfxCertificate -Cert "cert:\LocalMachine\My\$($cert.Thumbprint)" -FilePath "$certPath\$certFileName.pfx" -Password $pwd
# Export CER
#Export-Certificate -Cert "cert:\LocalMachine\My\$($cert.Thumbprint)" -FilePath "$certPath\$certFileName.cer"

# Export certificate in PEM format
& "C:\Program Files\Git\usr\bin\openssl.exe" pkcs12 -in "$certPath\$certFileName.pfx" -clcerts -nokeys -out "$certPath\$certFileName.crt" -password pass:password
# Export private key
& "C:\Program Files\Git\usr\bin\openssl.exe" pkcs12 -in "$certPath\$certFileName.pfx" -nocerts -out "$certPath\$certFileName.key" -nodes -password pass:password

# Trust the certificate
Import-PfxCertificate -FilePath "$certPath\$certFileName.pfx" -CertStoreLocation "cert:\LocalMachine\Root" -Password $pwd

Write-Host "Certificate created and trusted. Files saved to certs"
Write-Host "Thumbprint: $($cert.Thumbprint)"
```
### b) openssl
```commandline
openssl req -x509 -nodes -days 3650 -newkey rsa:2048 \
  -keyout certs/ingweland_localhost.key \
  -out certs/ingweland_localhost.crt \
  -subj "//CN=localhost" \
  -addext "subjectAltName=DNS:localhost,IP:127.0.0.1"
  -addext "friendlyName=Local Development Certificate"
```
# 2. nginx.conf
```
server {
    listen 80;
    server_name localhost;
    return 301 https://$host:9443$request_uri;
}

server {
    listen 443 ssl;
    server_name localhost;
    ssl_certificate /etc/nginx/ssl/ingweland_localhost.crt;
    ssl_certificate_key /etc/nginx/ssl/ingweland_localhost.key;
    ssl_protocols TLSv1.2 TLSv1.3;
    ssl_ciphers HIGH:!aNULL:!MD5;

    location ~* \.(png|jpg|jpeg|gif|ico|svg)$ {
        root /usr/share/nginx/html;
        add_header Access-Control-Allow-Origin "*" always;
        add_header Access-Control-Allow-Methods "GET, OPTIONS" always;
        add_header Access-Control-Allow-Headers "*" always;
        try_files $uri =404;
    }
	
	location / {
        root /usr/share/nginx/html;
		index index.html;
    }
}
```

# 3. Dockerfile
```dockerfile
FROM nginx:alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY certs/ingweland_localhost.crt /etc/nginx/ssl/
COPY certs/ingweland_localhost.key /etc/nginx/ssl/
RUN mkdir -p /etc/nginx/ssl
```


# 4. docker-compose.yml Change the www directory to the one you need. Server will serve files from this dir.
```yaml
version: '3.8'
services:
  nginx:
    container_name: localhost-cdn
    build: .
    image: localhost-cdn-nginx:latest
    ports:
      - "9080:80"
      - "9443:443"
    volumes:
      - ./index.html:/usr/share/nginx/html/index.html
      - ./www:/usr/share/nginx/html/
```


# 5. index.html
```html
<!DOCTYPE html>
<html>
<head>
    <title>NGINX HTTPS Test</title>
</head>
<body>
    <h1>NGINX is running with HTTPS!</h1>
</body>
</html>
```

# 6. docker-compose
```commandline
docker-compose up --build
```