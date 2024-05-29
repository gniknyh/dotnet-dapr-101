To convert pem key and cert to pfx format, don't forget your CAroot file, using
```
openssl pkcs12 -export -in identity.pem -inkey identity-key.pem -out identity.pfx -certfile link_mkdcert_rootCA.crt
```
