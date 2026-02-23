from flask import Flask, request, jsonify
from pypdf import PdfReader
from langchain.text_splitter import CharacterTextSplitter
from langchain.vectorstores import FAISS
from langchain.embeddings import HuggingFaceEmbeddings
import requests

app = Flask(__name__)

vectorstore = None
embeddings = HuggingFaceEmbeddings(model_name="all-MiniLM-L6-v2")

OLLAMA_URL = "http://localhost:11434/api/generate"
MODEL_NAME = "llama3"

@app.route("/upload", methods=["POST"])
def upload_pdf():
    global vectorstore

    file = request.files["file"]
    reader = PdfReader(file)

    text = ""
    for page in reader.pages:
        text += page.extract_text() or ""

    splitter = CharacterTextSplitter(chunk_size=500, chunk_overlap=50)
    chunks = splitter.split_text(text)

    vectorstore = FAISS.from_texts(chunks, embeddings)

    return "PDF processed successfully!"

@app.route("/ask", methods=["POST"])
def ask_question():
    global vectorstore

    if vectorstore is None:
        return "Upload a PDF first!", 400

    question = request.json["question"]

    docs = vectorstore.similarity_search(question, k=3)
    context = "\n".join([doc.page_content for doc in docs])

    prompt = f"""
Answer ONLY using the context below.
If answer not found, say 'Not found in document'.

Context:
{context}

Question:
{question}
"""

    response = requests.post(
        OLLAMA_URL,
        json={
            "model": MODEL_NAME,
            "prompt": prompt,
            "stream": False
        }
    )

    return jsonify(response.json())

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)
