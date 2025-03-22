<h2>A File Sharing Website - <a href="https://snakefile.com">Click here for live demo</a></h2>

<h3>Backend</h3>
* Files are stored in a bucket on Oracle Cloud
<br>
* One .NET API with Nginx running on a Ubuntu VM. The VM Shape for the live demo is very weak (VM.Standard.E2.1.Micro)
<br>
* IP rate-limits are included for scalability
<h3>Frontend</h3>
Built on standalone HTML, CSS and Javascript.
<br>
Fetch() is used to communicate with the backend api through a subdomain (api.snakepoint.com) which is DNS routed to the VM ip address.
