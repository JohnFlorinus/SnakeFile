<h1>Source code for a file sharing website.</h1>
Live Preview: <a href='https://snakefile.com'>Snakefile.com</a>


<h2>Backend</h2>
1 REST API that acts as a middleman to the Oracle Cloud Bucket containing the files.
<br>
.NET OCI SDK is used in the API solution for easy integration.
<br><br>
IP rate-limits are included in the API for scalability. The rate limiting is based on the X-Forwarded-For header which is inserted in the HTTP Request by the NGINX reverse proxy.
<br><br>
The API runs 24/7 as a systemd service on a Ubuntu VM. The VM Shape for the live preview is very weak (VM.Standard.E2.1.Micro).

<h2>Frontend</h2>
The frontend runs on standalone HTML, CSS and Javascript.
<br>
Fetch() is used to communicate with the backend api through a subdomain (api.snakepoint.com) which is DNS routed to the VM ip address.
