Live Preview: <a href='https://snakefile.com'>Snakefile.com</a>

What is not included in the source code is the actual setup of the cloud environment such as Object Storage, VM and VCN.
The authentication credentials for OCI have also been redacted and their variable locations can be found in Program.cs (I'm aware a keys.cs with .gitignore would have been better)

<h2>Backend</h2>
The backend is composed of a ASP.NET Web API with two HTTP endpoints, PUT for uploads and GET for downloads.

Files are stored in a bucket on Oracle Cloud and the API uses the OCI SDK to communicate with this bucket.

Uploading a file returns a unique ID which is inserted into the URL while downloading a file returns the file bytes of the aforementioned ID.

IP rate-limits and CORS is also included. The rate limiting is based on the X-Forwarded-For header which is inserted in the HTTP Request by the NGINX reverse proxy.

The API runs 24/7 as a systemd service on a Ubuntu VM. The VM Shape is a very weak (VM.Standard.E2.1.Micro) and also runs NGINX which forwards the traffic.
The VM Instance is connected to a VCN which opens port 80 and 443 for NGINX.

<h2>Frontend</h2>
The frontend runs on standalone HTML, CSS and JavaScript with screen-adaptive CSS so that the website looks good on different resolutions.

JavaScript FETCH is used to communicate with the backend api through a subdomain (api.snakepoint.com) which is DNS routed to the backend VM ip address.
