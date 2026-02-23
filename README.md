📘 LLM-Based Document Search System (Python + MVC)
🔷 Project Overview

LLM-Python-MVC Project is an end-to-end AI-powered document search system built using ASP.NET MVC and Python.

The system enables users to securely upload PDF documents, automatically process and vectorize their content using transformer-based embeddings, and perform semantic search queries across uploaded documents.

This project demonstrates a complete Retrieval-Augmented Generation (RAG) style architecture suitable for enterprise and government document intelligence use cases.

🔷 Key Features

🔐 Secure Login Authentication (MVC)

📄 PDF Document Upload & Storage

🧠 Automatic Text Extraction from PDFs

📊 Embedding Generation using all-MiniLM-L6-v2

🔎 Semantic Search over Uploaded Documents

⚡ Python API integration with MVC frontend

🗂 Modular architecture for scalability

🔷 Technology Stack
Frontend

ASP.NET MVC (.NET Framework / .NET Core)

Razor Views (.cshtml)

Backend API

Python

REST API (Flask/FastAPI)

AI & NLP

SentenceTransformers

Embedding Model: all-MiniLM-L6-v2

Vector similarity search

Supporting Libraries

PDF text extraction library

JSON-based data exchange

Local vector storage

🔷 System Architecture
User
  ↓
ASP.NET MVC Web Application
  ↓
Python API Service
  ↓
PDF Text Extraction
  ↓
Embedding Generation (all-MiniLM-L6-v2)
  ↓
Vector Storage
  ↓
Semantic Search
  ↓
Search Results Returned to MVC UI
🔷 End-to-End Workflow
1️⃣ Authentication

User logs into the system using secure MVC-based login.

2️⃣ Document Upload

User uploads a PDF document.

3️⃣ Document Processing

PDF content is extracted.

Text is chunked and processed.

Embeddings are generated using transformer-based model.

4️⃣ Storage

Embeddings are stored for similarity comparison.

5️⃣ Search

User submits a natural language query.

6️⃣ Semantic Retrieval

System computes similarity between query embedding and stored document embeddings.

7️⃣ Results

Most relevant document sections are returned and displayed.

🔷 Use Cases

Government document search

Policy document analysis

Legal document intelligence

Knowledge base search

Enterprise internal document search

Academic research repository search

🔷 Installation & Setup
1️⃣ Clone Repository
git clone https://github.com/GleamTechnologies/LLM-Python-MVC-project.git
2️⃣ Python API Setup

Install dependencies:

pip install -r requirements.txt

Run API service:

python app.py
3️⃣ MVC Application Setup

Open solution in Visual Studio

Configure Python API endpoint in settings

Build and run the MVC project

🔷 Configuration

Update API endpoint in MVC project:

API_BASE_URL = http://localhost:5000

Ensure Python service is running before starting MVC application.

🔷 Security Considerations

Authentication required before document upload

Controlled file upload handling

No direct exposure of embedding logic to client

API-level isolation between frontend and AI processing layer

🔷 Model Details

Embedding Model Used:

all-MiniLM-L6-v2

Lightweight transformer-based sentence embedding model

Efficient semantic similarity search

Suitable for enterprise-scale document indexing

🔷 Project Type

✔ End-to-End LLM-Based Application
✔ Retrieval-Augmented Search System
✔ Open Source Release
✔ Enterprise-Ready Architecture

🔷 Version

Current Version: v1.0
Release Type: Open Source
