Live Preview: <a href='https://snakefile.com'>Snakefile.com</a>

Source code for a file sharing website.
The redacted keys are located in Program.cs (keys class with gitignore would have been better, lazy)

<h2>Backend</h2>
One ASP.NET Web API With POST for uploads and GET for downloads.
<br>
The API communicates with a Oracle Cloud Bucket with the OCI SDK to upload and retrieve files.
<br><br>
IP rate-limits are also included as middleware in the API for scalability. The rate limiting is based on the X-Forwarded-For header which is inserted in the HTTP Request by the NGINX reverse proxy.
<br><br>
The API runs 24/7 as a systemd service on a Ubuntu VM. The VM Shape for the live preview is very weak (VM.Standard.E2.1.Micro).

<h2>Frontend</h2>
The frontend runs on standalone HTML, CSS and JavaScript with screen-adaptive CSS.
<br>
FETCH is used to communicate with the backend api through a subdomain (api.snakepoint.com) which is DNS routed to the backend VM ip address.
