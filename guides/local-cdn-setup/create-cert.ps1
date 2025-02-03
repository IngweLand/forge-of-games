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