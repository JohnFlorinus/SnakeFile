# 📂 File Sharing Website - [🔗 Live Demo](https://snakefile.com)

---

## 🌐 Project Overview
This is a simple **file sharing website** built with a lightweight stack.  
It allows users to upload and access files via a **.NET API backend** running on Oracle Cloud Infrastructure (OCI), with a static HTML/JS frontend.  

⚠️ This **README** was AI-generated for better readability and structure. However, the project itself was manually coded by myself.  

---

## 🛠️ Backend

- ☁️ **Storage**: Files are stored in an **Oracle Cloud Object Storage bucket**.  
- 🌐 **API Layer**:  
  - Backend is a **.NET Web API** running on an Oracle Cloud VM.  
  - VM shape for demo: **`VM.Standard.E2.1.Micro`** (very minimal resources).  
- 🔒 **Nginx Reverse Proxy**:  
  - Handles HTTPS and routing.  
  - Configured to link the domain → API.  
- 📈 **Scalability**:  
  - Basic **IP rate limiting + server-side checks** included to prevent abuse.  

---

## 🎨 Frontend

- 🖥️ Built with **standalone HTML, CSS, and JavaScript** (no frameworks).  
- 🔗 **API Communication**:  
  - Uses JavaScript `fetch()` to interact with backend API endpoints.  
  - API served at **`api.snakepoint.com`**, DNS-routed to the VM’s public IP.  
